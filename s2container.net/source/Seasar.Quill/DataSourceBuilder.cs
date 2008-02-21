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

using System.Collections.Generic;
using System.Configuration;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

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

            //  App.configにDataSourceの記述があればそこから取得
            if (ConfigurationManager.ConnectionStrings.Count > 0)
            {
                foreach (ConnectionStringSettings setting in ConfigurationManager.ConnectionStrings)
                {
                    IDataSource ds = new DataSourceImpl(setting.Name);
                    dataSources[setting.Name] = ds;
                }
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
    }
}
