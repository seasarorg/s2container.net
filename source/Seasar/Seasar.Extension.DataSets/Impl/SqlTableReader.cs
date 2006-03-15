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
			ValueTypes.Init(dataSource);
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

		public void SetTable(string tableName) 
		{
			SetTable(tableName, null);
		}
			
		public void SetTable(string tableName, String condition) 
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

		public void SetSql(string sql, string tableName) 
		{
			sql_ = sql;
			tableName_ = tableName;
		}

		#region ITableReader ÉÅÉìÉo

		public DataTable Read()
		{
			ISelectHandler selectHandler = new BasicSelectHandler(
				dataSource_,
				sql_,
				new DataTableResultSetHandler(tableName_)
				);
			DataTable table = (DataTable) selectHandler.Execute(null);
			table.AcceptChanges();
			return table;
		}

		#endregion
	}
}
