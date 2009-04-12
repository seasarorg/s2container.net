#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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

using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Extension.DataSets.States
{
    public abstract class AbstractRowState : RowState
    {
        protected AbstractRowState()
        {
        }

        #region RowState ÉÅÉìÉo

        public void Update(IDataSource dataSource, DataRow row, ICommandFactory commandFactory)
        {
            DataTable table = row.Table;
            string sql = GetSql(table);
            object[] args = GetArgs(row);
            IUpdateHandler handler = new BasicUpdateHandler(dataSource, sql, commandFactory);
            Execute(handler, args);
        }

        #endregion

        protected abstract string GetSql(DataTable table);

        protected abstract object[] GetArgs(DataRow row);

        protected void Execute(IUpdateHandler handler, object[] args)
        {
            handler.Execute(args);
        }
    }
}
