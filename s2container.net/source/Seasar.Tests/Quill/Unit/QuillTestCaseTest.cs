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

using MbUnit.Framework;
using Seasar.Dao.Attrs;
using Seasar.Extension.Unit;
using Seasar.Quill.Attrs;
using Seasar.Quill.Database.DataSource.Impl;
using Seasar.Quill.Unit;
using Seasar.Tests.Dao.Impl;

namespace Seasar.Tests.Quill.Unit
{
    [TestFixture]
    public class QuillTestCaseTest : QuillTestCase
    {
        private InjectionTarget _targetObject = null;

        /// <summary>
        /// QuillInjectorによるインジェクションが行われているかチェック
        /// </summary>
        [Test, Quill(Tx.NotSupported)]
        public void TestAlreadyInjected()
        {
            Assert.IsNotNull(_targetObject, "テストクラスに定義されたインスタンス変数");
            Assert.IsNotNull(_targetObject.Dao, "上記オブジェクトがもつinterfaceプロパティ");
            Assert.IsNotNull(_targetObject.DataSourceProxy, "上記オブジェクトがもつclassプロパティ");
        }

        /// <summary>
        /// ロールバックが自動的にかかっているか確認
        /// </summary>
        [Test, Quill(Tx.Rollback)]
        public void TestRollback()
        {
            const string TEST_NAME_BEFORE = "WARD";
            const string TEST_NAME_AFTER = "Hoge";
            const int TEST_EMPNO = 7521;
            EmpDao dao = GetQuillComponent(typeof(EmpDao)) as EmpDao;
            Assert.IsNotNull(dao, "dao");

            Employee empBefore = dao.GetEmployee(TEST_EMPNO);
            Assert.IsNotNull(empBefore, "employee");
            Assert.AreEqual(TEST_EMPNO, empBefore.Empno);
            Assert.AreEqual(TEST_NAME_BEFORE, empBefore.Ename);

            empBefore.Ename = TEST_NAME_AFTER;
            int result = dao.Update(empBefore);
            Assert.GreaterThan(result, 0, "update_result");

            Employee empAfter = dao.GetEmployee(TEST_EMPNO);
            Assert.AreEqual(TEST_NAME_AFTER, empAfter.Ename, "after update");

            //  ２回実行して二度ともテストが通ればＯＫ
        }
    }

    [Implementation]
    [S2Dao]
    [Bean(typeof(Employee))]
    public interface EmpDao
    {
        [Sql("SELECT * FROM EMP WHERE EMPNO=/*empno*/1")]
        Employee GetEmployee(int empno);
        int Update(Employee emp);
    }


    [Implementation]
    public class InjectionTarget
    {
        public EmpDao Dao = null;

        private SelectableDataSourceProxyWithDictionary _dataSourceProxy = null;
        public SelectableDataSourceProxyWithDictionary DataSourceProxy
        {
            set { _dataSourceProxy = value; }
            get { return _dataSourceProxy; }
        }
    }
}
