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
    public class DataSourceImpl : IDataSource
    {
        private IDataSource _instance;

        public DataSourceImpl()
        {
        }

        public DataSourceImpl(DataProvider provider, string connectionString)
            : this(new DataProviderDataSource(provider, connectionString))
        {
        }

        public DataSourceImpl(string providerInvariantName)
            : this(new DbProviderFactoryDataSource(providerInvariantName))
        {
        }

        public DataSourceImpl(IDataSource instance)
        {
            _instance = instance;
        }

        public DataProvider DataProvider
        {
            set
            {
                var ds = _instance as DataProviderDataSource;
                if (ds == null)
                {
                    ds = new DataProviderDataSource();
                    _instance = ds;
                }
                ds.DataProvider = value;
            }
            get
            {
                var ds = _instance as DataProviderDataSource;
                return ds?.DataProvider;
            }
        }

        public string ConnectionString
        {
            set
            {
                var ds = _instance as DataProviderDataSource;
                if (ds == null)
                {
                    ds = new DataProviderDataSource();
                    _instance = ds;
                }
                ds.ConnectionString = value;
            }
            get
            {
                var ds = _instance as DataProviderDataSource;
                return ds?.ConnectionString;
            }
        }

        public string ProviderInvariantName
        {
            set
            {
                var ds = _instance as DbProviderFactoryDataSource;
                if (ds == null)
                {
                    ds = new DbProviderFactoryDataSource(value);
                    _instance = ds;
                }
            }
        }

        public IDataSource Instance => _instance;

        #region IDataSource ƒƒ“ƒo

        public virtual IDbConnection GetConnection() => _instance.GetConnection();

        public virtual void CloseConnection(IDbConnection connection)
        {
            _instance.CloseConnection(connection);
        }

        public IDbCommand GetCommand() => _instance.GetCommand();

        public IDbCommand GetCommand(string text) => _instance.GetCommand(text);

        public IDbCommand GetCommand(string text, IDbConnection connection) => _instance.GetCommand(text, connection);

        public IDbCommand GetCommand(string text,
            IDbConnection connection, IDbTransaction transaction) => _instance.GetCommand(text, connection, transaction);

        public IDataParameter GetParameter() => _instance.GetParameter();

        public IDataParameter GetParameter(string name, DbType dataType) => _instance.GetParameter(name, dataType);

        public IDataParameter GetParameter(string name, object value) => _instance.GetParameter(name, value);

        public IDataParameter GetParameter(string name, DbType dataType, int size) => _instance.GetParameter(name, dataType, size);

        public IDataParameter GetParameter(string name, DbType dataType, int size, string srcColumn) => _instance.GetParameter(name, dataType, size, srcColumn);

        public IDataAdapter GetDataAdapter() => _instance.GetDataAdapter();

        public IDataAdapter GetDataAdapter(IDbCommand selectCommand) => _instance.GetDataAdapter(selectCommand);

        public IDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString) => _instance.GetDataAdapter(selectCommandText, selectConnectionString);

        public IDataAdapter GetDataAdapter(string selectCommandText, IDbConnection selectConnection) => _instance.GetDataAdapter(selectCommandText, selectConnection);

        public virtual IDbTransaction GetTransaction() => _instance.GetTransaction();

        public virtual void SetTransaction(IDbCommand cmd)
        {
            _instance.SetTransaction(cmd);
        }

        #endregion
    }
}
