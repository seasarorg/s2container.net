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

using System.Collections;
using System.Data;
using MbUnit.Framework;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.Unit
{
	[TestFixture]
	public class S2AssertTest : S2TestCase
	{
		[Test]
		public void TestAreDictionaryEqual()
		{
			DataSet expected = new DataSet();
			DataTable table = expected.Tables.Add("emp");
			table.Columns.Add("empno", typeof(long));
			DataRow row = table.NewRow();
			row["empno"] = 7788;
			table.Rows.Add(row);
			IDictionary dictionary = new Hashtable();
			dictionary.Add("EMPNO", 7788);
			S2Assert.AreEqual(expected, dictionary, "1");
		}

		[Test]
		public void TestAreDictionaryListEqual()
		{
			DataSet expected = new DataSet();
			DataTable table = expected.Tables.Add("emp");
			table.Columns.Add("empno", typeof(long));
			DataRow row = table.NewRow();
			row["empno"] = 7788;
			table.Rows.Add(row);
			IDictionary dictionary = new Hashtable();
			dictionary.Add("EMPNO", 7788);
			IList list = new ArrayList();
			list.Add(dictionary);
			S2Assert.AreEqual(expected, list, "1");
		}

		[Test]
		public void TestAreBeanEqual()
		{
			DataSet expected = new DataSet();
			DataTable table = expected.Tables.Add("emp");
			table.Columns.Add("aaa", typeof(string));
			DataRow row = table.NewRow();
			row["aaa"] = "111";
			table.Rows.Add(row);
			Hoge bean = new Hoge();
			bean.Aaa = "111";
			S2Assert.AreEqual(expected, bean, "1");
		}

		[Test]
		public void TestAreBeanListEqual()
		{
			DataSet expected = new DataSet();
			DataTable table = expected.Tables.Add("emp");
			table.Columns.Add("aaa", typeof(string));
			DataRow row = table.NewRow();
			row["aaa"] = "111";
			table.Rows.Add(row);
			Hoge bean = new Hoge();
			bean.Aaa = "111";
			IList list = new ArrayList();
			list.Add(bean);
			S2Assert.AreEqual(expected, bean, "1");
		}

		[Test]
		public void TestAreBeanEqual2()
		{
			DataSet expected = new DataSet();
			DataTable table = expected.Tables.Add("emp");
			table.Columns.Add("a_aa", typeof(string));
			DataRow row = table.NewRow();
			row["a_aa"] = "111";
			table.Rows.Add(row);
			Hoge bean = new Hoge();
			bean.Aaa = "111";
			S2Assert.AreEqual(expected, bean, "1");	
		}

		[Test]
		public void TestAreEqualForNull()
		{
			object hoge = null;
			S2Assert.AreEqual(null, hoge, "1");
		}

		internal class Hoge 
		{
			private string aaa;

			public string Aaa 
			{
				get { return aaa; }
				set { aaa = value; }
			}
		}
	}
}
