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
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.GetSqlCommand("Update");
            var cmd2 = dmd.GetSqlCommand("GetEmployee");
            var emp = (Employee) cmd2.Execute(new object[] { 7788 });
            var count = (int) cmd.Execute(new object[] { emp });
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
            var dmd = CreateDaoMetaData(typeof(IUnderscoreEntityDao));
            var cmd = dmd.GetSqlCommand("Update");
            var cmd2 = dmd.GetSqlCommand("GetUnderScoreEntity");
            var emp = (UnderscoreEntity) cmd2.Execute(new object[] { 1 });
            emp._Table_Name = "1";
            emp._Table_Name_ = "2";
            emp.Table_Name_ = "3";
            emp.TableName = "4";
            var count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
            var empLast = (UnderscoreEntity) cmd2.Execute(new object[] { 1 });
            Assert.AreEqual("1", empLast._Table_Name);
            Assert.AreEqual("2", empLast._Table_Name_);
            Assert.AreEqual("3", empLast.Table_Name_);
            Assert.AreEqual("4", empLast.TableName);
        }

        [Test, S2(Tx.Rollback)]
        public void TestUpdateNullableTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeNullableAutoDao));
            var cmd = dmd.GetSqlCommand("Update");
            var cmd2 = dmd.GetSqlCommand("GetEmployeeNullable");
            {
                var emp = (EmployeeNullable) cmd2.Execute(new object[] { 1 });
                emp.NullableNextRestDate = null;
                var count = (int) cmd.Execute(new object[] { emp });
                Assert.AreEqual(1, count, "1");
            }
            {
                var emp = (EmployeeNullable) cmd2.Execute(new object[] { 10 });
                Assert.IsFalse(emp.NullableNextRestDate.HasValue);
            }
            {
                var emp = (EmployeeNullable) cmd2.Execute(new object[] { 100 });
                emp.NullableNextRestDate = DateTime.Parse("2006/01/01");
                var count = (int) cmd.Execute(new object[] { emp });
                Assert.AreEqual(1, count, "2");
            }
        }
#if !NET_1_1
        [Test, S2(Tx.Rollback)]
        public void TestUpdateGenericNullableTx()
        {
            var dmd = CreateDaoMetaData(typeof(IGenericNullableEntityAutoDao));
            var cmdGet = dmd.GetSqlCommand("GetGenericNullableEntityByEntityNo");
            var cmdUpd = dmd.GetSqlCommand("Update");
            var cmdProps = dmd.GetSqlCommand("UpdateWithPersistentProps");
            var cmdNoProps = dmd.GetSqlCommand("UpdateWithNoPersistentProps");
            {
                var entity = (GenericNullableEntity) cmdGet.Execute(new object[] { 100 });
                var beforeTime = (DateTime) entity.Ddate;
                entity.EntityNo = 1000;
                var count = (int) cmdNoProps.Execute(new object[] { entity });
                Assert.AreEqual(1, count, "1");
                Assert.AreEqual(beforeTime, entity.Ddate);
            }
            {
                var beforeDate = DateTime.Now;
                var entity = (GenericNullableEntity) cmdGet.Execute(new object[] { 1000 });
                entity.EntityNo = 1001;
                var count = (int) cmdUpd.Execute(new object[] { entity });
                Assert.AreEqual(1, count, "1");
#if NET_4_0
                Assert.GreaterThanOrEqualTo<DateTime>(entity.Ddate.Value, beforeDate);
#else
#region NET2.0
                Assert.GreaterEqualThan(entity.Ddate.Value, beforeDate);
#endregion
#endif
            }
            {
                var entity = (GenericNullableEntity) cmdGet.Execute(new object[] { 101 });
                Assert.IsFalse(entity.Ddate.HasValue);
            }
            {
                var beforeDate = DateTime.Now;
                var entity = (GenericNullableEntity) cmdGet.Execute(new object[] { 1001 });
                entity.EntityNo = 1002;
                var count = (int) cmdProps.Execute(new object[] { entity });
                Assert.AreEqual(1, count, "2");
#if NET_4_0
                Assert.GreaterThanOrEqualTo<DateTime>(entity.Ddate.Value, beforeDate);
#else
#region NET2.0
                Assert.GreaterEqualThan(entity.Ddate.Value, beforeDate);
#endregion
#endif
            }
        }
