#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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

using Seasar.Framework.Container;

namespace Seasar.Extension.ADO.Impl
{
    /// <summary>
    /// S2Containerを使用してデータソースを返す
    /// </summary>
    public class SelectableDataSourceProxyWithContainer : AbstractSelectableDataSourceProxy
    {
        /// <summary>
        /// データソース名
        /// </summary>
        /// <remarks>
        /// static,スレッド毎に一意のデータソース名を保持する場合は
        /// IDataSourceSelector実装クラスを作成して実装して下さい
        /// </remarks>
        private string _dataSourceName;

        private IS2Container _container = null;

        public IS2Container Container
        {
            set
            {
                _container = value;
            }
            get
            {
                return _container;
            }
        }

        public override IDataSource GetDataSource(string dataSourceName)
        {
            if ( Container.HasComponentDef(dataSourceName) )
            {
                return (IDataSource)Container.GetComponent(dataSourceName);
            }
            else
            {
                throw new ComponentNotFoundRuntimeException(dataSourceName);
            }
        }

        public override string GetDataSourceName()
        {
            return _dataSourceName;
        }

        public override void SetDataSourceName(string dataSourceName)
        {
            _dataSourceName = dataSourceName;
        }
    }
}
