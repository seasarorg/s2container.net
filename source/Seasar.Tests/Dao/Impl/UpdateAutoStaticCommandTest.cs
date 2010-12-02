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

using System;
using System.Threading;
using MbUnit.Framework;
using Seasar.Dao;
using Seasar.Dao.Unit;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class UpdateAutoStaticCommandTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new object[] { 7788 });
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        [Ignore("アンスコ付きのテーブル名を正しく適用できない問題あり。要調査")]
        public void TestExecuteWithUnderscoreTx()
        {
            if ( Dbms.Dbms == KindOfDbms.Oracle )
            {
                //Assert.Ignore("Oracleでは[_]が先頭にくるSQLを実行することはできない");
            }
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IUnderscoreEntityDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetUnderScoreEntity");
            UnderscoreEntity emp = (UnderscoreEntity) cmd2.Execute(new object[] { 1 });
            emp._Table_Name = "1";
            emp._Table_Name_ = "2";
            emp.Table_Name_ = "3";
            emp.TableName = "4";
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
            UnderscoreEntity empLast = (UnderscoreEntity) cmd2.Execute(new object[] { 1 });
            Assert.AreEqual("1", empLast._Table_Name);
            Assert.AreEqual("2", empLast._Table_Name_);
            Assert.AreEqual("3", empLast.Table_Name_);
            Assert.AreEqual("4", empLast.TableName);
        }

        [Test, S2(Tx.Rollback)]
        public void TestUpdateNullableTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeNullableAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployeeNullable");
            {
                EmployeeNullable emp = (EmployeeNullable) cmd2.Execute(new object[] { 1 });
                emp.NullableNextRestDate = null;
                int count = (int) cmd.Execute(new object[] { emp });
                Assert.AreEqual(1, count, "1");
            }
            {
                EmployeeNullable emp = (EmployeeNullable) cmd2.Execute(new object[] { 10 });
                Assert.IsFalse(emp.NullableNextRestDate.HasValue);
            }
            {
                EmployeeNullable emp = (EmployeeNullable) cmd2.Execute(new object[] { 100 });
                emp.NullableNextRestDate = DateTime.Parse("2006/01/01");
                int count = (int) cmd.Execute(new object[] { emp });
                Assert.AreEqual(1, count, "2");
            }
        }
#if !NET_1_1
        [Test, S2(Tx.Rollback)]
        public void TestUpdateGenericNullableTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IGenericNullableEntityAutoDao));
            ISqlCommand cmdGet = dmd.GetSqlCommand("GetGenericNullableEntityByEntityNo");
            ISqlCommand cmdUpd = dmd.GetSqlCommand("Update");
            ISqlCommand cmdProps = dmd.GetSqlCommand("UpdateWithPersistentProps");
            ISqlCommand cmdNoProps = dmd.GetSqlCommand("UpdateWithNoPersistentProps");
            {
                GenericNullableEntity entity = (GenericNullableEntity) cmdGet.Execute(new object[] { 100 });
                DateTime beforeTime = (DateTime) entity.Ddate;
                entity.EntityNo = 1000;
                int count = (int) cmdNoProps.Execute(new object[] { entity });
                Assert.AreEqual(1, count, "1");
                Assert.AreEqual(beforeTime, entity.Ddate);
            }
            {
                DateTime beforeDate = DateTime.Now;
                GenericNullableEntity entity = (GenericNullableEntity) cmdGet.Execute(new object[] { 1000 });
                entity.EntityNo = 1001;
                int count = (int) cmdUpd.Execute(new object[] { entity });
                Assert.AreEqual(1, count, "1");
                Assert.GreaterThanOrEqualTo<DateTime>(entity.Ddate.Value, beforeDate);
            }
            {
                GenericNullableEntity entity = (GenericNullableEntity) cmdGet.Execute(new object[] { 101 });
                Assert.IsFalse(entity.Ddate.HasValue);
            }
            {
                DateTime beforeDate = DateTime.Now;
                GenericNullableEntity entity = (GenericNullableEntity) cmdGet.Execute(new object[] { 1001 });
                entity.EntityNo = 1002;
                int count = (int) cmdProps.Execute(new object[] { entity });
                Assert.AreEqual(1, count, "2");
                Assert.GreaterThanOrEqualTo<DateTime>(entity.Ddate.Value, beforeDate);
            }
        }
