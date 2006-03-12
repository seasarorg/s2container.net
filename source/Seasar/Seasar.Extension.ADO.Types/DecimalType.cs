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
    public class DecimalType :BaseValueType, IValueType
    {
        public DecimalType(IDataSource dataSource)
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
            BindValue(cmd, columnName, value, DbType.Decimal);
        }

        #endregion

        protected override object GetValue(object value, Type type)
        {
            if(typeof(decimal).Equals(type))
            {
                return GetPrimitiveValue(value);
            }
            else if(typeof(SqlDecimal).Equals(type))
            {
                return GetSqlDecimalValue(value);
            }
            else if(typeof(NullableDecimal).Equals(type))
            {
                return GetNullableDecimalValue(value);
            }
            else
            {
                return value;
            }
        }

        private decimal GetPrimitiveValue(object value)
        {
            return Convert.ToDecimal(value);
        }

        private SqlDecimal GetSqlDecimalValue(object value)
        {
            if(value == DBNull.Value)
            {
                return SqlDecimal.Null;
            }
            else if(value is decimal)
            {
                return new SqlDecimal((decimal) value);
            }
            else if(value is int)
            {
                return new SqlDecimal((int) value);
            }
            else if(value is long)
            {
                return new SqlDecimal((long) value);
            }
            else if(value is double)
            {
                return new SqlDecimal((double) value);
            }
            else
            {
                return SqlDecimal.Parse(value.ToString());
            }
        }

        private NullableDecimal GetNullableDecimalValue(object value)
        {
            if(value == DBNull.Value)
            {
                return null;
            }
            else if(value is decimal)
            {
                return new NullableDecimal((decimal) value);
            }
            else
            {
                return NullableDecimal.Parse(value.ToString());
            }
        }
    }
}
