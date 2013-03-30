#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using Seasar.Quill.Util;
using Seasar.Quill.Database.DataSource.Impl;

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
            const string UPD_NAME = "Updated";
            TxLogicTestParent actual = new TxLogicTestParent();
            QuillInjector injector = QuillInjector.GetInstance();
            injector.Inject(actual);

            // テスト前の情報を保持
            Employee originalEmp = actual.GetEmp(TEST_ID);
            try
            {
                actual.ExecuteUpdate(TEST_ID, UPD_NAME);
                Assert.Fail("1");
            }
            catch (QuillApplicationException)
            {
            }

            try
            {
                //  更新は確かに行われていたか
                Assert.AreEqual(TEST_ID, actual.Logic.TempEmployee.Empno, "2");
                Assert.AreEqual(UPD_NAME, actual.Logic.TempEmployee.Ename, "3");
                Assert.AreNotEqual(originalEmp.Ename, actual.Logic.TempEmployee.Ename, "3_5");

                Employee rollbackedEmp = actual.GetEmp(TEST_ID);
                //  ロールバックされているはずなので
                //  雇用者名が元に戻っているはず
                Assert.AreEqual(TEST_ID, rollbackedEmp.Empno, "4");
                Assert.AreEqual(originalEmp.Ename, rollbackedEmp.Ename, "5");
            }
            finally
            {
                injector.Destroy();
            }
        }

        [Test, Quill]
        public void TestRollback_NoTransaction()
        {
            const int TEST_ID = 7369;
            const string UPD_NAME = "Updated";
            
            TxLogicTestParent actual = new TxLogicTestParent();
            QuillInjector injector = QuillInjector.GetInstance();
            injector.Inject(actual);

            // テスト前の情報を保持
            Employee originalEmp = actual.GetEmp(TEST_ID);
            try
            {
                actual.ExecuteUpdateNoTransaction(TEST_ID, UPD_NAME);
                Assert.Fail("1");
            }
            catch (QuillApplicationException)
            {
            }

            try
            {
                //  更新は確かに行われていたか
                Assert.AreEqual(TEST_ID, actual.Logic.TempEmployee.Empno, "2");
                Assert.AreEqual(UPD_NAME, actual.Logic.TempEmployee.Ename, "3");
                Assert.AreNotEqual(originalEmp.Ename, actual.Logic.TempEmployee.Ename, "3_5");

                Employee rollbackedEmp = actual.GetEmp(TEST_ID);
                //  ロールバックされていないため
                //  雇用者名は変更されたまま
                Assert.AreEqual(TEST_ID, rollbackedEmp.Empno, "4");
                Assert.AreEqual(UPD_NAME, rollbackedEmp.Ename, "5");

                //  変更した名前をテスト前の状態に戻す
                actual.Logic.Revert(TEST_ID, originalEmp.Ename);
                Employee revertedEmp = actual.GetEmp(TEST_ID);
                Assert.AreEqual(originalEmp.Ename, revertedEmp.Ename, "6");
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

        [Test]
        public void TestDataSourceNameChange_Class()
        {
            QuillContainer container = new QuillContainer();
            IWithTxAttrDataSourceNameChange_Class actual =
                (IWithTxAttrDataSourceNameChange_Class) ComponentUtil.GetComponent(
                container, typeof (IWithTxAttrDataSourceNameChange_Class));
            SelectableDataSourceProxyWithDictionary proxy =
                (SelectableDataSourceProxyWithDictionary) ComponentUtil.GetComponent(
                container,typeof (SelectableDataSourceProxyWithDictionary));
            const string START_NAME = "Start";
            proxy.SetDataSourceName(START_NAME);
            Assert.AreEqual(START_NAME, proxy.GetDataSourceName(), "現在のデータソース名確認");

            actual.GetEmployee();   //  データソースが変更されるInterceptorがかかっているはず

            Assert.AreEqual(DataSourceNameChangeTxSetting.TEST_DATASOURCE_NAME,
                proxy.GetDataSourceName(), "データソース名が切り替わっているはず");
        }

        [Test]
        public void TestDataSourceNameChange_Method()
        {
            QuillContainer container = new QuillContainer();
            IWithTxAttrDataSourceNameChange_Method actual =
                (IWithTxAttrDataSourceNameChange_Method)ComponentUtil.GetComponent(
                container, typeof(IWithTxAttrDataSourceNameChange_Method));
            SelectableDataSourceProxyWithDictionary proxy =
                (SelectableDataSourceProxyWithDictionary)ComponentUtil.GetComponent(
                container, typeof(SelectableDataSourceProxyWithDictionary));
            const string START_NAME = "Start";
            proxy.SetDataSourceName(START_NAME);
            Assert.AreEqual(START_NAME, proxy.GetDataSourceName(), "現在のデータソース名確認");

            actual.GetEmployee();   //  データソースが変更されるInterceptorがかかっているはず

            Assert.AreEqual(DataSourceNameChangeTxSetting.TEST_DATASOURCE_NAME,
                proxy.GetDataSourceName(), "データソース名が切り替わっているはず");
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

    /// <summary>
    /// データソース切替テスト用IF
    /// </summary>
    [Transaction(typeof(DataSourceNameChangeTxSetting))]
    public interface IWithTxAttrDataSourceNameChange_Class
    {
        Employee GetEmployee();
    }

    /// <summary>
    /// データソース切替テスト用IF
    /// </summary> 
    public interface IWithTxAttrDataSourceNameChange_Method
    {
        [Transaction(typeof(DataSourceNameChangeTxSetting))]
        Employee GetEmployee();
    }

    public class TxLogicTestParent
    {
        public TxLogicTest Logic;

        public void ExecuteUpdate(int empno, string name)
        {
            Logic.UpdateAndError(empno, name);
        }

        public void ExecuteUpdateNoTransaction(int empno, string name)
        {
            Logic.UpdateAndError_NoTransaction(empno, name);
        }

        public void ExecuteRevert(int empno, string name)
        {
            Logic.Revert(empno, name);
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

        public Employee TempEmployee = null;

        [Transaction]
        public virtual void UpdateAndError(int empno, string name)
        {
            Employee emp = Dao.GetEmployee(empno);
            emp.Ename = name;

            Dao.Update(emp);

            Employee empX = Dao.GetEmployee(empno);
            TempEmployee = empX;

            throw new QuillApplicationException("Error");
        }

        /// <summary>
        /// Transaction属性を設定しない更新処理
        /// </summary>
        /// <param name="empno"></param>
        /// <param name="name"></param>
        public virtual void UpdateAndError_NoTransaction(int empno, string name)
        {
            Employee emp = Dao.GetEmployee(empno);
            emp.Ename = name;

            Dao.Update(emp);

            Employee empX = Dao.GetEmployee(empno);
            // 例外を発生させるため結果は戻り値として返すのではなく
            // プロパティとして設定しておく
            TempEmployee = empX;

            throw new QuillApplicationException("Error");
        }

        /// <summary>
        /// UpdateAndError_NoTransactionで変更した内容を元に戻すための更新処理
        /// </summary>
        /// <param name="empno"></param>
        /// <param name="name"></param>
        public virtual void Revert(int empno, string name)
        {
            Employee emp = Dao.GetEmployee(empno);
            emp.Ename = name;

            Dao.Update(emp);

            Employee empX = Dao.GetEmployee(empno);
            TempEmployee = empX;
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

    /// <summary>
    /// データソース変更テスト用
    /// </summary>
    public class DataSourceNameChangeTxSetting : AbstractTransactionSetting
    {
        public const string TEST_DATASOURCE_NAME = "ChangedDataSource";

        public override string DataSourceName
        {
            get
            {
                return TEST_DATASOURCE_NAME;
            }
        }

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
