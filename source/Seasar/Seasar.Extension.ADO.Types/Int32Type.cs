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
    public class Int32Type :BaseValueType, IValueType
    {
        public Int32Type(IDataSource dataSource)
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
            BindValue(cmd, columnName, value, DbType.Int32);
        }

        #endregion

        protected override object GetValue(object value, Type type)
        {
            if(typeof(int).Equals(type))
            {
                return GetPrimitiveValue(value);
            }
            else if(typeof(SqlInt32).Equals(type))
            {
                return GetSqlInt32Value(value);
            }
            else if(typeof(NullableInt32).Equals(type))
            {
                return GetNullableInt32Value(value);
            }
            else
            {
                return value;
            }
        }

        private int GetPrimitiveValue(object value)
        {
            return Convert.ToInt32(value);
        }

        private SqlInt32 GetSqlInt32Value(object value)
        {
            if(value == DBNull.Value)
            {
                return SqlInt32.Null;
            }
            else if(value is int)
            {
                return new SqlInt32((int) value);
            }
            else
            {
                return SqlInt32.Parse(value.ToString());
            }
        }

        private NullableInt32 GetNullableInt32Value(object value)
        {
            if(value == DBNull.Value)
            {
                return null;
            }
            else if(value is int)
            {
                return new NullableInt32((int) value);
            }
            else
            {
                return NullableInt32.Parse(value.ToString());
            }
        }
    }
}
