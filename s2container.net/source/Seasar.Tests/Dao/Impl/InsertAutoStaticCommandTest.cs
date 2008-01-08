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

using System;
using System.Diagnostics;
using MbUnit.Framework;
using Nullables;
using Seasar.Dao;
using Seasar.Dao.Unit;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Impl
{
    /// <summary>
    /// 内部的にはInsertAutoStaticCommandは使用していませんが、
    /// 通常のInsertのためにも兼ねてこのテストも残してあります。
    /// (Insertを実行するとInsertAutoDynamicCommandが
    /// 必ず使われるようになっているため）
    /// [DAONET-3]
    /// </summary>
    [TestFixture]
    public class InsertAutoStaticCommandTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            Employee emp = new Employee();
            emp.Empno = 99;
            emp.Ename = "hoge";
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteNullableDateInsertTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeNullableAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            {
                EmployeeNullable emp = new EmployeeNullable();
                emp.Empno = 99;
                emp.Ename = "hoge";
                emp.Deptno = 1;
                emp.NullableNextRestDate = null;
                int count = (int) cmd.Execute(new object[] { emp });
                Assert.AreEqual(1, count, "1");
            }
            {
                EmployeeNullable emp = new EmployeeNullable();
                emp.Empno = 98;
                emp.Ename = "hoge";
                emp.Deptno = 1;
                emp.NullableNextRestDate = NullableDateTime.Parse("2000/01/01");
                int count = (int) cmd.Execute(new object[] { emp });
                Assert.AreEqual(1, count, "2");
            }
        }
#if !NET_1_1
        [Test, S2(Tx.Rollback)]
        public void TestExecuteGenericNullableDateInsertTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IGenericNullableEntityAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            {
                DateTime beforeTime = DateTime.Now;
                GenericNullableEntity entity = new GenericNullableEntity();
                entity.EntityNo = 1;
                int count = (int) cmd.Execute(new object[] { entity });
                Assert.AreEqual(1, count, "Inserting");
                Assert.GreaterEqualThan(entity.Ddate, beforeTime);
            }
        }
#endif

        [Test, S2(Tx.Rollback)]
        public void TestExecuteWithUnderscoreTx()
        {
            if (Dbms.Dbms == KindOfDbms.Oracle)
            {
                Assert.Ignore("Oracleでカラム名の先頭が_の場合、\"(引用符)で囲む必要がある。");
            }
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IUnderscoreEntityDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetUnderScoreEntityByUnderScoreNo");
            UnderscoreEntity entity = new UnderscoreEntity();
            entity.UnderScoreNo = 100;
            entity._Table_Name = "1";
            entity._Table_Name_ = "2";
            entity.Table_Name_ = "3";
            entity.TableName = "4";
            int count = (int) cmd.Execute(new object[] { entity });
            Assert.AreEqual(1, count, "1");
            UnderscoreEntity entityLast = (UnderscoreEntity) cmd2.Execute(new object[] { 100 });
            Assert.AreEqual("1", entityLast._Table_Name);
            Assert.AreEqual("2", entityLast._Table_Name_);
            Assert.AreEqual("3", entityLast.Table_Name_);
            Assert.AreEqual("4", entityLast.TableName);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute2Tx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IIdTableAutoDao));

            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            IdTable table = new IdTable();
            table.IdName = "hoge";
            int count1 = (int) cmd.Execute(new object[] { table });
            Assert.AreEqual(1, count1, "1");
            int id1 = table.MyId;
            Trace.WriteLine(id1);
            int count2 = (int) cmd.Execute(new object[] { table });
            Assert.AreEqual(1, count2, "2");
            int id2 = table.MyId;
            Trace.WriteLine(id2);
            Assert.AreEqual(1, id2 - id1, "2");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute3_1Tx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IIdTableAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            IdTable table = new IdTable();
            table.IdName = "hoge";
            int count = (int) cmd.Execute(new object[] { table });
            Assert.AreEqual(1, count, "1");
            Trace.WriteLine(table.MyId);
            Assert.IsTrue(table.MyId > 0, "2");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute3_2Tx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IIdTableAuto2Dao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            IdTable2 table1 = new IdTable2();
            table1.IdName = "hoge";
            int count = (int) cmd.Execute(new object[] { table1 });
            Assert.AreEqual(1, count, "1");
            Trace.WriteLine(table1.MyId);
            Assert.IsTrue(table1.MyId > 0, "2");

            IdTable2 table2 = new IdTable2();
            table2.IdName = "foo";
            cmd.Execute(new object[] { table2 });
            Trace.WriteLine(table2.MyId);
            Assert.IsTrue(table2.MyId > table1.MyId, "3");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute4Tx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert2");
            Employee emp = new Employee();
            emp.Empno = 99;
            emp.Ename = "hoge";
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute5Tx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert3");
            Employee emp = new Employee();
            emp.Empno = 99;
            emp.Ename = "hoge";
            emp.Deptno = 10;
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }
    }
}
