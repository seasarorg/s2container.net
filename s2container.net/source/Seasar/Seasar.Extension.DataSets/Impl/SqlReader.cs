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

using System.Collections;
using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Extension.DataSets.Impl
{
    public class SqlReader : IDataReader
    {
        private readonly IDataSource _dataSource;
        private readonly IList _tableReaders = new ArrayList();

        public SqlReader(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public IDataSource DataSource
        {
            get { return _dataSource; }
        }

        public virtual void AddTable(string tableName)
        {
            AddTable(tableName, null);
        }

        public virtual void AddTable(string tableName, string condition)
        {
            SqlTableReader reader = new SqlTableReader(_dataSource);
            reader.SetTable(tableName, condition);
            _tableReaders.Add(reader);
        }

        public virtual void AddSql(string sql, string tableName)
        {
            SqlTableReader reader = new SqlTableReader(_dataSource);
            reader.SetSql(sql, tableName);
            _tableReaders.Add(reader);
        }

        #region IDataReader ÉÅÉìÉo

        public virtual DataSet Read()
        {
            DataSet dataSet = new DataSet();
            foreach (ITableReader reader in _tableReaders)
            {
                dataSet.Tables.Add(reader.Read());
            }
            return dataSet;
        }

        #endregion
    }
}
