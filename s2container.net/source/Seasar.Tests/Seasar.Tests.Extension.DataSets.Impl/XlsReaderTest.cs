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
using System.IO;
using System.Reflection;
using MbUnit.Framework;
using Seasar.Extension.DataSets.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Extension.DataSets.Impl
{
	[TestFixture]
	public class XlsReaderTest : S2TestCase
	{
		private const string PATH = "Seasar.Tests.Extension.DataSets.Impl.XlsReaderImplTest.xls";

		private DataSet dataSet_;

		[SetUp]
		public void SetUp() 
		{
			using (Stream stream = ResourceUtil.GetResourceAsStream(PATH, Assembly.GetExecutingAssembly()))  
			{
				dataSet_ = new XlsReader(stream).Read();
			}
		}

		[Test]
		public void TestCreateTable() 
		{
			Assert.AreEqual(4, dataSet_.Tables.Count, "1");
			DataSetInspector.OutWriteLine(dataSet_);
		}

		[Test]
		public void TestSetupColumns() 
		{
			// Java版と違い、テーブル順でソートされている？ので、indexではなく、nameで取得。
			DataTable table = dataSet_.Tables["TEST_TABLE"];
			Assert.AreEqual(4, table.Columns.Count, "1");
			for (int i = 0; i < table.Columns.Count; ++i) 
			{
				Assert.AreEqual("COLUMN" + i, table.Columns[i].ColumnName, "2");
			}
		}

		[Test]
		public void TestSetupRows() 
		{
			DataTable table = dataSet_.Tables["TEST_TABLE"];
			Assert.AreEqual(12, table.Rows.Count, "1");
			for (int i = 0; i < table.Rows.Count; ++i) 
			{
				DataRow row = table.Rows[i];
				for (int j = 0; j < table.Columns.Count; ++j) 
				{
					Assert.AreEqual("row " + i + " col " + j, row[j], "2");
				}
			}
			DataTable table2 = dataSet_.Tables["EMPTY_TABLE"];
			Assert.AreEqual(0, table2.Rows.Count, "3");
		}

		[Test]
		public void TestGetValue() 
		{
			DataTable table = dataSet_.Tables["あ"];
			DataRow row = table.Rows[0];
			Assert.AreEqual(
				new DateTime(2004, 3, 22),
				row[0],
				"1"
				);
			Assert.AreEqual(
				123m,
				row[1],
				"2"
				);
			Assert.AreEqual(
				"\u3042",
				row[2],
				"3"
				);
		}

		[Ignore("BASE64_FORMAT未対応のため")]
		public void TestGetValueIgnore() 
		{
			DataTable table = dataSet_.Tables["あ"];
			DataRow row = table.Rows[0];
			Assert.AreEqual(
				"YWJj",
				Convert.ToBase64String((byte[]) row[3]),
				"4"
				);
		}
		
		[Test]
		public void TestDataRowState() 
		{
			DataTable ret = dataSet_.Tables["TEST_TABLE"];
			Assert.AreEqual(DataRowState.Added, ret.Rows[0].RowState);
		}
	}
}
