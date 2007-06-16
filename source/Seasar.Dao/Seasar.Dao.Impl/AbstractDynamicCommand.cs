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
using Seasar.Dao.Context;
using Seasar.Dao.Parser;
using Seasar.Extension.ADO;

namespace Seasar.Dao.Impl
{
    public abstract class AbstractDynamicCommand : AbstractSqlCommand
    {
        private INode _rootNode;
        private string[] _argNames = new string[0];
        private Type[] _argTypes = new Type[0];

        public AbstractDynamicCommand(IDataSource dataSource, ICommandFactory commandFactory)
            : base(dataSource, commandFactory)
        {
        }

        public override string Sql
        {
            get { return base.Sql; }
            set
            {
                base.Sql = value;
                _rootNode = new SqlParserImpl(value).Parse();
            }
        }

        public string[] ArgNames
        {
            get { return _argNames; }
            set { _argNames = value; }
        }

        public Type[] ArgTypes
        {
            get { return _argTypes; }
            set { _argTypes = value; }
        }

        protected ICommandContext Apply(object[] args)
        {
            ICommandContext ctx = CreateCommandContext(args);
            _rootNode.Accept(ctx);
            return ctx;
        }

        protected ICommandContext CreateCommandContext(object[] args)
        {
            ICommandContext ctx = GetCommandContext();
            if (args != null)
            {
                for (int i = 0; i < args.Length; ++i)
                {
                    Type argType = null;
                    if (args[i] != null)
                    {
                        if (i < _argTypes.Length)
                            argType = _argTypes[i];
                        else if (args[i] != null)
                            argType = args[i].GetType();
                    }
                    if (i < _argNames.Length)
                        ctx.AddArg(_argNames[i], args[i], argType);
                    else
                        ctx.AddArg("$" + (i + 1), args[i], argType);
                }
            }
            return ctx;
        }

        private ICommandContext GetCommandContext()
        {
            return new CommandContextImpl();
        }
    }
}
