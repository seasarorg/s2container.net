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
using System.Data;
using System.Data.SqlTypes;
using Seasar.Extension.ADO;
using Nullables;

namespace Seasar.Extension.ADO.Types
{
    public class BooleanType :BaseValueType, IValueType
    {
        public BooleanType(IDataSource dataSource)
            : base(dataSource)
        {
        }

        #region IValueType ÉÅÉìÉo

        public object GetValue(IDataReader reader, int index, Type type)
        {
            return GetValue(reader[index], type);
        }

        public object GetValue(IDataReader reader, string columnName, Type type)
        {
            return GetValue(reader[columnName], type);
        }

        public void BindValue(IDbCommand cmd, string columnName, object value)
        {
            BindValue(cmd, columnName, value, DbType.Boolean);
        }

        #endregion

        protected override object GetValue(object value, Type type)
        {
            if(typeof(bool).Equals(type))
            {
                return GetPrimitiveValue(value);
            }
            else if(typeof(SqlBoolean).Equals(type))
            {
                return GetSqlBooleanValue(value);
            }
            else if(typeof(NullableBoolean).Equals(type))
            {
                return GetNullableBooleanValue(value);
            }
            else
            {
                return value;
            }
        }

        private bool GetPrimitiveValue(object value)
        {
            return Convert.ToBoolean(value);
        }

        private SqlBoolean GetSqlBooleanValue(object value)
        {
            if(value == DBNull.Value)
            {
                return SqlBoolean.Null;
            }
            else if(value is int)
            {
                return new SqlBoolean((int) value);
            }
            else if(value is bool)
            {
                return new SqlBoolean((bool) value);
            }
            else
            {
                return SqlBoolean.Parse(value.ToString());
            }
        }

        private NullableBoolean GetNullableBooleanValue(object value)
        {
            if(value == DBNull.Value)
            {
                return null;
            }
            else if(value is bool)
            {
                return new NullableBoolean((bool) value);
            }
            else
            {
                return NullableBoolean.Parse(value.ToString());
            }

        }
    }
}