#endif
        [Test, S2(Tx.Rollback)]
        public void TestExecute2Tx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IDepartmentAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            Department dept = new Department();
            dept.Deptno = 10;
            int count = (int) cmd.Execute(new object[] { dept });
            Assert.AreEqual(1, count, "1");
            Assert.AreEqual(1, dept.VersionNo, "2");
        }

        [Test, S2(Tx.Rollback)]
        [ExpectedException(typeof(NotSingleRowUpdatedRuntimeException))]
        public void TestExecute3Tx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IDepartmentAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            Department dept = new Department();
            dept.Deptno = 10;
            dept.VersionNo = -1;

            cmd.Execute(new object[] { dept });
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute4Tx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update2");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new object[] { 7788 });
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute5Tx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update3");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new object[] { 7788 });
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

#if NHIBERNATE_NULLABLES
        [Test, S2(Tx.Rollback)]
        public void TestExecuteNullableDecimalVersionNoTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeNullableDecimalVersionNoDao));
            ISqlCommand insCmd = dmd.GetSqlCommand("Insert");
            EmployeeNullableDecimalVersionNo emp = new EmployeeNullableDecimalVersionNo();
            emp.EmpNo = 1;
            emp.EmpName = "insert";
            insCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.VersionNo.HasValue);
            Assert.AreEqual(0, emp.VersionNo.Value);

            ISqlCommand updCmd = dmd.GetSqlCommand("Update");
            emp.EmpName = "update";
            updCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.VersionNo.HasValue);
            Assert.AreEqual(1, emp.VersionNo.Value);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteNullableIntVersionNoTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeNullableIntVersionNoDao));
            ISqlCommand insCmd = dmd.GetSqlCommand("Insert");
            EmployeeNullableIntVersionNo emp = new EmployeeNullableIntVersionNo();
            emp.EmpNo = 1;
            emp.EmpName = "insert";
            insCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.VersionNo.HasValue);
            Assert.AreEqual(0, emp.VersionNo.Value);

            ISqlCommand updCmd = dmd.GetSqlCommand("Update");
            emp.EmpName = "update";
            updCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.VersionNo.HasValue);
            Assert.AreEqual(1, emp.VersionNo.Value);
        }
#endif

#if !NET_1_1
        [Test, S2(Tx.Rollback)]
        public void TestExecuteGenericNullableDecimalVersionNoTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeGenericNullableDecimalVersionNoDao));
            ISqlCommand insCmd = dmd.GetSqlCommand("Insert");
            EmployeeGenericNullableDecimalVersionNo emp = new EmployeeGenericNullableDecimalVersionNo();
            emp.EmpNo = 1;
            emp.EmpName = "insert";
            insCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.VersionNo.HasValue);
            Assert.AreEqual(0, emp.VersionNo.Value);

            ISqlCommand updCmd = dmd.GetSqlCommand("Update");
            emp.EmpName = "update";
            updCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.VersionNo.HasValue);
            Assert.AreEqual(1, emp.VersionNo.Value);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteGenericNullableIntVersionNoTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeGenericNullableIntVersionNoDao));
            ISqlCommand insCmd = dmd.GetSqlCommand("Insert");
            EmployeeGenericNullableIntVersionNo emp = new EmployeeGenericNullableIntVersionNo();
            emp.EmpNo = 1;
            emp.EmpName = "insert";
            insCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.VersionNo.HasValue);
            Assert.AreEqual(0, emp.VersionNo.Value);

            ISqlCommand updCmd = dmd.GetSqlCommand("Update");
            emp.EmpName = "update";
            updCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.VersionNo.HasValue);
            Assert.AreEqual(1, emp.VersionNo.Value);
        }
