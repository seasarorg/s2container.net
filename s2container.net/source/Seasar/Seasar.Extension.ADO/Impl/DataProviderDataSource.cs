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
using System.Data;
using System.Reflection;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    public class DataProviderDataSource : IDataSource
    {
        private DataProvider _provider;
        private string _connectionString;

        public DataProviderDataSource()
        {
        }

        public DataProviderDataSource(DataProvider provider, string connectionString)
        {
            _provider = provider;
            _connectionString = connectionString;
        }

        public DataProvider DataProvider
        {
            set { _provider = value; }
            get { return _provider; }
        }

        public string ConnectionString
        {
            set { _connectionString = value; }
            get { return _connectionString; }
        }

        #region IDataSource ÉÅÉìÉo

        public virtual IDbConnection GetConnection()
        {
            IDbConnection cn = (IDbConnection) ClassUtil.NewInstance(ForName(_provider.ConnectionType));
            cn.ConnectionString = _connectionString;
            return cn;
        }

        public virtual void CloseConnection(IDbConnection connection)
        {
            ConnectionUtil.Close(connection);
        }

        public IDbCommand GetCommand()
        {
            return (IDbCommand) ClassUtil.NewInstance(ForName(_provider.CommandType));
        }

        public IDbCommand GetCommand(string cmdText)
        {
            IDbCommand cmd = GetCommand();
            cmd.CommandText = cmdText;
            return cmd;
        }

        public IDbCommand GetCommand(string cmdText, IDbConnection connection)
        {
            IDbCommand cmd = GetCommand(cmdText);
            cmd.Connection = connection;
            return cmd;
        }

        public IDbCommand GetCommand(string cmdText,
            IDbConnection connection, IDbTransaction transaction)
        {
            IDbCommand cmd = GetCommand(cmdText, connection);
            cmd.Transaction = transaction;
            return cmd;
        }

        public IDataParameter GetParameter()
        {
            return (IDataParameter) ClassUtil.NewInstance(ForName(_provider.ParameterType));
        }

        public IDataParameter GetParameter(string name, DbType dataType)
        {
            IDataParameter param = GetParameter();
            param.ParameterName = name;
            param.DbType = dataType;
            return param;
        }

        public IDataParameter GetParameter(string name, object value)
        {
            IDataParameter param = GetParameter();
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
            Type[] argTypes = new Type[] { typeof(string), typeof(DbType), typeof(int) };
            ConstructorInfo constructor = ClassUtil.GetConstructorInfo(ForName(_provider.ParameterType), argTypes);
            return (IDataParameter) ConstructorUtil.NewInstance(constructor,
                new object[] { name, dataType, size });
        }

        public IDataParameter GetParameter(string name, DbType dataType, int size, string srcColumn)
        {
            IDataParameter param = GetParameter(name, dataType, size);
            param.SourceColumn = srcColumn;
            return param;
        }

        public IDataAdapter GetDataAdapter()
        {
            return (IDataAdapter) ClassUtil.NewInstance(ForName(_provider.DataAdapterType));
        }

        public IDataAdapter GetDataAdapter(IDbCommand selectCommand)
        {
            Type[] argTypes = new Type[] { ForName(_provider.CommandType) };
            ConstructorInfo constructor = ClassUtil.GetConstructorInfo(ForName(_provider.DataAdapterType), argTypes);
            return (IDataAdapter) ConstructorUtil.NewInstance(constructor, new object[] { selectCommand });
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString)
        {
            Type[] argTypes = new Type[] { typeof(string), typeof(string) };
            ConstructorInfo constructor = ClassUtil.GetConstructorInfo(ForName(_provider.DataAdapterType), argTypes);
            return (IDataAdapter) ConstructorUtil.NewInstance(constructor,
                new object[] { selectCommandText, selectConnectionString });
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, IDbConnection selectConnection)
        {
            Type[] argTypes = new Type[] { typeof(string), ForName(_provider.ConnectionType) };
            ConstructorInfo constructor = ClassUtil.GetConstructorInfo(ForName(_provider.DataAdapterType), argTypes);
            return (IDataAdapter) ConstructorUtil.NewInstance(constructor,
                new object[] { selectCommandText, selectConnection });
        }

        public virtual IDbTransaction GetTransaction()
        {
            throw new NotSupportedException("GetTransaction");
        }

        public virtual void SetTransaction(IDbCommand cmd)
        {
        }

        #endregion

        private static Type ForName(string name)
        {
            Type retType = ClassUtil.ForName(name, AppDomain.CurrentDomain.GetAssemblies());
            if(retType == null)
            {
                throw new ClassNotFoundRuntimeException(name);
            }
            return retType;
        }
    }
}
