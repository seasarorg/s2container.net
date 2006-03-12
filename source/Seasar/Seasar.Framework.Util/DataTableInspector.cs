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
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Seasar.Framework.Log;

namespace Seasar.Framework.Util
{
	/// <summary>
	/// DataTableInspector ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class DataTableInspector
	{
		private static readonly Logger Log = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private DataTableInspector()
		{
		}

		public static string ToString(DataTable dataTable)
		{
			StringBuilder result = new StringBuilder();

			result.AppendFormat("{0}\r\n", dataTable.TableName);

			DataRowCollection tableRows = dataTable.Rows;
			DataColumnCollection tableColumns = dataTable.Columns;

			for (int ctrRow = 0; ctrRow < tableRows.Count; ctrRow++)
			{
				DataRow row = tableRows[ctrRow] as DataRow;
				result.AppendFormat("Row #{0}-\r\n", ctrRow + 1);
				object[] rowItems = row.ItemArray;

				for (int ctrColumn = 0; ctrColumn < tableColumns.Count; ctrColumn++)
				{
					DataColumn column = tableColumns[ctrColumn] as DataColumn;
					result.AppendFormat("\t{0}: {1}\r\n", column.ColumnName,
					                    rowItems[ctrColumn].ToString());
				}
			}
			result.Append("\r\n");
			return result.ToString();
		}

		public static void DebugLog(DataTable dataTable, string header)
		{
			string output = string.Format("{0}\r\n{1}", header,
				DataTableInspector.ToString(dataTable));
			Log.Debug(output);
		}

		public static void DebugLog(DataTable dataTable)
		{
			Log.Debug(DataTableInspector.ToString(dataTable));
		}

		public static void DebugWriteLine(DataTable dataTable, string header)
		{
			string output = string.Format("{0}\r\n{1}", header,
			                              DataTableInspector.ToString(dataTable));
			Debug.WriteLine(output);
		}

		public static void DebugWriteLine(DataTable dataTable)
		{
			Debug.WriteLine(DataTableInspector.ToString(dataTable));
		}

		public static void OutWriteLine(DataTable dataTable, string header)
		{
			string output = string.Format("{0}\r\n{1}", header,
				DataTableInspector.ToString(dataTable));
			Console.Out.WriteLine(output);
		}

		public static void OutWriteLine(DataTable dataTable)
		{
			Console.Out.WriteLine(DataTableInspector.ToString(dataTable));
		}

		public static void TraceWriteLine(DataTable dataTable, string header)
		{
			string output = string.Format("{0}\r\n{1}", header,
			                              DataTableInspector.ToString(dataTable));
			Trace.WriteLine(output);
		}

		public static void TraceWriteLine(DataTable dataTable)
		{
			Trace.WriteLine(DataTableInspector.ToString(dataTable));
		}
	}
}