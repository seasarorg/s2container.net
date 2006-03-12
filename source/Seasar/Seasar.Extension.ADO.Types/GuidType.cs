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
	/// GuidType
	/// </summary>
	public class GuidType : BaseValueType, IValueType
	{

		public GuidType(IDataSource dataSource)
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
            BindValue(cmd, columnName, value, DbType.Guid);
        }

        #endregion

        protected override object GetValue(object value, Type type)
        {
            if(typeof(Guid).Equals(type))
            {
                return GetGuidValue(value);
            }
            else if(typeof(SqlGuid).Equals(type))
            {
                return GetSqlGuidValue(value);
            }
            else if(typeof(NullableGuid).Equals(type))
            {
                return GetNullableGuid(value);
            }
            else
            {
                return value;
            }
        }

        private Guid GetGuidValue(object value)
        {
            if(value == DBNull.Value)
            {
                return Guid.Empty;
            }
            else if(value is Guid)
            {
                return (Guid) value;
            }
            else if(value is string)
            {
                return new Guid((string) value);
            }
            else if(value is byte[])
            {
                return new Guid((byte[]) value);
            }
            else
            {
                return new Guid(value.ToString());
            }
        }

        private SqlGuid GetSqlGuidValue(object value)
        {
            if(value == DBNull.Value)
            {
                return SqlGuid.Null;
            }
            else if(value is Guid)
            {
                return new SqlGuid((Guid) value);
            }
            else if(value is string)
            {
                return new SqlGuid((string) value);
            }
            else if(value is byte[])
            {
                return new SqlGuid((byte[]) value);
            }
            else
            {
                return SqlGuid.Parse(value.ToString());
            }
        }

        private NullableGuid GetNullableGuid(object value)
        {
            if(value == DBNull.Value)
            {
                return null;
            }
            else if(value is Guid)
            {
                return new NullableGuid((Guid) value);
            }
            else if(value is string)
            {
                return new NullableGuid((string) value);
            }
            else
            {
                return new NullableGuid(value.ToString());
            }
        }
    }
}
