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
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Dao;
using Seasar.Dao.Unit;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Impl
{
    /// <summary>
    /// 内部的にはInsertAutoStaticCommandは使用していませんが
    /// 通常のInsertのためにも兼ねてこのテストも残してあります。
    /// (Insertを実行するとInsertAutoDynamicCommandが
    /// 必ず使われるようになっているため
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
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.GetSqlCommand("Insert");
            var emp = new Employee();
            emp.Empno = 99;
            emp.Ename = "hoge";
            var count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteNullableDateInsertTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeNullableAutoDao));
            var cmd = dmd.GetSqlCommand("Insert");
            {
                var emp = new EmployeeNullable();
                emp.Empno = 99;
                emp.Ename = "hoge";
                emp.Deptno = 1;
                emp.NullableNextRestDate = null;
                var count = (int) cmd.Execute(new object[] { emp });
                Assert.AreEqual(1, count, "1");
            }
            {
                var emp = new EmployeeNullable();
                emp.Empno = 98;
                emp.Ename = "hoge";
                emp.Deptno = 1;
                emp.NullableNextRestDate = DateTime.Parse("2000/01/01");
                var count = (int) cmd.Execute(new object[] { emp });
                Assert.AreEqual(1, count, "2");
            }
        }
#if !NET_1_1
        [Test, S2(Tx.Rollback)]
        public void TestExecuteGenericNullableDateInsertTx()
        {
            var dmd = CreateDaoMetaData(typeof(IGenericNullableEntityAutoDao));
            var cmd = dmd.GetSqlCommand("Insert");
            {
                var beforeTime = DateTime.Now;
                var entity = new GenericNullableEntity();
                entity.EntityNo = 1;
                var count = (int) cmd.Execute(new object[] { entity });
                Assert.AreEqual(1, count, "Inserting");
#if NET_4_0
                Assert.GreaterThanOrEqualTo<DateTime>(entity.Ddate.Value, beforeTime);
#else
                #region NET2.0
                Assert.GreaterEqualThan(entity.Ddate.Value, beforeTime);
                #endregion
#endif
            }
        }
#endif

        [Test, S2(Tx.Rollback)]
        [Ignore("#.NET4.0 アンダースコアを含むテーブル名の扱いの違いが見られるため一時的に")]
        public void TestExecuteWithUnderscoreTx()
        {
            if (Dbms.Dbms == KindOfDbms.Oracle)
            {
                // #.NET4.0 Assert.Ignoreが使えないのでreturnで戻す
                //Assert.Ignore("Oracleでカラム名の先頭が_の場合、\"(引用符)で囲む必要がある。");
                Console.WriteLine("Oracleでカラム名の先頭が_の場合、\"(引用符)で囲む必要がある。");
                return;
            }
            var dmd = CreateDaoMetaData(typeof(IUnderscoreEntityDao));
            var cmd = dmd.GetSqlCommand("Insert");
            var cmd2 = dmd.GetSqlCommand("GetUnderScoreEntityByUnderScoreNo");
            var entity = new UnderscoreEntity();
            entity.UnderScoreNo = 100;
            entity._Table_Name = "1";
            entity._Table_Name_ = "2";
            entity.Table_Name_ = "3";
            entity.TableName = "4";
            var count = (int) cmd.Execute(new object[] { entity });
            Assert.AreEqual(1, count, "1");
            var entityLast = (UnderscoreEntity) cmd2.Execute(new object[] { 100 });
            Assert.AreEqual("1", entityLast._Table_Name);
            Assert.AreEqual("2", entityLast._Table_Name_);
            Assert.AreEqual("3", entityLast.Table_Name_);
            Assert.AreEqual("4", entityLast.TableName);
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute2Tx()
        {
            var dmd = CreateDaoMetaData(typeof(IIdTableAutoDao));

            var cmd = dmd.GetSqlCommand("Insert");
            var table = new IdTable();
            table.IdName = "hoge";
            var count1 = (int) cmd.Execute(new object[] { table });
            Assert.AreEqual(1, count1, "1");
            var id1 = table.MyId;
            Trace.WriteLine(id1);
            var count2 = (int) cmd.Execute(new object[] { table });
            Assert.AreEqual(1, count2, "2");
            var id2 = table.MyId;
            Trace.WriteLine(id2);
            Assert.AreEqual(1, id2 - id1, "2");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute3_1Tx()
        {
            var dmd = CreateDaoMetaData(typeof(IIdTableAutoDao));
            var cmd = dmd.GetSqlCommand("Insert");
            var table = new IdTable();
            table.IdName = "hoge";
            var count = (int) cmd.Execute(new object[] { table });
            Assert.AreEqual(1, count, "1");
            Trace.WriteLine(table.MyId);
            Assert.IsTrue(table.MyId > 0, "2");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute3_2Tx()
        {
            var dmd = CreateDaoMetaData(typeof(IIdTableAuto2Dao));
            var cmd = dmd.GetSqlCommand("Insert");
            var table1 = new IdTable2();
            table1.IdName = "hoge";
            var count = (int) cmd.Execute(new object[] { table1 });
            Assert.AreEqual(1, count, "1");
            Trace.WriteLine(table1.MyId);
            Assert.IsTrue(table1.MyId > 0, "2");

            var table2 = new IdTable2();
            table2.IdName = "foo";
            cmd.Execute(new object[] { table2 });
            Trace.WriteLine(table2.MyId);
            Assert.IsTrue(table2.MyId > table1.MyId, "3");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute4Tx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.GetSqlCommand("Insert2");
            var emp = new Employee();
            emp.Empno = 99;
            emp.Ename = "hoge";
            var count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute5Tx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.GetSqlCommand("Insert3");
            var emp = new Employee();
            emp.Empno = 99;
            emp.Ename = "hoge";
            emp.Deptno = 10;
            var count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }
    }
}
