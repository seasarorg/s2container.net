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
    public class DoubleType :BaseValueType, IValueType
    {
        public DoubleType(IDataSource dataSource)
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
            BindValue(cmd, columnName, value, DbType.Double);
        }

        #endregion

        protected override object GetValue(object value, Type type)
        {
            if(typeof(double).Equals(type))
            {
                return GetPrimitiveValue(value);
            }
            else if(typeof(SqlDouble).Equals(type))
            {
                return GetSqlDoubleValue(value);
            }
            else if(typeof(NullableDouble).Equals(type))
            {
                return GetNullableDouble(value);
            }
            else
            {
                return value;
            }
        }

        private double GetPrimitiveValue(object value)
        {
            return Convert.ToDouble(value);
        }

        private SqlDouble GetSqlDoubleValue(object value)
        {
            if(value == DBNull.Value)
            {
                return SqlDouble.Null;
            }
            else if(value is double)
            {
                return new SqlDouble((double) value);
            }
            else
            {
                return SqlDouble.Parse(value.ToString());
            }
        }

        private NullableDouble GetNullableDouble(object value)
        {
            if(value == DBNull.Value)
            {
                return null;
            }
            else if(value is double)
            {
                return new NullableDouble((double) value);
            }
            else
            {
                return NullableDouble.Parse(value.ToString());
            }
        }
    }
}
