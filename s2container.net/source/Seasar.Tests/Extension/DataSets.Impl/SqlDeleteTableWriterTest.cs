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
using MbUnit.Framework;
using Seasar.Extension.DataSets;
using Seasar.Extension.DataSets.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.DataSets.Impl
{
    [TestFixture]
    public class SqlDeleteTableWriterTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Ado.dicon";

        public void SetUpWrite()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void Write()
        {
            DataTable table = new DataTable("emp");
            table.Columns.Add("empno", typeof(int));
            table.Columns.Add("ename", typeof(string));
            DataRow row = table.NewRow();
            row["empno"] = 9900;
            row["ename"] = "hoge";
            table.Rows.Add(row);
            ITableWriter writer = new SqlTableWriter(DataSource);
            writer.Write(table);
            ITableWriter writer2 = new SqlDeleteTableWriter(DataSource);
            writer2.Write(table);
            DataTable table2 = ReadDbByTable("emp", "empno = 9900");
            Assert.AreEqual(0, table2.Rows.Count, "1");
        }
    }
}
