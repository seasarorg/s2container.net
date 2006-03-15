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
using Seasar.Extension.ADO;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    public class BasicSelectHandler : BasicHandler, ISelectHandler
    {
        private static readonly Logger logger = Logger.GetLogger(typeof(BasicSelectHandler));

        private IDataReaderFactory dataReaderFactory = BasicDataReaderFactory.INSTANCE;
        private IDataReaderHandler dataReaderHandler;
        private int fetchSize = 100;
        private int maxRows = -1;

        public BasicSelectHandler()
        {
        }

        public BasicSelectHandler(IDataSource dataSource, string sql,
            IDataReaderHandler dataReaderHandler)
            : this(dataSource, sql, dataReaderHandler,
                    BasicCommandFactory.INSTANCE, BasicDataReaderFactory.INSTANCE)
        {
            this.DataSource = dataSource;
            this.Sql = sql;
            this.DataReaderHandler = dataReaderHandler;
        }

        public BasicSelectHandler(IDataSource dataSource, string sql,
            IDataReaderHandler dataReaderHandler,
            ICommandFactory commandFactory, IDataReaderFactory dataReaderFactory)
        {
            DataSource = dataSource;
            Sql = sql;
            DataReaderHandler = dataReaderHandler;
            CommandFactory = commandFactory;
            DataReaderFactory = dataReaderFactory;
        }

        public IDataReaderFactory DataReaderFactory
        {
            get { return dataReaderFactory; }
            set { dataReaderFactory = value; }
        }

        public IDataReaderHandler DataReaderHandler
        {
            get { return this.dataReaderHandler; }
            set { this.dataReaderHandler = value; }
        }

        public int FetchSize
        {
            get { return this.fetchSize; }
            set { this.fetchSize = value; }
        }

        public int MaxRows
        {
            get { return this.maxRows; }
            set { this.maxRows = value; }
        }

        public object Execute(object[] args)
        {
			int argLength = 0;
			Type[] argTypes = new Type[0];
			if(args != null)
			{
				argLength = args.Length;
				argTypes = Type.GetTypeArray(args);
			}
            return this.Execute(args, argTypes, new string[argLength]);
        }

        public object Execute(object[] args, Type[] argTypes, string[] argNames)
        {
            if(logger.IsDebugEnabled) logger.Debug(this.GetCompleteSql(args));
            IDbConnection con = this.Connection;
            try
            {
                return this.Execute(con, args, argTypes, argNames);
            }
            catch(Exception ex)
            {
                throw new SQLRuntimeException(ex);
            }
            finally
            {
                DataSourceUtil.CloseConnection(this.DataSource, con);
            }
        }

        protected object Execute(IDbConnection connection, object[] args, Type[] argTypes,
            string[] argNames)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = this.Command(connection);
                this.BindArgs(cmd, args, argTypes, argNames);
                return this.Execute(cmd);
            }
            finally
            {
                CommandUtil.Close(cmd);
            }
        }

        protected object[] Setup(IDbConnection con, object[] args)
        {
            return args;
        }

        protected override IDbCommand Command(IDbConnection connection)
        {
            IDbCommand cmd = base.Command(connection);
            //if(fetchSize > -1) CommandUtil.SetFetchSize(cmd, fetchSize);
            //if(maxRows > -1) CommandUtil.SetMaxRows(cmd, maxRows);
            return cmd;
        }

        protected object Execute(IDbCommand cmd)
        {
            if(dataReaderHandler == null)
                throw new EmptyRuntimeException("dataReaderHandler");
            IDataReader dataReader = null;
            try
            {
                if(dataReaderHandler is ObjectDataReaderHandler)
                    return CommandUtil.ExecuteScalar(this.DataSource, cmd);
                else
                {
                    dataReader = this.CreateDataReader(cmd);
                    return dataReaderHandler.Handle(dataReader);
                }
            }
            finally
            {
                DataReaderUtil.Close(dataReader);
            }
        }

        protected void SetupDataTable(DataTable dataTable)
        {
        }

        protected IDataReader CreateDataReader(IDbCommand cmd)
        {
            return dataReaderFactory.CreateDataReader(this.DataSource, cmd);
        }
    }
}
