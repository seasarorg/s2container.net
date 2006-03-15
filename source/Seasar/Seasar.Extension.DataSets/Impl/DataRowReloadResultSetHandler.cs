#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
