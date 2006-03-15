using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Extension.DataSets.Impl
{
	public class SqlWriter : IDataWriter
	{
		private IDataSource dataSource_;

		public SqlWriter(IDataSource dataSource)
		{
			dataSource_ = dataSource;
		}

		public IDataSource DataSource 
		{
			get { return dataSource_; }
		}

		#region IDataWriter ÉÅÉìÉo

		public void Write(DataSet dataSet)
		{
			ITableWriter writer = new SqlTableWriter(DataSource);
			foreach (DataTable table in dataSet.Tables) 
			{
				writer.Write(table);
			}
		}

		#endregion
	}
}
