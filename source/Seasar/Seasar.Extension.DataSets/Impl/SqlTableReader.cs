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
using System.Data;
using System.Text;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;

namespace Seasar.Extension.DataSets.Impl
{
    public class SqlTableReader : ITableReader
    {
        private IDataSource dataSource_;

        private string tableName_;

        private string sql_;

        public SqlTableReader(IDataSource dataSource)
        {
            dataSource_ = dataSource;
        }

        public IDataSource DataSource
        {
            get { return dataSource_; }
        }

        public string TableName
        {
            get { return tableName_; }
        }

        public string Sql
        {
            get { return sql_; }
        }

        public virtual void SetTable(string tableName)
        {
            SetTable(tableName, null);
        }

        public virtual void SetTable(string tableName, String condition)
        {
            tableName_ = tableName;
            StringBuilder sqlBuf = new StringBuilder(100);
            sqlBuf.Append("SELECT * FROM ");
            sqlBuf.Append(tableName);
            if (condition != null)
            {
                sqlBuf.Append(" WHERE ");
                sqlBuf.Append(condition);
            }
            sql_ = sqlBuf.ToString();
        }

        public virtual void SetSql(string sql, string tableName)
        {
            sql_ = sql;
            tableName_ = tableName;
        }

        #region ITableReader ÉÅÉìÉo

        public virtual DataTable Read()
        {
            ISelectHandler selectHandler = new BasicSelectHandler(
                dataSource_,
                sql_,
                new DataTableDataReaderHandler(tableName_)
                );
            DataTable table = (DataTable) selectHandler.Execute(null);
            table.AcceptChanges();
            return table;
        }

        #endregion
    }
}
