#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Quill.Attrs;
using Seasar.Quill.Dao;
using Seasar.Quill.Dao.Interceptor;
using Seasar.Quill.Database.DataSource.Impl;
using Seasar.Quill.Database.Tx;
using Seasar.Quill.Exception;
using Seasar.Quill.Util;

namespace Seasar.Quill
{
    /// <summary>
    /// Aspect定義を構築するクラス
    /// </summary>
    /// <remarks>
    /// <para>
    /// Aspectを適用する場合はインターフェース・クラスもしくはメソッドに
    /// <see cref="Seasar.Quill.Attrs.AspectAttribute"/>(属性)が
    /// 設定されている必要がある。
    /// </para>
    /// <para>
    /// S2Daoの機能を(diconなしで)使用するにはインターフェース・クラスもしくはメソッドに
    /// <see cref="Seasar.Quill.Attrs.S2DaoAttribute"/>(属性)が
    /// 設定されている必要がある。
    /// </para>
    /// <para>
    /// Transactionを(diconなしで)使用するにはインターフェース・クラスもしくはメソッドに
    /// <see cref="Seasar.Quill.Attrs.TransactionAttribute"/>(属性)が
    /// 設定されている必要がある。
    /// </para>
    /// </remarks>
    public class AspectBuilder
    {
        // AspectBuilder内で使用するQuillContainer
        // (Interceptorを取得する為に使用する)
        protected QuillContainer container;

        /// <summary>
        /// AspectBuilderを初期化するためのコンストラクタ
        /// </summary>
        /// <param name="container">AspectBuilder内で使用するQuillContainer</param>
        public AspectBuilder(QuillContainer container)
        {
            // AspectBuilder内で使用するためのQuillContainerを
            // コンストラクタ引数から受け取る
            this.container = container;
        }

