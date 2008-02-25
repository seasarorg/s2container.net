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
using System.Xml.Serialization;
using System.Collections;

namespace Seasar.Quill.Xml
{
    /// <summary>
    /// Quillの構成セクション
    /// </summary>
    [Serializable]
    [XmlRoot(QuillConstants.QUILL_CONFIG)]
    public class QuillSection
    {
        private string _daoSetting = null;
        private string _transactionSetting = null;
        private IList _dataSources = new ArrayList();

        [XmlElement(QuillConstants.CONFIG_DAO_SETTING_KEY)]
        public string DaoSetting
        {
            set { _daoSetting = value; }
            get { return _daoSetting; }
        }

        [XmlElement(QuillConstants.CONFIG_TX_SETTING_KEY)]
        public string TransactionSetting
        {
            set { _transactionSetting = value; }
            get { return _transactionSetting; }
        }

        [XmlArray(QuillConstants.CONFIG_DATASOURCES_KEY)]
        [XmlArrayItem(QuillConstants.CONFIG_DATASOURCE_KEY, typeof(DataSourceSection))]
        public IList DataSources
        {
            set { _dataSources = value; }
            get { return _dataSources; }
        }
    }
}
