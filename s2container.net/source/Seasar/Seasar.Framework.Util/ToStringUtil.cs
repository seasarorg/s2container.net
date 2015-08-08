#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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

namespace Seasar.Framework.Util
{
    public sealed class ToStringUtil
    {
        private ToStringUtil()
        {
        }

        public static string ToString(object target)
        {
            if (target == null)
            {
                return "null";
            }

            string ret;
            if (target is IDictionary)
            {
                ret = ToString(target as IDictionary);
            }
            else if (target is IList)
            {
                ret = ToString(target as IList);
            }
            else if (target is DataSet)
            {
                ret = ToString(target as DataSet);
            }
            else if (target is DataTable)
            {
                ret = ToString(target as DataTable);
            }
            else
            {
                ret = target.ToString();
            }
            return ret;
        }

        public static string ToString(IDictionary target)
        {
            if (target == null)
            {
                return "null";
            }

            var buf = new StringBuilder();

            buf.Append("{");
            foreach (var key in target.Keys)
            {
                buf.Append($"{ToString(key)}={ToString(target[key])}, ");
            }
            if (target.Keys.Count > 0)
            {
                buf.Length -= 2;
            }
            buf.Append("}");

            return buf.ToString();
        }

        public static string ToString(IList target)
        {
            if (target == null)
            {
                return "null";
            }

            var buf = new StringBuilder();
            buf.Append("{");
            foreach (var o in target)
            {
                buf.Append(ToString(o));
                buf.Append(", ");
            }
            if (target.Count > 0)
            {
                buf.Length -= 2;
            }
            buf.Append("}");

            return buf.ToString();
        }

        public static string ToString(DataSet target)
        {
            if (target == null)
            {
                return "null";
            }

            var buf = new StringBuilder();

            foreach (DataTable table in target.Tables)
            {
                buf.Append(ToString(table));
                buf.Append(Environment.NewLine);
            }

            return buf.ToString();
        }

        public static string ToString(DataTable target)
        {
            if (target == null)
            {
                return "null";
            }

            var buf = new StringBuilder();

            buf.AppendFormat(target.TableName);
            buf.Append(Environment.NewLine);

            var tableRows = target.Rows;
            var tableColumns = target.Columns;
            for (var ctrRow = 0; ctrRow < tableRows.Count; ctrRow++)
            {
                var row = tableRows[ctrRow];
                buf.Append($"Row #{(ctrRow + 1)}-");
                buf.Append(Environment.NewLine);
                var rowItems = row.ItemArray;

                for (var ctrColumn = 0; ctrColumn < tableColumns.Count; ctrColumn++)
                {
                    var column = tableColumns[ctrColumn];
                    buf.Append($"\t{column.ColumnName}: {rowItems[ctrColumn]}");
                    buf.Append(Environment.NewLine);
                }
            }
            buf.Append(Environment.NewLine);

            return buf.ToString();
        }
    }
}
