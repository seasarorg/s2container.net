#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
        private readonly PropertyInfo _propertyInfo;
        private readonly string _propertyName;
        private string _columnName;
        private readonly IValueType _valueType;
        private bool _primaryKey = false;
        private bool _persistent = true;
        private readonly Type _propertyType;

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
            _propertyInfo = propertyInfo;
            _propertyName = propertyInfo.Name;
            _valueType = valueType;
            _columnName = columnName;
            _propertyType = propertyInfo.PropertyType;
        }

        public PropertyTypeImpl(string propertyName, IValueType valueType, Type propertyType)
        {
            _propertyName = propertyName;
            _valueType = valueType;
            _columnName = propertyName;
            _propertyType = propertyType;
        }

        #region IPropertyType ÉÅÉìÉo

        public PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
        }

        public IValueType ValueType
        {
            get { return _valueType; }
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public bool IsPrimaryKey
        {
            get { return _primaryKey; }
            set { _primaryKey = value; }
        }

        public bool IsPersistent
        {
            get { return _persistent; }
            set { _persistent = value; }
        }

        public Type PropertyType
        {
            get { return _propertyType; }
        }

        #endregion
    }
}
