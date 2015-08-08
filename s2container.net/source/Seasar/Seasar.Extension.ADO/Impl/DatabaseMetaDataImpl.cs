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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    public class DatabaseMetaDataImpl : IDatabaseMetaData
    {
        private readonly IDictionary _primaryKeys = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
        private readonly IDictionary _columns = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
        private readonly IDictionary _autoIncrementColumns = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

        private DataSet _metaDataSet;

        private readonly IDataSource _dataSource;

        public DatabaseMetaDataImpl(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        #region IDatabaseMetaData メンバ

        public IList GetPrimaryKeySet(string tableName)
        {
            if (!_primaryKeys.Contains(tableName)) CreateTableMetaData(tableName);
            return (IList) _primaryKeys[tableName];
        }

        public IList GetColumnSet(string tableName)
        {
            if (!_columns.Contains(tableName)) CreateTableMetaData(tableName);
            return (IList) _columns[tableName];
        }

        public IList GetAutoIncrementColumnSet(string tableName)
        {
            if (!_autoIncrementColumns.Contains(tableName)) CreateTableMetaData(tableName);
            return (IList) _autoIncrementColumns[tableName];
        }

        #endregion

        private string _metaDataSetClassName;

        /// <summary>
        /// DBのメタ情報を格納する<seealso cref="DataSet"/>の完全修飾名を設定する
        /// <seealso cref="DataSet"/>を含むアセンブリは、
        /// 現在のアプリケーション・ドメインに含まれる必要がある
        /// </summary>
        public string MetaDataSetClassName
        {
            set { _metaDataSetClassName = value; }
        }

        /// <summary>
        /// テーブル定義情報を作成する
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        protected virtual void CreateTableMetaData(string tableName)
        {
            lock (this)
            {
                // テーブル定義情報を取得する
                DataTable metaDataTable;
                if (_metaDataSetClassName == null)
                {
                    metaDataTable = _GetMetaDataForDatabase(tableName);
                }
                else
                {
                    metaDataTable = _GetMetaDataForDataSet(tableName);
                }

                if (metaDataTable != null)
                {
                    // テーブル定義情報からプライマリキーを取得する
                    _primaryKeys[tableName] = GetPrimaryKeySet(metaDataTable.PrimaryKey);

                    // テーブル定義情報からカラムを取得する
                    _columns[tableName] = GetColumnSet(metaDataTable.Columns);

                    // テーブル定義情報からAutoIncrementカラムを取得する
                    _autoIncrementColumns[tableName] = GetAutoIncrementColumnSet(metaDataTable.Columns);
                }
            }
        }

        /// <summary>
        /// DBからテーブル定義情報を取得する
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <returns>テーブル定義情報。取得できなかった場合、nullを返す</returns>
        private DataTable _GetMetaDataForDatabase(string tableName)
        {
            // テーブル定義
            DataTable[] metaDataTables;

            // IDbConnectionを取得する
            var cn = DataSourceUtil.GetConnection(_dataSource);
            try
            {
                // テーブル定義情報を取得するためのSQLを作成する
                var sql = $"SELECT * FROM {tableName} WHERE 1 = 0";

                // IDbCommandを取得する
                var cmd = _dataSource.GetCommand(sql, cn);

                // Transactionの処理を行う
                _dataSource.SetTransaction(cmd);

                // IDataAdapterを取得する
                var adapter = _dataSource.GetDataAdapter(cmd);

                // テーブル定義情報を取得する
                try
                {
                    metaDataTables = adapter.FillSchema(new DataSet(), SchemaType.Mapped);
                }
                catch
                {
                    return null;
                }
            }
            finally
            {
                // IDbConnectionのClose処理を行う
                _dataSource.CloseConnection(cn);
            }

            return metaDataTables[0];
        }

        /// <summary>
        /// DBのメタ情報を格納する<seealso cref="DataSet"/>からテーブル定義情報を取得する
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <returns>テーブル定義情報。取得できなかった場合、nullを返す</returns>
        private DataTable _GetMetaDataForDataSet(string tableName)
        {
            if (_metaDataSet == null)
            {
                var loadedAssembly = AppDomain.CurrentDomain.GetAssemblies();
                var dataSetType = ClassUtil.ForName(_metaDataSetClassName, loadedAssembly);
                _metaDataSet = (DataSet) ClassUtil.NewInstance(dataSetType);
            }

            if (!_metaDataSet.Tables.Contains(tableName))
            {
                return null;
            }
            else
            {
                return _metaDataSet.Tables[tableName];
            }
        }

        private IList GetPrimaryKeySet(IEnumerable<DataColumn> primarykeys)
        {
            IList list = new CaseInsentiveSet();
            foreach (var pkey in primarykeys)
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
