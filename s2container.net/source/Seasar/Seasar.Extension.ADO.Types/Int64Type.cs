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
    public class Int64Type :BaseValueType, IValueType
    {
        public Int64Type(IDataSource dataSource)
            : base(dataSource)
        {
        }

        #region IValueType ÉÅÉìÉo

        public object GetValue(System.Data.IDataReader reader, int index, Type type)
        {
            return GetValue(reader[index], type);
        }

        public object GetValue(System.Data.IDataReader reader, string columnName, Type type)
        {
            return GetValue(reader[columnName], type);
        }

        public void BindValue(System.Data.IDbCommand cmd, string columnName, object value)
        {
            BindValue(cmd, columnName, value, DbType.Int64);
        }

        #endregion

        protected override object GetValue(object value, Type type)
        {
            if(typeof(long).Equals(type))
            {
                return GetPrimitiveValue(value);
            }
            else if(typeof(SqlInt64).Equals(type))
            {
                return GetSqlInt64Value(value);
            }
            else if(typeof(NullableInt64).Equals(type))
            {
                return GetNullableInt64Value(value);
            }
            else
            {
                return value;
            }
        }

        private long GetPrimitiveValue(object value)
        {
            return Convert.ToInt64(value);
        }

        private SqlInt64 GetSqlInt64Value(object value)
        {
            if(value == DBNull.Value)
            {
                return SqlInt64.Null;
            }
            else if(value is long)
            {
                return new SqlInt64((long) value);
            }
            else
            {
                return SqlInt64.Parse(value.ToString());
            }
        }

        private NullableInt64 GetNullableInt64Value(object value)
        {
            if(value == DBNull.Value)
            {
                return null;
            }
            else if(value is long)
            {
                return new NullableInt64((long) value);
            }
            else
            {
                return NullableInt64.Parse(value.ToString());
            }
        }
    }
}
