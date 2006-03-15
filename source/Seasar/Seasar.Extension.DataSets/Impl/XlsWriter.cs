#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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

namespace Seasar.Extension.DataSets.Impl
{
	public class XlsWriter : IDataWriter
	{
		private string path_;

		public XlsWriter(string path)
		{
			path_ = Path.GetFullPath(path);
		}

		#region IDataWriter メンバ

		public void Write(DataSet dataSet)
		{
			string connectonString = string.Format(
				"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes\"",
				path_
				);

			using (OleDbConnection con = new OleDbConnection(connectonString)) 
			{
				if (con.State != ConnectionState.Open) 
				{
					con.Open();
				}

				foreach (DataTable table in dataSet.Tables) 
				{
					string createTableSql = CreateTableSql(table);
					using (OleDbCommand cmd = new OleDbCommand(createTableSql, con)) 
					{
						cmd.ExecuteNonQuery();
					}

					string insertSql = CreateInsertSql(table);
					using (OleDbCommand cmd = new OleDbCommand(insertSql, con)) 
					{
						foreach (DataRow row in table.Rows) 
						{
							cmd.Parameters.Clear();
							foreach (DataColumn column in row.Table.Columns) 
							{
								IDbDataParameter param = cmd.CreateParameter();
								param.ParameterName = "@" + column.ColumnName;
								param.Value = row[column.ColumnName];
								param.DbType = ConvertColumnDbType(column.DataType);

								cmd.Parameters.Add(param);
							}
							cmd.ExecuteNonQuery();
						}
					}
				}
			}
		}

		#endregion

		private string CreateTableSql(DataTable targetTable) 
		{
			StringBuilder sql = new StringBuilder();

			sql.AppendFormat("create table [{0}] (", targetTable.TableName);

			foreach (DataColumn column in targetTable.Columns) 
			{
				sql.AppendFormat(
					"{0} {1},",
					column.ColumnName,
					ConvertColumnDataType(column.DataType)
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

		private string CreateInsertSql(DataTable targetTable) 
		{
			StringBuilder sql = new StringBuilder();
			StringBuilder sqlParam = new StringBuilder();

			sql.AppendFormat("insert into [{0}$] (", targetTable.TableName);

			foreach (DataColumn column in targetTable.Columns) 
			{
				sql.Append(column.ColumnName);
				sql.Append(",");

				sqlParam.Append("?,");
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

		private string ConvertColumnDataType(Type dataType) 
		{
			string result = null;

			if (dataType == typeof(bool)) 
			{
				result = "BINARY";
			}
			else if (dataType == typeof(byte)) 
			{
				result = "BYTE";
			}
			else if (dataType == typeof(char)) 
			{
				result = "CHAR";
			}
			else if (dataType == typeof(string)) 
			{
				result = "VARCHAR";
			}
			else if (dataType == typeof(DateTime)) 
			{
				result = "DATETIME";
			}
			else if (dataType == typeof(TimeSpan)) 
			{
				result = "DATETIME";
			}
			else if (dataType == typeof(float)) 
			{
				result = "SINGLE";
			}
			else if (dataType == typeof(double)) 
			{
				result = "DOUBLE";
			}
			else if (dataType == typeof(short)) 
			{
				result = "SHORT";
			}
			else if (dataType == typeof(int)) 
			{
				result = "LONG";
			}
			else if (dataType == typeof(long)) 
			{
				result = "LONG";
			}
			else if (dataType == typeof(decimal)) 
			{
				result = "CURRENCY";	/// TODO: オーバフローしないかチェックすること。
			} 
			else 
			{
				result = "LONGBINARY";
			}

			return result;
		}

		private DbType ConvertColumnDbType(Type dataType) 
		{
			DbType result;

			if (dataType == typeof(bool)) 
			{
				result = DbType.Binary;
			}
			else if (dataType == typeof(byte)) 
			{
				result = DbType.Byte;
			}
			else if (dataType == typeof(char)) 
			{
				result = DbType.String;
			}
			else if (dataType == typeof(string)) 
			{
				result = DbType.String;
			}
			else if (dataType == typeof(DateTime)) 
			{
				result = DbType.DateTime;
			}
			else if (dataType == typeof(TimeSpan)) 
			{
				result = DbType.DateTime;
			}
			else if (dataType == typeof(float)) 
			{
				result = DbType.Single;
			}
			else if (dataType == typeof(double)) 
			{
				result = DbType.Double;
			}
			else if (dataType == typeof(short)) 
			{
				result = DbType.Int16;
			}
			else if (dataType == typeof(int)) 
			{
				result = DbType.Int32;
			}
			else if (dataType == typeof(long)) 
			{
				result = DbType.Int64;
			}
			else if (dataType == typeof(decimal)) 
			{
				result = DbType.Decimal;
			} 
			else 
			{
				result = DbType.Object;
			}

			return result;
		}
	}
}
