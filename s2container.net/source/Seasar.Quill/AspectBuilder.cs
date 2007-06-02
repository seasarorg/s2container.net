#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using Seasar.Framework.Aop;
using System.Reflection;
using Seasar.Quill.Attrs;
using Seasar.Quill.Util;
using Seasar.Framework.Aop.Impl;

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
            List<IAspect> aspectList = new List<IAspect>();

            // Typeに指定されたAspectを指定する属性を取得する
            AspectAttribute[] attrsByType = AttributeUtil.GetAspectAttrs(type);

            // Typeに指定されているAspectの件数分、Aspectをリストに追加する
            foreach (AspectAttribute attrByType in attrsByType)
            {
                // Pointcutに全てのメソッドが追加されているAspectを作成する
                IAspect aspect = CreateAspect(attrByType.InterceptorType);

                // 作成したAspectをリストに追加する
                aspectList.Add(aspect);
            }

            // typeで宣言されているメソッドを取得する
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | 
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            // Methodに指定されているAspectをリストに追加する
            aspectList.AddRange(CreateAspectList(methods));

            // Aspectの配列を返す
            return aspectList.ToArray();
        }

        /// <summary>
        /// 全てのメソッドにAspectが有効となるAspect定義を作成する
        /// </summary>
        /// <param name="interceptorType">InterceptorのType</param>
        /// <returns>全てのメソッドにAspectが有効となるAspect定義</returns>
        protected virtual IAspect CreateAspect(Type interceptorType)
        {
            // Interceptorのコンポーネントを取得する
            QuillComponent component =
                container.GetComponent(interceptorType, interceptorType);

            // Interceptorを作成する
            IMethodInterceptor interceptor = (IMethodInterceptor)
                component.GetComponentObject(interceptorType);

            // InterceptorからAspectを作成する
            // (Pointcutは指定しないので全てのメソッドが対象となる)
            IAspect aspect = new AspectImpl(interceptor);

            // Aspectを返す
            return aspect;
        }

        /// <summary>
        /// 指定されたメソッドからAspectが有効となるAspect定義のリストを作成する
        /// </summary>
        /// <param name="methods">メソッド情報の配列</param>
        /// <returns>指定されたメソッドにAspectが有効となるAspect定義のリスト</returns>
        protected virtual IList<IAspect> CreateAspectList(MethodInfo[] methods)
        {
            // Interceptor毎にpointcutとなるメソッド名を格納する
            IDictionary<Type, List<string>> methodNames =
                new Dictionary<Type, List<string>>();

            // メソッドの件数分、Aspect属性を確認してPointcutを作成する為のメソッド名を追加する
            foreach (MethodInfo method in methods)
            {
                // Aspect属性を確認してPointcutを作成する為のメソッド名を追加する
                AddMethodNamesForPointcut(methodNames, method);
            }

            // Aspectのリスト
            List<IAspect> aspectList = new List<IAspect>();

            // Interceptorの件数分、Aspectを作成する
            foreach (Type interceptorType in methodNames.Keys)
            {
                // InterceptorのTypeとメソッド名の配列からAspectを作成する
                IAspect aspect = CreateAspect(
                    interceptorType, methodNames[interceptorType].ToArray());

                // Aspectのリストに追加する
                aspectList.Add(aspect);
            }

            // Aspectのリストを返す
            return aspectList.ToArray();
        }


        /// <summary>
        /// 指定されたメソッドからAspectが有効となるAspect定義を作成する
        /// </summary>
        /// <param name="interceptorType">InterceptorのType</param>
        /// <param name="methodNames">Aspectを適用するメソッド名の配列</param>
        /// <returns>指定されたメソッドにAspectが有効となるAspect定義</returns>
        protected virtual IAspect CreateAspect(
            Type interceptorType, string[] methodNames)
        {
            // Interceptorのコンポーネントを取得する
            QuillComponent component = 
                container.GetComponent(interceptorType, interceptorType);

            // Interceptorを作成する
            IMethodInterceptor interceptor = (IMethodInterceptor)
                component.GetComponentObject(interceptorType);

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
        /// <param name="methodNames">
        /// Interceptor毎にpointcutとなるメソッド名を格納したコレクション
        /// </param>
        /// <param name="method">メソッド情報</param>
        protected void AddMethodNamesForPointcut(
            IDictionary<Type, List<string>> methodNames, MethodInfo method)
        {
            // メソッドに指定されているAspect属性を取得する
            AspectAttribute[] aspectAttrs =
                AttributeUtil.GetAspectAttrsByMethod(method);

            // Aspect属性の件数分、Pointcutを作成する為のメソッド名を追加する
            foreach (AspectAttribute aspectAttr in aspectAttrs)
            {
                // Pointcutを作成する為のメソッド名を追加する
                AddMethodNamesForPointcut(
                    methodNames, method.Name, aspectAttr.InterceptorType);
            }
        }

        /// <summary>
        /// Aspect属性を確認してPointcutを作成する為のメソッド名を追加する
        /// </summary>
        /// <param name="methodNames">
        /// Interceptor毎にpointcutとなるメソッド名を格納したコレクション
        /// </param>
        /// <param name="methodName">メソッド名</param>
        /// <param name="interceptorType">InterceptorのType</param>
        protected void AddMethodNamesForPointcut(
            IDictionary<Type, List<string>> methodNames, string methodName,
            Type interceptorType)
        {
            if (!methodNames.ContainsKey(interceptorType))
            {
                // Typeの中で始めてのInterceptorの場合はstringのリストを初期化する
                methodNames.Add(interceptorType, new List<string>());
            }

            // メソッド名を追加する
            methodNames[interceptorType].Add(methodName);
        }
    }
}
