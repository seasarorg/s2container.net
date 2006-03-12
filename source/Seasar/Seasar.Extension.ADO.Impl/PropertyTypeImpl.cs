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
using System.Reflection;
using Seasar.Extension.ADO.Types;

namespace Seasar.Extension.ADO.Impl
{
    public class PropertyTypeImpl : IPropertyType
    {
        private PropertyInfo propertyInfo;
        private string propertyName;
        private string columnName;
        private IValueType valueType;
        private bool primaryKey = false;
        private bool persistent = true;

        public PropertyTypeImpl(PropertyInfo propertyInfo)
            : this(propertyInfo, ValueTypes.OBJECT, propertyInfo.Name)
        {
        }

        public PropertyTypeImpl(PropertyInfo propertyInfo, IValueType valueType)
            : this(propertyInfo, valueType, propertyInfo.Name)
        {
        }

        public PropertyTypeImpl(PropertyInfo propertyInfo, IValueType valueType,
            string columnName)
        {
            this.propertyInfo = propertyInfo;
            this.propertyName = propertyInfo.Name;
            this.valueType  = valueType;
            this.columnName = columnName;
        }

        public PropertyTypeImpl(string propertyName, IValueType valueType)
        {
            this.propertyName = propertyName;
            this.valueType = valueType;
            this.columnName = propertyName;
        }

        #region IPropertyType ÉÅÉìÉo

        public System.Reflection.PropertyInfo PropertyInfo
        {
            get
            {
                return propertyInfo;
            }
        }

        public IValueType ValueType
        {
            get
            {
                return valueType;
            }
        }

        public string PropertyName
        {
            get
            {
                return propertyName;
            }
        }

        public string ColumnName
        {
            get
            {
                return columnName;
            }
            set
            {
                columnName = value;
            }
        }

        public bool IsPrimaryKey
        {
            get
            {
                return primaryKey;
            }
            set
            {
                primaryKey = value;
            }
        }

        public bool IsPersistent
        {
            get
            {
                return persistent;
            }
            set
            {
                persistent = value;
            }
        }

        #endregion
    }
}
