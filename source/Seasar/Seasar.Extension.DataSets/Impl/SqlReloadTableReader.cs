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

using System.Collections;
using System.Data;
using System.Text;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Util;

namespace Seasar.Extension.DataSets.Impl
{
    public class SqlReloadTableReader : ITableReader
    {
        private readonly IDataSource _dataSource;
        private readonly DataTable _table;
        private string _sql;
        private string[] _primaryKeys;

        public SqlReloadTableReader(IDataSource dataSource, DataTable table)
        {
            _dataSource = dataSource;
            _table = table;
            IDbConnection con = DataSourceUtil.GetConnection(DataSource);
            try
            {
                IDatabaseMetaData dbMetaData = new DatabaseMetaDataImpl(DataSource);
                DataTableUtil.SetupMetaData(dbMetaData, table);
            }
            finally
            {
                DataSourceUtil.CloseConnection(dataSource, con);
            }
            SetUp();
        }

        private void SetUp()
        {
            StringBuilder buf = new StringBuilder(100);
            buf.Append("SELECT ");
            StringBuilder whereBuf = new StringBuilder(100);
            whereBuf.Append(" WHERE");
            ArrayList primaryKeyList = new ArrayList();
            foreach (DataColumn column in _table.Columns)
            {
                buf.Append(column.ColumnName);
                buf.Append(", ");
                if (DataTableUtil.IsPrimaryKey(_table, column))
                {
                    whereBuf.AppendFormat(" {0} = @{1} AND", column.ColumnName, column.ColumnName);
                    primaryKeyList.Add(column.ColumnName);
                }
            }
            buf.Length -= 2;
            whereBuf.Length -= 4;
            buf.Append(" FROM ");
            buf.Append(_table.TableName);
            buf.Append(whereBuf);
            _sql = buf.ToString();
            _primaryKeys = (string[]) primaryKeyList.ToArray(typeof(string));
        }

        public IDataSource DataSource
        {
            get { return _dataSource; }
        }

        public DataTable Table
        {
            get { return _table; }
        }

        #region ITableReader ÉÅÉìÉo

        public virtual DataTable Read()
        {
            DataTable newTable = _table.Clone();
            foreach (DataRow row in _table.Rows)
            {
                DataRow newRow = newTable.NewRow();
                Reload(row, newRow);
                newTable.Rows.Add(newRow);
            }
            newTable.AcceptChanges();
            return newTable;
        }

        #endregion

        protected virtual void Reload(DataRow row, DataRow newRow)
        {
            ISelectHandler selectHandler = new BasicSelectHandler(
                _dataSource,
                _sql,
                new DataRowReloadDataReaderHandler(row, newRow)
                );
            object[] args = new object[_primaryKeys.Length];
            for (int i = 0; i < _primaryKeys.Length; ++i)
            {
                args[i] = row[_primaryKeys[i]];
            }
            selectHandler.Execute(args);
        }
    }
}
