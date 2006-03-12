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
	/// <summary>
	/// ByteType
	/// </summary>
	public class ByteType : BaseValueType, IValueType
	{
		public ByteType(IDataSource dataSource)
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
            BindValue(cmd, columnName, value, DbType.Byte);
        }

        #endregion

        protected override object GetValue(object value, Type type)
        {
            if(typeof(byte).Equals(type))
            {
                return GetPrimitiveValue(value);
            }
            else if(typeof(SqlByte).Equals(type))
            {
                return GetSqlByteValue(value);
            }
            else if(typeof(NullableByte).Equals(type))
            {
                return GetNullableByteValue(value);
            }
            else
            {
                return value;
            }
        }

        private byte GetPrimitiveValue(object value)
        {
            return Convert.ToByte(value);
        }

        private SqlByte GetSqlByteValue(object value)
        {
            if(value == DBNull.Value)
            {
                return SqlByte.Null;
            }
            if(value is byte)
            {
                return new SqlByte((byte) value);
            }
            else
            {
                return SqlByte.Parse(value.ToString());
            }
        }

        private NullableByte GetNullableByteValue(object value)
        {
            if(value == DBNull.Value)
            {
                return null;
            }
            if(value is byte)
            {
                return new NullableByte((byte) value);
            }
            else
            {
                return NullableByte.Parse(value.ToString());
            }
        }
    }
}
