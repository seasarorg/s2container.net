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
        private IDictionary primaryKeys = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());

        private IDictionary columns = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());

        private IDataSource dataSource;

        public DatabaseMetaDataImpl(IDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        #region IDatabaseMetaData メンバ

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
                // IDbConnectionを取得する
                IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
                try
                {
                    // テーブル定義情報を取得するためのSQLを作成する
                    string sql = "SELECT * FROM " + tableName;

                    // IDbCommandを取得する
                    IDbCommand cmd = dataSource.GetCommand(sql, cn);

                    // Transactionの処理を行う
                    DataSourceUtil.SetTransaction(dataSource, cmd);

                    // DbDataAdapterを取得する
                    DbDataAdapter adapter = dataSource.GetDataAdapter(cmd) as DbDataAdapter;

                    // テーブル定義
                    DataTable metaDataTable = new DataTable(tableName);

                    try
                    {
                        // テーブル定義を取得する
                        adapter.FillSchema(metaDataTable, SchemaType.Mapped);
                    }
                    catch
                    {
                    }
                    
                    // テーブル定義情報からプライマリキーを取得する
                    primaryKeys[tableName] = GetPrimaryKeySet(metaDataTable.PrimaryKey);
                    
                    // テーブル定義情報からカラムを取得する
                    columns[tableName] = GetColumnSet(metaDataTable.Columns);
                }
                finally
                {
                    // IDbConnectionのClose処理を行う
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
