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
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;

namespace Seasar.Extension.DataSets.Impl
{
    public class XlsReader : IDataReader
    {
        private const int DEFAULT_BUFFER_SIZE = 1024 * 4;
        private static readonly Regex ILLIGAL_COLUMN_NAME_PATTERN = new Regex("^F[0-9]+$", RegexOptions.Compiled);
        private static readonly Regex WORK_SHEET_PREFIX_PATTERN = new Regex(@"^#[0-9]+\s+.+$", RegexOptions.Compiled);
        private readonly DataSet _dataSet;

        public XlsReader(string path)
        {
            string fullPath = Path.GetFullPath(path);
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("xls file not found.", fullPath);
            }
            _dataSet = CreateDataSet(fullPath);
        }

        public XlsReader(Stream stream)
        {
            string tempPath = Path.GetTempFileName();
            using (Stream fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
            {
                byte[] b = new byte[DEFAULT_BUFFER_SIZE];
                int n;
                while (0 < (n = stream.Read(b, 0, b.Length)))
                {
                    fs.Write(b, 0, n);
                }
            }
            _dataSet = CreateDataSet(tempPath);
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }

        #region IDataReader メンバ

        public DataSet Read()
        {
            return _dataSet;
        }

        #endregion

        private DataSet CreateDataSet(string path)
        {
            string connectonString = string.Format(
                "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"",
                path
                );

            DataSet ds = new DataSet();
            using (OleDbConnection con = new OleDbConnection(connectonString))
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                DataTable tableList = con.GetOleDbSchemaTable(
                    OleDbSchemaGuid.Tables_Info,
                    new object[] { null, null, null, "TABLE" }
                    );

                foreach (DataRow row in tableList.Rows)
                {
                    string tableName = (string)row["TABLE_NAME"];
                    if (!tableName.EndsWith("$") && !tableName.EndsWith("$'"))
                    {
                        continue;
                    }

                    string sql = string.Format("select * from [{0}]", tableName);
                    using (OleDbCommand cmd = new OleDbCommand(sql, con))
                    {
                        DbDataAdapter adapter = new OleDbDataAdapter(cmd);
                        adapter.AcceptChangesDuringFill = false;
                        DataTable table = new DataTable(tableName);
                        adapter.Fill(table);
                        SetupTable(table);
                        ds.Tables.Add(table);
                        //adapter.Fill(ds, _tableName.Replace("$", string.Empty));
                    }
                }
            }

            return ds;
        }

        private void SetupTable(DataTable table)
        {
            string tableName = table.TableName.Trim();
            if (tableName.EndsWith("$"))
            {
                tableName = tableName.Remove(tableName.Length - 1, 1);
            }
            if (tableName.EndsWith("$'"))
            {
                tableName = tableName.Substring(1, tableName.Length - 3);
            }
            if (WORK_SHEET_PREFIX_PATTERN.IsMatch(tableName))
            {
                tableName = tableName.Split(new char[] { ' ', '　' })[1];
            }
            table.TableName = tableName;

            int rowCount = table.Rows.Count;
            if (rowCount > 0)
            {
                SetupColumns(table.Columns);
            }
        }

        private void SetupColumns(DataColumnCollection columns)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                DataColumn column = columns[i];
                string columnName = column.ColumnName.Trim();

                bool isRemove = false;

                if (string.Empty == columnName)
                {
                    isRemove = true;
                }

                // カラム名未入力時、F01, F02 ... のカラムを取得する場合があるので無視する。
                if (ILLIGAL_COLUMN_NAME_PATTERN.IsMatch(columnName))
                {
                    isRemove = true;
                }

                if (isRemove)
                {
                    columns.Remove(column);
                    i--;
                }
            }
        }
    }
}
