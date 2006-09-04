#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
	public class DataSetInspector
	{
		private static readonly Logger Log = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private DataSetInspector()
		{
		}

        [Obsolete("ToStringUtil.ToString()")]
        public static string ToString(DataSet dataSet)
		{
			StringBuilder result = new StringBuilder();

			foreach (DataTable table in dataSet.Tables)
			{	result.Append(DataTableInspector.ToString(table));
				result.Append("\r\n");
			}
			return result.ToString();
		}

        [Obsolete("ToStringUtil.ToString()")]
        public static void DebugLog(DataSet dataSet, string header)
		{
			string output = string.Format("{0}\r\n{1}", header,
				DataSetInspector.ToString(dataSet));
			Log.Debug(output);
		}

        [Obsolete("ToStringUtil.ToString()")]
        public static void DebugLog(DataSet dataSet)
		{
			Log.Debug(DataSetInspector.ToString(dataSet));
		}

        [Obsolete("ToStringUtil.ToString()")]
        public static void OutWriteLine(DataSet dataSet, string header)
		{
			string output = string.Format("{0}\r\n{1}", header,
				DataSetInspector.ToString(dataSet));
			Console.Out.WriteLine(output);
		}

        [Obsolete("ToStringUtil.ToString()")]
        public static void OutWriteLine(DataSet dataSet)
		{
			Console.Out.WriteLine(DataSetInspector.ToString(dataSet));
            System.Diagnostics.Trace.WriteLine(DataSetInspector.ToString(dataSet));
		}

        [Obsolete("ToStringUtil.ToString()")]
        public static void DebugWriteLine(DataSet dataSet, string header)
		{
			string output = string.Format("{0}\r\n{1}", header,
				DataSetInspector.ToString(dataSet));
			Debug.WriteLine(output);
		}

        [Obsolete("ToStringUtil.ToString()")]
        public static void DebugWriteLine(DataSet dataSet)
		{
			Debug.WriteLine(DataSetInspector.ToString(dataSet));
		}

        [Obsolete("ToStringUtil.ToString()")]
        public static void TraceWriteLine(DataSet dataSet, string header)
		{
			string output = string.Format("{0}\r\n{1}", header,
				DataSetInspector.ToString(dataSet));
			Trace.WriteLine(output);
		}

        [Obsolete("ToStringUtil.ToString()")]
        public static void TraceWriteLine(DataSet dataSet)
		{
			Trace.WriteLine(DataSetInspector.ToString(dataSet));
		}
	}
}
