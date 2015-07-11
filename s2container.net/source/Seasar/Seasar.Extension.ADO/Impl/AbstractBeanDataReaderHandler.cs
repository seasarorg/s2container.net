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

        public AbstractBeanDataReaderHandler(Type beanType)
        {
            SetBeanType(beanType);
        }

        public Type BeanType
        {
            get { return _beanType; }
        }

        public void SetBeanType(Type beanType)
        {
            _beanType = beanType;
        }

        protected IPropertyType[] CreatePropertyTypes(DataTable metaData)
        {
            int count = metaData.Rows.Count;
            IPropertyType[] propertyTypes = new PropertyTypeImpl[count];
            for (int i = 0; i < count; ++i)
            {
                DataRow row = metaData.Rows[i];
                string columnName = (string) row["ColumnName"];
                string propertyName = columnName.Replace("_", string.Empty);
                PropertyInfo pi = _beanType.GetProperty(
                    propertyName,
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase
                    );
                IValueType valueType = ValueTypes.GetValueType(pi.PropertyType);
                propertyTypes[i] = new PropertyTypeImpl(pi, valueType, columnName);
            }
            return propertyTypes;
        }

        protected object CreateRow(IDataReader dataReader, IPropertyType[] propertyTypes)
        {
            object row = ClassUtil.NewInstance(_beanType);
            for (int i = 0; i < propertyTypes.Length; ++i)
            {
                IPropertyType pt = propertyTypes[i];
                IValueType valueType = pt.ValueType;
                object value = valueType.GetValue(dataReader, pt.ColumnName);
                PropertyInfo pi = pt.PropertyInfo;
                pi.SetValue(row, value, null);
            }
            return row;
        }

        #region IDataReaderHandler ƒƒ“ƒo

        public abstract object Handle(IDataReader dataReader);

        #endregion
    }
}