#endif
        [Test, S2(Tx.Rollback)]
        public void TestExecute2Tx()
        {
            var dmd = CreateDaoMetaData(typeof(IDepartmentAutoDao));
            var cmd = dmd.GetSqlCommand("Update");
            var dept = new Department();
            dept.Deptno = 10;
            var count = (int) cmd.Execute(new object[] { dept });
            Assert.AreEqual(1, count, "1");
            Assert.AreEqual(1, dept.VersionNo, "2");
        }

        [Test, S2(Tx.Rollback)]
        [ExpectedException(typeof(NotSingleRowUpdatedRuntimeException))]
        public void TestExecute3Tx()
        {
            var dmd = CreateDaoMetaData(typeof(IDepartmentAutoDao));
            var cmd = dmd.GetSqlCommand("Update");
            var dept = new Department();
            dept.Deptno = 10;
            dept.VersionNo = -1;

            cmd.Execute(new object[] { dept });
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute4Tx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.GetSqlCommand("Update2");
            var cmd2 = dmd.GetSqlCommand("GetEmployee");
            var emp = (Employee) cmd2.Execute(new object[] { 7788 });
            var count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute5Tx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.GetSqlCommand("Update3");
            var cmd2 = dmd.GetSqlCommand("GetEmployee");
            var emp = (Employee) cmd2.Execute(new object[] { 7788 });
            var count = (int) cmd.Execute(new object[] { emp });
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
            var dmd = CreateDaoMetaData(typeof(IEmployeeGenericNullableDecimalVersionNoDao));
            var insCmd = dmd.GetSqlCommand("Insert");
            var emp = new EmployeeGenericNullableDecimalVersionNo();
            emp.EmpNo = 1;
            emp.EmpName = "insert";
            insCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.VersionNo.HasValue);
            Assert.AreEqual(0, emp.VersionNo.Value);

            var updCmd = dmd.GetSqlCommand("Update");
            emp.EmpName = "update";
            updCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.VersionNo.HasValue);
            Assert.AreEqual(1, emp.VersionNo.Value);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteGenericNullableIntVersionNoTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeGenericNullableIntVersionNoDao));
            var insCmd = dmd.GetSqlCommand("Insert");
            var emp = new EmployeeGenericNullableIntVersionNo();
            emp.EmpNo = 1;
            emp.EmpName = "insert";
            insCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.VersionNo.HasValue);
            Assert.AreEqual(0, emp.VersionNo.Value);

            var updCmd = dmd.GetSqlCommand("Update");
            emp.EmpName = "update";
            updCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.VersionNo.HasValue);
            Assert.AreEqual(1, emp.VersionNo.Value);
        }
#endif
        [Test, S2(Tx.Rollback)]
        public void TestExecuteDecimalVersionNoTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDecimalVersionNoDao));
            var insCmd = dmd.GetSqlCommand("Insert");
            var emp = new EmployeeDecimalVersionNo();
            emp.EmpNo = 1;
            emp.EmpName = "insert";
            insCmd.Execute(new object[] { emp });
            Assert.AreEqual(0, emp.VersionNo);

            var updCmd = dmd.GetSqlCommand("Update");
            emp.EmpName = "update";
            updCmd.Execute(new object[] { emp });
            Assert.AreEqual(1, emp.VersionNo);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteIntVersionNoTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeIntVersionNoDao));
            var insCmd = dmd.GetSqlCommand("Insert");
            var emp = new EmployeeIntVersionNo();
            emp.EmpNo = 1;
            emp.EmpName = "insert";
            insCmd.Execute(new object[] { emp });
            Assert.AreEqual(0, emp.VersionNo);

            var updCmd = dmd.GetSqlCommand("Update");
            emp.EmpName = "update";
            updCmd.Execute(new object[] { emp });
            Assert.AreEqual(1, emp.VersionNo);
        }

        [Test, S2(Tx.Rollback)]
        public void TestEmployeeGenericNullableTimestampTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeGenericNullableTimestampDao));
            var insCmd = dmd.GetSqlCommand("Insert");
            var emp = new EmployeeGenericNullableTimestamp();
            emp.EmpNo = 1;
            insCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.Timestamp.HasValue);
            Assert.AreEqual(DateTime.Today, emp.Timestamp.Value.Date);

            var insTimestamp = emp.Timestamp.Value;

            // Timestampの更新を確認するため、1秒待機。
            Thread.Sleep(1000);

            var updCmd = dmd.GetSqlCommand("Update");

            updCmd.Execute(new object[] { emp });
            Assert.IsTrue(emp.Timestamp.HasValue);
            Assert.IsTrue(insTimestamp < emp.Timestamp.Value);
        }

        [Test, S2(Tx.Rollback)]
        public void TestEmployeeTimestampTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeTimestampDao));
            var insCmd = dmd.GetSqlCommand("Insert");
            var emp = new EmployeeTimestamp();
            emp.EmpNo = 1;
            insCmd.Execute(new object[] { emp });
            Assert.AreEqual(DateTime.Today, emp.Timestamp.Date);

            // Timestampの更新を確認するため、1秒待機。
            Thread.Sleep(1000);

            var updCmd = dmd.GetSqlCommand("Update");
            updCmd.Execute(new object[] { emp });
            Assert.IsTrue(DateTime.Today < emp.Timestamp);
        }

        [Test, S2(Tx.Rollback)]
        public void TestEmployeeSqlTimestampTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeSqlTimestampDao));
            var insCmd = dmd.GetSqlCommand("Insert");
            var emp = new EmployeeSqlTimestamp();
            emp.EmpNo = 1;
            insCmd.Execute(new object[] { emp });
            Assert.IsFalse(emp.Timestamp.IsNull);
            Assert.AreEqual(DateTime.Today, emp.Timestamp.Value.Date);

            // Timestampの更新を確認するため、1秒待機。
            Thread.Sleep(1000);

            var insTimestamp = emp.Timestamp.Value;

            var updCmd = dmd.GetSqlCommand("Update");
            updCmd.Execute(new object[] { emp });
            Assert.IsFalse(emp.Timestamp.IsNull);
            Assert.IsTrue(insTimestamp < emp.Timestamp.Value);
        }
    }
}
