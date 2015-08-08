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

using System.Collections;
using System.Data;
using Seasar.Framework.Util;
#if NET_4_0
using Seasar.Unit;
#endif

namespace Seasar.Extension.Unit
{
    public class DictionaryReader : DataSets.IDataReader
    {
        private readonly DataSet _dataSet;
        private readonly DataTable _table;

        public DictionaryReader()
            : this(null)
        {
        }

        public DictionaryReader(IDictionary dictionary)
        {
            _dataSet = new DataSet();
            _table = _dataSet.Tables.Add("Map");

            if (dictionary != null)
            {
                SetupColumns(dictionary);
                SetupRow(dictionary);
            }
        }

        protected virtual void SetupColumns(IDictionary dictionary)
        {
            foreach (string key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value != null)
                {
                    var type = PropertyUtil.GetPrimitiveType(value.GetExType());
                    _table.Columns.Add(key, type);
                }
                else
                {
                    _table.Columns.Add(key);
                }
            }
        }

        protected virtual void SetupRow(IDictionary dictionary)
        {
            var row = _table.NewRow();
            foreach (DataColumn column in _table.Columns)
            {
                var value = dictionary[column.ColumnName];
                row[column.ColumnName] = PropertyUtil.GetPrimitiveValue(value);
            }
            _table.Rows.Add(row);
            row.AcceptChanges();
        }

        #region IDataReader �����o

        public DataSet Read() => _dataSet;

        #endregion
    }
}
