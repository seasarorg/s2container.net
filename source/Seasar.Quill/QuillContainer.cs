#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using Seasar.Extension.ADO;
using Seasar.Framework.Aop;
using Seasar.Framework.Log;
using Seasar.Quill.Database.DataSource.Impl;
using Seasar.Quill.Database.Tx;
using Seasar.Quill.Exception;
using Seasar.Quill.Util;

namespace Seasar.Quill
{
    /// <summary>
    /// コンポーネントを格納するコンテナクラス
    /// </summary>
    /// <remarks>
    /// <para>
    /// 格納するコンポーネントのインスタンスは1度生成されると
    /// 同じものが使用される(singleton)</para>
    /// </remarks>
    public class QuillContainer : IDisposable
    {
        /// <summary>
        /// ログ
        /// </summary>
        private readonly Logger _log = Logger.GetLogger(typeof(QuillContainer));

        // 作成済みにコンポーネントを格納する
        protected IDictionary<Type, QuillComponent> components =
            new Dictionary<Type, QuillComponent>();

        // Aspectを構築するBuilder
        protected AspectBuilder aspectBuilder;

        /// <summary>
        /// QuillContainerの初期化を行うコンストラクタ
        /// </summary>
        public QuillContainer()
        {
            ////  アセンブリをロードする
            //RegistAssembly();

            // QuillContainer内で使用するAspectBuilderを作成する
            aspectBuilder = new AspectBuilder(this);

            //  Quill設定情報の初期化
            QuillConfig.InitializeQuillConfig(this);
            QuillConfig config = QuillConfig.GetInstance();
            _log.Info(MessageUtil.GetMessage("IQLL0003", new object[] { config.HasQuillConfig() }));
            if (config.HasQuillConfig())
            {
                //  設定情報がある場合はアセンブリ、データソースを登録
                config.RegisterAssembly();
                RegistDataSource(
                    config.CreateDataSources(),
                    config.GetTransationSettingType());
            }

            ////  DataSourceが定義されていればQuillContainerに登録する
            //RegistDataSource();
        }

        /// <summary>
        /// Quillコンポーネントを取得する
        /// </summary>
        /// <remarks>
        /// <para>インスタンスの受け側のTypeとインスタンスのTypeが同じ場合の
        /// QuillComponentを取得する。</para>
        /// <para>Quillコンポーネントが生成済みの場合は生成済みの
        /// Quillコンポーネントを返す。</para>        
        /// </remarks>
        /// <param name="type">インスタンスの受け側のType</param>
        /// <returns>Quillコンポーネント</returns>
        public virtual QuillComponent GetComponent(Type type)
        {
            // Quillコンポーネントを取得して返す
            return GetComponent(type, type);
        }

        /// <summary>
        /// Quillコンポーネントを取得する
        /// </summary>
        /// <remarks>
        /// Quillコンポーネントが生成済みの場合は生成済みの
        /// Quillコンポーネントを返す。
        /// </remarks>
        /// <param name="type">インスタンスの受け側のType</param>
        /// <param name="implType">インスタンスのType</param>
        /// <returns>Quillコンポーネント</returns>
        public virtual QuillComponent GetComponent(Type type, Type implType)
        {
            if (components == null)
            {
                // Destroyされている場合は例外を発生する
                throw new QuillApplicationException("EQLL0018");
            }

            lock (components)
            {
                // 既に作成済みのインスタンスであるか確認する
                if (components.ContainsKey(type))
                {
                    // 既に作成済みであれば作成済みのインスタンスを返す
                    return components[type];
                }

                // Aspectを作成する（Aspect属性が指定されていなければサイズ0となる)
                IAspect[] aspects = aspectBuilder.CreateAspects(implType);

                if (implType.IsInterface && aspects.Length == 0)
                {
                    // InterfaceでAspectが定義されていない場合は例外をスローする
                    throw new QuillApplicationException("EQLL0008",
                        new string[] { implType.FullName });
                }

                // Quillコンポーネントを作成する
                QuillComponent component = new QuillComponent(implType, type, aspects);

                // 作成済みのQuillコンポーネントを保存する
                components[type] = component;

                // Quillコンポーネントを返す
                return component;
            }
        }

