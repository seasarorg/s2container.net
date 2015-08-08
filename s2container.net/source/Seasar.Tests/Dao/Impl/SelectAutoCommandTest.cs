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
using System.Data;
using MbUnit.Framework;
using Seasar.Dao.Unit;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class SelectAutoCommandTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestExecuteQueryAnnotationAndReturnDataTable()
        {
            const int EMP_NO = 7499;
            const int DEPT_NO = 30;

            var dmd = CreateDaoMetaData(typeof(IEmployeeDataSetDao));
            var cmd = dmd.GetSqlCommand("SelectDataTable");

            var ret = cmd.Execute(new object[] { EMP_NO });
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret is EmployeeDataSet.EmpAndDeptDataTable, ret.GetExType().Name);
            var actualTable = (DataTable)ret;
            Assert.AreEqual(1, actualTable.Rows.Count);
            var actualRow = actualTable.Rows[0];
            // #.NET4.0 型を揃えないと違う値として扱われる
            //Assert.AreEqual(EMP_NO, actualRow["EMPNO"]);
            Assert.AreEqual(EMP_NO, Int32.Parse(actualRow["EMPNO"].ToString()));

            Assert.AreEqual("ALLEN", actualRow["ENAME"]);
            // #.NET4.0 型を揃えないと違う値として扱われる
            //Assert.AreEqual(DEPT_NO, actualRow["DEPTNO"]);
            Assert.AreEqual(DEPT_NO, Int32.Parse(actualRow["DEPTNO"].ToString()));

            Assert.AreEqual("SALES", actualRow["DNAME"]);
        }

        [Test, S2]
        public void TestExecuteQueryAnnotationAndReturnDataSet()
        {
            const int EMP_NO = 7499;
            const int DEPT_NO = 30;

            var dmd = CreateDaoMetaData(typeof(IEmployeeDataSetDao));
            var cmd = dmd.GetSqlCommand("SelectDataSet");

            var ret = cmd.Execute(new object[] { EMP_NO });
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret is EmployeeDataSet);
            var actualSet = (DataSet)ret;
            Assert.AreEqual(1, actualSet.Tables.Count);
            var actualTable = actualSet.Tables[0];
            Assert.IsTrue(actualTable is EmployeeDataSet.EmpAndDeptDataTable);
            Assert.AreEqual(1, actualTable.Rows.Count);
            var actualRow = actualTable.Rows[0];
            // #.NET4.0 型を揃えないと違う値として扱われる
            //Assert.AreEqual(EMP_NO, actualRow["EMPNO"]);
            Assert.AreEqual(EMP_NO, Int32.Parse(actualRow["EMPNO"].ToString()));
            Assert.AreEqual("ALLEN", actualRow["ENAME"]);
            // #.NET4.0 型を揃えないと違う値として扱われる
            Assert.AreEqual(DEPT_NO, Int32.Parse(actualRow["DEPTNO"].ToString()));
            //Assert.AreEqual(DEPT_NO, actualRow["DEPTNO"]);
            Assert.AreEqual("SALES", actualRow["DNAME"]);
        }
    }
}
