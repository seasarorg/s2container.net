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
using System.Reflection;
using Seasar.Extension.ADO.Types;

namespace Seasar.Extension.ADO.Impl
{
    public class PropertyTypeImpl : IPropertyType
    {
        public PropertyTypeImpl(PropertyInfo propertyInfo)
            : this(propertyInfo, ValueTypes.OBJECT, propertyInfo.Name)
        {
        }

        public PropertyTypeImpl(PropertyInfo propertyInfo, IValueType valueType)
            : this(propertyInfo, valueType, propertyInfo.Name)
        {
        }

        public PropertyTypeImpl(PropertyInfo propertyInfo, IValueType valueType, string columnName)
        {
            PropertyInfo = propertyInfo;
            PropertyName = propertyInfo.Name;
            ValueType = valueType;
            ColumnName = columnName;
            PropertyType = propertyInfo.PropertyType;
        }

        public PropertyTypeImpl(string propertyName, IValueType valueType, Type propertyType)
        {
            PropertyName = propertyName;
            ValueType = valueType;
            ColumnName = propertyName;
            PropertyType = propertyType;
        }

        #region IPropertyType ÉÅÉìÉo

        public PropertyInfo PropertyInfo { get; }

        public IValueType ValueType { get; }

        public string PropertyName { get; }

        public string ColumnName { get; set; }

        public bool IsPrimaryKey { get; set; } = false;

        public bool IsPersistent { get; set; } = true;

        public Type PropertyType { get; }

        #endregion
    }
}
