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
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using Seasar.Extension.ADO.Types;
using Seasar.Extension.DataSets.Types;
using Seasar.Framework.Util;

namespace Seasar.Extension.DataSets.Impl
{
    public class XlsWriter : IDataWriter
    {
        public XlsWriter(string fullPath)
        {
            FullPath = Path.GetFullPath(fullPath);
        }

        public string FullPath { get; }

        #region IDataWriter ƒƒ“ƒo

        public virtual void Write(DataSet dataSet)
        {
            string connectonString =
                $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={FullPath};Extended Properties=\"Excel 8.0;HDR=Yes\"";
            _CreateDir();
            if (File.Exists(FullPath))
            {
                File.Delete(FullPath);
            }
            using (var con = new OleDbConnection(connectonString))
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                foreach (DataTable table in dataSet.Tables)
                {
                    var createTableSql = _CreateTableSql(table);
                    using (var cmd = new OleDbCommand(createTableSql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    foreach (DataRow row in table.Rows)
                    {
                        var insertSql = CreateInsertSql(table, row);
                        using (var cmd = new OleDbCommand(insertSql, con))
                        {
                            foreach (DataColumn column in row.Table.Columns)
                            {
                                var columnType = ColumnTypes.GetColumnType(column.DataType);
                                var value = columnType.Convert(row[column.ColumnName], null);
                                if (value != DBNull.Value)
                                {
                                    IDbDataParameter param = cmd.CreateParameter();
                                    param.ParameterName = "@" + column.ColumnName;
                                    var dbType = columnType.GetDbType();
                                    param.DbType = (dbType == DbType.Binary) ? DbType.String : dbType;
                                    param.Value = _ConvertValue(value);
                                    cmd.Parameters.Add(param);
                                }
                            }
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        #endregion

        private string _CreateTableSql(DataTable targetTable)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("create table [{0}] (", targetTable.TableName);
            foreach (DataColumn column in targetTable.Columns)
            {
                sql.AppendFormat(
                    "{0} {1},",
                    column.ColumnName,
                    ColumnTypes.GetColumnType(column.DataType).ToDbTypeString()
                    );
            }
            if (sql.Length > 0)
            {
                // remove last comma
                sql.Length = sql.Length - 1;
            }
            sql.Append(")");
            return sql.ToString();
        }

        private string CreateInsertSql(DataTable targetTable, DataRow row)
        {
            var sql = new StringBuilder();
            var sqlParam = new StringBuilder();
            sql.AppendFormat("insert into [{0}$] (", targetTable.TableName);
            foreach (DataColumn column in targetTable.Columns)
            {
                if (row[column] != DBNull.Value)
                {
                    sql.Append(column.ColumnName);
                    sql.Append(",");
                    sqlParam.Append("?,");
                }
            }

            if (sql.Length > 0)
            {
                // remove last comma
                sql.Length = sql.Length - 1;
            }
            if (sqlParam.Length > 0)
            {
                // remove last comma
                sqlParam.Length = sqlParam.Length - 1;
            }
            sql.Append(") VALUES (");
            sql.Append(sqlParam);
            sql.Append(")");
            return sql.ToString();
        }

        private object _ConvertValue(object value)
        {
            object ret = null;
            if (value.GetExType() == ValueTypes.BYTE_ARRAY_TYPE)
            {
                if (value != null) ret = DataSetConstants.BASE64_FORMAT + Convert.ToBase64String(value as byte[]);
            }
            else
            {
                ret = value;
            }
            return ret;
        }

        private void _CreateDir()
        {
            var dir = Path.GetDirectoryName(FullPath);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
