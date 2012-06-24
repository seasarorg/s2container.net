#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

#if NET_4_0
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;

namespace Seasar.Unit.Core
{
    /// <summary>
    /// テスト補助基底クラス
    /// </summary>
    public class S2TestCaseBase
    {
        private IDataSource _dataSource;
        public IDataSource DataSource
        {
            get 
            {
                if (_dataSource == null)
                {
                    throw new EmptyRuntimeException("_dataSource");
                }
                return _dataSource; 
            }

            set 
            { 
                _dataSource = value;
                _testExcel = null;
                if (_dataSource != null)
                {
                    _testExcel = new S2TestExcel(_dataSource, CommandFactory);
                }
            }
        }

        private IDbConnection _connection;
        public IDbConnection Connection
        {
            get
            {
                if (_connection != null)
                {
                    return _connection;
                }
                _connection = DataSourceUtil.GetConnection(_dataSource);
                return _connection;
            }

            set
            {
                _connection = value;
            }
        }

        private S2TestExcel _testExcel;
        protected S2TestExcel TestExcel
        {
            get { return _testExcel; }
        }

        protected virtual ICommandFactory CommandFactory
        {
            get
            {
                //  テストデータの書き込みにカスタムしたCommandFactoryが
                //  必要になることはないと思われるため
                //  現状では実質BasicCommandFactory固定としています。
                return BasicCommandFactory.INSTANCE;
            }
        }

        public bool HasConnection
        {
            get { return _connection != null; }
        }
        
        /// <summary>
        /// Excelファイルを読み、DataSetを作成します。
        /// シート名をテーブル名、一行目をカラム名、二行目以降をデータ として読み込みます。
        /// 
        /// パスはAssemblyで指定されているディレクトリをルートとする。
        /// 設定ファイルの絶対パスか、ファイル名のみを指定します。
        /// ファイル名のみの場合、テストケースと同じパッケージにあるものとします。
        /// <seealso cref="Seasar.Unit.DataSets.Impl.XlsReader.Read"/>
        /// </summary>
        /// <param name="path">Excelファイルのパス</param>
        /// <returns>Excelファイルの内容から作成したDataSet</returns>
        public virtual DataSet ReadXls(string path)
        {
            return TestExcel.ReadXls(path);
        }

        /// <summary>
        /// DataSetの内容から、Excelファイルを作成します。
        /// シート名にテーブル名、一行目にカラム名、二行目以降にデータ を書き込みます。
        /// 
        /// パスはAssemblyで指定されているディレクトリをルートとする。
        /// 設定ファイルの絶対パスか、ファイル名のみを指定します。
        /// ファイル名のみの場合、テストケースと同じパッケージにあるものとします。
        /// <seealso cref="Seasar.Unit.DataSets.Impl.XlsWriter.Write"/>
        /// </summary>
        /// <param name="path">Excelファイルのパス</param>
        /// <param name="dataSet">Excelファイルに書き込む内容のDataSet</param>
        public virtual void WriteXls(string path, DataSet dataSet)
        {
            TestExcel.WriteXls(path, dataSet);
        }

        /// <summary>
        /// DataSetをDBに書き込みます。
        /// <seealso cref="Seasar.Unit.DataSets.Impl.SqlWriter.Write"/>
        /// </summary>
        /// <param name="dataSet">データベースに書き込む内容のDataSet</param>
        public virtual void WriteDb(DataSet dataSet)
        {
            TestExcel.WriteDb(dataSet);
        }

        /// <summary>
        /// DBからレコードを読み込み、DataTableを作成します。
        /// <seealso cref="Seasar.Unit.DataSets.Impl.SqlTableReader.Read"/>
        /// </summary>
        /// <param name="table">読み込むテーブル名</param>
        /// <returns>読み込んだ内容から作成したDataTable</returns>
        public virtual DataTable ReadDbByTable(string table)
        {
            return TestExcel.ReadDbByTable(table);
        }

        /// <summary>
        /// DBからレコードを読み込み、DataTableを作成します。
        /// 読み込むレコードはconditionの条件を満たすレコードです。 conditionには" WHERE "より後ろをセットしてください。
        /// <seealso cref="Seasar.Unit.DataSets.Impl.SqlTableReader.Read"/>
        /// </summary>
        /// <param name="table">読み込むテーブル名</param>
        /// <param name="condition">条件句(WHEREの後ろ)</param>
        /// <returns>読み込んだ内容から作成したDataTable</returns>
        public virtual DataTable ReadDbByTable(string table, string condition)
        {
            return TestExcel.ReadDbByTable(table, condition);
        }

