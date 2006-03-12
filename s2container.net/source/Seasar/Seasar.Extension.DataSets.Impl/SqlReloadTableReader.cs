using System;
using System.Collections;
using System.Data;
using System.Text;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Util;

namespace Seasar.Extension.DataSets.Impl
{
	public class SqlReloadTableReader : ITableReader
	{
		private IDataSource dataSource_;

		private DataTable table_;

		private string sql_;

		private string[] primaryKeys_;

		private string[] argNames_;

		public SqlReloadTableReader(IDataSource dataSource, DataTable table)
		{
			dataSource_ = dataSource;
			table_ = table;

			ValueTypes.Init(dataSource);

			IDbConnection con = DataSourceUtil.GetConnection(dataSource);
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
			foreach (DataColumn column in table_.Columns) 
			{
				buf.Append(column.ColumnName);
				buf.Append(", ");
				if (DataTableUtil.IsPrimaryKey(table_, column)) 
				{
					whereBuf.AppendFormat(" {0} = @{1} AND", column.ColumnName, column.ColumnName);
					primaryKeyList.Add(column.ColumnName);
				}
			}
			buf.Length -= 2;
			whereBuf.Length -= 4;
			buf.Append(" FROM ");
			buf.Append(table_.TableName);
			buf.Append(whereBuf);
			sql_ = buf.ToString();
			primaryKeys_ = (string[]) primaryKeyList.ToArray(typeof(string));
			argNames_ = primaryKeys_;
		}

		public IDataSource DataSource 
		{
			get { return dataSource_; }
		}

		public DataTable Table
		{
			get { return table_; }
		}

		#region ITableReader ÉÅÉìÉo

		public DataTable Read()
		{
			DataTable newTable = table_.Clone();
			foreach (DataRow row in table_.Rows) 
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
				dataSource_,
				sql_,
				new DataRowReloadResultSetHandler(row, newRow)
				);
			object[] args = new object[primaryKeys_.Length];
			for (int i = 0; i < primaryKeys_.Length; ++i) 
			{
				args[i] = row[primaryKeys_[i]];
			}
			selectHandler.Execute(args, Type.GetTypeArray(args), argNames_);
		}
	}
}
