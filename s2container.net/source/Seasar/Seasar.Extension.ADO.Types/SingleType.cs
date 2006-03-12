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
    public class SingleType :BaseValueType, IValueType
    {
        public SingleType(IDataSource dataSource)
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
            BindValue(cmd, columnName, value, DbType.Single);
        }

        #endregion

        protected override object GetValue(object value, Type type)
        {
            if(typeof(float).Equals(type))
            {
                return GetPrimitiveValue(value);
            }
            else if(typeof(SqlSingle).Equals(type))
            {
                return GetSqlSingleValue(value);
            }
            else if(typeof(NullableSingle).Equals(type))
            {
                return GetNullableSingleValue(value);
            }
            else
            {
                return value;
            }
        }

        private float GetPrimitiveValue(object value)
        {
            return Convert.ToSingle(value);
        }

        private SqlSingle GetSqlSingleValue(object value)
        {
            if(value == DBNull.Value)
            {
                return SqlSingle.Null;
            }
            else if(value is double)
            {
                return new SqlSingle((double) value);
            }
            else if(value is float)
            {
                return new SqlSingle((float) value);
            }
            else
            {
                return SqlSingle.Parse(value.ToString());
            }
        }

        private NullableSingle GetNullableSingleValue(object value)
        {
            if(value == DBNull.Value)
            {
                return null;
            }
            else if(value is float)
            {
                return new NullableSingle((float) value);
            }
            else
            {
                return NullableSingle.Parse(value.ToString());
            }
        }
    }
}