        /// <summary>
        /// 指定された<code>type</code>の属性を確認して
        /// Aspect定義の配列を作成する
        /// </summary>
        /// <param name="type">Aspect定義を確認するType</param>
        /// <returns>作成されたAspect定義の配列</returns>
        public virtual IAspect[] CreateAspects(Type type)
        {
            // Aspectのリスト
            var aspectList = new List<IAspect>();

            // typeで宣言されているメソッドを取得する
            var methods = type.GetMethods(BindingFlags.Public | 
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            CreateFromAspectAttribute(type, methods, aspectList);
            CreateFromTransactionAttribute(type, methods, aspectList);
            CreateFromS2DaoAttribute(type, methods, aspectList);

            // Aspectの配列を返す
            return aspectList.ToArray();
        }

        /// <summary>
        /// Aspect属性からアスペクトを作成する
        /// </summary>
        /// <param name="targetType">Aspectを適用するメソッドをもつ型</param>
        /// <param name="methods">Aspectを適用するメソッド</param>
        /// <param name="aspectList">適用するAspectのリスト</param>
        protected virtual void CreateFromAspectAttribute(
            Type targetType, MethodInfo[] methods, List<IAspect> aspectList)
        {
            // Typeに指定されたAspectを指定する属性を取得する
            var attrsByType = AttributeUtil.GetAspectAttrs(targetType);

            // Typeに指定されているAspectの件数分、Aspectをリストに追加する
            foreach (var attrByType in attrsByType)
            {
                // Pointcutに全てのメソッドが追加されているAspectを作成する
                var aspect = CreateAspect(attrByType);

                // 作成したAspectをリストに追加する
                aspectList.Add(aspect);
            }

            // Methodに指定されているAspectをリストに追加する
            aspectList.AddRange(CreateAspectList(methods));
        }

        /// <summary>
        /// Transaction属性からアスペクトを作成する
        /// </summary>
        /// <param name="targetType">Aspectを適用するメソッドをもつ型</param>
        /// <param name="methods">Aspectを適用するメソッド</param>
        /// <param name="aspectList">適用するAspectのリスト</param>
        protected virtual void CreateFromTransactionAttribute(
            Type targetType, MethodInfo[] methods, List<IAspect> aspectList)
        {
            //  Typeに指定されたトランザクションを指定する属性を取得する
            var txAttrsByType = AttributeUtil.GetTransactionAttrs(targetType);
            if (txAttrsByType != null)
            {
                
                foreach (var txAttrByType in txAttrsByType)
                {
                    //  データソース選択Interceptorが定義されている場合は
                    //  先にAspectを登録
                    var dataSourceSelctAspect = GetDataSourceSelectAspect(txAttrByType);
                    if (dataSourceSelctAspect != null)
                    {
                        aspectList.Add(dataSourceSelctAspect);
                    }

                    var txAspect = CreateTxAspect(txAttrByType);
                    if (txAspect != null)
                    {
                        aspectList.Add(txAspect);
                    }
                }
            }

            // Methodに指定されているTransaction用Aspectをリストに追加する
            aspectList.AddRange(CreateTxAspectList(methods));
        }

        /// <summary>
        /// S2Dao属性からアスペクトを作成する
        /// </summary>
        /// <param name="targetType">Aspectを適用するメソッドをもつ型</param>
        /// <param name="methods">Aspectを適用するメソッド</param>
        /// <param name="aspectList">適用するAspectのリスト</param>
        protected virtual void CreateFromS2DaoAttribute(
            Type targetType, MethodInfo[] methods, List<IAspect> aspectList)
        {
            //  Typeに指定されたDaoInterceptorを指定する属性を取得する
            var daoAttrByType = AttributeUtil.GetS2DaoAttr(targetType);

            if (daoAttrByType != null)
            {
                //  データソース選択Interceptorが定義されている場合は
                //  先にAspectを登録
                var dataSourceSelctAspect = GetDataSourceSelectAspect(daoAttrByType, targetType);
                if (dataSourceSelctAspect != null)
                {
                    aspectList.Add(dataSourceSelctAspect);
                }

                //  S2DaoInterceptorを登録
                var daoAspectToClass = CreateS2DaoAspect(daoAttrByType);
                if (daoAspectToClass != null)
                {
                    aspectList.Add(daoAspectToClass);
                }
            }

            // Methodに指定されているTransaction用Aspectをリストに追加する
            IList<IAspect> daoAspectsToMethod = CreateS2DaoAspectList(methods);
            if (daoAspectsToMethod != null && daoAspectsToMethod.Count > 0)
            {
                aspectList.AddRange(daoAspectsToMethod);
            }
        }

        /// <summary>
        /// データソース選択Interceptorの取得
        /// </summary>
        /// <param name="daoAttr">S2Dao属性</param>
        /// <param name="targetMember">Interceptorをかける対象</param>
        /// <returns>データソース選択Interceptor</returns>
        protected virtual IMethodInterceptor GetDataSourceSelectInterceptor(S2DaoAttribute daoAttr, MemberInfo targetMember)
        {
            DataSourceSelectInterceptor dsInterceptor = null;
            var daoSettingType = daoAttr.DaoSettingType;
            if (daoSettingType != null)
            {
                var daoSetting = (IDaoSetting)ComponentUtil.GetComponent(container, daoSettingType);
                var dataSourceName = daoSetting.DataSourceName;
                dsInterceptor = CreateDataSourceSelectInterceptor(dataSourceName);
            }
            return dsInterceptor;
        }

        /// <summary>
        /// データソース選択Interceptorの取得
        /// </summary>
        /// <param name="txAttr">Transaction属性</param>
        /// <returns>データソース選択Interceptor</returns>
        protected virtual IMethodInterceptor GetDataSourceSelectInterceptor(TransactionAttribute txAttr)
        {
            DataSourceSelectInterceptor dsInterceptor = null;
            var txSettingType = txAttr.TransactionSettingType;
            if (txSettingType != null)
            {
                var txSetting = (ITransactionSetting)ComponentUtil.GetComponent(container, txSettingType);
                var dataSourceName = txSetting.DataSourceName;
                dsInterceptor = CreateDataSourceSelectInterceptor(dataSourceName);
            }
            return dsInterceptor;
        }

        /// <summary>
        /// データソース選択Interceptorの取得
        /// </summary>
        /// <param name="txAttr">Transaction属性</param>
        /// <param name="targetMember">Interceptorをかける対象</param>
        /// <returns>データソース選択Interceptor</returns>
        protected virtual IMethodInterceptor GetDataSourceSelectInterceptor(TransactionAttribute txAttr, MemberInfo targetMember)
        {
            DataSourceSelectInterceptor dsInterceptor = null;
            var txSettingType = txAttr.TransactionSettingType;
            if (txSettingType != null)
            {
                var txSetting = (ITransactionSetting)ComponentUtil.GetComponent(container, txSettingType);
                var dataSourceName = txSetting.DataSourceName;
                dsInterceptor = CreateDataSourceSelectInterceptor(dataSourceName);
            }
            return dsInterceptor;
        }

        /// <summary>
        /// データソース選択Aspectの取得
        /// </summary>
        /// <param name="daoAttr">S2Dao属性</param>
        /// <param name="targetMember">Aspectをかける対象</param>
        /// <returns>データソース選択Aspect</returns>
        protected virtual IAspect GetDataSourceSelectAspect(S2DaoAttribute daoAttr, MemberInfo targetMember)
        {
            var dataSourceSelectInterceptor = GetDataSourceSelectInterceptor(daoAttr, targetMember);

            IAspect dataSourceSelectAspect = null;
            if(dataSourceSelectInterceptor != null)
            {
                dataSourceSelectAspect = new AspectImpl(dataSourceSelectInterceptor);
            }
            return dataSourceSelectAspect;
        }

        /// <summary>
        /// データソース選択Aspectの取得
        /// </summary>
        /// <param name="txAttr">Transaction属性</param>
        /// <param name="targetMember">Aspectをかける対象</param>
        /// <returns>データソース選択Aspect</returns>
        protected virtual IAspect GetDataSourceSelectAspect(TransactionAttribute txAttr, MemberInfo targetMember)
        {
            var dataSourceSelectInterceptor = 
                GetDataSourceSelectInterceptor(txAttr, targetMember);
            //IMethodInterceptor dataSourceSelectInterceptor = GetDataSourceSelectInterceptor(txAttr);

            IAspect dataSourceSelectAspect = null;
            if (dataSourceSelectInterceptor != null)
            {
                dataSourceSelectAspect = new AspectImpl(dataSourceSelectInterceptor);
            }
            return dataSourceSelectAspect;
        }

        /// <summary>
        /// データソース選択Aspectの取得
        /// </summary>
        /// <param name="txAttr">Transaction属性</param>
        /// <returns>データソース選択Aspect</returns>
        protected virtual IAspect GetDataSourceSelectAspect(TransactionAttribute txAttr)
        {
            var dataSourceSelectInterceptor = GetDataSourceSelectInterceptor(txAttr);

            IAspect dataSourceSelectAspect = null;
            if (dataSourceSelectInterceptor != null)
            {
                dataSourceSelectAspect = new AspectImpl(dataSourceSelectInterceptor);
            }
            return dataSourceSelectAspect;
        }

        /// <summary>
        /// 指定されたメソッドからAspectが有効となるAspect定義のリストを作成する
        /// </summary>
        /// <param name="methods">メソッド情報の配列</param>
        /// <returns>指定されたメソッドにAspectが有効となるAspect定義のリスト</returns>
        protected virtual IList<IAspect> CreateAspectList(MethodInfo[] methods)
        {
            // Interceptor毎にpointcutとなるメソッド名を格納する
            IDictionary<IMethodInterceptor, List<string>> methodNames =
                new Dictionary<IMethodInterceptor, List<string>>();

            // Interceptor毎にpointcutとなるメソッド名を格納する
            foreach (var method in methods)
            {
                // Aspect属性を確認してPointcutを作成する為のメソッド名を追加する
                AddMethodNamesForPointcut(methodNames, method);
            }

            // Aspectのリスト
            var aspectList = new List<IAspect>();

            // Interceptorの件数分、Aspectを作成する
            foreach (var interceptor in methodNames.Keys)
            {
                // Interceptorとメソッド名の配列からAspectを作成する
                var aspect = CreateAspect(interceptor, methodNames[interceptor].ToArray());

                // Aspectのリストに追加する
                aspectList.Add(aspect);
            }

            // Aspectのリストを返す
            return aspectList.ToArray();
        }

        /// <summary>
        /// 全てのメソッドにAspectが有効となるAspect定義を作成する
        /// </summary>
        /// <param name="aspectAttr">Aspectを設定する属性</param>
        /// <returns>全てのメソッドにAspectが有効となるAspect定義</returns>
        protected virtual IAspect CreateAspect(AspectAttribute aspectAttr)
        {
            // Interceptorを作成する
            var interceptor = GetMethodInterceptor(aspectAttr);

            // InterceptorからAspectを作成する
            // (Pointcutは指定しないので全てのメソッドが対象となる)
            IAspect aspect = new AspectImpl(interceptor);

            // Aspectを返す
            return aspect;
        }

        /// <summary>
        /// インターセプターとメソッドを指定してAspect定義を作成する
        /// </summary>
        /// <param name="interceptor">インターセプター</param>
        /// <param name="methodNames">Aspectを適用するメソッド名の配列</param>
        /// <returns>指定されたメソッドにAspectが有効となるAspect定義</returns>
        protected virtual IAspect CreateAspect(
            IMethodInterceptor interceptor, string[] methodNames)
        {
            // Pointcutを作成する
            IPointcut pointcut = new PointcutImpl(methodNames);

            // InterceptorとPointcutからAspectを作成する
            IAspect aspect = new AspectImpl(interceptor, pointcut);

            // Aspectを返す
            return aspect;
        }

        /// <summary>
        /// Aspect属性を確認してPointcutを作成する為のメソッド名を追加する
        /// </summary>
        /// <param name="methods">対象クラスのメソッド情報</param>
        /// <returns>適用するAspectのリスト</returns>
        protected virtual IAspect[] CreateTxAspectList(MethodInfo[] methods)
        {
            // データソース選択Interceptor適用メソッド名コレクション
            IDictionary<IMethodInterceptor, List<string>> dataSourceSelectMethodNames =
                new Dictionary<IMethodInterceptor, List<string>>();
            // TransacionInterceptor適用メソッド名コレクション
            IDictionary<IMethodInterceptor, List<string>> txMethodNames =
                new Dictionary<IMethodInterceptor, List<string>>();
            //  メソッド名とIntarceptorの対応付け
            foreach (var method in methods)
            {
                var txAttr = AttributeUtil.GetTransactionAttrByMethod(method);
                if (txAttr != null)
                {
                    AddMethodNamesForDataSourceSelectPointcut(dataSourceSelectMethodNames, method, txAttr);
                    AddMethodNamesForTxPointcut(txMethodNames, method.Name, txAttr);
                }
            }

            // Aspectのリスト
            var txAspectList = new List<IAspect>();

            //  データソース選択Interceptorは先に登録
            foreach (var dataSourceSelectInterceptor in dataSourceSelectMethodNames.Keys)
            {
                // Interceptorとメソッド名の配列からAspectを作成する
                var dataSourceSelectAspect = CreateAspect(dataSourceSelectInterceptor,
                    dataSourceSelectMethodNames[dataSourceSelectInterceptor].ToArray());

                // Aspectのリストに追加する
                txAspectList.Add(dataSourceSelectAspect);
            }

            // Interceptorの件数分、Aspectを作成する
            foreach (var txInterceptor in txMethodNames.Keys)
            {
                // Interceptorとメソッド名の配列からAspectを作成する
                var daoAspect = CreateAspect(
                    txInterceptor, txMethodNames[txInterceptor].ToArray());

                // Aspectのリストに追加する
                txAspectList.Add(daoAspect);
            }

            // Aspectのリストを返す
            return txAspectList.ToArray();
        }

        /// <summary>
        /// 全てのメソッドにAspectが有効となるAspect定義を作成する
        /// </summary>
        /// <param name="txAttr">Aspectを設定する属性</param>
        /// <returns>全てのメソッドにAspectが有効となるAspect定義</returns>
        protected virtual IAspect CreateTxAspect(TransactionAttribute txAttr)
        {
            // Interceptorを作成する
            var interceptor = GetMethodInterceptor(txAttr);

            // InterceptorからAspectを作成する
            // (Pointcutは指定しないので全てのメソッドが対象となる)
            IAspect aspect = new AspectImpl(interceptor);

            // Aspectを返す
            return aspect;
        }

        /// <summary>
        /// メソッド情報から追加するためのAspectのリストを作成する
        /// </summary>
        /// <param name="methods">対象クラスのメソッド情報</param>
        /// <returns>適用するAspectのリスト</returns>
        protected virtual IAspect[] CreateS2DaoAspectList(MethodInfo[] methods)
        {
            // データソース選択Interceptor適用メソッド名コレクション
            IDictionary<IMethodInterceptor, List<string>> dataSourceSelectMethodNames =
                new Dictionary<IMethodInterceptor, List<string>>();
            // S2DaoInterceptor適用メソッド名コレクション
            IDictionary<IMethodInterceptor, List<string>> daoMethodNames =
                new Dictionary<IMethodInterceptor, List<string>>();
            //  メソッド名とIntarceptorの対応付け
            foreach (var method in methods)
            {
                var daoAttr = AttributeUtil.GetS2DaoAttrByMethod(method);
                if (daoAttr != null)
                {
                    AddMethodNamesForDataSourceSelectPointcut(dataSourceSelectMethodNames, method, daoAttr);
                    AddMethodNamesForS2DaoPointcut(daoMethodNames, method.Name, daoAttr);
                }
            }

            // Aspectのリスト
            var daoAspectList = new List<IAspect>();

            //  データソース選択Interceptorは先に登録
            foreach (var dataSourceSelectInterceptor in dataSourceSelectMethodNames.Keys)
            {
                // Interceptorとメソッド名の配列からAspectを作成する
                var dataSourceSelectAspect = CreateAspect(dataSourceSelectInterceptor,
                    dataSourceSelectMethodNames[dataSourceSelectInterceptor].ToArray());

                // Aspectのリストに追加する
                daoAspectList.Add(dataSourceSelectAspect);
            }

            // Interceptorの件数分、Aspectを作成する
            foreach (var daoInterceptor in daoMethodNames.Keys)
            {
                // Interceptorとメソッド名の配列からAspectを作成する
                var daoAspect = CreateAspect(
                    daoInterceptor, daoMethodNames[daoInterceptor].ToArray());

                // Aspectのリストに追加する
                daoAspectList.Add(daoAspect);
            }

            // Aspectのリストを返す
            return daoAspectList.ToArray();
        }

        /// <summary>
        /// 全てのメソッドにAspectが有効となるAspect定義を作成する
        /// </summary>
        /// <param name="daoAttr">Aspectを設定する属性</param>
        /// <returns>全てのメソッドにAspectが有効となるAspect定義</returns>
        protected virtual IAspect CreateS2DaoAspect(S2DaoAttribute daoAttr)
        {
            // Interceptorを作成する
            var interceptor = GetMethodInterceptor(daoAttr);

            // InterceptorからAspectを作成する
            // (Pointcutは指定しないので全てのメソッドが対象となる)
            IAspect aspect = new AspectImpl(interceptor);

            // Aspectを返す
            return aspect;
        }

        /// <summary>
        /// Aspect属性を確認してPointcutを作成する為のメソッド名を追加する
        /// </summary>
        /// <param name="methodNames">
        /// Interceptor毎にpointcutとなるメソッド名を格納したコレクション
        /// </param>
        /// <param name="method">メソッド情報</param>
        protected void AddMethodNamesForPointcut(
            IDictionary<IMethodInterceptor, List<string>> methodNames, MethodInfo method)
        {
            // メソッドに指定されているAspect属性を取得する
            var aspectAttrs =
                AttributeUtil.GetAspectAttrsByMethod(method);

            // Aspect属性の件数分、Pointcutを作成する為のメソッド名を追加する
            foreach (var aspectAttr in aspectAttrs)
            {
                // Pointcutを作成する為のメソッド名を追加する
                AddMethodNamesForPointcut(methodNames, method.Name, aspectAttr);
            }
        }

        /// <summary>
        /// Aspect属性を確認してPointcutを作成する為のメソッド名を追加する
        /// </summary>
        /// <param name="methodNames">
        /// Interceptor毎にpointcutとなるメソッド名を格納したコレクション
        /// </param>
        /// <param name="methodName">メソッド名</param>
        /// <param name="aspectAttr">Aspect属性</param>
        protected void AddMethodNamesForPointcut(
            IDictionary<IMethodInterceptor, List<string>> methodNames,
             string methodName, AspectAttribute aspectAttr)
        {
            // インターセプターを取得する
            var interceptor = GetMethodInterceptor(aspectAttr);

            if (!methodNames.ContainsKey(interceptor))
            {
                // 始めてのInterceptorの場合はstringのリストを初期化する
                methodNames.Add(interceptor, new List<string>());
            }

            // メソッド名を追加する
            methodNames[interceptor].Add(methodName);
        }

        /// <summary>
        /// Aspect属性を確認してPointcutを作成する為のメソッド名を追加する
        /// </summary>
        /// <param name="methodNames">
        /// Interceptor毎にpointcutとなるメソッド名を格納したコレクション
        /// </param>
        /// <param name="methodName">メソッド名</param>
        /// <param name="txAttr">Aspect属性</param>
        protected void AddMethodNamesForTxPointcut(
            IDictionary<IMethodInterceptor, List<string>> methodNames,
             string methodName, TransactionAttribute txAttr)
        {
            // インターセプターを取得する
            var interceptor = GetMethodInterceptor(txAttr);
            if ( !methodNames.ContainsKey(interceptor) )
            {
                // 始めてのInterceptorの場合はstringのリストを初期化する
                methodNames.Add(interceptor, new List<string>());
            }

            // メソッド名を追加する
            methodNames[interceptor].Add(methodName);
        }

        /// <summary>
        /// S2Dao属性のデータソース名定義を確認してPointcutを作成する為のメソッド名を追加する
        /// </summary>
        /// <param name="methodNames">
        /// Interceptor毎にpointcutとなるメソッド名を格納したコレクション
        /// </param>
        /// <param name="method">Aspectを適用するメソッド情報</param>
        /// <param name="daoAttr">S2Dao属性</param>
        protected void AddMethodNamesForDataSourceSelectPointcut(
            IDictionary<IMethodInterceptor, List<string>> methodNames, 
            MethodInfo method, S2DaoAttribute daoAttr)
        {
            //  データソース選択Interceptorの取得
            var dataSourceSelectInterceptor =
                GetDataSourceSelectInterceptor(daoAttr, method);

            if (dataSourceSelectInterceptor != null)
            {
                if (!methodNames.ContainsKey(dataSourceSelectInterceptor))
                {
                    // 始めてのInterceptorの場合はstringのリストを初期化する
                    methodNames.Add(dataSourceSelectInterceptor, new List<string>());
                }
                methodNames[dataSourceSelectInterceptor].Add(method.Name);
            }
        }

        /// <summary>
        /// S2Dao属性のデータソース名定義を確認してPointcutを作成する為のメソッド名を追加する
        /// </summary>
        /// <param name="methodNames">
        /// Interceptor毎にpointcutとなるメソッド名を格納したコレクション
        /// </param>
        /// <param name="method">Aspectを適用するメソッド情報</param>
        /// <param name="txAttr">Transaction属性</param>
        protected void AddMethodNamesForDataSourceSelectPointcut(
            IDictionary<IMethodInterceptor, List<string>> methodNames,
            MethodInfo method, TransactionAttribute txAttr)
        {
            //  データソース選択Interceptorの取得
            var dataSourceSelectInterceptor = GetDataSourceSelectInterceptor(txAttr, method);

            if (dataSourceSelectInterceptor != null)
            {
                if (!methodNames.ContainsKey(dataSourceSelectInterceptor))
                {
                    // 始めてのInterceptorの場合はstringのリストを初期化する
                    methodNames.Add(dataSourceSelectInterceptor, new List<string>());
                }
                methodNames[dataSourceSelectInterceptor].Add(method.Name);
            }
        }

        /// <summary>
        /// S2Dao属性のデータソース名定義を確認してPointcutを作成する為のメソッド名を追加する
        /// </summary>
        /// <param name="methodNames">
        /// Interceptor毎にpointcutとなるメソッド名を格納したコレクション
        /// </param>
        /// <param name="methodName">Aspectを適用するメソッド名</param>
        /// <param name="txAttr">S2Dao属性</param>
        protected void AddMethodNamesForDataSourceSelectPointcut(
            IDictionary<IMethodInterceptor, List<string>> methodNames,
            string methodName, TransactionAttribute txAttr)
        {
            //  データソース選択Interceptorの取得
            var dataSourceSelectInterceptor = GetDataSourceSelectInterceptor(txAttr);

            if (dataSourceSelectInterceptor != null)
            {
                if (!methodNames.ContainsKey(dataSourceSelectInterceptor))
                {
                    // 始めてのInterceptorの場合はstringのリストを初期化する
                    methodNames.Add(dataSourceSelectInterceptor, new List<string>());
                }
                methodNames[dataSourceSelectInterceptor].Add(methodName);
            }
        }

        /// <summary>
        /// S2Dao属性を確認してPointcutを作成する為のメソッド名を追加する
        /// </summary>
        /// <param name="methodNames">
        /// Interceptor毎にpointcutとなるメソッド名を格納したコレクション
        /// </param>
        /// <param name="methodName">メソッド名</param>
        /// <param name="daoAttr">S2Dao属性</param>
        protected void AddMethodNamesForS2DaoPointcut(
            IDictionary<IMethodInterceptor, List<string>> methodNames,
             string methodName, S2DaoAttribute daoAttr)
        {
            // インターセプターを取得する
            var interceptor = GetMethodInterceptor(daoAttr);

            if (!methodNames.ContainsKey(interceptor))
            {
                // 始めてのInterceptorの場合はstringのリストを初期化する
                methodNames.Add(interceptor, new List<string>());
            }

            // メソッド名を追加する
            methodNames[interceptor].Add(methodName);
        }

        /// <summary>
        /// Aspect属性からインターセプターを取得する
        /// </summary>
        /// <param name="aspectAttr">Aspect属性</param>
        /// <returns>インターセプター</returns>
        protected virtual IMethodInterceptor GetMethodInterceptor(AspectAttribute aspectAttr)
        {
            if (aspectAttr.InterceptorType != null)
            {
                // interceptorTypeが指定されている場合は
                // QuillからTypeを指定してインターセプターを取得する
                return GetMethodInterceptor(aspectAttr.InterceptorType);
            }
            //else if (aspectAttr.ComponentName != null)
            //{
            //    // コンポーネント名が指定されている場合は
            //    // S2Containerからコンポーネント名を指定してインターセプターを取得する
            //    return GetMethodInterceptor(aspectAttr.ComponentName);
            //}
            else
            {
                // Aspect属性にinterceptorTypeとcomponentNameのどちらの指定も
                // されていない場合は例外をスローする
                throw new QuillApplicationException("EQLL0013");
            }
        }

        /// <summary>
        /// QuillからTypeを指定してインターセプターを取得する
        /// </summary>
        /// <param name="interceptorType">インターセプターのType</param>
        /// <returns>インターセプター</returns>
        protected virtual IMethodInterceptor GetMethodInterceptor(Type interceptorType)
        {
            // Interceptorのコンポーネントを取得する
            var component = container.GetComponent(interceptorType);

            if (typeof(IMethodInterceptor).IsAssignableFrom(component.ComponentType))
            {
                // IMethodInterceptorに代入ができる場合はInterceptorを返す
                return (IMethodInterceptor)component.GetComponentObject(interceptorType);
            }
            else
            {
                // IMethodInterceptorに代入できない場合は例外をスローする
                throw new QuillApplicationException("EQLL0012", new [] { component.ComponentType.FullName });
            }
        }

        ///// <summary>
        ///// S2Containerからコンポーネント名を指定してインターセプターを取得する
        ///// </summary>
        ///// <param name="componentName">コンポーネント名</param>
        ///// <returns>インターセプター</returns>
        //protected virtual IMethodInterceptor GetMethodInterceptor(
        //    string componentName)
        //{
        //    // S2Containerからコンポーネントのオブジェクトを取得する
        //    object interceptor =
        //        SingletonS2ContainerConnector.GetComponent(componentName);

        //    // インターセプターのTypeを取得する
        //    Type type = TypeUtil.GetType(interceptor);

        //    if (typeof(IMethodInterceptor).IsAssignableFrom(type))
        //    {
        //        // IMethodInterceptorに代入ができる場合はInterceptorを返す
        //        return (IMethodInterceptor)interceptor;
        //    }
        //    else
        //    {
        //        // IMethodInterceptorに代入できない場合は例外をスローする
        //        throw new QuillApplicationException("EQLL0012",
        //            new object[] { type.FullName });
        //    }
        //}

        /// <summary>
        /// Transaction属性からインターセプターを取得する
        /// </summary>
        /// <param name="txAttr">Transaction属性</param>
        /// <returns>インターセプター</returns>
        protected virtual IMethodInterceptor GetMethodInterceptor(TransactionAttribute txAttr)
        {
            var settingType = txAttr.TransactionSettingType;
            if (settingType == null)
            {
                // トランザクション設定が指定されていない場合は
                // 例外をスローする
                throw new QuillApplicationException("EQLL0013");
            }

            var txSetting =
                (ITransactionSetting)ComponentUtil.GetComponent(container, settingType);
            if (txSetting.IsNeedSetup())
            {
                //  DataSourceの取得
                var dataSource = (IDataSource)ComponentUtil.GetComponent(
                    container, typeof(SelectableDataSourceProxyWithDictionary));
                //  トランザクション関係の設定
                txSetting.Setup(dataSource);
            }

            if (txSetting.TransactionInterceptor == null)
            {
                //  Interceptorが作られていない場合は例外とする
                throw new QuillApplicationException("EQLL0024");
            }
            return txSetting.TransactionInterceptor;
        }

        /// <summary>
        /// S2Dao属性からインターセプターを取得する
        /// </summary>
        /// <param name="daoAttr">S2Dao属性</param>
        /// <returns>インターセプター</returns>
        protected virtual IMethodInterceptor GetMethodInterceptor(S2DaoAttribute daoAttr)
        {
            var settingType = daoAttr.DaoSettingType;
            if (settingType == null)
            {
                // 使用するHandlerが指定されていない場合は
                // 例外をスローする
                throw new QuillApplicationException("EQLL0013");
            }

            var daoSetting = (IDaoSetting)ComponentUtil.GetComponent(
                container, settingType);

            if (daoSetting.IsNeedSetup())
            {
                //  DataSourceの取得
                var dataSource = (IDataSource)ComponentUtil.GetComponent(
                    container, typeof(SelectableDataSourceProxyWithDictionary));
                //  S2DaoInterceptor等の設定
                daoSetting.Setup(dataSource);
            }

            if (daoSetting.DaoInterceptor == null)
            {
                //  Interceptorが作られていない場合は例外とする
                throw new QuillApplicationException("EQLL0023");
            }
            return daoSetting.DaoInterceptor;
        }

        /// <summary>
        /// DataSourceSelectInterceptorを作って返す
        /// </summary>
        /// <param name="dataSourceName"></param>
        /// <returns></returns>
        protected virtual DataSourceSelectInterceptor CreateDataSourceSelectInterceptor(string dataSourceName)
        {
            if(string.IsNullOrEmpty(dataSourceName))
            {
                return null;
            }

            var dsInterceptor =
                DataSourceSelectInterceptor.GetInstance(dataSourceName);

            //  データソースが未設定の場合はセットする
            if (dsInterceptor.DataSourceProxy == null)
            {
                var ds =
                    (SelectableDataSourceProxyWithDictionary)ComponentUtil.GetComponent(
                    container, typeof(SelectableDataSourceProxyWithDictionary));
                dsInterceptor.DataSourceProxy = ds;
            }
            return dsInterceptor;
        }
    }
}
