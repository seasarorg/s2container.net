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

        public BasicSelectHandler()
        {
        }

        public BasicSelectHandler(IDataSource dataSource, string sql,
            IDataReaderHandler dataReaderHandler)
            : this(dataSource, sql, dataReaderHandler,
                    BasicCommandFactory.INSTANCE, BasicDataReaderFactory.INSTANCE)
        {
            DataSource = dataSource;
            Sql = sql;
            DataReaderHandler = dataReaderHandler;
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

        public object Execute(object[] args)
        {
            return Execute(args, GetArgTypes(args));
        }

        public object Execute(object[] args, Type[] argTypes)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug(GetCompleteSql(args));
            }
            IDbConnection con = Connection;
            try
            {
                return Execute(con, args, argTypes, GetArgNames());
            }
            finally
            {
                DataSourceUtil.CloseConnection(this.DataSource, con);
            }
        }

        public object Execute(object[] args, Type[] argTypes, string[] argNames)
        {
            return Execute(args, argTypes);
        }

        protected virtual object Execute(IDbConnection connection, object[] args, Type[] argTypes,
            string[] argNames)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = this.Command(connection);
                BindArgs(cmd, args, argTypes, argNames);
                return this.Execute(cmd);
            }
            finally
            {
                CommandUtil.Close(cmd);
            }
        }

        protected virtual object[] Setup(IDbConnection con, object[] args)
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

        protected virtual object Execute(IDbCommand cmd)
        {
            if (dataReaderHandler == null)
                throw new EmptyRuntimeException("dataReaderHandler");
            IDataReader dataReader = null;
            try
            {
                if (dataReaderHandler is ObjectDataReaderHandler)
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

        protected virtual void SetupDataTable(DataTable dataTable)
        {
        }

        protected virtual IDataReader CreateDataReader(IDbCommand cmd)
        {
            return dataReaderFactory.CreateDataReader(this.DataSource, cmd);
        }
    }
}
