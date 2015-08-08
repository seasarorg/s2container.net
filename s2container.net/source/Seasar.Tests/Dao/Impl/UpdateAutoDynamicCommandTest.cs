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

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Seasar.Dao.Unit;
using Seasar.Dao;
using Seasar.Dao.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class UpdateAutoDynamicCommandTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteTx()
        {
            const int TEST_EMP_NO = 7369;
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDynamicTimestampDao));
            var select = dmd.GetSqlCommand("GetEmployee");
            var before = (EmployeeAutoDynamicTimestamp)select.Execute(new object[] { TEST_EMP_NO });

            var e = new EmployeeAutoDynamicTimestamp();
            e.Empno = before.Empno;
            e.Ename = "HOGE";
            e.Hiredate = null;
            e.Mgr = null;
            e.Sal = null;
            e.Timestamp = before.Timestamp;
            
            var unlessNull = dmd.GetSqlCommand("UpdateUnlessNull");
            Assert.IsTrue(unlessNull is UpdateAutoDynamicCommand);
            unlessNull.Execute(new object[] { e });

            var after = (EmployeeAutoDynamicTimestamp)select.Execute(new object[] { TEST_EMP_NO });
            Console.WriteLine(after.ToString());
            Assert.AreEqual(e.Ename, after.Ename);
            Assert.AreEqual(before.Hiredate, after.Hiredate);
            Assert.AreEqual(before.Mgr, after.Mgr);
            Assert.AreEqual(before.Sal, after.Sal);
            Assert.AreNotEqual(before.Timestamp, after.Timestamp);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteOnePropertyTx()
        {
            const int TEST_EMP_NO = 7369;
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDynamicTimestampDao));
            var select = dmd.GetSqlCommand("GetEmployee");
            var before = (EmployeeAutoDynamicTimestamp)select.Execute(new object[] { TEST_EMP_NO });

            var e = new EmployeeAutoDynamicTimestamp();
            e.Empno = before.Empno;
            e.Timestamp = before.Timestamp;

            var unlessNull = dmd.GetSqlCommand("UpdateUnlessNull");
            Assert.IsTrue(unlessNull is UpdateAutoDynamicCommand);
            unlessNull.Execute(new object[] { e });

            var after = (EmployeeAutoDynamicTimestamp)select.Execute(new object[] { TEST_EMP_NO });
            Console.WriteLine(after.ToString());
            Assert.AreEqual(before.Hiredate, after.Hiredate);
            Assert.AreEqual(before.Mgr, after.Mgr);
            Assert.AreNotEqual(before.Timestamp, after.Timestamp);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteNullVersionNoTx()
        {
            const int TEST_DEPT_NO = 10;
            var dmd = CreateDaoMetaData(typeof(IDepartmentAutoDynamicDao));
            var select = dmd.GetSqlCommand("GetDepartment");
            var before = (Department)select.Execute(new object[] { TEST_DEPT_NO });

            var e = new Department();
            e.Deptno = before.Deptno;
            e.Dname = "HOGE";
            e.Dummy = null;
            e.VersionNo = before.VersionNo;

            var unlessNull = dmd.GetSqlCommand("UpdateUnlessNull");
            Assert.IsTrue(unlessNull is UpdateAutoDynamicCommand);
            unlessNull.Execute(new object[] { e });

            var after = (Department)select.Execute(new object[] { TEST_DEPT_NO });
            Console.WriteLine(after.ToString());
            Assert.AreEqual(e.Dname, after.Dname);
            Assert.AreEqual(before.Dummy, after.Dummy);
            Assert.AreEqual(before.VersionNo + 1, after.VersionNo);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteAllNulTx()
        {
            const int TEST_EMP_NO = 7369;
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDynamicDao));
            var select = dmd.GetSqlCommand("GetEmployee");
            var before = (EmployeeAutoDynamic)select.Execute(new object[] { TEST_EMP_NO });

            var e = new EmployeeAutoDynamic();
            e.Empno = before.Empno;

            var unlessNull = dmd.GetSqlCommand("UpdateUnlessNull");
            Assert.IsTrue(unlessNull is UpdateAutoDynamicCommand);

            try
            {
                unlessNull.Execute(new object[] { e });
                Assert.Fail();
            }
            catch ( NoUpdatePropertyTypeRuntimeException ex )
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
