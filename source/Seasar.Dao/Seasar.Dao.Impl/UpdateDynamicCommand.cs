#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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

using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Dao.Impl
{
    public class UpdateDynamicCommand : AbstractDynamicCommand
    {
        public UpdateDynamicCommand(IDataSource dataSource, ICommandFactory commandFactory)
            : base(dataSource, commandFactory)
        {
        }

        public override object Execute(object[] args)
        {
            ICommandContext ctx = Apply(args);
            IUpdateHandler handler = new BasicUpdateHandler(
                DataSource, ctx.Sql, CommandFactory);
            return handler.Execute(ctx.BindVariables, ctx.BindVariableTypes);
        }
    }
}