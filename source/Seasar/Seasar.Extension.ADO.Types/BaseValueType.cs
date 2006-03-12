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
using Seasar.Extension.ADO;
using Seasar.Framework.Util;
using Nullables;

namespace Seasar.Extension.ADO.Types
{
    public abstract class BaseValueType
    {
        private IDataSource dataSource;
        protected BindVariableType bindVariableType = BindVariableType.None;

        protected BindVariableType BindVariableType
        {
            get
            {
                if(bindVariableType == BindVariableType.None)
                {
                    bindVariableType = DataProviderUtil.GetBindVariableType(dataSource.GetConnection());
                }
                return bindVariableType;
            }
        }

        public BaseValueType(IDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        public void BindValue(System.Data.IDbCommand cmd, string columnName, object value, DbType dbType)
        {
            switch(BindVariableType)
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

            IDataParameter parameter = dataSource.GetParameter(columnName, GetBindValue(value));
			if("OleDbCommand".Equals(cmd.GetType().Name) && dbType == DbType.String)
			{
				OleDbParameter oleDbParam = parameter as OleDbParameter;
				oleDbParam.OleDbType = OleDbType.VarChar;
			}
			else
			{
				parameter.DbType = dbType;
			}
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

        protected abstract object GetValue(object value, Type type);
    }
}
