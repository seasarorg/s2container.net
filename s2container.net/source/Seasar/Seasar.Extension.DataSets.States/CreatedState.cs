#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

namespace Seasar.Extension.DataSets.States
{
    public class CreatedState : AbstractRowState
    {
        private static readonly Hashtable _sqlCache = new Hashtable();

        public override string ToString()
        {
            return DataRowState.Added.ToString();
        }

        protected override string GetSql(DataTable table)
        {
            string sql;
            WeakReference reference = (WeakReference) _sqlCache[table];
            if (reference == null || !reference.IsAlive)
            {
                sql = CreateSql(table);
                _sqlCache[table] = new WeakReference(sql);
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
            StringBuilder paramBuf = new StringBuilder(100);
            buf.Append("INSERT INTO ");
            buf.Append(table.TableName);
            buf.Append(" (");
            foreach (DataColumn column in table.Columns)
            {
                if (!column.ReadOnly)
                {
                    buf.Append(column.ColumnName);
                    buf.Append(", ");

                    paramBuf.Append("@");
                    paramBuf.Append(column.ColumnName);
                    paramBuf.Append(", ");
                }
            }
            buf.Length -= 2;
            buf.Append(") VALUES (");

            paramBuf.Length -= 2;
            paramBuf.Append(")");

            buf.Append(paramBuf);

            return buf.ToString();
        }

        protected override object[] GetArgs(DataRow row)
        {
            DataTable table = row.Table;
            ArrayList bindVariables = new ArrayList();
            for (int i = 0; i < table.Columns.Count; ++i)
            {
                DataColumn column = table.Columns[i];
                if (!column.ReadOnly)
                {
                    bindVariables.Add(row[i]);
                }
            }
            return bindVariables.ToArray();
        }
    }
}
