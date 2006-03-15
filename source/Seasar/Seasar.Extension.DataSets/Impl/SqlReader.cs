using System.Collections;
using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Extension.DataSets.Impl
{
	public class SqlReader : IDataReader
	{
		private IDataSource dataSource_;

		private IList tableReaders_ = new ArrayList();

		public SqlReader(IDataSource dataSource)
		{
			dataSource_ = dataSource;
		}

		public IDataSource DataSource 
		{
			get { return dataSource_; }
		}

		public void AddTable(string tableName) 
		{
			AddTable(tableName, null);
		}

		public void AddTable(string tableName, string condition) 
		{
			SqlTableReader reader = new SqlTableReader(dataSource_);
			reader.SetTable(tableName, condition);
			tableReaders_.Add(reader);
		}

		public void AddSql(string sql, string tableName) 
		{
			SqlTableReader reader = new SqlTableReader(dataSource_);
			reader.SetSql(sql, tableName);
			tableReaders_.Add(reader);
		}

		#region IDataReader ÉÅÉìÉo

		public DataSet Read()
		{
			DataSet dataSet = new DataSet();
			foreach (ITableReader reader in tableReaders_) 
			{
				dataSet.Tables.Add(reader.Read());
			}
			return dataSet;
		}

		#endregion
	}
}
