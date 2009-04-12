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

using System;
using System.Collections;
using System.Data;
using System.Text;
using Seasar.Framework.Util;

namespace Seasar.Extension.DataSets.States
{
    public class RemovedState : AbstractRowState
    {
        private static readonly Hashtable _sqlCache = new Hashtable();

        public override string ToString()
        {
            return DataRowState.Deleted.ToString();
        }

        protected override string GetSql(DataTable table)
        {
            string sql;
            WeakReference reference = (WeakReference) _sqlCache[table];
            if (reference == null || !reference.IsAlive)
            {
                sql = CreateSql(table);
                _sqlCache.Add(table, new WeakReference(sql));
            }
            else
            {
                sql = (string) reference.Target;
            }
            return sql;
        }

        private static string CreateSql(DataTable table)
        {
            StringBuilder buf = new StringBuilder(100);
            buf.Append("DELETE FROM ");
            buf.Append(table.TableName);
            buf.Append(" WHERE ");
            foreach (DataColumn column in table.Columns)
            {
                if (DataTableUtil.IsPrimaryKey(table, column))
                {
                    buf.Append(column.ColumnName);
                    buf.AppendFormat(" = @{0} AND ", column.ColumnName);
                }
            }
            buf.Length -= 5;

            return buf.ToString();
        }

        protected override object[] GetArgs(DataRow row)
        {
            DataTable table = row.Table;
            ArrayList bindVariables = new ArrayList();
            for (int i = 0; i < table.Columns.Count; ++i)
            {
                DataColumn column = table.Columns[i];
                if (DataTableUtil.IsPrimaryKey(table, column))
                {
                    if (row.RowState == DataRowState.Detached)
                    {
                        bindVariables.Add(row[i, DataRowVersion.Proposed]);
                    }
                    else if (row.RowState == DataRowState.Deleted)
                    {
                        bindVariables.Add(row[i, DataRowVersion.Original]);
                    }
                    else
                    {
                        bindVariables.Add(row[i, DataRowVersion.Current]);
                    }
                }
            }
            return bindVariables.ToArray();
        }
    }
}
