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
using Seasar.Framework.Util;
using IDataReader = Seasar.Extension.DataSets.IDataReader;

namespace Seasar.Extension.Unit
{
    public class BeanReader : IDataReader
    {
        private readonly DataSet _dataSet;
        private readonly DataTable _table;

        protected BeanReader()
            : this(null)
        {
        }

        public BeanReader(object bean)
        {
            _dataSet = new DataSet();
            _table = _dataSet.Tables.Add("Bean");

            if (bean != null)
            {
                var beanType = bean.GetExType();
                SetupColumns(beanType);
                SetupRow(beanType, bean);
            }
        }

        protected virtual void SetupColumns(Type beanType)
        {
            foreach (var pi in beanType.GetProperties())
            {
                var propertyType = PropertyUtil.GetPrimitiveType(pi.PropertyType);
                _table.Columns.Add(pi.Name, propertyType);
            }
        }

        protected virtual void SetupRow(Type beanType, object bean)
        {
            var row = _table.NewRow();
            foreach (var pi in beanType.GetProperties())
            {
                var value = pi.GetValue(bean, null);
                row[pi.Name] = PropertyUtil.GetPrimitiveValue(value);
            }
            _table.Rows.Add(row);
            row.AcceptChanges();
        }

        #region IDataReader ƒƒ“ƒo

        public DataSet Read() => _dataSet;

        #endregion
    }
}
