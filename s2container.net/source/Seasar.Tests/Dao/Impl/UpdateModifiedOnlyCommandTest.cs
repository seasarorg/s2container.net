#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
    public class UpdateModifiedOnlyCommandTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteModifiedOnlyTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeModifiedOnlyDao));
            ISqlCommand updateCommand = dmd.GetSqlCommand("UpdateModifiedOnly");
            ISqlCommand selectCommand = dmd.GetSqlCommand("GetEmployee");
            const int TEST_EMP_NO = 7369;
            {
                EmployeeModifiedOnly entity = new EmployeeModifiedOnly();
                Assert.AreEqual(0, entity.ModifiedPropertyNames.Count);
                entity.Empno = TEST_EMP_NO;
                entity.JobName = "Hoge";
                Assert.IsTrue(entity.ModifiedPropertyNames.Count > 0);

                int modifiedCount = (int)updateCommand.Execute(new object[] { entity });

                Assert.IsTrue(modifiedCount > 0);
                EmployeeModifiedOnly afterEntity = selectCommand.Execute(new object[] { TEST_EMP_NO }) as EmployeeModifiedOnly;
                Assert.IsNotNull(afterEntity, "更新したエンティティが存在する");
                Console.WriteLine(afterEntity.ToString());
                Assert.AreEqual(entity.JobName, afterEntity.JobName, "更新したプロパティは更新後の値になっている");
                Assert.IsFalse(string.IsNullOrEmpty(afterEntity.Ename), "更新していないプロパティは更新実行前の値のまま1");
                Assert.IsTrue(afterEntity.Hiredate.HasValue, "更新していないプロパティは更新実行前の値のまま2");
                Assert.IsTrue(afterEntity.Mgr.HasValue, "更新していないプロパティは更新実行前の値のまま3");
                Assert.IsTrue(afterEntity.Sal.HasValue, "更新していないプロパティは更新実行前の値のまま4");
            }
            {
                EmployeeModifiedOnly entity = selectCommand.Execute(new object[] { TEST_EMP_NO }) as EmployeeModifiedOnly;
                Assert.IsNotNull(entity);
                Assert.AreEqual(0, entity.ModifiedPropertyNames.Count, "select直後は更新プロパティは無しの状態");
                entity.Empno = TEST_EMP_NO;
                entity.JobName = "Hoge";
                Assert.IsTrue(entity.ModifiedPropertyNames.Count > 0);

                int modifiedCount = (int)updateCommand.Execute(new object[] { entity });

                Assert.IsTrue(modifiedCount > 0);
                EmployeeModifiedOnly afterEntity = selectCommand.Execute(new object[] { TEST_EMP_NO }) as EmployeeModifiedOnly;
                Assert.IsNotNull(afterEntity, "更新したエンティティが存在する");
                Console.WriteLine(afterEntity.ToString());
                Assert.AreEqual(entity.JobName, afterEntity.JobName, "更新したプロパティは更新後の値になっている");
                Assert.IsFalse(string.IsNullOrEmpty(afterEntity.Ename), "更新していないプロパティは更新実行前の値のまま1");
                Assert.IsTrue(afterEntity.Hiredate.HasValue, "更新していないプロパティは更新実行前の値のまま2");
                Assert.IsTrue(afterEntity.Mgr.HasValue, "更新していないプロパティは更新実行前の値のまま3");
                Assert.IsTrue(afterEntity.Sal.HasValue, "更新していないプロパティは更新実行前の値のまま4");
            }
            {
                EmployeeModifiedOnly entity = selectCommand.Execute(new object[] { TEST_EMP_NO }) as EmployeeModifiedOnly;
                Assert.IsNotNull(entity);
                Assert.AreEqual(0, entity.ModifiedPropertyNames.Count, "select直後は更新プロパティは無しの状態");
                entity.Empno = TEST_EMP_NO;

                int modifiedCount = (int)updateCommand.Execute(new object[] { entity });

                Assert.AreEqual(0, modifiedCount, "更新プロパティがないときは更新が実行されない");
            }
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteModifiedOnlyWithoutClearMethodTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeModifiedOnlyWithoutClearMethodDao));
            ISqlCommand updateCommand = dmd.GetSqlCommand("UpdateModifiedOnly");
            ISqlCommand selectCommand = dmd.GetSqlCommand("GetEmployee");
            const int TEST_EMP_NO = 7369;
            {
                EmployeeModifiedOnlyWithoutClearMethod entity = new EmployeeModifiedOnlyWithoutClearMethod();
                Assert.AreEqual(0, entity.ModifiedPropertyNames.Count);
                entity.Empno = TEST_EMP_NO;
                entity.JobName = "Hoge";
                Assert.IsTrue(entity.ModifiedPropertyNames.Count > 0);

                int modifiedCount = (int)updateCommand.Execute(new object[] { entity });

                Assert.IsTrue(modifiedCount > 0);
                EmployeeModifiedOnlyWithoutClearMethod afterEntity = selectCommand.Execute(new object[] { TEST_EMP_NO }) as EmployeeModifiedOnlyWithoutClearMethod;
                Assert.IsNotNull(afterEntity, "更新したエンティティが存在する");
                Console.WriteLine(afterEntity.ToString());
                Assert.AreEqual(entity.JobName, afterEntity.JobName, "更新したプロパティは更新後の値になっている");
                Assert.IsFalse(string.IsNullOrEmpty(afterEntity.Ename), "更新していないプロパティは更新実行前の値のまま1");
                Assert.IsTrue(afterEntity.Hiredate.HasValue, "更新していないプロパティは更新実行前の値のまま2");
                Assert.IsTrue(afterEntity.Mgr.HasValue, "更新していないプロパティは更新実行前の値のまま3");
                Assert.IsTrue(afterEntity.Sal.HasValue, "更新していないプロパティは更新実行前の値のまま4");
            }
            {
                EmployeeModifiedOnlyWithoutClearMethod entity = selectCommand.Execute(new object[] { TEST_EMP_NO }) as EmployeeModifiedOnlyWithoutClearMethod;
                Assert.IsNotNull(entity);
                Assert.AreEqual(0, entity.ModifiedPropertyNames.Count, "select直後は更新プロパティは無しの状態");
                entity.Empno = TEST_EMP_NO;
                entity.JobName = "Hoge";
                Assert.IsTrue(entity.ModifiedPropertyNames.Count > 0);

                int modifiedCount = (int)updateCommand.Execute(new object[] { entity });

                Assert.IsTrue(modifiedCount > 0);
                EmployeeModifiedOnlyWithoutClearMethod afterEntity = selectCommand.Execute(new object[] { TEST_EMP_NO }) as EmployeeModifiedOnlyWithoutClearMethod;
                Assert.IsNotNull(afterEntity, "更新したエンティティが存在する");
                Console.WriteLine(afterEntity.ToString());
                Assert.AreEqual(entity.JobName, afterEntity.JobName, "更新したプロパティは更新後の値になっている");
                Assert.IsFalse(string.IsNullOrEmpty(afterEntity.Ename), "更新していないプロパティは更新実行前の値のまま1");
                Assert.IsTrue(afterEntity.Hiredate.HasValue, "更新していないプロパティは更新実行前の値のまま2");
                Assert.IsTrue(afterEntity.Mgr.HasValue, "更新していないプロパティは更新実行前の値のまま3");
                Assert.IsTrue(afterEntity.Sal.HasValue, "更新していないプロパティは更新実行前の値のまま4");
            }
            {
                EmployeeModifiedOnlyWithoutClearMethod entity = selectCommand.Execute(new object[] { TEST_EMP_NO }) as EmployeeModifiedOnlyWithoutClearMethod;
                Assert.IsNotNull(entity);
                Assert.AreEqual(0, entity.ModifiedPropertyNames.Count, "select直後は更新プロパティは無しの状態");
                entity.Empno = TEST_EMP_NO;

                int modifiedCount = (int)updateCommand.Execute(new object[] { entity });

                Assert.AreEqual(0, modifiedCount, "更新プロパティがないときは更新が実行されない");
            }
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteClearModifiedMethodOnlyTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeClearModifiedMethodOnlyDao));
            ISqlCommand updateCommand1 = dmd.GetSqlCommand("UpdateModifiedOnly");
            ISqlCommand selectCommand = dmd.GetSqlCommand("GetEmployee");
            const int TEST_EMP_NO = 7369;
            {
                EmployeeClearModifiedMethodOnly entity = selectCommand.Execute(new object[] { TEST_EMP_NO }) as EmployeeClearModifiedMethodOnly;
                Assert.IsTrue(entity.IsClearMethodCalled, "ClearModifiedOnlyPropertyNamesメソッドがあればそれは呼ばれている");
                entity.Empno = TEST_EMP_NO;
                entity.JobName = "Hoge";

                try
                {
                    updateCommand1.Execute(new object[] { entity });
                    Assert.Fail("ModifiedOnlyPropertyNamesがない場合は例外が発生する。");
                }
                catch ( NotFoundModifiedPropertiesRuntimeException ex )
                {
                    Console.WriteLine(ex.Message);
                }
            }
            ISqlCommand updateCommand2 = dmd.GetSqlCommand("Update");
            {
                EmployeeClearModifiedMethodOnly entity = new EmployeeClearModifiedMethodOnly();
                entity.Empno = TEST_EMP_NO;
                entity.JobName = "Hoge";

                int modifiedCount = (int)updateCommand2.Execute(new object[] { entity });

                Assert.IsTrue(modifiedCount > 0, "通常のupdateは例外とならずに実行される");
                EmployeeClearModifiedMethodOnly afterEntity = selectCommand.Execute(new object[] { TEST_EMP_NO }) as EmployeeClearModifiedMethodOnly;
                Assert.IsNotNull(afterEntity, "更新したエンティティが存在する");
                Assert.AreEqual(entity.JobName, afterEntity.JobName);
                Assert.IsNull(afterEntity.Ename, "更新していないプロパティはnull更新1");
                Assert.IsFalse(afterEntity.Hiredate.HasValue, "更新していないプロパティはnull更新2");
                Assert.IsFalse(afterEntity.Mgr.HasValue, "更新していないプロパティはnull更新3");
                Assert.IsFalse(afterEntity.Sal.HasValue, "更新していないプロパティはnull更新4");
            }
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteNoModifiedPropertyNamesAndMethodTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeNoModifiedPropertyNamesAndMethodDao));
            ISqlCommand updateCommand1 = dmd.GetSqlCommand("UpdateModifiedOnly");
            ISqlCommand selectCommand = dmd.GetSqlCommand("GetEmployee");
            const int TEST_EMP_NO = 7369;
            {
                EmployeeNoModifiedPropertyNamesAndMethod entity = selectCommand.Execute(new object[] { TEST_EMP_NO }) as EmployeeNoModifiedPropertyNamesAndMethod;
                entity.Empno = TEST_EMP_NO;
                entity.JobName = "Hoge";

                try
                {
                    updateCommand1.Execute(new object[] { entity });
                    Assert.Fail("ModifiedOnlyPropertyNamesがない場合は例外が発生する。");
                }
                catch ( NotFoundModifiedPropertiesRuntimeException ex )
                {
                    Console.WriteLine(ex.Message);
                }
            }
            ISqlCommand updateCommand2 = dmd.GetSqlCommand("Update");
            {
                EmployeeNoModifiedPropertyNamesAndMethod entity = new EmployeeNoModifiedPropertyNamesAndMethod();
                entity.Empno = TEST_EMP_NO;
                entity.JobName = "Hoge";

                int modifiedCount = (int)updateCommand2.Execute(new object[] { entity });

                Assert.IsTrue(modifiedCount > 0, "通常のupdateは例外とならずに実行される");
                EmployeeNoModifiedPropertyNamesAndMethod afterEntity = selectCommand.Execute(new object[] { TEST_EMP_NO }) as EmployeeNoModifiedPropertyNamesAndMethod;
                Assert.IsNotNull(afterEntity, "更新したエンティティが存在する");
                Assert.AreEqual(entity.JobName, afterEntity.JobName);
                Assert.IsNull(afterEntity.Ename, "更新していないプロパティはnull更新1");
                Assert.IsFalse(afterEntity.Hiredate.HasValue, "更新していないプロパティはnull更新2");
                Assert.IsFalse(afterEntity.Mgr.HasValue, "更新していないプロパティはnull更新3");
                Assert.IsFalse(afterEntity.Sal.HasValue, "更新していないプロパティはnull更新4");
            }
        }
    }
}
