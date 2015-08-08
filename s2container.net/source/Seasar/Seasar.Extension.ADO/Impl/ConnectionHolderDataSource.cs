#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
            return _holderConnection ?? (_holderConnection = _dataSource.GetConnection());
        }

        public virtual void CloseConnection(IDbConnection connection)
        {
            if (!IsHolderConnection)
            {
                Current.CloseConnection(connection);
            }
        }

        public IDbCommand GetCommand() => _dataSource.GetCommand();

        public IDbCommand GetCommand(string text) => _dataSource.GetCommand(text);

        public IDbCommand GetCommand(string text, IDbConnection connection) => _dataSource.GetCommand(text, connection);

        public IDbCommand GetCommand(string text, 
            IDbConnection connection, IDbTransaction transaction)
        {
            return _dataSource.GetCommand(text, connection, transaction);
        }

        public IDataParameter GetParameter() => _dataSource.GetParameter();

        public IDataParameter GetParameter(string name, DbType dataType) => _dataSource.GetParameter(name, dataType);

        public IDataParameter GetParameter(string name, object value) => _dataSource.GetParameter(name, value);

        public IDataParameter GetParameter(string name, DbType dataType, int size) => _dataSource.GetParameter(name, dataType, size);

        public IDataParameter GetParameter(string name, DbType dataType, int size, string srcColumn) => _dataSource.GetParameter(name, dataType, size, srcColumn);

        public IDataAdapter GetDataAdapter() => _dataSource.GetDataAdapter();

        public IDataAdapter GetDataAdapter(IDbCommand selectCommand) => _dataSource.GetDataAdapter(selectCommand);

        public IDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString)
        {
            return _dataSource.GetDataAdapter(selectCommandText, selectConnectionString);
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, IDbConnection selectConnection)
        {
            return _dataSource.GetDataAdapter(selectCommandText, selectConnection);
        }

        public virtual IDbTransaction GetTransaction() => _dataSource.GetTransaction();

        public virtual void SetTransaction(IDbCommand cmd) => Current.SetTransaction(cmd);

        public IDataSource Current => _dataSource;

        public bool IsHolderConnection => _holderConnection != null;

        public void ReleaseConnection() => _holderConnection = null;
    }
}