        /// <summary>
        /// DBからSQL文の実行結果を取得し、DataTableを作成します。
        /// 作成したDataTableのテーブル名はtableNameになります。
        /// <seealso cref="Seasar.Unit.DataSets.Impl.SqlTableReader.Read"/>
        /// </summary>
        /// <param name="sql">実行するSQL文</param>
        /// <param name="tableName">作成するDataTableのテーブル名</param>
        /// <returns>読み出した内容のDataTable</returns>
        public virtual DataTable ReadDbBySql(string sql, string tableName)
        {
            return TestExcel.ReadDbBySql(sql, tableName);
        }

        /// <summary>
        /// Excelファイルを読み込み、DBに書き込みます。
        /// シート名をテーブル名、一行目をカラム名、二行目以降をデータ として読み込みます。
        /// 
        /// パスはAssemblyで指定されているディレクトリをルートとする。
        /// 設定ファイルの絶対パスか、ファイル名のみを指定します。
        /// ファイル名のみの場合、テストケースと同じパッケージにあるものとします。
        /// <seealso cref="Seasar.Unit.DataSets.Impl.XlsReader.Read"/>
        /// <seealso cref="Seasar.Unit.DataSets.Impl.SqlWriter.Write"/>
        /// </summary>
        /// <param name="path">Excelファイルのパス</param>
        public virtual void ReadXlsWriteDb(string path)
        {
            TestExcel.ReadXlsWriteDb(path);
        }

        /// <summary>
        /// Excelファイルを読み込み、DBに書き込みます。
        /// シート名をテーブル名、一行目をカラム名、二行目以降をデータ として読み込みます。
        /// Excelの内容とDBのレコードとで主キーが一致するものがあれば、 そのレコードを削除した後に書き込みます。
        /// 
        /// パスはAssemblyで指定されているディレクトリをルートとする。
        /// 設定ファイルの絶対パスか、ファイル名のみを指定します。
        /// ファイル名のみの場合、テストケースと同じパッケージにあるものとします。
        /// <seealso cref="Seasar.Unit.DataSets.Impl.XlsReader.Read"/>
        /// <seealso cref="Seasar.Unit.DataSets.Impl.SqlWriter.Write"/>
        /// </summary>
        /// <param name="path">Excelファイルのパス</param>
        public virtual void ReadXlsReplaceDb(string path)
        {
            TestExcel.ReadXlsReplaceDb(path);
        }

        /// <summary>
        /// Excelファイルを読み込み、DBに書き込みます。
        /// シート名をテーブル名、一行目をカラム名、二行目以降をデータ として読み込みます。
        /// 対象となるテーブルのレコードを全て削除した後に書き込みます。
        /// 
        /// パスはAssemblyで指定されているディレクトリをルートとする。
        /// 設定ファイルの絶対パスか、ファイル名のみを指定します。
        /// ファイル名のみの場合、テストケースと同じパッケージにあるものとします。
        /// <seealso cref="Seasar.Unit.DataSets.Impl.XlsReader.Read"/>
        /// <seealso cref="Seasar.Unit.DataSets.Impl.SqlWriter.Write"/>
        /// </summary>
        /// <param name="path">Excelファイルのパス</param>
        public virtual void ReadXlsAllReplaceDb(string path)
        {
            TestExcel.ReadXlsAllReplaceDb(path);
        }

        /// <summary>
        /// DataSetに対応するDBのレコードを読み込み、DataSetを作成します 。
        /// <seealso cref="Seasar.Unit.DataSets.Impl.SqlReloadReader.Read"/>
        /// </summary>
        /// <param name="dataSet">対象DBに対応するDataSet</param>
        /// <returns>最新状態のDataSet</returns>
        public virtual DataSet Reload(DataSet dataSet)
        {
            return TestExcel.Reload(dataSet);
        }

        /// <summary>
        /// DataTableに対応するDBのレコードを読み込み、DataTableを作成 します。
        /// <seealso cref="Seasar.Unit.DataSets.Impl.SqlReloadReader.Read"/>
        /// </summary>
        /// <param name="table">対象DBに対応するDataTable</param>
        /// <returns>最新状態のDataTable</returns>
        public virtual DataTable Reload(DataTable table)
        {
            return TestExcel.Reload(table);
        }

        /// <summary>
        /// DataSetに対応するDBのレコードを削除します。
        /// <seealso cref="Seasar.Unit.DataSets.Impl.SqlDeleteTableWriter.Write"/>
        /// </summary>
        /// <param name="dataSet">対象DBに対応するDataSet</param>
        public virtual void DeleteDb(DataSet dataSet)
        {
            TestExcel.DeleteDb(dataSet);
        }

        /// <summary>
        /// DBから指定するテーブルの全レコードを削除します。
        /// </summary>
        /// <param name="tableName">削除対象のテーブル名</param>
        public virtual void DeleteTable(string tableName)
        {
            TestExcel.DeleteTable(tableName);
        }
    }
}
#endif