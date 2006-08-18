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
using System.Collections;
using System.Data;
using Seasar.Framework.Util;

namespace Seasar.Extension.Unit
{
    public class DictionaryReader : Seasar.Extension.DataSets.IDataReader
    {
        private DataSet dataSet_;

        private DataTable table_;

        public DictionaryReader()
            : this(null)
        {
        }

        public DictionaryReader(IDictionary dictionary)
        {
            dataSet_ = new DataSet();
            table_ = dataSet_.Tables.Add("Map");

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
                object value = dictionary[key];
                if (value != null)
                {
                    Type type = PropertyUtil.GetPrimitiveType(value.GetType());
                    table_.Columns.Add(key, type);
                }
                else
                {
                    table_.Columns.Add(key);
                }
            }
        }

        protected virtual void SetupRow(IDictionary dictionary)
        {
            DataRow row = table_.NewRow();
            foreach (DataColumn column in table_.Columns)
            {
                object value = dictionary[column.ColumnName];
                row[column.ColumnName] = PropertyUtil.GetPrimitiveValue(value);
            }
            table_.Rows.Add(row);
            row.AcceptChanges();
        }

        #region IDataReader ÉÅÉìÉo

        public DataSet Read()
        {
            return dataSet_;
        }

        #endregion
    }
}
