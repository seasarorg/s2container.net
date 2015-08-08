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

using System.Collections;
using System.Data;
using System.Data.SqlTypes;
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
            var expected = new DataSet();
            var table = expected.Tables.Add("emp");
            table.Columns.Add("empno", typeof(long));
            var row = table.NewRow();
            row["empno"] = 7788;
            table.Rows.Add(row);
            IDictionary dictionary = new Hashtable();
            dictionary.Add("EMPNO", 7788);
            S2Assert.AreEqual(expected, dictionary, "1");
        }

        [Test]
        public void TestAreDictionaryListEqual()
        {
            var expected = new DataSet();
            var table = expected.Tables.Add("emp");
            table.Columns.Add("empno", typeof(long));
            var row = table.NewRow();
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
            var expected = new DataSet();
            var table = expected.Tables.Add("emp");
            table.Columns.Add("aaa", typeof(string));
            table.Columns.Add("bbb", typeof(decimal));
            var row = table.NewRow();
            row["aaa"] = "111";
            row["bbb"] = decimal.MaxValue;
            table.Rows.Add(row);
            var bean = new Hoge();
            bean.Aaa = "111";
            bean.Bbb = decimal.MaxValue;
            S2Assert.AreEqual(expected, bean, "1");
        }

        [Test]
        public void TestAreBeanEqual3()
        {
            var expected = new DataSet();
            var table = expected.Tables.Add("emp");
            table.Columns.Add("aaa", typeof(string));
            table.Columns.Add("bbb", typeof(int));
            var row = table.NewRow();
            row["aaa"] = "111";
            row["bbb"] = int.MaxValue;
            table.Rows.Add(row);
            var bean = new Hoge();
            bean.Aaa = "111";
            bean.Bbb = int.MaxValue;
            S2Assert.AreEqual(expected, bean, "1");
        }

        [Test]
        public void TestAreBeanListEqual()
        {
            var expected = new DataSet();
            var table = expected.Tables.Add("emp");
            table.Columns.Add("aaa", typeof(string));
            table.Columns.Add("bbb", typeof(decimal));
            var row = table.NewRow();
            row["aaa"] = "111";
            row["bbb"] = decimal.MaxValue;
            table.Rows.Add(row);
            var bean = new Hoge();
            bean.Aaa = "111";
            bean.Bbb = decimal.MaxValue;
            IList list = new ArrayList();
            list.Add(bean);
            S2Assert.AreEqual(expected, bean, "1");
        }

        [Test]
        public void TestAreBeanEqual2()
        {
            var expected = new DataSet();
            var table = expected.Tables.Add("emp");
            table.Columns.Add("a_aa", typeof(string));
            table.Columns.Add("b_bb", typeof(decimal));
            var row = table.NewRow();
            row["a_aa"] = "111";
            row["b_bb"] = decimal.MaxValue;
            table.Rows.Add(row);
            var bean = new Hoge();
            bean.Aaa = "111";
            bean.Bbb = decimal.MaxValue;
            S2Assert.AreEqual(expected, bean, "1");
        }

        [Test]
        public void TestAreEqualForNull()
        {
            object hoge = null;
            S2Assert.AreEqual(null, hoge, "1");
        }

        [Test]
        public void TestAreBeanEqualSqlType()
        {
            // TODO decimal.MaxValueを超える値を指定するとOverflowException
            var expected = new DataSet();
            var table = expected.Tables.Add("emp");
            table.Columns.Add("aaa", typeof(SqlString));
            table.Columns.Add("bbb", typeof(SqlDecimal));
            var row = table.NewRow();
            row["aaa"] = "111";
            row["bbb"] = decimal.MaxValue;
            table.Rows.Add(row);
            var bean = new HogeSqlType();
            bean.Aaa = "111";
            bean.Bbb = decimal.MaxValue;
            S2Assert.AreEqual(expected, bean, "1");
        }

        [Test]
        public void TestAreBeanEqualSqlType2()
        {
            var expected = new DataSet();
            var table = expected.Tables.Add("emp");
            table.Columns.Add("aaa", typeof(string));
            table.Columns.Add("bbb", typeof(decimal));
            var row = table.NewRow();
            row["aaa"] = "111";
            row["bbb"] = decimal.MaxValue;
            table.Rows.Add(row);
            var bean = new HogeSqlType();
            bean.Aaa = "111";
            bean.Bbb = decimal.MaxValue;
            S2Assert.AreEqual(expected, bean, "1");
        }

        internal class Hoge
        {
            public string Aaa { get; set; }

            public decimal Bbb { get; set; }
        }

        internal class HogeSqlType
        {
            public SqlString Aaa { get; set; }

            public SqlDecimal Bbb { get; set; }
        }
    }
}
