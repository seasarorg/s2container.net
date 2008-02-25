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


namespace Seasar.Quill
{
    /// <summary>
    /// Quill上で使用する定数を定義
    /// </summary>
    public class QuillConstants
    {
        public const string QUILL_CONFIG = "quill";
        public const string CONFIG_DAO_SETTING_KEY = "daoSetting";
        public const string CONFIG_TX_SETTING_KEY = "transactionSetting";
        public const string CONFIG_DATASOURCES_KEY = "dataSources";
        public const string CONFIG_DATASOURCE_KEY = "dataSource";
        public const string CONFIG_CONNECTION_STRING_KEY = "connectionString";
        public const string CONFIG_PROVIDER = "provider";
        public const string CONFIG_DATASOURCE_CLASS_KEY = "class";
        public const string CONFIG_DATASOURCE_NAME_ATTR = "name";

        public const string NAMESPACE_PROVIDER = "Seasar.Quill.Database.Provider";
        public const string NAMESPACE_DAOSETTING = "Seasar.Quill.Dao.Impl";
        public const string NAMESPACE_TXSETTING = "Seasar.Quill.Database.Tx.Impl";
        public const string NAMESPACE_DATASOURCE = "Seasar.Extension.ADO.Impl";
    }
}
