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
using System.Collections;
using System.Text;
using MbUnit.Framework;
using System.Data;
using Seasar.Extension.Unit;
using Seasar.Dao.Unit;
using System.Diagnostics;
using log4net;
using Seasar.Dao;

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
        public void TestExecute_QueryAnnotationAndReturnDataTable()
        {
            const int EMP_NO = 7499;
            const int DEPT_NO = 30;

            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDataSetDao));
            ISqlCommand cmd = dmd.GetSqlCommand("SelectDataTable");

            object ret = cmd.Execute(new object[] { EMP_NO });
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret is EmployeeDataSet.EmpAndDeptDataTable, ret.GetType().Name);
            DataTable actualTable = (DataTable)ret;
            Assert.AreEqual(1, actualTable.Rows.Count);
            DataRow actualRow = actualTable.Rows[0];
            Assert.AreEqual(EMP_NO, actualRow["EMPNO"]);
            Assert.AreEqual("ALLEN", actualRow["ENAME"]);
            Assert.AreEqual(DEPT_NO, actualRow["DEPTNO"]);
            Assert.AreEqual("SALES", actualRow["DNAME"]);
        }

        [Test, S2]
        public void TestExecute_QueryAnnotationAndReturnDataSet()
        {
            const int EMP_NO = 7499;
            const int DEPT_NO = 30;

            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDataSetDao));
            ISqlCommand cmd = dmd.GetSqlCommand("SelectDataSet");

            object ret = cmd.Execute(new object[] { EMP_NO });
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret is EmployeeDataSet);
            DataSet actualSet = (DataSet)ret;
            Assert.AreEqual(1, actualSet.Tables.Count);
            DataTable actualTable = actualSet.Tables[0];
            Assert.IsTrue(actualTable is EmployeeDataSet.EmpAndDeptDataTable);
            Assert.AreEqual(1, actualTable.Rows.Count);
            DataRow actualRow = actualTable.Rows[0];
            Assert.AreEqual(EMP_NO, actualRow["EMPNO"]);
            Assert.AreEqual("ALLEN", actualRow["ENAME"]);
            Assert.AreEqual(DEPT_NO, actualRow["DEPTNO"]);
            Assert.AreEqual("SALES", actualRow["DNAME"]);
        }
    }
}
