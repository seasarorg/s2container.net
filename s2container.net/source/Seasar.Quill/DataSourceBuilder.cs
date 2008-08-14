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
using System.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;
using Seasar.Extension.ADO;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;
using Seasar.Quill.Database.DataSource.Connection;
using Seasar.Quill.Database.Tx;
using Seasar.Quill.Database.Tx.Impl;
using Seasar.Quill.Exception;
using Seasar.Quill.Util;
using Seasar.Quill.Xml;

namespace Seasar.Quill
{
    /// <summary>
    /// DataSourceを構築するクラス
    /// </summary>
    public class DataSourceBuilder
    {
        private const string DEFALT_DATASOURCE_NAME = "DataSource";

        /// <summary>
        /// 文字列("で囲まれているか)判定するための正規表現
        /// </summary>
        private readonly Regex _regexIsString = new Regex("^\".*\"$");

        private readonly QuillContainer _container;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">Quillコンテナ</param>
        public DataSourceBuilder(QuillContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// app.config,diconの定義からIDataSourceのCollectionを生成
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, IDataSource> CreateDataSources()
        {
            IDictionary<string, IDataSource> dataSources = new Dictionary<string, IDataSource>();

            //  App.configにquillセクションの記述があればそこから取得
            SetupByQuillSection(dataSources);
            if (dataSources.Count > 0)
            {
                return dataSources;
            }

            //  App.configにConnectionStringの記述があればそこから取得
            SetupByConnectionStringSection(dataSources);
            if (dataSources.Count > 0)
            {
                return dataSources;
            }

            //  App.configにデータがなければdiconから取得
            if ( SingletonS2ContainerConnector.HasComponentDef(DEFALT_DATASOURCE_NAME) )
            {
                dataSources[DEFALT_DATASOURCE_NAME] = 
                    (IDataSource)SingletonS2ContainerConnector.GetComponent(
                    DEFALT_DATASOURCE_NAME, typeof(IDataSource));
            }
            else if ( SingletonS2ContainerConnector.HasComponentDef(typeof(IDataSource)) )
            {
                dataSources[typeof(IDataSource).Name] =
                    (IDataSource)SingletonS2ContainerConnector.GetComponent(typeof(IDataSource));
            }

            return dataSources;
        }

        /// <summary>
        /// app.configのconnectionStringセクション情報からデータソースを生成する
        /// </summary>
        /// <param name="dataSources"></param>
        protected virtual void SetupByConnectionStringSection(
            IDictionary<string, IDataSource> dataSources)
        {
            if (ConfigurationManager.ConnectionStrings.Count > 0)
            {
                foreach (ConnectionStringSettings setting in ConfigurationManager.ConnectionStrings)
                {
                    //IDataSource ds = new DataSourceImpl(setting.Name);
                    IDataSource ds = new TxDataSource(setting.Name);
                    dataSources[setting.Name] = ds;
                }
            }
        }

        /// <summary>
        /// app.configのQuillセクション情報からデータソースを生成する
        /// </summary>
        /// <param name="dataSources"></param>
        protected virtual void SetupByQuillSection(IDictionary<string, IDataSource> dataSources)
        {
            QuillSection section = QuillSectionHandler.GetQuillSection();
            if (section != null && section.DataSources.Count > 0)
            {
                //  既定のトランザクション設定
                ITransactionSetting defaultTxSetting = null;
                if (_container == null)
                {
                    defaultTxSetting = new TypicalTransactionSetting();
                }
                else
                {
                    defaultTxSetting =
                        (ITransactionSetting) ComponentUtil.GetComponent(
                        _container, typeof (TypicalTransactionSetting));
                }

                foreach (object item in section.DataSources)
                {
                    if(item is DataSourceSection)
                    {
                        DataSourceSection dsSection = (DataSourceSection)item;

                        //  データソース
                        string dataSourceClassName = GetDataSourceClassName(dsSection);

                        //  プロバイダ
                        string providerName = GetProviderName(dsSection);

                        //  接続文字列
                        string connectionString = GetConnectionString(dsSection);

                        //  クラス組み立て
                        DataProvider provider = (DataProvider)ClassUtil.NewInstance(
                            ClassUtil.ForName(providerName));
                        ConstructorInfo constructorInfo = ClassUtil.GetConstructorInfo(ClassUtil.ForName(
                            dataSourceClassName), new Type[] { typeof(DataProvider), typeof(string) });
                        IDataSource dataSource = (IDataSource)constructorInfo.Invoke(
                             new object[] { provider, connectionString });
                        SetupDataSourceDefault(defaultTxSetting, dataSource);
                        dataSources[dsSection.DataSourceName] = dataSource;
                    }     
                }
            }
        }

        /// <summary>
        /// プロバイダ名の取得
        /// </summary>
        /// <param name="dataSourceSection"></param>
        /// <returns></returns>
        protected virtual string GetProviderName(DataSourceSection dataSourceSection)
        {
            string providerName = dataSourceSection.ProviderName;
            if (string.IsNullOrEmpty(providerName))
            {
                throw new ClassNotFoundRuntimeException("(ProviderName=Empty)");
            }

            if (TypeUtil.HasNamespace(providerName) == false)
            {
                //  名前空間が指定されていない場合は既定の
                //  名前空間を使用する
                providerName = string.Format("{0}.{1}",
                    QuillConstants.NAMESPACE_PROVIDER, providerName);
            }
            return providerName;
        }

        /// <summary>
        /// データソースクラス名の取得
        /// </summary>
        /// <param name="dataSourceSection"></param>
        /// <returns></returns>
        protected virtual string GetDataSourceClassName(DataSourceSection dataSourceSection)
        {
            string dataSourceClassName = dataSourceSection.DataSourceClassName;
            if (string.IsNullOrEmpty(dataSourceClassName))
            {
                throw new ClassNotFoundRuntimeException("(DataSourceClass=Empty)");
            }

            if (TypeUtil.HasNamespace(dataSourceClassName) == false)
            {
                //  名前空間が指定されていない場合は既定の
                //  名前空間を使用する
                dataSourceClassName = string.Format("{0}.{1}",
                    QuillConstants.NAMESPACE_DATASOURCE, dataSourceClassName);
            }
            return dataSourceClassName;
        }

        /// <summary>
        /// 接続文字列の取得
        /// </summary>
        /// <param name="dataSourceSection"></param>
        /// <returns></returns>
        protected virtual string GetConnectionString(DataSourceSection dataSourceSection)
        {
            //  接続文字列
            string configString = dataSourceSection.ConnectionString;
            string connectionString = null;
            if (_regexIsString.IsMatch(configString))
            {
                //  最初と最後の「"」を取り除く
                connectionString = configString.Substring(1, configString.Length - 2);
            }
            else
            {
                //  「"」で囲まれていない場合はクラスが指定されていると見なす
                Type connectionStringType = ClassUtil.ForName(configString);
                if (typeof(IConnectionString).IsAssignableFrom(connectionStringType))
                {
                    IConnectionString cs = (IConnectionString)ComponentUtil.GetComponent(
                        _container, connectionStringType);
                    connectionString = cs.GetConnectionString();
                }
                else
                {
                    throw new QuillInvalidClassException(
                        connectionStringType, typeof(IConnectionString));
                }
            }
            //  接続文字列が設定されていない
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("(ConnectionString=Empty)");
            }

            return connectionString;
        }

        /// <summary>
        /// データソースの既定設定を行う
        /// </summary>
        /// <param name="txSetting">デフォルトのトランザクション設定</param>
        /// <param name="dataSource">設定するデータソース</param>
        protected virtual void SetupDataSourceDefault(ITransactionSetting txSetting,
            IDataSource dataSource)
        {
            //  TxDataSourceかそれを継承するクラスの場合は予め既定設定を
            //  行っておく
            //  これ以外の設定が使われる場合はコンポーネント生成時に上書きされる
            if(typeof(TxDataSource).IsAssignableFrom(dataSource.GetType()))
            {
                if (txSetting.IsNeedSetup())
                {
                    txSetting.Setup(dataSource);
                }
            }
        }
    }
}
