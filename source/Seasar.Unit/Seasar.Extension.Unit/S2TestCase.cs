#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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

using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
#if NET_4_0
using System;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Unit.Core;
#else
#region NET2.0
using System.Data;
using Seasar.Extension.DataSets;
using Seasar.Extension.DataSets.Impl;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Unit;
using Seasar.Framework.Util;
using IDataReader = Seasar.Extension.DataSets.IDataReader;
#endregion
#endif

namespace Seasar.Extension.Unit
{
    /// <summary>
    /// S2Container用テスト補助クラス
    /// </summary>
#if NET_4_0
    /// <remarks>
    /// 継承して使用
    /// S2Containerへの登録やコンポーネントの取得等をショートカット
    /// </remarks>
    public class S2TestCase : S2TestCaseBase
#else
#region NET2.0
    public class S2TestCase : S2FrameworkTestCaseBase
#endregion
#endif
    {
        private ICommandFactory _commandFactory = null;
#if NET_4_0
        private IS2Container _container;
        public IS2Container Container
        {
            get { return _container; }
            set { _container = value; }
        }

        protected override ICommandFactory CommandFactory
        {
            get
            {
                if (_commandFactory == null)
                {
                    if (Container.HasComponentDef(typeof(ICommandFactory)))
                    {
                        //*
                        Console.WriteLine("□□□");
                        //*/
                        _commandFactory = Container.GetComponent(typeof(ICommandFactory)) as ICommandFactory;
                    }
                    else
                    {
                        //*
                        Console.WriteLine("☆☆☆");
                        //*/
                        _commandFactory = BasicCommandFactory.INSTANCE;
                    }
                }
                //*
                Console.WriteLine("○○○");
                //*/
                return _commandFactory;
            }
        }

        public object GetComponent(string componentName)
        {
            return _container.GetComponent(componentName);
        }

        public object GetComponent(Type componentClass)
        {
            return _container.GetComponent(componentClass);
        }

        public IComponentDef GetComponentDef(string componentName)
        {
            return _container.GetComponentDef(componentName);
        }

        public IComponentDef GetComponentDef(Type componentClass)
        {
            return _container.GetComponentDef(componentClass);
        }

        public void Register(Type componentClass)
        {
            _container.Register(componentClass);
        }

        public void Register(Type componentClass, string componentName)
        {
            _container.Register(componentClass, componentName);
        }

        public void Register(object component)
        {
            _container.Register(component);
        }

        public void Register(object component, string componentName)
        {
            _container.Register(component, componentName);
        }

        public void Register(IComponentDef componentDef)
        {
            _container.Register(componentDef);
        }

        public void Include(string path)
        {
            S2ContainerFactory.Include(Container, S2TestUtils.ConvertPath(GetType(), path));
        }
#else
#region NET2.0
        private IDataSource _dataSource;
        private IDbConnection _connection;

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
        }

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
        }

        public bool HasConnection
        {
            get { return _connection != null; }
        }

        public virtual ICommandFactory CommandFactory
        {
            get
            {
                if (_commandFactory == null && Container.HasComponentDef(typeof(ICommandFactory)))
                {
                    _commandFactory = Container.GetComponent(typeof(ICommandFactory)) as ICommandFactory;
                }
                if (_commandFactory == null)
                {
                    _commandFactory = BasicCommandFactory.INSTANCE;
                }
                return _commandFactory;
            }
        }

        internal void SetConnection(IDbConnection connection)
        {
            _connection = connection;
        }

        internal void SetDataSource(IDataSource dataSource)
        {
            _dataSource = dataSource;
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
            IDataWriter writer = SqlWriterFactory.GetSqlWriter(DataSource, CommandFactory);
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
#endregion
#endif
    }
}