        /// <summary>
        /// QuillContainerが持つ参照を破棄する
        /// </summary>
        public virtual void Destroy()
        {
            if (components == null)
            {
                return;
            }

            // 保持しているQuillComponentを反復処理する為の列挙子を取得する
            IEnumerator<QuillComponent> componentValues =
                components.Values.GetEnumerator();

            while (componentValues.MoveNext())
            {
                // QuillComponentのDestroyを呼び出す
                componentValues.Current.Destroy();
            }

            components = null;
            aspectBuilder = null;
        }

        /// <summary>
        /// データソースを登録
        /// </summary>
        public virtual void RegistDataSource(IDictionary<string, IDataSource> dataSources,
            Type defaultTxSettingType)
        {
            // データソースの定義がなければ以後の処理は行わない
            if (dataSources.Count == 0)
            {
                return;
            }
            //  Quill用データソースの生成
            SelectableDataSourceProxyWithDictionary dataSourceProxy =
                (SelectableDataSourceProxyWithDictionary)ComponentUtil.GetComponent(
                this, typeof(SelectableDataSourceProxyWithDictionary));
            //  データソースの定義があれば登録
            foreach (KeyValuePair<string, IDataSource> dataSourcePair in dataSources)
            {
                dataSourceProxy.RegistDataSource(dataSourcePair.Key, dataSourcePair.Value);
            }

            ITransactionSetting defaultTxSetting = (ITransactionSetting)ComponentUtil.GetComponent(
                this, defaultTxSettingType);
            //  トランザクションのデフォルト設定を行う
            if (defaultTxSetting != null && defaultTxSetting.IsNeedSetup())
            {
                defaultTxSetting.Setup(dataSourceProxy);
            }
        }

        ///// <summary>
        ///// データソースを登録
        ///// </summary>
        //public virtual void RegistDataSource()
        //{
        //    DataSourceBuilder builder = new DataSourceBuilder(this);
        //    IDictionary<string, IDataSource> dataSources = builder.CreateDataSources();
        //    // データソースの定義がなければ以後の処理は行わない
        //    if ( dataSources.Count == 0 )
        //    {
        //        return;
        //    }

        //    SelectableDataSourceProxyWithDictionary dataSourceProxy =
        //        (SelectableDataSourceProxyWithDictionary)ComponentUtil.GetComponent(
        //        this, typeof(SelectableDataSourceProxyWithDictionary));
        //    //  データソースの定義があれば登録
        //    foreach ( KeyValuePair<string, IDataSource> dataSourcePair in dataSources )
        //    {
        //        dataSourceProxy.RegistDataSource(dataSourcePair.Key, dataSourcePair.Value);
        //    }

        //    //  トランザクションのデフォルト設定を行う
        //    ITransactionSetting defaultTxSetting = (ITransactionSetting)ComponentUtil.GetComponent(
        //                this, typeof(TypicalTransactionSetting));
        //    if (defaultTxSetting.IsNeedSetup())
        //    {
        //        defaultTxSetting.Setup(dataSourceProxy);
        //    }
        //}

        ///// <summary>
        ///// アセンブリをロードする
        ///// </summary>
        //protected virtual void RegistAssembly()
        //{
        //    QuillSection section = QuillSectionHandler.GetQuillSection();
        //    if (section != null && section.Assemblys != null && section.Assemblys.Count > 0)
        //    {
        //        //  設定ファイルに書かれたアセンブリ名を取得する
        //        foreach (object item in section.Assemblys)
        //        {
        //            string assemblyName = item as string;
        //            if (!string.IsNullOrEmpty(assemblyName))
        //            {
        //                //  指定されたアセンブリをロードする
        //                AppDomain.CurrentDomain.Load(assemblyName);
        //            }
        //        }
        //    }
        //}

        #region IDisposable メンバ

        /// <summary>
        /// 保持しているQuillComponentのDisposeを呼び出す
        /// </summary>
        public virtual void Dispose()
        {
            if (components == null)
            {
                // Destroyされている場合は例外を発生する
                throw new QuillApplicationException("EQLL0018");
            }

            // 保持しているQuillComponentを反復処理する為の列挙子を取得する
            IEnumerator<QuillComponent> componentValues = 
                components.Values.GetEnumerator();

            while (componentValues.MoveNext())
            {
                // QuillComponentのDisposeを呼び出す
                componentValues.Current.Dispose();
            }
        }

        #endregion
    }
}
