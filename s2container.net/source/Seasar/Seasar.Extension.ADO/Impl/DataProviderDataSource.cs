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
using System.Data;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    public class DataProviderDataSource : IDataSource
    {
        public DataProviderDataSource()
        {
        }

        public DataProviderDataSource(DataProvider provider, string connectionString)
        {
            DataProvider = provider;
            ConnectionString = connectionString;
        }

        public DataProvider DataProvider { set; get; }

        public string ConnectionString { set; get; }

        #region IDataSource ÉÅÉìÉo

        public virtual IDbConnection GetConnection()
        {
            var cn = (IDbConnection) ClassUtil.NewInstance(_ForName(DataProvider.ConnectionType));
            cn.ConnectionString = ConnectionString;
            return cn;
        }

        public virtual void CloseConnection(IDbConnection connection)
        {
            ConnectionUtil.Close(connection);
        }

        public IDbCommand GetCommand()
        {
            return (IDbCommand) ClassUtil.NewInstance(_ForName(DataProvider.CommandType));
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

        public IDbCommand GetCommand(string text, IDbConnection connection, IDbTransaction transaction)
        {
            var cmd = GetCommand(text, connection);
            cmd.Transaction = transaction;
            return cmd;
        }

        public IDataParameter GetParameter()
        {
            return (IDataParameter) ClassUtil.NewInstance(_ForName(DataProvider.ParameterType));
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
            var argTypes = new[] { typeof(string), typeof(DbType), typeof(int) };
            var constructor = ClassUtil.GetConstructorInfo(_ForName(DataProvider.ParameterType), argTypes);
            return (IDataParameter) ConstructorUtil.NewInstance<string, DbType, int>(constructor, new object[] { name, dataType, size });
        }

        public IDataParameter GetParameter(string name, DbType dataType, int size, string srcColumn)
        {
            var param = GetParameter(name, dataType, size);
            param.SourceColumn = srcColumn;
            return param;
        }

        public IDataAdapter GetDataAdapter()
        {
            return (IDataAdapter) ClassUtil.NewInstance(_ForName(DataProvider.DataAdapterType));
        }

        public IDataAdapter GetDataAdapter(IDbCommand selectCommand)
        {
//            var argTypes = new[] { _ForName(DataProvider.CommandType) };
//            var constructor = ClassUtil.GetConstructorInfo(_ForName(DataProvider.DataAdapterType), argTypes);
//            return (IDataAdapter) ConstructorUtil.NewInstance<IDbCommand>(constructor, new object[] { selectCommand });

            var adapter = (IDbDataAdapter)ClassUtil.NewInstance(_ForName(DataProvider.DataAdapterType));
            adapter.SelectCommand = selectCommand;
            return adapter;
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString)
        {
            var argTypes = new[] { typeof(string), typeof(string) };
            var constructor = ClassUtil.GetConstructorInfo(_ForName(DataProvider.DataAdapterType), argTypes);
            return (IDataAdapter) ConstructorUtil.NewInstance<string>(constructor, new object[] { selectCommandText, selectConnectionString });
        }

        public IDataAdapter GetDataAdapter(string selectCommandText, IDbConnection selectConnection)
        {
//            var argTypes = new[] { typeof(string), _ForName(DataProvider.ConnectionType) };
//            var constructor = ClassUtil.GetConstructorInfo(_ForName(DataProvider.DataAdapterType), argTypes);
//            return (IDataAdapter) ConstructorUtil.NewInstance<string, IDbConnection>(constructor, new object[] { selectCommandText, selectConnection });

            var adapter = (IDbDataAdapter)ClassUtil.NewInstance(_ForName(DataProvider.DataAdapterType));
            var cmd = selectConnection.CreateCommand();
            cmd.CommandText = selectCommandText;
//            cmd.CommandType = CommandType.Text;
            adapter.SelectCommand = cmd;
            return adapter;
        }

        public virtual IDbTransaction GetTransaction()
        {
            throw new NotSupportedException("GetTransaction");
        }

        public virtual void SetTransaction(IDbCommand cmd)
        {
        }

        #endregion

        private static Type _ForName(string name)
        {
            var retType = ClassUtil.ForName(name, AppDomain.CurrentDomain.GetAssemblies());
            if(retType == null)
            {
                throw new ClassNotFoundRuntimeException(name);
            }
            return retType;
        }
    }
}
