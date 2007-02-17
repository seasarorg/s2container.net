#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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

using System.Data;

namespace Seasar.Extension.ADO.Impl
{
    public class DbCommandWrapper : IDbCommand
    {
        private readonly IDbCommand original;

        public DbCommandWrapper(IDbCommand original)
        {
            this.original = original;
        }

        #region IDbCommand ÉÅÉìÉo

        public virtual void Cancel()
        {
            original.Cancel();
        }

        public virtual string CommandText
        {
            get { return original.CommandText; }
            set { original.CommandText = value; }
        }

        public virtual int CommandTimeout
        {
            get { return original.CommandTimeout; }
            set { original.CommandTimeout = value; }
        }

        public virtual CommandType CommandType
        {
            get { return original.CommandType; }
            set { original.CommandType = value; }
        }

        public virtual IDbConnection Connection
        {
            get { return original.Connection; }
            set { original.Connection = value; }
        }

        public virtual IDbDataParameter CreateParameter()
        {
            return original.CreateParameter();
        }

        public virtual int ExecuteNonQuery()
        {
            return original.ExecuteNonQuery();
        }

        public virtual IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return original.ExecuteReader(behavior);
        }

        public virtual IDataReader ExecuteReader()
        {
            return original.ExecuteReader();
        }

        public virtual object ExecuteScalar()
        {
            return original.ExecuteScalar();
        }

        public virtual IDataParameterCollection Parameters
        {
            get { return original.Parameters; }
        }

        public virtual void Prepare()
        {
            original.Prepare();
        }

        public virtual IDbTransaction Transaction
        {
            get { return original.Transaction; }
            set { original.Transaction = value; }
        }

        public virtual UpdateRowSource UpdatedRowSource
        {
            get { return original.UpdatedRowSource; }
            set { original.UpdatedRowSource = value; }
        }

        #endregion

        #region IDisposable ÉÅÉìÉo

        public virtual void Dispose()
        {
            original.Dispose();
        }

        #endregion
    }
}
