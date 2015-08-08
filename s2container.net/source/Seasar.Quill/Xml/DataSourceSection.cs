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
using System.Xml.Serialization;

namespace Seasar.Quill.Xml
{
    [Serializable]
    public class DataSourceSection
    {
        private string _dataSourceName;
        private string _connectionString;
        private string _providerName;
        private string _dataSourceClassName;

        /// <summary>
        /// データソース名
        /// </summary>
        [XmlAttribute(QuillConstants.CONFIG_DATASOURCE_NAME_ATTR)]
        public string DataSourceName
        {
            set { _dataSourceName = value; }
            get { return _dataSourceName; }
        }

        /// <summary>
        /// 接続文字列
        /// </summary>
        [XmlElement(QuillConstants.CONFIG_CONNECTION_STRING_KEY)]
        public string ConnectionString
        {
            set { _connectionString = value; }
            get { return _connectionString; }
        }

        /// <summary>
        /// DataProviderクラス名
        /// </summary>
        [XmlElement(QuillConstants.CONFIG_PROVIDER)]
        public string ProviderName
        {
            set { _providerName = value; }
            get { return _providerName; }
        }

        /// <summary>
        /// DataSourceクラス名
        /// </summary>
        [XmlElement(QuillConstants.CONFIG_DATASOURCE_CLASS_KEY)]
        public string DataSourceClassName
        {
            set { _dataSourceClassName = value; }
            get { return _dataSourceClassName; }
        }

        /// <summary>
        /// インスタンスがもつ情報を文字列で返す
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"DataSourceName={DataSourceName},ConnectionString={ConnectionString},Provider={ProviderName},DataSoruceClassName={DataSourceClassName}";
        }
    }
}