#endif
        [Test, S2(Tx.Rollback)]
        public void TestExecuteDecimalVersionNoTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDecimalVersionNoDao));
            ISqlCommand insCmd = dmd.GetSqlCommand("Insert");
            EmployeeDecimalVersionNo emp = new EmployeeDecimalVersionNo();
            emp.EmpNo = 1;
            emp.EmpName = "insert";
            insCmd.Execute(new object[] { emp });
            Assert.AreEqual(0, emp.VersionNo);

            ISqlCommand updCmd = dmd.GetSqlCommand("Update");
            emp.EmpName = "update";
            updCmd.Execute(new object[] { emp });
            Assert.AreEqual(1, emp.VersionNo);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteIntVersionNoTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeIntVersionNoDao));
            ISqlCommand insCmd = dmd.GetSqlCommand("Insert");
            EmployeeIntVersionNo emp = new EmployeeIntVersionNo();
            emp.EmpNo = 1;
            emp.EmpName = "insert";
            insCmd.Execute(new object[] { emp });
            Assert.AreEqual(0, emp.VersionNo);

            ISqlCommand updCmd = dmd.GetSqlCommand("Update");
            emp.EmpName = "update";
            updCmd.Execute(new object[] { emp });
            Assert.AreEqual(1, emp.VersionNo);
        }

        [Test, S2(Tx.Rollback)]
        public void TestEmployeeGenericNullableTimestampTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeGenericNullableTimestampDao));
            ISqlCommand insCmd = dmd.GetSqlCommand("Insert");
            EmployeeGenericNullableTimestamp emp = new EmployeeGenericNullableTimestamp();
            emp.EmpNo = 1;
            insCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.Timestamp.HasValue);
            Assert.AreEqual(DateTime.Today, emp.Timestamp.Value.Date);

            DateTime insTimestamp = emp.Timestamp.Value;

            // Timestampの更新を確認するため、1秒待機。
            Thread.Sleep(1000);

            ISqlCommand updCmd = dmd.GetSqlCommand("Update");

            updCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.Timestamp.HasValue);
            Assert.IsTrue(insTimestamp < emp.Timestamp.Value);
        }

        [Test, S2(Tx.Rollback)]
        public void TestEmployeeTimestampTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeTimestampDao));
            ISqlCommand insCmd = dmd.GetSqlCommand("Insert");
            EmployeeTimestamp emp = new EmployeeTimestamp();
            emp.EmpNo = 1;
            insCmd.Execute(new object[] { emp });
            Assert.AreEqual(DateTime.Today, emp.Timestamp.Date);

            // Timestampの更新を確認するため、1秒待機。
            Thread.Sleep(1000);

            ISqlCommand updCmd = dmd.GetSqlCommand("Update");
            updCmd.Execute(new object[] { emp });
            Assert.IsTrue(DateTime.Today < emp.Timestamp);
        }

        [Test, S2(Tx.Rollback)]
        public void TestEmployeeSqlTimestampTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeSqlTimestampDao));
            ISqlCommand insCmd = dmd.GetSqlCommand("Insert");
            EmployeeSqlTimestamp emp = new EmployeeSqlTimestamp();
            emp.EmpNo = 1;
            insCmd.Execute(new object[] { emp });
            Assert.IsFalse(emp.Timestamp.IsNull);
            Assert.AreEqual(DateTime.Today, emp.Timestamp.Value.Date);

            // Timestampの更新を確認するため、1秒待機。
            Thread.Sleep(1000);

            DateTime insTimestamp = emp.Timestamp.Value;

            ISqlCommand updCmd = dmd.GetSqlCommand("Update");
            updCmd.Execute(new object[] { emp });
            Assert.IsFalse(emp.Timestamp.IsNull);
            Assert.IsTrue(insTimestamp < emp.Timestamp.Value);
        }
    }
}
