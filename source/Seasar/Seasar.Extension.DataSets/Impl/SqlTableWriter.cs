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

using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.DataSets.States;
using Seasar.Framework.Util;

namespace Seasar.Extension.DataSets.Impl
{
    public class SqlTableWriter : ITableWriter
    {
        private IDataSource dataSource_;
        private ICommandFactory commandFactory_ = BasicCommandFactory.INSTANCE;

        public SqlTableWriter(IDataSource dataSource)
            : this(dataSource, BasicCommandFactory.INSTANCE)
        {
        }

        public SqlTableWriter(IDataSource dataSource, ICommandFactory commandFactory)
        {
            dataSource_ = dataSource;
            commandFactory_ = commandFactory;
        }

        public IDataSource DataSource
        {
            get { return dataSource_; }
        }

        public ICommandFactory CommandFactory
        {
            get { return commandFactory_; }
            set { commandFactory_ = value; }
        }

        #region ITableWriter ÉÅÉìÉo

        public virtual void Write(DataTable table)
        {
            SetupMetaData(table);

            DoWrite(table);
        }

        #endregion

        protected virtual void BeginDoWrite(DataTable table)
        {
        }

        protected virtual void DoWrite(DataTable table)
        {
            try
            {
                BeginDoWrite(table);

                foreach (DataRow row in table.Rows)
                {
                    RowState state = RowStateFactory.GetRowState(row.RowState);
                    state.Update(DataSource, row, CommandFactory);
                }
                table.AcceptChanges();
            }
            finally
            {
                EndDoWrite(table);
            }
        }

        protected virtual void EndDoWrite(DataTable table)
        {
        }

        private void SetupMetaData(DataTable table)
        {
            IDatabaseMetaData dbMetaData = new DatabaseMetaDataImpl(DataSource);
            DataTableUtil.SetupMetaData(dbMetaData, table);
        }
    }
}
