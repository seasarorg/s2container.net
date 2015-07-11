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
    public class DataRowReloadDataReaderHandler : IDataReaderHandler
    {
        private readonly DataRow _row;
        private readonly DataRow _newRow;

        public DataRowReloadDataReaderHandler(DataRow row, DataRow newRow)
        {
            _row = row;
            _newRow = newRow;
        }

        public DataRow Row
        {
            get { return _row; }
        }

        public DataRow NewRow
        {
            get { return _newRow; }
        }

        #region IDataReaderHandler ÉÅÉìÉo

        public object Handle(System.Data.IDataReader reader)
        {
            IPropertyType[] propertyTypes = PropertyTypeUtil.CreatePropertyTypes(reader.GetSchemaTable());
            if (reader.Read())
            {
                Reload(reader, propertyTypes);
            }
            return _newRow;
        }

        #endregion

        private void Reload(System.Data.IDataReader reader, IPropertyType[] propertyTypes)
        {
            for (int i = 0; i < propertyTypes.Length; ++i)
            {
                object value = propertyTypes[i].ValueType.GetValue(reader, i);
                if (value == null)
                {
                    _newRow[i] = DBNull.Value;
                }
                else
                {
                    _newRow[i] = ColumnTypes.GetColumnType(propertyTypes[i].PropertyType).Convert(value, null);
                }
            }
        }
    }
}
