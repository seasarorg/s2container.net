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
using Seasar.Framework.Util;

namespace Seasar.Extension.DataSets.Impl
{
	public class DataTableDataReaderHandler : IDataReaderHandler
	{
		private string tableName_;

		public DataTableDataReaderHandler(string tableName)
		{
			tableName_ = tableName;
		}

		#region IDataReaderHandler ÉÅÉìÉo

		public object Handle(System.Data.IDataReader reader)
		{
			IPropertyType[] propertyTypes = PropertyTypeUtil.CreatePropertyTypes(reader.GetSchemaTable());
			DataTable table = new DataTable(tableName_);
			for (int i = 0; i < propertyTypes.Length; ++i)
			{
				String propertyName = propertyTypes[i].PropertyName;
				Type type = ColumnTypes.GetColumnType(propertyTypes[i].PropertyType).GetColumnType();
				table.Columns.Add(propertyName, type);
			}
			while (reader.Read()) 
			{
				AddRow(reader, propertyTypes, table);
			}
			return table;
		}

		#endregion

		private void AddRow(System.Data.IDataReader reader, IPropertyType[] propertyTypes, DataTable table) 
		{
			DataRow row = table.NewRow();
			for (int i = 0; i < table.Columns.Count; ++i) 
			{
				row[i] = GetValue(reader, i, propertyTypes);
			}
			table.Rows.Add(row);
		}

		private object GetValue(System.Data.IDataReader reader, int index, IPropertyType[] propertyTypes)
		{
			Type type = propertyTypes[index].PropertyType;
			object value = propertyTypes[index].ValueType.GetValue(reader, index, type);
			if (value == null)
			{
				return DBNull.Value;
			}
			object ret = ColumnTypes.GetColumnType(type).Convert(value, null);
			if (ret is string)
			{
				string s = ret as string;
				if (s != null)
				{
					s = s.TrimEnd(null);
				}
				if (IsCellBase64Formatted(s))
				{
					return Convert.FromBase64String(s);
				}
				return s;
			}
			return ret;
		}

		private bool IsCellBase64Formatted(string s)
		{
			return DataSetConstants.BASE64_FORMAT.StartsWith(s);
		}
	}
}
