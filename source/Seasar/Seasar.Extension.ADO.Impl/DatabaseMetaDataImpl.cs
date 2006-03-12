#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
using System.Collections;
using System.Data;
using System.Data.Common;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    public class DatabaseMetaDataImpl : IDatabaseMetaData
    {
        private IDictionary primaryKeys = new Hashtable(
               CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);

        private IDictionary columns = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
            CaseInsensitiveComparer.Default);

        private IDataSource dataSource;

        public DatabaseMetaDataImpl(IDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        #region IDatabaseMetaData ÉÅÉìÉo

        public System.Collections.IList GetPrimaryKeySet(string tableName)
        {
            if(!this.primaryKeys.Contains(tableName)) CreateTableMetaData(tableName);
            return (IList) primaryKeys[tableName];
        }

        public IList GetColumnSet(string tableName)
        {
            if(!this.columns.Contains(tableName)) CreateTableMetaData(tableName);
            return (IList) columns[tableName];
        }

        #endregion

        private void CreateTableMetaData(string tableName)
        {
            lock(this)
            {
                IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
                try
                {
                    string sql = "SELECT * FROM " + tableName;

                    IDbCommand cmd = dataSource.GetCommand(sql, cn);
                    DataSourceUtil.SetTransaction(dataSource, cmd);
                    DbDataAdapter adapter = dataSource.GetDataAdapter(cmd) as DbDataAdapter;
                    DataTable metaDataTable = new DataTable(tableName);
                    adapter.FillSchema(metaDataTable, SchemaType.Mapped);
                    primaryKeys[tableName] = GetPrimaryKeySet(metaDataTable.PrimaryKey);
                    columns[tableName] = GetColumnSet(metaDataTable.Columns);
                }
                finally
                {
                    DataSourceUtil.CloseConnection(dataSource, cn);
                }
            }
        }

        private IList GetPrimaryKeySet(DataColumn[] primarykeys)
        {
            IList list = new CaseInsentiveSet();
            foreach (DataColumn pkey in primarykeys)
            {
                list.Add(pkey.ColumnName);
            }
            return list;
        }

        private IList GetColumnSet(DataColumnCollection columns)
        {
            IList list = new CaseInsentiveSet();
            foreach (DataColumn column in columns)
            {
                list.Add(column.ColumnName);
            }
            return list;
        }
        
    }
}
