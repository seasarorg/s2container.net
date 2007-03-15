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
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    public class BasicHandler
    {
        private IDataSource dataSource;
        private string sql;
        private ICommandFactory commandFactory = BasicCommandFactory.INSTANCE;
        private int commandTimeout = -1;

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

        public IDataSource DataSource
        {
            get { return this.dataSource; }
            set { this.dataSource = value; }
        }

        public string Sql
        {
            get { return this.sql; }
            set { this.sql = value; }
        }

        public ICommandFactory CommandFactory
        {
            get { return commandFactory; }
            set { commandFactory = value; }
        }

        [Obsolete("BasicCommandFactory.CommandTimeoutを使用してください。")]
        public int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }

        protected IDbConnection Connection
        {
            get
            {
                if (this.dataSource == null)
                {
                    throw new EmptyRuntimeException("dataSource");
                }
                return DataSourceUtil.GetConnection(this.dataSource);
            }
        }

        protected virtual IDbCommand Command(IDbConnection connection)
        {
            if (this.sql == null)
            {
                throw new EmptyRuntimeException("sql");
            }
            IDbCommand cmd = commandFactory.CreateCommand(connection, this.sql);
            if (this.commandTimeout > -1)
            {
                cmd.CommandTimeout = this.commandTimeout;
            }
            return cmd;
        }

        [Obsolete("BindArgs(IDbCommand, object[], Type[])を使用してください。")]
        protected virtual void BindArgs(IDbCommand command, object[] args, Type[] argTypes,
            string[] argNames)
        {
            BindArgs(command, args, argTypes);
        }

        protected virtual void BindArgs(IDbCommand command, object[] args, Type[] argTypes)
        {
            if (args == null) return;
            string[] argNames = GetArgNames(args);
            for (int i = 0; i < args.Length; ++i)
            {
                IValueType valueType = ValueTypes.GetValueType(argTypes[i]);
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
            Type[] argTypes = new Type[args.Length];
            for (int i = 0; i < args.Length; ++i)
            {
                object arg = args[i];
                if (arg != null)
                {
                    argTypes[i] = arg.GetType();
                }
            }
            return argTypes;
        }

        private string[] GetArgNames(object[] args)
        {
            string[] argNames = new string[args.Length];
            for (int i = 0; i < argNames.Length; ++i)
            {
                argNames[i] = Convert.ToString(i);
            }
            return argNames;
        }

        protected virtual string GetCompleteSql(object[] args)
        {
            if (args == null || args.Length == 0) return this.sql;
            return commandFactory.GetCompleteSql(sql, args);
        }
    }
}
