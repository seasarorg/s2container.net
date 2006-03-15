using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Types;

namespace Seasar.Extension.DataSets.Impl
{
	public class SqlReloadReader : IDataReader
	{
		private IDataSource dataSource_;

		private DataSet dataSet_;

		public SqlReloadReader(IDataSource dataSource, DataSet dataSet)
		{
			dataSource_ = dataSource;
			dataSet_ = dataSet;
			ValueTypes.Init(dataSource);
		}

		public IDataSource DataSource 
		{
			get { return dataSource_; }
		}

		public DataSet DataSet 
		{
			get { return DataSet; }
		}

		#region IDataReader ƒƒ“ƒo

		public DataSet Read()
		{
			DataSet newDataSet = new DataSet();
			foreach (DataTable table in dataSet_.Tables) 
			{
				ITableReader reader = new SqlReloadTableReader(dataSource_, table);
				newDataSet.Tables.Add(reader.Read());
			}
			return newDataSet;
		}

		#endregion
	}
}
