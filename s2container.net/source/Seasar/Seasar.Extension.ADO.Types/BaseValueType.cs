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
using System.Data.OleDb;
using System.Data;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Types
{
	public abstract class BaseValueType : IValueType
    {
        public BaseValueType()
        {
        }

		#region IValueType ÉÅÉìÉo

		public virtual object GetValue(IDataReader reader, int index, Type type)
		{
			return GetValue(reader[index]);
		}

		public virtual object GetValue(IDataReader reader, string columnName, Type type)
		{
			return GetValue(reader[columnName]);
		}

		public virtual object GetValue(IDataReader reader, int index)
		{
			return GetValue(reader[index]);
		}

		public virtual object GetValue(IDataReader reader, string columnName)
		{
			return GetValue(reader[columnName]);
		}

		public abstract void BindValue(IDbCommand cmd, string columnName, object value);

		#endregion

		public void BindValue(IDbCommand cmd, string columnName, object value, DbType dbType)
        {
			BindVariableType vt = DataProviderUtil.GetBindVariableType(cmd);
            switch(vt)
            {
                case BindVariableType.QuestionWithParam:
                    columnName = "?" + columnName;
                    break;
                case BindVariableType.ColonWithParam:
                    columnName = ":" + columnName;
                    break;
				case BindVariableType.ColonWithParamToLower:
					columnName = ":" + columnName.ToLower();
					break;
                default:
                    columnName = "@" + columnName;
                    break;
            }

			IDbDataParameter parameter = cmd.CreateParameter();
			parameter.ParameterName = columnName;
			parameter.DbType = dbType;
			if("OleDbCommand".Equals(cmd.GetType().Name) && dbType == DbType.String)
			{
				OleDbParameter oleDbParam = parameter as OleDbParameter;
				oleDbParam.OleDbType = OleDbType.VarChar;
			}
			parameter.Value = GetBindValue(value);
			cmd.Parameters.Add(parameter);
        }

        protected object GetBindValue(object value)
        {
            if(value == null)
            {
                return DBNull.Value;
            }
            else if(value.GetType().IsPrimitive)
            {
                return value;
            }
            else if(value is System.Data.SqlTypes.INullable)
            {
                System.Data.SqlTypes.INullable nValue = (System.Data.SqlTypes.INullable) value;
                if(nValue.IsNull)
                {
                    return DBNull.Value;
                }
                else
                {
                    System.Reflection.PropertyInfo pi = value.GetType().GetProperty("Value");
                    return pi.GetValue(value, null);
                }
            }
            else if(value is Nullables.INullableType)
            {
                Nullables.INullableType nValue = value as Nullables.INullableType;
                if(nValue == null || !nValue.HasValue)
                {
                    return DBNull.Value;
                }
                else
                {
                    return nValue.Value;
                }
            }
            else
            {
                return value == null ? DBNull.Value : value;
            }
        }

        protected abstract object GetValue(object value);
    }
}
