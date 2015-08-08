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
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    public class BasicHandler
    {
        private int _commandTimeout = -1;

        public BasicHandler()
        {
        }

        public BasicHandler(IDataSource ds, string sql)
            : this(ds, sql, BasicCommandFactory.INSTANCE)
        {
        }

        public BasicHandler(IDataSource ds, string sql, ICommandFactory commandFactory)
        {
            DataSource = ds;
            Sql = sql;
            CommandFactory = commandFactory;
        }

        public IDataSource DataSource { get; set; }

        public string Sql { get; set; }

        public ICommandFactory CommandFactory { get; set; } = BasicCommandFactory.INSTANCE;

        protected IDbConnection Connection
        {
            get
            {
                if (DataSource == null)
                {
                    throw new EmptyRuntimeException("_dataSource");
                }
                return DataSourceUtil.GetConnection(DataSource);
            }
        }

        protected virtual IDbCommand Command(IDbConnection connection)
        {
            if (Sql == null)
            {
                throw new EmptyRuntimeException("_sql");
            }
            var cmd = CommandFactory.CreateCommand(connection, Sql);
            if (_commandTimeout > -1)
            {
                cmd.CommandTimeout = _commandTimeout;
            }
            return cmd;
        }

        protected virtual void BindArgs(IDbCommand command, object[] args, Type[] argTypes)
        {
            if (args == null) return;
            var argNames = CommandFactory.GetArgNames(command, args);
            for (var i = 0; i < args.Length; ++i)
            {
                var valueType = ValueTypes.GetValueType(argTypes[i]);
                try
                {
                    valueType.BindValue(command, argNames[i], args[i]);
                }
                catch (Exception ex)
                {
                    throw new SQLRuntimeException(ex);
                }
            }
        }

        protected virtual Type[] GetArgTypes(object[] args)
        {
            if (args == null)
            {
                return null;
            }
            var argTypes = new Type[args.Length];
            for (var i = 0; i < args.Length; ++i)
            {
                var arg = args[i];
                if (arg != null)
                {
                    argTypes[i] = arg.GetExType();
                }
            }
            return argTypes;
        }

        protected virtual string GetCompleteSql(object[] args)
        {
            return CommandFactory.GetCompleteSql(Sql, args);
        }
    }
}
