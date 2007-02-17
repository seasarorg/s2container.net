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
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    public class ConnectionHolderDataSource : IDataSource, IDbConnectionHolder
    {
        private IDataSource dataSource;

        private IDbConnection holderConnection;

        public ConnectionHolderDataSource(IDataSource dataSource) 
        {
            this.dataSource = dataSource;
        }

        public IDbConnection GetConnection()
        {
            if (holderConnection == null) 
            {
                holderConnection = dataSource.GetConnection();
            }
            return holderConnection;
        }

        public IDbCommand GetCommand()
        {
            return dataSource.GetCommand();
        }

        public IDbCommand GetCommand(string cmdText)
        {
            return dataSource.GetCommand(cmdText);
        }

        public IDbCommand GetCommand(string cmdText, IDbConnection connection)
        {
            return dataSource.GetCommand(cmdText, connection);
        }

        public IDbCommand GetCommand(string cmdText, 
            IDbConnection connection, IDbTransaction transaction)
        {
            return dataSource.GetCommand(cmdText, connection, transaction);
        }

        public IDataParameter GetParameter()
        {
            return dataSource.GetParameter();
        }

        public IDataParameter GetParameter(string name, DbType dataType)
        {
            return dataSource.GetParameter(name, dataType);
        }

        public IDataParameter GetParameter(string name, object value)
        {
            return dataSource.GetParameter(name, value);
        }

        public IDataParameter GetParameter(string name, DbType dataType, int size)
        {
            return dataSource.GetParameter(name, dataType, size);
        }

        public IDataParameter GetParameter(string name, DbType dataType, int size, string srcColumn)
        {
            return dataSource.GetParameter(name, dataType, size, srcColumn);
        }

        public IDataAdapter GetDataAdapter()
        {
            return dataSource.GetDataAdapter();
        }

        public IDataAdapter GetDataAdapter(IDbCommand selectCommand)
        {
            return dataSource.GetDataAdapter(selectCommand);
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString)
        {
            return dataSource.GetDataAdapter(selectCommandText, selectConnectionString);
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, IDbConnection selectConnection)
        {
            return dataSource.GetDataAdapter(selectCommandText, selectConnection);
        }

        public virtual IDbTransaction GetTransaction()
        {
            return dataSource.GetTransaction();
        }

        public IDataSource Current
        {
            get { return dataSource; }
        }

        public bool IsHolderConnection
        {
            get { return holderConnection != null; }
        }

        public void ReleaseConnection()
        {
            holderConnection = null;
        }
    }
}
