#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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

namespace Seasar.Extension.ADO.Types
{
    public abstract class BaseValueType : IValueType
    {
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
            BindValue(cmd, columnName, value, dbType, ParameterDirection.Input);
        }

        public void BindValue(
            IDbCommand cmd,
            string columnName,
            object value,
            DbType dbType,
            ParameterDirection direction
            )
        {
            IDbDataParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = columnName;
            parameter.DbType = dbType;
            if ("OleDbCommand".Equals(cmd.GetType().Name) && dbType == DbType.String)
            {
                OleDbParameter oleDbParam = parameter as OleDbParameter;
                oleDbParam.OleDbType = OleDbType.VarChar;
            }
            parameter.Value = GetBindValue(value);
            parameter.Direction = direction;
            cmd.Parameters.Add(parameter);
        }

        protected abstract object GetBindValue(object value);

        protected abstract object GetValue(object value);
    }
}
