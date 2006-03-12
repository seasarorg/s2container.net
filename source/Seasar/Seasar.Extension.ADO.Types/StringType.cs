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
using System.Data.SqlClient;
using Seasar.Extension.ADO;
using Nullables;

namespace Seasar.Extension.ADO.Types
{
    public class StringType : BaseValueType, IValueType
    {

        public StringType(IDataSource dataSource)
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
            BindValue(cmd, columnName, value, DbType.String);
        }

        #endregion

        protected override object GetValue(object value, Type type)
        {
            if(typeof(string).Equals(type))
            {
                return GetStringValue(value);
            }
            else if(typeof(SqlString).Equals(type))
            {
                return GetSqlStringValue(value);
            }
            else if(typeof(NullableChar).Equals(type))
            {
                return GetNullableCharValue(value);
            }
            else
            {
                return value;
            }
        }

        private string GetStringValue(object value)
        {
            if(value == DBNull.Value)
            {
                return null;
            }
            else if(value is string)
            {
                return (string) value;
            }
            else
            {
                return value.ToString();
            }
        }

        private SqlString GetSqlStringValue(object value)
        {
            if( value == DBNull.Value)
            {
                return SqlString.Null;
            }
            else if(value is string)
            {
                return new SqlString((string) value);
            }
            else
            {
                return new SqlString(value.ToString());
            }
        }

        private NullableChar GetNullableCharValue(object value)
        {
            if(value == DBNull.Value)
            {
                return null;
            }
            else if(value is string)
            {
                return new NullableChar(((string) value)[0]);
            }
            else
            {
                return NullableChar.Parse(value.ToString());
            }
        }
    }
}
