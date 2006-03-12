using System;
using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Extension.DataSets.Impl
{
	public class DataTableResultSetHandler : IDataReaderHandler
	{
		private string tableName_;

		public DataTableResultSetHandler(string tableName)
		{
			tableName_ = tableName;
		}

		#region IDataReaderHandler ÉÅÉìÉo

		public object Handle(System.Data.IDataReader dataReader)
		{
			DataTable metaData = dataReader.GetSchemaTable();

			DataTable table = new DataTable(tableName_);
			foreach (DataRow row in metaData.Rows) 
			{
				table.Columns.Add(
					(string) row["ColumnName"],
					(Type) row["DataType"]
					);
			}

			while (dataReader.Read()) 
			{
				AddRow(dataReader, metaData, table);
			}

			return table;
		}

		#endregion

		private void AddRow(System.Data.IDataReader dataReader, DataTable metaData, DataTable table) 
		{
			DataRow row = table.NewRow();
			int count = table.Columns.Count;
			for (int i = 0; i < count; ++i) 
			{
				string columnName = table.Columns[i].ColumnName;
				object value = dataReader[columnName];
				row[i] = value;
			}
			table.Rows.Add(row);
		}
	}
}
