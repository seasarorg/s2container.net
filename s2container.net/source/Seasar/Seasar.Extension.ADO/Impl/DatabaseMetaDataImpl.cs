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
using System.Collections;
using System.Data;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    public class DatabaseMetaDataImpl : IDatabaseMetaData
    {
#if NET_1_1
        private IDictionary _primaryKeys = new Hashtable(
            new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
        private IDictionary _columns = new Hashtable(
            new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
        private IDictionary _autoIncrementColumns = new Hashtable(
            new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
#else
        private readonly IDictionary _primaryKeys = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
        private readonly IDictionary _columns = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
        private readonly IDictionary _autoIncrementColumns = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
#endif

        private readonly IDataSource _dataSource;

        public DatabaseMetaDataImpl(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        #region IDatabaseMetaData メンバ

        public IList GetPrimaryKeySet(string tableName)
        {
            if(!_primaryKeys.Contains(tableName)) CreateTableMetaData(tableName);
            return (IList) _primaryKeys[tableName];
        }

        public IList GetColumnSet(string tableName)
        {
            if(!_columns.Contains(tableName)) CreateTableMetaData(tableName);
            return (IList) _columns[tableName];
        }

        public IList GetAutoIncrementColumnSet(string tableName)
        {
            if (!_autoIncrementColumns.Contains(tableName)) CreateTableMetaData(tableName);
            return (IList) _autoIncrementColumns[tableName];
        }

        #endregion

        /// <summary>
        /// テーブル定義情報を作成する
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        private void CreateTableMetaData(string tableName)
        {
            lock(this)
            {
                // IDbConnectionを取得する
                IDbConnection cn = DataSourceUtil.GetConnection(_dataSource);
                try
                {
                    // テーブル定義情報を取得するためのSQLを作成する
                    string sql = "SELECT * FROM " + tableName;

                    // IDbCommandを取得する
                    IDbCommand cmd = _dataSource.GetCommand(sql, cn);

                    // Transactionの処理を行う
                    _dataSource.SetTransaction(cmd);

                    // IDataAdapterを取得する
                    IDataAdapter adapter = _dataSource.GetDataAdapter(cmd);

                    // テーブル定義
                    DataTable[] metaDataTables;

                    // テーブル定義情報を取得する
                    try
                    {
                        metaDataTables = adapter.FillSchema(new DataSet(), SchemaType.Mapped);
                    }
                    catch
                    {
                        return;
                    }

                    // テーブル定義情報からプライマリキーを取得する
                    _primaryKeys[tableName] = GetPrimaryKeySet(metaDataTables[0].PrimaryKey);

                    // テーブル定義情報からカラムを取得する
                    _columns[tableName] = GetColumnSet(metaDataTables[0].Columns);

                    // テーブル定義情報からAutoIncrementカラムを取得する
                    _autoIncrementColumns[tableName] = GetAutoIncrementColumnSet(metaDataTables[0].Columns);
                }
                finally
                {
                    // IDbConnectionのClose処理を行う
                    _dataSource.CloseConnection(cn);
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

        private IList GetAutoIncrementColumnSet(DataColumnCollection columns)
        {
            IList list = new CaseInsentiveSet();
            foreach (DataColumn column in columns)
            {
                if (column.AutoIncrement)
                {
                    list.Add(column.ColumnName);
                }
            }
            return list;
        }
    }
}
