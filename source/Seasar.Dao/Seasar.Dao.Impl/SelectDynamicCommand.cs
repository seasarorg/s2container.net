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

using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Dao.Impl
{
    public class SelectDynamicCommand : AbstractDynamicCommand
    {
        private readonly IDataReaderHandler _dataReaderHandler;
        private readonly IDataReaderFactory _dataReaderFactory;

        public SelectDynamicCommand(IDataSource dataSource,
            ICommandFactory commandFactory,
            IDataReaderHandler dataReaderHandler, IDataReaderFactory dataReaderFactory)
            : base(dataSource, commandFactory)
        {
            _dataReaderHandler = dataReaderHandler;
            _dataReaderFactory = dataReaderFactory;
        }

        public IDataReaderHandler DataReaderHandler
        {
            get { return _dataReaderHandler; }
        }

        public override object Execute(object[] args)
        {
            ICommandContext ctx = Apply(args);
            ISelectHandler handler = new BasicSelectHandler(DataSource,
                ctx.Sql, _dataReaderHandler, CommandFactory, _dataReaderFactory);
            return handler.Execute(ctx.BindVariables, ctx.BindVariableTypes);
        }
    }
}
