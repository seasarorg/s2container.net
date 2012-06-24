#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
        private readonly IDbCommand _original;

        public DbCommandWrapper(IDbCommand original)
        {
            _original = original;
        }

        #region IDbCommand ÉÅÉìÉo

        public virtual void Cancel()
        {
            _original.Cancel();
        }

        public virtual string CommandText
        {
            get { return _original.CommandText; }
            set { _original.CommandText = value; }
        }

        public virtual int CommandTimeout
        {
            get { return _original.CommandTimeout; }
            set { _original.CommandTimeout = value; }
        }

        public virtual CommandType CommandType
        {
            get { return _original.CommandType; }
            set { _original.CommandType = value; }
        }

        public virtual IDbConnection Connection
        {
            get { return _original.Connection; }
            set { _original.Connection = value; }
        }

        public virtual IDbDataParameter CreateParameter()
        {
            return _original.CreateParameter();
        }

        public virtual int ExecuteNonQuery()
        {
            return _original.ExecuteNonQuery();
        }

        public virtual IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return _original.ExecuteReader(behavior);
        }

        public virtual IDataReader ExecuteReader()
        {
            return _original.ExecuteReader();
        }

        public virtual object ExecuteScalar()
        {
            return _original.ExecuteScalar();
        }

        public virtual IDataParameterCollection Parameters
        {
            get { return _original.Parameters; }
        }

        public virtual void Prepare()
        {
            _original.Prepare();
        }

        public virtual IDbTransaction Transaction
        {
            get { return _original.Transaction; }
            set { _original.Transaction = value; }
        }

        public virtual UpdateRowSource UpdatedRowSource
        {
            get { return _original.UpdatedRowSource; }
            set { _original.UpdatedRowSource = value; }
        }

        #endregion

        #region IDisposable ÉÅÉìÉo

        public virtual void Dispose()
        {
            _original.Dispose();
        }

        #endregion
    }
}
