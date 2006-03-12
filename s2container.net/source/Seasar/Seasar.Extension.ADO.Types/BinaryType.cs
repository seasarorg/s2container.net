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
    public class BinaryType : BaseValueType, IValueType
    {
        public BinaryType(IDataSource dataSource)
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
            BindValue(cmd, columnName, value, DbType.Binary);
        }

        #endregion

        protected override object GetValue(object value, Type type)
        {
            if(ValueTypes.BYTE_ARRAY_TYPE.Equals(type))
            {
                return GetPrimitiveValue(value);
            }
            else if(typeof(SqlBinary).Equals(type))
            {
                return GetSqlBinaryValue(value);
            }
            else if(ValueTypes.NULLABLE_BYTE_ARRAY_TYPE.Equals(type))
            {
                return GetNullableByteArrayValue(value);
            }
            else
            {
                return value;
            }
        }

        private byte[] GetPrimitiveValue(object value)
        {
            if(value == DBNull.Value)
            {
                return null;
            }
            else
            {
                return (byte[]) value;
            }
        }

        private SqlBinary GetSqlBinaryValue(object value)
        {
            if(value == DBNull.Value)
            {
                return SqlBinary.Null;
            }
            else
            {
                return new SqlBinary((byte[]) value);
            }
        }

        private NullableByte[] GetNullableByteArrayValue(object value)
        {
            if(value == DBNull.Value)
            {
                return null;
            }
            else
            {
                byte[] bytes = (byte[]) value;
                NullableByte[] nBytes = new NullableByte[bytes.Length];
                for(int i = 0; i < bytes.Length; ++i)
                {
                    nBytes[i] = new NullableByte(bytes[i]);
                }
                return nBytes;
            }
        }
    }
}
