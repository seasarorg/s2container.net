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
using System.Reflection;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    public class BasicSelectHandler : BasicHandler, ISelectHandler
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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

        public IDataReaderFactory DataReaderFactory { get; set; } = BasicDataReaderFactory.INSTANCE;

        public IDataReaderHandler DataReaderHandler { get; set; }

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
            var con = Connection;
            try
            {
                return Execute(con, args, argTypes);
            }
            finally
            {
                DataSource.CloseConnection(con);
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
            if (DataReaderHandler == null)
            {
                throw new EmptyRuntimeException("dataReaderHandler");
            }
            IDataReader dataReader = null;
            try
            {
                if (DataReaderHandler is ObjectDataReaderHandler)
                {
                    return CommandFactory.ExecuteScalar(DataSource, cmd);
                }
                else
                {
                    dataReader = CreateDataReader(cmd);
                    return DataReaderHandler.Handle(dataReader);
                }
            }
            finally
            {
                DataReaderUtil.Close(dataReader);
            }
        }

        protected virtual IDataReader CreateDataReader(IDbCommand cmd)
        {
            return DataReaderFactory.CreateDataReader(DataSource, cmd);
        }
    }
}
