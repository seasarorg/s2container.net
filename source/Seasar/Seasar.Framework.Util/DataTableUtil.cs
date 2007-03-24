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
using System.Collections;
using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Framework.Util
{
    public sealed class DataTableUtil
    {
        private DataTableUtil()
        {
        }

        public static bool IsPrimaryKey(DataTable table, DataColumn column)
        {
            return Array.IndexOf(table.PrimaryKey, column) != -1;
        }

        public static void SetupMetaData(IDatabaseMetaData dbMetaData, DataTable table)
        {
            IList primaryKeySet = dbMetaData.GetPrimaryKeySet(table.TableName);
            IList columnSet = dbMetaData.GetColumnSet(table.TableName);
            ArrayList primaryKeyList = new ArrayList();
            foreach (DataColumn column in table.Columns)
            {
                if (primaryKeySet.Contains(column.ColumnName))
                {
                    primaryKeyList.Add(column);
                }

                if (columnSet.Contains(column.ColumnName))
                {
                    column.ReadOnly = false;
                }
                else
                {
                    column.ReadOnly = true;
                }
            }

            table.BeginLoadData();
            table.PrimaryKey = (DataColumn[]) primaryKeyList.ToArray(typeof(DataColumn));
            table.EndLoadData();
        }

        public static DataColumn GetColumn(DataTable table, string columnName)
        {
            DataColumn column = table.Columns[columnName];
            if (column == null)
            {
                string name = columnName.Replace("_", string.Empty);
                column = table.Columns[name];
            }
            return column;
        }
    }
}
