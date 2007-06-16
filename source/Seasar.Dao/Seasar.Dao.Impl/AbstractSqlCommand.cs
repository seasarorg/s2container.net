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
using Seasar.Extension.ADO;

namespace Seasar.Dao.Impl
{
    public abstract class AbstractSqlCommand : ISqlCommand
    {
        private readonly IDataSource _dataSource;
        private readonly ICommandFactory _commandFactory;
        private string _sql;
        private Type _notSingleRowUpdatedExceptionType;

        public AbstractSqlCommand(IDataSource dataSource,
            ICommandFactory commandFactory)
        {
            _dataSource = dataSource;
            _commandFactory = commandFactory;
        }

        public IDataSource DataSource
        {
            get { return _dataSource; }
        }

        public ICommandFactory CommandFactory
        {
            get { return _commandFactory; }
        }

        public virtual string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }

        public Type NotSingleRowUpdatedExceptionType
        {
            get { return _notSingleRowUpdatedExceptionType; }
            set { _notSingleRowUpdatedExceptionType = value; }
        }

        #region ISqlCommand ÉÅÉìÉo

        public abstract object Execute(object[] args);

        #endregion
    }
}
