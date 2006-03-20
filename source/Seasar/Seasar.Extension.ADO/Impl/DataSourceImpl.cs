#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
using Seasar.Framework.Util;
using Seasar.Extension.ADO.Types;

namespace Seasar.Extension.ADO.Impl
{
	/// <summary>
	/// DataSourceImpl ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class DataSourceImpl : IDataSource
	{
		private DataProvider provider_;
		private string connectionString_;

		public DataSourceImpl()
		{
			ValueTypes.Init(this);
		}

		public DataSourceImpl(DataProvider provider, string connectionString)
		{
			ValueTypes.Init(this);
			provider_ = provider;
			connectionString_ = connectionString;
		}

		public DataProvider DataProvider
		{
			set { provider_ = value; }
			get { return provider_; }
		}

		public string ConnectionString
		{
			set { connectionString_ = value; }
			get { return connectionString_; }
		}

		#region IDataSource ÉÅÉìÉo

		public System.Data.IDbConnection GetConnection()
		{
			IDbConnection cn = (IDbConnection) ClassUtil.NewInstance(ForName(provider_.ConnectionType));
			cn.ConnectionString = connectionString_;
			return cn;
		}

		public System.Data.IDbCommand GetCommand()
		{
			return (IDbCommand) ClassUtil.NewInstance(ForName(provider_.CommandType));
		}

		public System.Data.IDbCommand GetCommand(string cmdText)
		{
			IDbCommand cmd = this.GetCommand();
			cmd.CommandText = cmdText;
			return cmd;
		}

		public System.Data.IDbCommand GetCommand(string cmdText, System.Data.IDbConnection connection)
		{
			IDbCommand cmd = this.GetCommand(cmdText);
			cmd.Connection = connection;
			return cmd;
		}

		public System.Data.IDbCommand GetCommand(string cmdText, 
			System.Data.IDbConnection connection, System.Data.IDbTransaction transaction)
		{
			IDbCommand cmd = this.GetCommand(cmdText, connection);
			cmd.Transaction = transaction;
			return cmd;
		}

		public System.Data.IDataParameter GetParameter()
		{
			return (IDataParameter) ClassUtil.NewInstance(ForName(provider_.ParameterType));
		}

		public System.Data.IDataParameter GetParameter(string name, System.Data.DbType dataType)
		{
			IDataParameter param = this.GetParameter();
			param.ParameterName = name;
			param.DbType = dataType;
			return param;
		}

		public System.Data.IDataParameter GetParameter(string name, object value)
		{
			IDataParameter param = this.GetParameter();
			param.ParameterName = name;
			if(value == null)
			{
				param.Value = DBNull.Value;
			}
			else
			{
				param.Value = value;
			}
			return param;
		}

		public System.Data.IDataParameter GetParameter(string name, System.Data.DbType dataType, int size)
		{
			Type[] argTypes = new Type[] { typeof(string), typeof(DbType), typeof(int) };
			ConstructorInfo constructor = ClassUtil.GetConstructorInfo(ForName(provider_.ParameterType),argTypes);
			return (IDataParameter) ConstructorUtil.NewInstance(constructor,
				new object[] { name, dataType, size });
		}

		public System.Data.IDataParameter GetParameter(string name, System.Data.DbType dataType, int size, string srcColumn)
		{
			IDataParameter param = this.GetParameter(name, dataType, size);
			param.SourceColumn = srcColumn;
			return param;
		}

		public IDataAdapter GetDataAdapter()
		{
			return (IDataAdapter) ClassUtil.NewInstance(ForName(provider_.DataAdapterType));
		}

		public IDataAdapter GetDataAdapter(IDbCommand selectCommand)
		{
			Type[] argTypes = new Type[] { ForName(provider_.CommandType) };
			ConstructorInfo constructor = ClassUtil.GetConstructorInfo(ForName(provider_.DataAdapterType), argTypes);
			return (IDataAdapter) ConstructorUtil.NewInstance(constructor, new object[] { selectCommand });
		}
		
		public IDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString)
		{
			Type[] argTypes = new Type[] { typeof(string), typeof(string) };
			ConstructorInfo constructor = ClassUtil.GetConstructorInfo(ForName(provider_.DataAdapterType), argTypes);
			return (IDataAdapter) ConstructorUtil.NewInstance(constructor,
				new object[] { selectCommandText, selectConnectionString });
		}

		public IDataAdapter GetDataAdapter(string selectCommandText, IDbConnection selectConnection)
		{
			Type[] argTypes = new Type[] { typeof(string), ForName(provider_.ConnectionType) };
			ConstructorInfo constructor = ClassUtil.GetConstructorInfo(ForName(provider_.DataAdapterType), argTypes);
			return (IDataAdapter) ConstructorUtil.NewInstance(constructor,
				new object[] { selectCommandText, selectConnection });
		}

        public virtual IDbTransaction GetTransaction()
        {
            throw new NotSupportedException("GetTransaction");
        }

		#endregion

		private static Type ForName(string name)
		{
			return ClassUtil.ForName(name, AppDomain.CurrentDomain.GetAssemblies());
		}
	}
}
