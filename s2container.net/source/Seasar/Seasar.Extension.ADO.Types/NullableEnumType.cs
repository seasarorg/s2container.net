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

namespace Seasar.Extension.ADO.Types
{
    public class NullableEnumType : NullableBaseType
    {
        private Type enumType;
        private Type underlyingType;
        private IValueType underlyingValueType;

        public NullableEnumType(Type enumType)
        {
            this.enumType = enumType;
            this.underlyingType = Enum.GetUnderlyingType(this.enumType);
            this.underlyingValueType = ValueTypes.GetValueType(this.underlyingType);
        }

        public override object GetValue(IDataReader reader, int index)
        {
            return GetValue(this.underlyingValueType.GetValue(reader, index));
        }

        public override object GetValue(IDataReader reader, string columnName)
        {
            return GetValue(this.underlyingValueType.GetValue(reader, columnName));
        }

        public override void BindValue(
            IDbCommand cmd, string columnName, object value)
        {
            object convertedValue = null;

            if (value != null)
            {
                convertedValue = Convert.ChangeType(value, this.underlyingType);
            }

            this.underlyingValueType.BindValue(cmd, columnName, convertedValue);
        }

        protected override object GetBindValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            return Convert.ChangeType(value, this.underlyingType);
        }

        protected override object GetValue(object value)
        {
            if (value == DBNull.Value)
            {
                return null;
            }
            else if (value == null)
            {
                return null;
            }
            else
            {
                return Enum.ToObject(this.enumType, value);
            }
        }
    }
}
