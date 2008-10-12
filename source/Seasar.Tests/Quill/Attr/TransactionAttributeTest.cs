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
using MbUnit.Framework;
using Seasar.Dao.Attrs;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Quill;
using Seasar.Quill.Attrs;
using Seasar.Quill.Database.Tx.Impl;
using Seasar.Quill.Exception;
using Seasar.Quill.Unit;
using Seasar.Tests.Dao.Impl;

namespace Seasar.Tests.Quill.Attr
{
    [TestFixture]
    public class TransactionAttributeTest : QuillTestCase
    {
        [Test]
        public void TestSetTypicalTxSetting()
        {
            using (QuillContainer container = new QuillContainer())
            {
                object[] attrs = typeof(IWithTxAttr0).GetCustomAttributes(false);
                Assert.AreEqual(1, attrs.Length);
                Assert.IsTrue(attrs[0] is TransactionAttribute);
                Assert.AreEqual(typeof(TypicalTransactionSetting).Name,
                    ((TransactionAttribute)attrs[0]).TransactionSettingType.Name);
            }
        }

        [Test]
        public void TestIntercept()
        {
            using (QuillContainer container = new QuillContainer())
            {
                QuillComponent qc = container.GetComponent(typeof(IWithTxAttr1));
                IWithTxAttr1 actual = (IWithTxAttr1)qc.GetComponentObject(
                    typeof(IWithTxAttr1));
                Assert.IsNotNull(actual);

                Employee emp = actual.GetEmployee();
                Assert.IsNotNull(emp);
                Assert.AreEqual(9999, emp.Empno);
            }
        }

        [Test, Quill]
        public void TestCommit()
        {

            using (QuillContainer container = new QuillContainer())
            {
                QuillComponent qc = container.GetComponent(typeof(IWithTxAttr2));
                IWithTxAttr2 actual = (IWithTxAttr2)qc.GetComponentObject(
                    typeof(IWithTxAttr2));
               
                Employee emp0 = actual.GetEmployee(9999);
                Assert.IsNull(emp0);

                Employee emp = new Employee();
                emp.Empno = 9999;
                emp.Ename = "TestCommit";
                //  トランザクション境界がInsert,Deleteといったメソッドに
                //  なっているはずなので実行ごとにコミットされているはず
                Assert.Greater(actual.Insert(emp), 0);

                Employee emp1 = actual.GetEmployee((int)emp.Empno);
                Assert.IsNotNull(emp1);
                Assert.AreEqual(emp.Ename, emp1.Ename);

                Assert.Greater(actual.Delete(emp1), 0);

                Employee emp2 = actual.GetEmployee((int)emp.Empno);
                Assert.IsNull(emp2);
            }
        }

        [Test, Quill]
        public void TestRollback()
        {
            const int TEST_ID = 7369;
            TxLogicTestParent actual = new TxLogicTestParent();
            QuillInjector injector = QuillInjector.GetInstance();
            injector.Inject(actual);

            try
            {
                actual.Execute(TEST_ID);
                Assert.Fail("1");
            }
            catch (QuillApplicationException)
            {
            }

            try
            {
                //  更新は確かに行われていたか
                Assert.AreEqual(TEST_ID, actual.Logic._TempEmployee.Empno, "2");
                Assert.AreEqual("Updated", actual.Logic._TempEmployee.Ename, "3");

                Employee rollbackedEmp = actual.GetEmp(TEST_ID);
                //  ロールバックされているはずなので
                //  雇用者名が元に戻っているはず
                Assert.AreEqual(TEST_ID, rollbackedEmp.Empno, "4");
                Assert.AreEqual("SMITH", rollbackedEmp.Ename, "5");
            }
            finally
            {
                injector.Destroy();
            }
        }

        [Test]
        public void TestIllegalArgument()
        {
            using (QuillContainer container = new QuillContainer())
            {
                try
                {
                    container.GetComponent(typeof(IWithTxAttr3));
                    Assert.Fail();
                }
                catch (QuillApplicationException)
                {
                    Console.WriteLine("OK");
                }
            }
        }
    }

    [Transaction]
    public interface IWithTxAttr0
    {
        Employee GetEmployee();
        int Insert(Employee emp);
        int Delete(Employee emp);
    }

    [Transaction(typeof(CustomTxSetting))]
    public interface IWithTxAttr1
    {
        Employee GetEmployee();
        int Insert(Employee emp);
        int Delete(Employee emp);
    }

    [Transaction]
    [S2Dao]
    [Bean(typeof(Employee))]
    public interface IWithTxAttr2
    {
        [Sql("SELECT * FROM EMP WHERE EMPNO = /*empno*/1")]
        Employee GetEmployee(int empno);
        int Insert(Employee emp);
        int Delete(Employee emp);
    }

    /// <summary>
    /// ITransactionSettingを実装していないクラスをあえて指定
    /// </summary>
    [Transaction(typeof(TxLogicTestParent))]
    public interface IWithTxAttr3
    {
        Employee GetEmployee();
        int Insert(Employee emp);
        int Delete(Employee emp);
    }

    public class TxLogicTestParent
    {
        public TxLogicTest Logic;

        public void Execute(int empno)
        {
            Logic.UpdateAndError(empno);
        }

        public Employee GetEmp(int empno)
        {
            return Logic.GetEmp(empno);
        }
    }

    [Implementation]
    public class TxLogicTest
    {
        public IWithDaoAttrEx Dao;

        public Employee _TempEmployee = null;

        [Transaction]
        public virtual void UpdateAndError(int empno)
        {
            Employee emp = Dao.GetEmployee(empno);
            emp.Ename = "Updated";

            Dao.Update(emp);

            Employee empX = Dao.GetEmployee(empno);
            _TempEmployee = empX;

            throw new QuillApplicationException("Error");
        }

        public Employee GetEmp(int empno)
        {
            return Dao.GetEmployee(empno);
        }
    }

    [S2Dao]
    [Bean(typeof(Employee))]
    [Implementation]
    public interface IWithDaoAttrEx
    {
        [Sql("SELECT * FROM EMP WHERE EMPNO = /*empno*/1")]
        Employee GetEmployee(int empno);
        int Update(Employee emp);
    }

    public class CustomTxSetting : AbstractTransactionSetting
    {
        protected override void SetupTransaction(Seasar.Extension.ADO.IDataSource dataSource)
        {
            _transactionContext = new TransactionContext();
            _transactionInterceptor = new DummyInterceptor();
        }
    }

    public class DummyInterceptor : AbstractInterceptor
    {
        public override object Invoke(Seasar.Framework.Aop.IMethodInvocation invocation)
        {
            Employee dummy = new Employee();
            dummy.Empno = 9999;
            dummy.Ename = "Dummy";
            return dummy;
        }
    }

}
