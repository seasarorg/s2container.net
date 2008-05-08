#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
    public class ConnectionHolderDataSource : IDataSource, IDbConnectionHolder
    {
        private readonly IDataSource _dataSource;
        private IDbConnection _holderConnection;

        public ConnectionHolderDataSource(IDataSource dataSource) 
        {
            _dataSource = dataSource;
        }

        public IDbConnection GetConnection()
        {
            if (_holderConnection == null) 
            {
                _holderConnection = _dataSource.GetConnection();
            }
            return _holderConnection;
        }

        public virtual void CloseConnection(IDbConnection connection)
        {
            if (!IsHolderConnection)
            {
                Current.CloseConnection(connection);
            }
        }

        public IDbCommand GetCommand()
        {
            return _dataSource.GetCommand();
        }

        public IDbCommand GetCommand(string cmdText)
        {
            return _dataSource.GetCommand(cmdText);
        }

        public IDbCommand GetCommand(string cmdText, IDbConnection connection)
        {
            return _dataSource.GetCommand(cmdText, connection);
        }

        public IDbCommand GetCommand(string cmdText, 
            IDbConnection connection, IDbTransaction transaction)
        {
            return _dataSource.GetCommand(cmdText, connection, transaction);
        }

        public IDataParameter GetParameter()
        {
            return _dataSource.GetParameter();
        }

        public IDataParameter GetParameter(string name, DbType dataType)
        {
            return _dataSource.GetParameter(name, dataType);
        }

        public IDataParameter GetParameter(string name, object value)
        {
            return _dataSource.GetParameter(name, value);
        }

        public IDataParameter GetParameter(string name, DbType dataType, int size)
        {
            return _dataSource.GetParameter(name, dataType, size);
        }

        public IDataParameter GetParameter(string name, DbType dataType, int size, string srcColumn)
        {
            return _dataSource.GetParameter(name, dataType, size, srcColumn);
        }

        public IDataAdapter GetDataAdapter()
        {
            return _dataSource.GetDataAdapter();
        }

        public IDataAdapter GetDataAdapter(IDbCommand selectCommand)
        {
            return _dataSource.GetDataAdapter(selectCommand);
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString)
        {
            return _dataSource.GetDataAdapter(selectCommandText, selectConnectionString);
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, IDbConnection selectConnection)
        {
            return _dataSource.GetDataAdapter(selectCommandText, selectConnection);
        }

        public virtual IDbTransaction GetTransaction()
        {
            return _dataSource.GetTransaction();
        }

        public virtual void SetTransaction(IDbCommand cmd)
        {
            Current.SetTransaction(cmd);
        }

        public IDataSource Current
        {
            get { return _dataSource; }
        }

        public bool IsHolderConnection
        {
            get { return _holderConnection != null; }
        }

        public void ReleaseConnection()
        {
            _holderConnection = null;
        }
    }
}
