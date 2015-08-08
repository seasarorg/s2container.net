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
using System.Reflection;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    public abstract class AbstractBeanDataReaderHandler : IDataReaderHandler
    {
        private Type _beanType;

        protected AbstractBeanDataReaderHandler(Type beanType)
        {
            SetBeanType(beanType);
        }

        public Type BeanType => _beanType;

        public void SetBeanType(Type beanType)
        {
            _beanType = beanType;
        }

        protected IPropertyType[] CreatePropertyTypes(DataTable metaData)
        {
            var count = metaData.Rows.Count;
            IPropertyType[] propertyTypes = new PropertyTypeImpl[count];
            for (var i = 0; i < count; ++i)
            {
                var row = metaData.Rows[i];
                var columnName = (string) row["ColumnName"];
                var propertyName = columnName.Replace("_", string.Empty);
                var pi = _beanType.GetProperty(
                    propertyName,
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase
                    );
                var valueType = ValueTypes.GetValueType(pi.PropertyType);
                propertyTypes[i] = new PropertyTypeImpl(pi, valueType, columnName);
            }
            return propertyTypes;
        }

        protected object CreateRow(IDataReader dataReader, IPropertyType[] propertyTypes)
        {
            var row = ClassUtil.NewInstance(_beanType);
            for (var i = 0; i < propertyTypes.Length; ++i)
            {
                var pt = propertyTypes[i];
                var valueType = pt.ValueType;
                var value = valueType.GetValue(dataReader, pt.ColumnName);
                var pi = pt.PropertyInfo;
//                pi.SetValue(row, value, null);
                PropertyUtil.SetValue(row, row.GetExType(), pi.Name, pi.PropertyType, value);
            }
            return row;
        }

        #region IDataReaderHandler ƒƒ“ƒo

        public abstract object Handle(IDataReader dataReader);

        #endregion
    }
}
