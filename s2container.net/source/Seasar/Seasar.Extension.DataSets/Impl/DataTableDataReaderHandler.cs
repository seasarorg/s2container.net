#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
    public class DataTableDataReaderHandler : IDataReaderHandler
    {
        private readonly string _tableName;

        public DataTableDataReaderHandler(string tableName)
        {
            _tableName = tableName;
        }

        #region IDataReaderHandler ÉÅÉìÉo

        public object Handle(System.Data.IDataReader reader)
        {
            var propertyTypes = PropertyTypeUtil.CreatePropertyTypes(reader.GetSchemaTable());
            var table = new DataTable(_tableName);
            for (var i = 0; i < propertyTypes.Length; ++i)
            {
                var propertyName = propertyTypes[i].PropertyName;
                var type = ColumnTypes.GetColumnType(propertyTypes[i].PropertyType).GetColumnType();
                table.Columns.Add(propertyName, type);
            }
            while (reader.Read())
            {
                _AddRow(reader, propertyTypes, table);
            }
            return table;
        }

        #endregion

        private void _AddRow(System.Data.IDataReader reader, IPropertyType[] propertyTypes, DataTable table)
        {
            var row = table.NewRow();
            for (var i = 0; i < table.Columns.Count; ++i)
            {
                row[i] = _GetValue(reader, i, propertyTypes);
            }
            table.Rows.Add(row);
        }

        private object _GetValue(System.Data.IDataReader reader, int index, IPropertyType[] propertyTypes)
        {
            var type = propertyTypes[index].PropertyType;
            var value = propertyTypes[index].ValueType.GetValue(reader, index);
            if (value == null)
            {
                return DBNull.Value;
            }
            var ret = ColumnTypes.GetColumnType(type).Convert(value, null);
            if (ret is string)
            {
                var s = ret as string;
                s = s.TrimEnd(null);
                if (_IsCellBase64Formatted(s))
                {
                    return Convert.FromBase64String(s);
                }
                return s;
            }
            return ret;
        }

        private bool _IsCellBase64Formatted(string s)
        {
            return DataSetConstants.BASE64_FORMAT.StartsWith(s);
        }
    }
}
