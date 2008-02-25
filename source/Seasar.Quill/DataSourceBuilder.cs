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
using Seasar.Extension.ADO;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;
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
        /// DataProviderのキャッシュ
        /// </summary>
        protected readonly IDictionary<string, DataProvider> _providerCash
            = new Dictionary<string, DataProvider>();

        /// <summary>
        /// app.config,diconの定義からIDataSourceのCollectionを生成
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, IDataSource> CreateDataSources()
        {
            IDictionary<string, IDataSource> dataSources = new Dictionary<string, IDataSource>();

            //  App.configにConnectionStringの記述があればそこから取得
            SetupByConnectionStringSection(dataSources);
            SetupByQuillSection(dataSources);

            if (dataSources.Count > 0)
            {
                //  app.config分のデータソース定義があれば
                //  以降のdiconは使わない
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
                foreach (object item in section.DataSources)
                {
                    if(item is DataSourceSection)
                    {
                        DataSourceSection dsSection = (DataSourceSection)item;

                        //  データソース
                        string dataSourceClassName = dsSection.DataSourceClassName;
                        if(string.IsNullOrEmpty(dataSourceClassName))
                        {
                            throw new ClassNotFoundRuntimeException("(DataSourceClass=Empty)");
                        }

                        if(TypeUtil.HasNamespace(dataSourceClassName) == false)
                        {
                            //  名前空間が指定されていない場合は既定の
                            //  名前空間を使用する
                            dataSourceClassName = string.Format("{0}.{1}",
                                QuillConstants.NAMESPACE_DATASOURCE, dataSourceClassName);
                        }

                        //  プロバイダ
                        string providerName = dsSection.ProviderName;
                        if(string.IsNullOrEmpty(providerName))
                        {
                            throw new ClassNotFoundRuntimeException("(ProviderName=Empty)");
                        }

                        if(TypeUtil.HasNamespace(providerName) == false)
                        {
                            //  名前空間が指定されていない場合は既定の
                            //  名前空間を使用する
                            providerName = string.Format("{0}.{1}",
                                QuillConstants.NAMESPACE_PROVIDER, providerName);
                        }

                        //  接続文字列
                        string connectionString = dsSection.ConnectionString;
                        if(string.IsNullOrEmpty(connectionString))
                        {
                            throw new ArgumentException("(ConnectionString=Empty)");
                        }

                        //  クラス組み立て
                        DataProvider provider = (DataProvider)ClassUtil.NewInstance(
                            ClassUtil.ForName(providerName));
                        ConstructorInfo constructorInfo = ClassUtil.GetConstructorInfo(ClassUtil.ForName(
                            dataSourceClassName), new Type[] { typeof(DataProvider), typeof(string) });
                        IDataSource dataSource = (IDataSource)constructorInfo.Invoke(
                             new object[] { provider, connectionString });
                        dataSources[dsSection.DataSourceName] = dataSource;
                    }     
                }
            }
        }
    }
}
