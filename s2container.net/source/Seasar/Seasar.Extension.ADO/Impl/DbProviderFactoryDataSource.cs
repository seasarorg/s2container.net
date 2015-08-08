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

using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    public class DbProviderFactoryDataSource : IDataSource
    {
        private readonly ConnectionStringSettings _settings;
        private readonly DbProviderFactory _dbProviderFactory;

        public DbProviderFactoryDataSource()
        {
        }

        public DbProviderFactoryDataSource(string providerInvariantName)
        {
            _settings = ConfigurationManager.ConnectionStrings[providerInvariantName];
            if (_settings == null)
            {
                throw new ConfigurationManagerException("connectionStrings", providerInvariantName);
            }
            _dbProviderFactory = DbProviderFactories.GetFactory(_settings.ProviderName);
        }

        public DbProviderFactory DbProviderFactory => _dbProviderFactory;

        #region IDataSource ƒƒ“ƒo

        public virtual IDbConnection GetConnection()
        {
            IDbConnection cn = _dbProviderFactory.CreateConnection();
            if (cn != null)
            {
                cn.ConnectionString = _settings.ConnectionString;
                return cn;
            }
            return null;
        }

        public virtual void CloseConnection(IDbConnection connection)
        {
            ConnectionUtil.Close(connection);
        }

        public IDbCommand GetCommand()
        {
            return _dbProviderFactory.CreateCommand();
        }

        public IDbCommand GetCommand(string text)
        {
            var cmd = GetCommand();
            cmd.CommandText = text;
            return cmd;
        }

        public IDbCommand GetCommand(string text, IDbConnection connection)
        {
            var cmd = GetCommand(text);
            cmd.Connection = connection;
            return cmd;
        }

        public IDbCommand GetCommand(string text,
            IDbConnection connection, IDbTransaction transaction)
        {
            var cmd = GetCommand(text, connection);
            cmd.Transaction = transaction;
            return cmd;
        }

        public IDataParameter GetParameter()
        {
            return _dbProviderFactory.CreateParameter();
        }

        public IDataParameter GetParameter(string name, DbType dataType)
        {
            var param = GetParameter();
            param.ParameterName = name;
            param.DbType = dataType;
            return param;
        }

        public IDataParameter GetParameter(string name, object value)
        {
            var param = GetParameter();
            param.ParameterName = name;
            if (value == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = value;
            }
            return param;
        }

        public IDataParameter GetParameter(string name, DbType dataType, int size)
        {
            var param = _dbProviderFactory.CreateParameter();
            if (param != null)
            {
                param.ParameterName = name;
                param.DbType = dataType;
                param.Size = size;
                return param;
            }
            return null;
        }

        public IDataParameter GetParameter(string name, DbType dataType, int size, string srcColumn)
        {
            var param = GetParameter(name, dataType, size);
            param.SourceColumn = srcColumn;
            return param;
        }

        public IDataAdapter GetDataAdapter() => _dbProviderFactory.CreateDataAdapter();

        public IDataAdapter GetDataAdapter(IDbCommand selectCommand)
        {
            var ret = _dbProviderFactory.CreateDataAdapter();
            if (ret != null)
            {
                ret.SelectCommand = (DbCommand)selectCommand;
                return ret;
            }
            return null;
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString)
        {
            throw new NotSupportedException("GetDataAdapter");
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, IDbConnection selectConnection)
        {
            throw new NotSupportedException("GetDataAdapter");
        }

        public virtual IDbTransaction GetTransaction()
        {
            throw new NotSupportedException("GetTransaction");
        }

        public virtual void SetTransaction(IDbCommand cmd)
        {
        }

        #endregion
    }
}
