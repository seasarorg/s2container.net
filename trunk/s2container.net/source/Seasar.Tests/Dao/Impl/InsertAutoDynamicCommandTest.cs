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
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDefaultTimestampDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            EmployeeDefaultTimestamp emp = new EmployeeDefaultTimestamp();
            emp.Empno = 99;
            emp.Ename = null;
            emp.JobName = null;
            emp.Sal = 99;
            int count = (int)cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");

            ISqlCommand afterCmd = dmd.GetSqlCommand("GetEmployee");
            EmployeeDefaultTimestamp after = (EmployeeDefaultTimestamp)afterCmd.Execute(new object[] { emp.Empno });
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
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDefaultVersionNoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            EmployeeDefaultVersionNo emp = new EmployeeDefaultVersionNo();
            emp.Empno = 99;
            emp.Ename = null;
            emp.JobName = null;
            emp.Sal = 99;
            int count = (int)cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");

            ISqlCommand afterCmd = dmd.GetSqlCommand("GetEmployee");
            EmployeeDefaultVersionNo after = (EmployeeDefaultVersionNo)afterCmd.Execute(new object[] { emp.Empno });
            Assert.IsNotNull(after);
            Assert.AreEqual(emp.Empno, after.Empno);
            Assert.AreEqual("def_name", after.Ename, "デフォルト値が設定されている。");
            Assert.IsNull(after.JobName, "デフォルト値は設定されていない列にはnull設定");
            Assert.AreEqual(emp.Sal, after.Sal, "通常の値設定も行われる");
            Assert.IsNotNull(emp.Version);
        }
    }
}
