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

using System.Diagnostics;
using Seasar.Dao;
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.Unit;
using MbUnit.Framework;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class DeleteAutoStaticCommandTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Delete");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new object[] { 7788 });
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute2Tx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IDepartmentAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Delete");
            Department dept = new Department();
            dept.Deptno = 10;
            Trace.WriteLine(dept.ToString());
            int count = (int) cmd.Execute(new object[] { dept });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        [ExpectedException(typeof(NotSingleRowUpdatedRuntimeException))]
        public void TestExecute3Tx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IDepartmentAutoDao));
            DeleteAutoStaticCommand cmd =
                (DeleteAutoStaticCommand) dmd.GetSqlCommand("Delete");
            Department dept = new Department();
            dept.Deptno = 10;
            dept.VersionNo = -1;
            cmd.Execute(new object[] { dept });
        }

#if NHIBERNATE_NULLABLES
        [Test, S2(Tx.Rollback)]
        public void TestExecuteNullableDecimalVersionNoTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeNullableDecimalVersionNoDao));
            DeleteAutoStaticCommand cmd =
                (DeleteAutoStaticCommand)dmd.GetSqlCommand("Delete");
            EmployeeNullableDecimalVersionNo emp = new EmployeeNullableDecimalVersionNo();
            emp.EmpNo = 10;
            emp.VersionNo = 100;
            cmd.Execute(new object[] { emp });

            ISqlCommand delCmd = (ISqlCommand)dmd.GetSqlCommand("GetEmployee");
            object result = delCmd.Execute(new object[] { emp.EmpNo });
            Assert.IsNull(result);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteNullableIntVersionNoTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeNullableIntVersionNoDao));
            DeleteAutoStaticCommand cmd =
                (DeleteAutoStaticCommand)dmd.GetSqlCommand("Delete");
            EmployeeNullableIntVersionNo emp = new EmployeeNullableIntVersionNo();
            emp.EmpNo = 10;
            emp.VersionNo = 100;
            cmd.Execute(new object[] { emp });

            ISqlCommand delCmd = (ISqlCommand)dmd.GetSqlCommand("GetEmployee");
            object result = delCmd.Execute(new object[] { emp.EmpNo });
            Assert.IsNull(result);
        }
#endif

#if !NET_1_1
        [Test, S2(Tx.Rollback)]
        public void TestExecuteGenericNullableDecimalVersionNoTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeGenericNullableDecimalVersionNoDao));
            DeleteAutoStaticCommand cmd =
                (DeleteAutoStaticCommand)dmd.GetSqlCommand("Delete");
            EmployeeGenericNullableDecimalVersionNo emp = new EmployeeGenericNullableDecimalVersionNo();
            emp.EmpNo = 10;
            emp.VersionNo = 100;
            cmd.Execute(new object[] { emp });

            ISqlCommand delCmd = (ISqlCommand)dmd.GetSqlCommand("GetEmployee");
            object result = delCmd.Execute(new object[] { emp.EmpNo });
            Assert.IsNull(result);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteGenericNullableIntVersionNoTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeGenericNullableIntVersionNoDao));
            DeleteAutoStaticCommand cmd =
                (DeleteAutoStaticCommand)dmd.GetSqlCommand("Delete");
            EmployeeGenericNullableIntVersionNo emp = new EmployeeGenericNullableIntVersionNo();
            emp.EmpNo = 10;
            emp.VersionNo = 100;
            cmd.Execute(new object[] { emp });

            ISqlCommand delCmd = (ISqlCommand)dmd.GetSqlCommand("GetEmployee");
            object result = delCmd.Execute(new object[] { emp.EmpNo });
            Assert.IsNull(result);
        }
#endif
        [Test, S2(Tx.Rollback)]
        public void TestExecuteDecimalVersionNoTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDecimalVersionNoDao));
            DeleteAutoStaticCommand cmd =
                (DeleteAutoStaticCommand)dmd.GetSqlCommand("Delete");
            EmployeeDecimalVersionNo emp = new EmployeeDecimalVersionNo();
            emp.EmpNo = 10;
            emp.VersionNo = 100;
            cmd.Execute(new object[] { emp });

            ISqlCommand delCmd = (ISqlCommand)dmd.GetSqlCommand("GetEmployee");
            object result = delCmd.Execute(new object[] { emp.EmpNo });
            Assert.IsNull(result);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteIntVersionNoTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeIntVersionNoDao));
            DeleteAutoStaticCommand cmd =
                (DeleteAutoStaticCommand)dmd.GetSqlCommand("Delete");
            EmployeeIntVersionNo emp = new EmployeeIntVersionNo();
            emp.EmpNo = 10;
            emp.VersionNo = 100;
            cmd.Execute(new object[] { emp });

            ISqlCommand delCmd = (ISqlCommand)dmd.GetSqlCommand("GetEmployee");
            object result = delCmd.Execute(new object[] { emp.EmpNo });
            Assert.IsNull(result);
        }
    }
}
