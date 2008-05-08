#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
    public class SqlTableReaderTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Ado.dicon";

        public void SetUpRead()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void Read()
        {
            SqlTableReader reader = new SqlTableReader(DataSource);
            reader.SetTable("emp");
            DataTable ret = reader.Read();
            Trace.WriteLine(ToStringUtil.ToString(ret));
            Assert.AreEqual(14, ret.Rows.Count, "1");
            Assert.AreEqual(DataRowState.Unchanged, ret.Rows[0].RowState, "2");
        }

        public void SetUpRead2()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void Read2()
        {
            SqlTableReader reader = new SqlTableReader(DataSource);
            reader.SetTable("emp", "empno = 7788");
            DataTable ret = reader.Read();
            Trace.WriteLine(ToStringUtil.ToString(ret));
            Assert.AreEqual(1, ret.Rows.Count, "1");
        }

        public void SetUpRead3()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void Read3()
        {
            SqlTableReader reader = new SqlTableReader(DataSource);
            reader.SetSql("select * from emp", "emp");
            DataTable ret = reader.Read();
            Trace.WriteLine(ToStringUtil.ToString(ret));
            Assert.AreEqual(14, ret.Rows.Count, "1");
        }
    }
}
