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

using System;
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.DataSets.Types;

namespace Seasar.Extension.DataSets.Impl
{
	public class DataRowDataReaderHandler : IDataReaderHandler
	{
		private DataRow row_;
		private DataRow newRow_;

		public DataRowDataReaderHandler(DataRow row, DataRow newRow)
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

		public object Handle(System.Data.IDataReader reader)
		{
			IPropertyType[] propertyTypes = PropertyTypeUtil.CreatePropertyTypes(reader.GetSchemaTable());
			if (reader.Read()) 
			{
				Reload(reader, propertyTypes);
			}
			return newRow_;
		}

		#endregion

		private void Reload(System.Data.IDataReader reader, IPropertyType[] propertyTypes) 
		{
			for (int i = 0; i < propertyTypes.Length; ++i)
			{
				object value = propertyTypes[i].ValueType.GetValue(reader, i, propertyTypes[i].PropertyType);
				if (value == null)
				{
					newRow_[i] = DBNull.Value;
				}
				else
				{
					newRow_[i] = ColumnTypes.GetColumnType(propertyTypes[i].PropertyType).Convert(value, null);
				}
			}
		}
	}
}
