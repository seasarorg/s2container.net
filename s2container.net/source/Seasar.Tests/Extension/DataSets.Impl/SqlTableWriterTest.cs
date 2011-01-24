#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Extension.DataSets.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Extension.DataSets.Impl
{
    [TestFixture]
    public class SqlTableWriterTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Ado.dicon";

        public void SetUpCreated()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void Created()
        {
            DataTable table = new DataTable("emp");
            table.Columns.Add("empno", typeof(int));
            table.Columns.Add("ename", typeof(string));
            table.Columns.Add("dname", typeof(string));
            DataRow row = table.NewRow();
            row["empno"] = 9900;
            row["ename"] = "hoge";
            row["dname"] = "aaa";
            table.Rows.Add(row);

            SqlTableWriter writer = new SqlTableWriter(DataSource);


            writer.Write(table);

            SqlTableReader reader = new SqlTableReader(DataSource);
            reader.SetTable("emp", "empno = 9900");
            DataTable ret = reader.Read();
            Trace.WriteLine(ToStringUtil.ToString(ret));
            Assert.IsNotNull(ret, "1");
        }

        public void SetUpModified()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void Modified()
        {
            SqlTableReader reader = new SqlTableReader(DataSource);
            string sql = "SELECT empno, ename, dname FROM emp, dept WHERE empno = 7788 AND emp.deptno = dept.deptno";
            reader.SetSql(sql, "emp");
            DataTable table = reader.Read();
            DataRow row = table.Rows[0];
            row["ename"] = "hoge";
            row["dname"] = "aaa";

            SqlTableWriter writer = new SqlTableWriter(DataSource);
            writer.Write(table);

            SqlTableReader reader2 = new SqlTableReader(DataSource);
            reader2.SetTable("emp", "empno = 7788");
            DataTable table2 = reader2.Read();
            Trace.WriteLine(ToStringUtil.ToString(table2));
            Assert.AreEqual("hoge", table2.Rows[0]["ename"], "1");
        }

        public void SetUpRemoved()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void Removed()
        {
            SqlTableReader reader = new SqlTableReader(DataSource);
            string sql = "SELECT empno, ename, dname FROM emp, dept WHERE empno = 7788 AND emp.deptno = dept.deptno";
            reader.SetSql(sql, "emp");
            DataTable table = reader.Read();
            DataRow row = table.Rows[0];
            row.Delete();

            SqlTableWriter writer = new SqlTableWriter(DataSource);
            writer.Write(table);

            SqlTableReader reader2 = new SqlTableReader(DataSource);
            reader2.SetTable("emp", "empno = 7788");
            DataTable table2 = reader2.Read();
            Trace.WriteLine(ToStringUtil.ToString(table2));
            Assert.AreEqual(0, table2.Rows.Count, "1");
        }
    }
}
