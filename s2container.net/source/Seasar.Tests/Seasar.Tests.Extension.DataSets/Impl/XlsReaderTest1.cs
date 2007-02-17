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
using System.Data;
using System.IO;
using System.Reflection;
using MbUnit.Framework;
using Seasar.Extension.DataSets.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;
using IDataReader = Seasar.Extension.DataSets.IDataReader;

namespace Seasar.Tests.Extension.DataSets.Impl
{
	[TestFixture]
	public class XlsReaderTest1 : S2TestCase
	{
		private const string PATH = "Seasar.Tests.Extension.DataSets.Impl.XlsReaderTest1.xls";

		[Test]
		public void TestRead() 
		{
			using (Stream stream = ResourceUtil.GetResourceAsStream(PATH, Assembly.GetExecutingAssembly())) 
			{
				IDataReader reader = new XlsReader(stream);
				DataSet dataSet = reader.Read();

				Assert.AreEqual(1, dataSet.Tables.Count);

				// type valid
				Assert.AreEqual(6, dataSet.Tables["Type"].Columns.Count);
				Assert.AreEqual(4, dataSet.Tables["Type"].Rows.Count);

				Assert.AreEqual(typeof(string), dataSet.Tables["Type"].Columns["stringValue"].DataType);
				Assert.AreEqual(typeof(double), dataSet.Tables["Type"].Columns["intValue"].DataType);
				Assert.AreEqual(typeof(double), dataSet.Tables["Type"].Columns["longValue"].DataType);
				Assert.AreEqual(typeof(double), dataSet.Tables["Type"].Columns["decimalValue"].DataType);
				Assert.AreEqual(typeof(double), dataSet.Tables["Type"].Columns["floatValue"].DataType);
				Assert.AreEqual(typeof(DateTime), dataSet.Tables["Type"].Columns["dateTimeValue"].DataType);

				Assert.AreEqual("stringValue", dataSet.Tables["Type"].Columns["stringValue"].ColumnName);
				Assert.AreEqual("intValue", dataSet.Tables["Type"].Columns["intValue"].ColumnName);
				Assert.AreEqual("longValue", dataSet.Tables["Type"].Columns["longValue"].ColumnName);
				Assert.AreEqual("decimalValue", dataSet.Tables["Type"].Columns["decimalValue"].ColumnName);
				Assert.AreEqual("floatValue", dataSet.Tables["Type"].Columns["floatValue"].ColumnName);
				Assert.AreEqual("dateTimeValue", dataSet.Tables["Type"].Columns["dateTimeValue"].ColumnName);

				Assert.AreEqual("a", dataSet.Tables["Type"].Rows[0]["stringValue"]);
				Assert.AreEqual(int.MinValue, dataSet.Tables["Type"].Rows[0]["intValue"]);
				Assert.AreEqual(
					-999999999999999,
					dataSet.Tables["Type"].Rows[0]["longValue"],
					"Excelの有効桁数(15桁)まで認識すれば、とりあえずOK。指数の場合は、保留。"
					);
				Assert.AreEqual(
					-999999999999999m,
					dataSet.Tables["Type"].Rows[0]["decimalValue"],
					"Excelの有効桁数(15桁)まで認識すれば、とりあえずOK。指数の場合は、保留。"
					);
				Assert.AreEqual(float.MinValue, dataSet.Tables["Type"].Rows[0]["floatValue"]);
				Assert.AreEqual(
					new DateTime(1900, 1, 1, 0, 0, 0),
					dataSet.Tables["Type"].Rows[0]["dateTimeValue"],
					"Excelの計算で使用できる最も古い日付。"
					);

				Assert.AreEqual("日本語～", dataSet.Tables["Type"].Rows[1]["stringValue"]);
				Assert.AreEqual(0, dataSet.Tables["Type"].Rows[1]["intValue"]);
				Assert.AreEqual(0, dataSet.Tables["Type"].Rows[1]["longValue"]);
				Assert.AreEqual(0m, dataSet.Tables["Type"].Rows[1]["decimalValue"]);
				Assert.AreEqual(0f, dataSet.Tables["Type"].Rows[1]["floatValue"]);
				Assert.AreEqual(
					new DateTime(1999, 12, 31, 23, 59, 59),
					dataSet.Tables["Type"].Rows[1]["dateTimeValue"]
					);

				Assert.AreEqual(DBNull.Value, dataSet.Tables["Type"].Rows[2]["stringValue"]);
				Assert.AreEqual(DBNull.Value, dataSet.Tables["Type"].Rows[2]["intValue"]);
				Assert.AreEqual(DBNull.Value, dataSet.Tables["Type"].Rows[2]["longValue"]);
				Assert.AreEqual(DBNull.Value, dataSet.Tables["Type"].Rows[2]["decimalValue"]);
				Assert.AreEqual(DBNull.Value, dataSet.Tables["Type"].Rows[2]["floatValue"]);
				Assert.AreEqual(DBNull.Value, dataSet.Tables["Type"].Rows[2]["dateTimeValue"]);

				Assert.AreEqual(
					string.Empty.PadLeft(32767, '*'),
					dataSet.Tables["Type"].Rows[3]["stringValue"],
					"セルに入力できる最大文字数は、32767文字。※表示できるのは1024文字。"
					);
				Assert.AreEqual(int.MaxValue, dataSet.Tables["Type"].Rows[3]["intValue"]);
				Assert.AreEqual(
					999999999999999,
					dataSet.Tables["Type"].Rows[3]["longValue"]
					);
				Assert.AreEqual(
					999999999999999m,
					dataSet.Tables["Type"].Rows[3]["decimalValue"]
					);
				Assert.AreEqual(float.MaxValue, dataSet.Tables["Type"].Rows[3]["floatValue"]);
				Assert.AreEqual(
					new DateTime(9999, 12, 31, 23, 59, 59),
					dataSet.Tables["Type"].Rows[3]["dateTimeValue"],
					"Excelの計算で使用できる最も新しい日付。"
					);
			}
		}
	}
}
