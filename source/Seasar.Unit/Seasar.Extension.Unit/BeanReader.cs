#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using System.Reflection;
using Seasar.Framework.Util;

namespace Seasar.Extension.Unit
{
    public class BeanReader : Seasar.Extension.DataSets.IDataReader
    {
        private DataSet dataSet_;

        private DataTable table_;

        protected BeanReader()
            : this(null)
        {
        }

        public BeanReader(object bean)
        {
            dataSet_ = new DataSet();
            table_ = dataSet_.Tables.Add("Bean");

            if (bean != null)
            {
                Type beanType = bean.GetType();
                SetupColumns(beanType);
                SetupRow(beanType, bean);
            }
        }

        protected virtual void SetupColumns(Type beanType)
        {
            foreach (PropertyInfo pi in beanType.GetProperties())
            {
                Type propertyType = PropertyUtil.GetPrimitiveType(pi.PropertyType);
                table_.Columns.Add(pi.Name, propertyType);
            }
        }

        protected virtual void SetupRow(Type beanType, object bean)
        {
            DataRow row = table_.NewRow();
            foreach (PropertyInfo pi in beanType.GetProperties())
            {
                object value = pi.GetValue(bean, null);
                row[pi.Name] = PropertyUtil.GetPrimitiveValue(value);
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
