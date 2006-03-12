using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Extension.DataSets.Impl
{
	public class DataRowReloadResultSetHandler : IDataReaderHandler
	{
		private DataRow row_;

		private DataRow newRow_;

		public DataRowReloadResultSetHandler(DataRow row, DataRow newRow)
		{
			row_ = row;
			newRow_ = newRow;
		}

		public DataRow Row 
		{
			get { return row_; }
		}

		public DataRow NewRow 
		{
			get { return newRow_; }
		}

		#region IDataReaderHandler ÉÅÉìÉo

		public object Handle(System.Data.IDataReader dataReader)
		{
			if (dataReader.Read()) 
			{
				Reload(dataReader);
			}
			return newRow_;
		}

		#endregion

		private void Reload(System.Data.IDataReader dataReader) 
		{
			int count = newRow_.Table.Columns.Count;
			for (int i = 0; i < count; ++i) 
			{
				string columnName = newRow_.Table.Columns[i].ColumnName;
				object value = dataReader[columnName];
				newRow_[i] = value;
			}
		}
	}
}
