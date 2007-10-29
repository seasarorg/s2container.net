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
                DataProviderDataSource ds = _instance as DataProviderDataSource;
                if (ds == null)
                {
                    ds = new DataProviderDataSource();
                    _instance = ds;
                }
                ds.DataProvider = value;
            }
            get
            {
                DataProviderDataSource ds = _instance as DataProviderDataSource;
                if (ds == null)
                {
                    return null;
                }
                else
                {
                    return ds.DataProvider;
                }
            }
        }

        public string ConnectionString
        {
            set
            {
                DataProviderDataSource ds = _instance as DataProviderDataSource;
                if (ds == null)
                {
                    ds = new DataProviderDataSource();
                    _instance = ds;
                }
                ds.ConnectionString = value;
            }
            get
            {
                DataProviderDataSource ds = _instance as DataProviderDataSource;
                if (ds == null)
                {
                    return null;
                }
                else
                {
                    return ds.ConnectionString;
                }
            }
        }

        public string ProviderInvariantName
        {
            set
            {
                DbProviderFactoryDataSource ds = _instance as DbProviderFactoryDataSource;
                if (ds == null)
                {
                    ds = new DbProviderFactoryDataSource(value);
                    _instance = ds;
                }
            }
        }

        public IDataSource Instance
        {
            get { return _instance; }
        }

        #region IDataSource ÉÅÉìÉo

        public virtual IDbConnection GetConnection()
        {
            return _instance.GetConnection();
        }

        public virtual void CloseConnection(IDbConnection connection)
        {
            _instance.CloseConnection(connection);
        }

        public IDbCommand GetCommand()
        {
            return _instance.GetCommand();
        }

        public IDbCommand GetCommand(string cmdText)
        {
            return _instance.GetCommand(cmdText);
        }

        public IDbCommand GetCommand(string cmdText, IDbConnection connection)
        {
            return _instance.GetCommand(cmdText, connection);
        }

        public IDbCommand GetCommand(string cmdText,
            IDbConnection connection, IDbTransaction transaction)
        {
            return _instance.GetCommand(cmdText, connection, transaction);
        }

        public IDataParameter GetParameter()
        {
            return _instance.GetParameter();
        }

        public IDataParameter GetParameter(string name, DbType dataType)
        {
            return _instance.GetParameter(name, dataType);
        }

        public IDataParameter GetParameter(string name, object value)
        {
            return _instance.GetParameter(name, value);
        }

        public IDataParameter GetParameter(string name, DbType dataType, int size)
        {
            return _instance.GetParameter(name, dataType, size);
        }

        public IDataParameter GetParameter(string name, DbType dataType, int size, string srcColumn)
        {
            return _instance.GetParameter(name, dataType, size, srcColumn);
        }

        public IDataAdapter GetDataAdapter()
        {
            return _instance.GetDataAdapter();
        }

        public IDataAdapter GetDataAdapter(IDbCommand selectCommand)
        {
            return _instance.GetDataAdapter(selectCommand);
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString)
        {
            return _instance.GetDataAdapter(selectCommandText, selectConnectionString);
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, IDbConnection selectConnection)
        {
            return _instance.GetDataAdapter(selectCommandText, selectConnection);
        }

        public virtual IDbTransaction GetTransaction()
        {
            return _instance.GetTransaction();
        }

        public virtual void SetTransaction(IDbCommand cmd)
        {
            _instance.SetTransaction(cmd);
        }

        #endregion
    }
}
