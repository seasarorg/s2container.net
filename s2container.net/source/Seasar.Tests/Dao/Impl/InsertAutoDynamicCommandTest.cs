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

using MbUnit.Framework;
using Seasar.Dao;
using Seasar.Dao.Unit;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class InsertAutoDynamicCommandTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute_TimestampTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDefaultTimestampDao));
            var cmd = dmd.GetSqlCommand("Insert");
            var emp = new EmployeeDefaultTimestamp();
            emp.Empno = 99;
            emp.Ename = null;
            emp.JobName = null;
            emp.Sal = 99;
            var count = (int)cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");

            var afterCmd = dmd.GetSqlCommand("GetEmployee");
            var after = (EmployeeDefaultTimestamp)afterCmd.Execute(new object[] { emp.Empno });
            Assert.IsNotNull(after);
            Assert.AreEqual(emp.Empno, after.Empno);
            Assert.AreEqual("def_name", after.Ename, "デフォルト値が設定されている。");
            Assert.IsNull(after.JobName, "デフォルト値は設定されていない列にはnull設定");
            Assert.AreEqual(emp.Sal, after.Sal, "通常の値設定も行われる");
            Assert.IsNotNull(emp.Timestamp);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute_VersionNoTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDefaultVersionNoDao));
            var cmd = dmd.GetSqlCommand("Insert");
            var emp = new EmployeeDefaultVersionNo();
            emp.Empno = 99;
            emp.Ename = null;
            emp.JobName = null;
            emp.Sal = 99;
            var count = (int)cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");

            var afterCmd = dmd.GetSqlCommand("GetEmployee");
            var after = (EmployeeDefaultVersionNo)afterCmd.Execute(new object[] { emp.Empno });
            Assert.IsNotNull(after);
            Assert.AreEqual(emp.Empno, after.Empno);
            Assert.AreEqual("def_name", after.Ename, "デフォルト値が設定されている。");
            Assert.IsNull(after.JobName, "デフォルト値は設定されていない列にはnull設定");
            Assert.AreEqual(emp.Sal, after.Sal, "通常の値設定も行われる");
            Assert.IsNotNull(emp.Version);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute_VersionNoIgnoreCaseTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDefaultVersionNoIgnoreCaseDao));
            var cmd = dmd.GetSqlCommand("Insert");
            var emp = new EmployeeDefaultVersionNoIgnoreCase();
            emp.Empno = 99;
            emp.Ename = null;
            emp.JobName = null;
            emp.Sal = 99;
            var count = (int)cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");

            var afterCmd = dmd.GetSqlCommand("GetEmployee");
            var after = (EmployeeDefaultVersionNoIgnoreCase)afterCmd.Execute(new object[] { emp.Empno });
            Assert.IsNotNull(after);
            Assert.AreEqual(emp.Empno, after.Empno);
            Assert.AreEqual("def_name", after.Ename, "デフォルト値が設定されている。");
            Assert.IsNull(after.JobName, "デフォルト値は設定されていない列にはnull設定");
            Assert.AreEqual(emp.Sal, after.Sal, "通常の値設定も行われる");
            Assert.IsNotNull(emp.vERSION);
            Assert.AreEqual(0, emp.vERSION);
        }
    }
}
