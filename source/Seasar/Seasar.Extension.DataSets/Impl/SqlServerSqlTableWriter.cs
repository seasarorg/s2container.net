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

using System.Collections;
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Extension.DataSets.Impl
{
    public class SqlServerSqlTableWriter : SqlTableWriter
    {
        public SqlServerSqlTableWriter(IDataSource dataSource)
            : base(dataSource)
        {
        }

        public SqlServerSqlTableWriter(IDataSource dataSource, ICommandFactory commandFactory)
            : base(dataSource, commandFactory)
        {
        }

        protected override void BeginDoWrite(DataTable table)
        {
            if (HasIdentityColumn(table))
            {
                ExecuteSql(string.Format("SET IDENTITY_INSERT {0} ON", table.TableName));
            }
        }

        protected override void EndDoWrite(DataTable table)
        {
            if (HasIdentityColumn(table))
            {
                ExecuteSql(string.Format("SET IDENTITY_INSERT {0} OFF", table.TableName));
            }
        }

        private bool HasIdentityColumn(DataTable table)
        {
            IDatabaseMetaData dbMetaData = new DatabaseMetaDataImpl(DataSource);
            IList autoIncrementColumns = dbMetaData.GetAutoIncrementColumnSet(table.TableName);
            if (autoIncrementColumns.Count == 0)
            {
                return false;
            }
            foreach (DataColumn column in table.Columns)
            {
                if (autoIncrementColumns.Contains(column.ColumnName))
                {
                    return true;
                }
            }
            return false;
        }

        private void ExecuteSql(string sql)
        {
            IUpdateHandler handler = new BasicUpdateHandler(DataSource, sql, CommandFactory);
            handler.Execute(null);
        }
    }
}