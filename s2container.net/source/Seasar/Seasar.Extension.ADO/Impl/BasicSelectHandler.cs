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
        private static readonly Logger _logger = Logger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDataReaderFactory _dataReaderFactory = BasicDataReaderFactory.INSTANCE;
        private IDataReaderHandler _dataReaderHandler;

        public BasicSelectHandler()
        {
        }

        public BasicSelectHandler(IDataSource dataSource, string sql,
            IDataReaderHandler dataReaderHandler)
            : this(dataSource, sql, dataReaderHandler,
                    BasicCommandFactory.INSTANCE, BasicDataReaderFactory.INSTANCE)
        {
        }

        public BasicSelectHandler(IDataSource dataSource, string sql,
            IDataReaderHandler dataReaderHandler,
            ICommandFactory commandFactory, IDataReaderFactory dataReaderFactory)
            : base(dataSource, sql, commandFactory)
        {
            DataReaderHandler = dataReaderHandler;
            DataReaderFactory = dataReaderFactory;
        }

        public IDataReaderFactory DataReaderFactory
        {
            get { return _dataReaderFactory; }
            set { _dataReaderFactory = value; }
        }

        public IDataReaderHandler DataReaderHandler
        {
            get { return _dataReaderHandler; }
            set { _dataReaderHandler = value; }
        }

        #region ISelectHandler ÉÅÉìÉo

        public virtual object Execute(object[] args)
        {
            return Execute(args, GetArgTypes(args));
        }

        public virtual object Execute(object[] args, Type[] argTypes)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug(GetCompleteSql(args));
            }
            IDbConnection con = Connection;
            try
            {
                return Execute(con, args, argTypes);
            }
            finally
            {
                DataSourceUtil.CloseConnection(DataSource, con);
            }
        }

        public virtual object Execute(object[] args, Type[] argTypes, string[] argNames)
        {
            return Execute(args, argTypes);
        }

        #endregion

        protected virtual object Execute(IDbConnection connection, object[] args, Type[] argTypes)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = Command(connection);
                BindArgs(cmd, args, argTypes);
                return Execute(cmd);
            }
            finally
            {
                CommandUtil.Close(cmd);
            }
        }

        protected virtual object Execute(IDbCommand cmd)
        {
            if (_dataReaderHandler == null)
            {
                throw new EmptyRuntimeException("dataReaderHandler");
            }
            IDataReader dataReader = null;
            try
            {
                if (_dataReaderHandler is ObjectDataReaderHandler)
                {
                    return CommandFactory.ExecuteScalar(DataSource, cmd);
                }
                else
                {
                    dataReader = CreateDataReader(cmd);
                    return _dataReaderHandler.Handle(dataReader);
                }
            }
            finally
            {
                DataReaderUtil.Close(dataReader);
            }
        }

        protected virtual IDataReader CreateDataReader(IDbCommand cmd)
        {
            return _dataReaderFactory.CreateDataReader(DataSource, cmd);
        }
    }
}
