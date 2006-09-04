#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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

using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.DataSets;
using Seasar.Extension.DataSets.Impl;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Unit;
using Seasar.Framework.Util;
using IDataReader = Seasar.Extension.DataSets.IDataReader;

namespace Seasar.Extension.Unit
{
    public class S2TestCase : S2FrameworkTestCaseBase
    {
        private IDataSource dataSource;

        private IDbConnection connection;

        public S2TestCase()
        {
        }

        public IDataSource DataSource
        {
            get
            {
                if (dataSource == null)
                {
                    throw new EmptyRuntimeException("dataSource");
                }
                return dataSource;
            }
        }

        public IDbConnection Connection
        {
            get
            {
                if (connection != null)
                {
                    return connection;
                }
                connection = DataSourceUtil.GetConnection(dataSource);
                return connection;
            }
        }

        public bool HasConnection
        {
            get { return connection != null; }
        }

        internal void SetConnection(IDbConnection connection)
        {
            this.connection = connection;
        }

        internal void SetDataSource(IDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        /// <summary>
        /// Excelファイルを読み、DataSetを作成します。
        /// シート名をテーブル名、一行目をカラム名、二行目以降をデータ として読み込みます。
        /// 
        /// パスはAssemblyで指定されているディレクトリをルートとする。
        /// 設定ファイルの絶対パスか、ファイル名のみを指定します。
        /// ファイル名のみの場合、テストケースと同じパッケージにあるものとします。
        /// <seealso cref="Seasar.Extension.DataSets.Impl.XlsReader.Read"/>
        /// </summary>
        /// <param name="path">Excelファイルのパス</param>
        /// <returns>Excelファイルの内容から作成したDataSet</returns>
        public virtual DataSet ReadXls(string path)
        {
            IDataReader reader = new XlsReader(ConvertPath(path));
            return reader.Read();
        }

        /// <summary>
        /// DataSetの内容から、Excelファイルを作成します。
        /// シート名にテーブル名、一行目にカラム名、二行目以降にデータ を書き込みます。
        /// 
        /// パスはAssemblyで指定されているディレクトリをルートとする。
        /// 設定ファイルの絶対パスか、ファイル名のみを指定します。
        /// ファイル名のみの場合、テストケースと同じパッケージにあるものとします。
        /// <seealso cref="Seasar.Extension.DataSets.Impl.XlsWriter.Write"/>
        /// </summary>
        /// <param name="path">Excelファイルのパス</param>
        /// <param name="dataSet">Excelファイルに書き込む内容のDataSet</param>
        public virtual void WriteXls(string path, DataSet dataSet)
        {
            IDataWriter writer = new XlsWriter(ConvertPath(path));
            writer.Write(dataSet);
        }

        /// <summary>
        /// DataSetをDBに書き込みます。
        /// <seealso cref="Seasar.Extension.DataSets.Impl.SqlWriter.Write"/>
        /// </summary>
        /// <param name="dataSet">データベースに書き込む内容のDataSet</param>
        public virtual void WriteDb(DataSet dataSet)
        {
            IDataWriter writer = new SqlWriter(DataSource);
            writer.Write(dataSet);
        }

        /// <summary>
        /// DBからレコードを読み込み、DataTableを作成します。
        /// <seealso cref="Seasar.Extension.DataSets.Impl.SqlTableReader.Read"/>
        /// </summary>
        /// <param name="table">読み込むテーブル名</param>
        /// <returns>読み込んだ内容から作成したDataTable</returns>
        public virtual DataTable ReadDbByTable(string table)
        {
            return ReadDbByTable(table, null);
        }

        /// <summary>
        /// DBからレコードを読み込み、DataTableを作成します。
        /// 読み込むレコードはconditionの条件を満たすレコードです。 conditionには" WHERE "より後ろをセットしてください。
        /// <seealso cref="Seasar.Extension.DataSets.Impl.SqlTableReader.Read"/>
        /// </summary>
        /// <param name="table">読み込むテーブル名</param>
        /// <param name="condition">条件句(WHEREの後ろ)</param>
        /// <returns>読み込んだ内容から作成したDataTable</returns>
        public virtual DataTable ReadDbByTable(string table, string condition)
        {
            SqlTableReader reader = new SqlTableReader(DataSource);
            reader.SetTable(table, condition);
            return reader.Read();
        }

        /// <summary>
        /// DBからSQL文の実行結果を取得し、DataTableを作成します。
        /// 作成したDataTableのテーブル名はtableNameになります。
        /// <seealso cref="Seasar.Extension.DataSets.Impl.SqlTableReader.Read"/>
        /// </summary>
        /// <param name="sql">実行するSQL文</param>
        /// <param name="tableName">作成するDataTableのテーブル名</param>
        /// <returns>読み出した内容のDataTable</returns>
        public virtual DataTable ReadDbBySql(string sql, string tableName)
        {
            SqlTableReader reader = new SqlTableReader(DataSource);
            reader.SetSql(sql, tableName);
            return reader.Read();

        }

        /// <summary>
        /// Excelファイルを読み込み、DBに書き込みます。
        /// シート名をテーブル名、一行目をカラム名、二行目以降をデータ として読み込みます。
        /// 
        /// パスはAssemblyで指定されているディレクトリをルートとする。
        /// 設定ファイルの絶対パスか、ファイル名のみを指定します。
        /// ファイル名のみの場合、テストケースと同じパッケージにあるものとします。
        /// <seealso cref="Seasar.Extension.DataSets.Impl.XlsReader.Read"/>
        /// <seealso cref="Seasar.Extension.DataSets.Impl.SqlWriter.Write"/>
        /// </summary>
        /// <param name="path">Excelファイルのパス</param>
        public virtual void ReadXlsWriteDb(string path)
        {
            WriteDb(ReadXls(path));
        }

        /// <summary>
        /// Excelファイルを読み込み、DBに書き込みます。
        /// シート名をテーブル名、一行目をカラム名、二行目以降をデータ として読み込みます。
        /// Excelの内容とDBのレコードとで主キーが一致するものがあれば、 そのレコードを削除した後に書き込みます。
        /// 
        /// パスはAssemblyで指定されているディレクトリをルートとする。
        /// 設定ファイルの絶対パスか、ファイル名のみを指定します。
        /// ファイル名のみの場合、テストケースと同じパッケージにあるものとします。
        /// <seealso cref="Seasar.Extension.DataSets.Impl.XlsReader.Read"/>
        /// <seealso cref="Seasar.Extension.DataSets.Impl.SqlWriter.Write"/>
        /// </summary>
        /// <param name="path">Excelファイルのパス</param>
        public virtual void ReadXlsReplaceDb(string path)
        {
            DataSet dataSet = ReadXls(path);
            DeleteDb(dataSet);
            WriteDb(dataSet);
        }

        /// <summary>
        /// Excelファイルを読み込み、DBに書き込みます。
        /// シート名をテーブル名、一行目をカラム名、二行目以降をデータ として読み込みます。
        /// 対象となるテーブルのレコードを全て削除した後に書き込みます。
        /// 
        /// パスはAssemblyで指定されているディレクトリをルートとする。
        /// 設定ファイルの絶対パスか、ファイル名のみを指定します。
        /// ファイル名のみの場合、テストケースと同じパッケージにあるものとします。
        /// <seealso cref="Seasar.Extension.DataSets.Impl.XlsReader.Read"/>
        /// <seealso cref="Seasar.Extension.DataSets.Impl.SqlWriter.Write"/>
        /// </summary>
        /// <param name="path">Excelファイルのパス</param>
        public virtual void ReadXlsAllReplaceDb(string path)
        {
            DataSet dataSet = ReadXls(path);
            for (int i = dataSet.Tables.Count - 1; i >= 0; --i)
            {
                DeleteTable(dataSet.Tables[i].TableName);
            }
            WriteDb(dataSet);
        }

        /// <summary>
        /// DataSetに対応するDBのレコードを読み込み、DataSetを作成します 。
        /// <seealso cref="Seasar.Extension.DataSets.Impl.SqlReloadReader.Read"/>
        /// </summary>
        /// <param name="dataSet">対象DBに対応するDataSet</param>
        /// <returns>最新状態のDataSet</returns>
        public virtual DataSet Reload(DataSet dataSet)
        {
            return new SqlReloadReader(DataSource, dataSet).Read();
        }

        /// <summary>
        /// DataTableに対応するDBのレコードを読み込み、DataTableを作成 します。
        /// <seealso cref="Seasar.Extension.DataSets.Impl.SqlReloadReader.Read"/>
        /// </summary>
        /// <param name="table">対象DBに対応するDataTable</param>
        /// <returns>最新状態のDataTable</returns>
        public virtual DataTable Reload(DataTable table)
        {
            return new SqlReloadTableReader(DataSource, table).Read();
        }

        /// <summary>
        /// DataSetに対応するDBのレコードを削除します。
        /// <seealso cref="Seasar.Extension.DataSets.Impl.SqlDeleteTableWriter.Write"/>
        /// </summary>
        /// <param name="dataSet">対象DBに対応するDataSet</param>
        public virtual void DeleteDb(DataSet dataSet)
        {
            SqlDeleteTableWriter writer = new SqlDeleteTableWriter(DataSource);
            for (int i = dataSet.Tables.Count - 1; i >= 0; --i)
            {
                writer.Write(dataSet.Tables[i]);
            }
        }

        /// <summary>
        /// DBから指定するテーブルの全レコードを削除します。
        /// </summary>
        /// <param name="tableName">削除対象のテーブル名</param>
        public virtual void DeleteTable(string tableName)
        {
            IUpdateHandler handler = new BasicUpdateHandler(
                DataSource,
                "DELETE FROM " + tableName
                );
            handler.Execute(null);
        }
    }
}
