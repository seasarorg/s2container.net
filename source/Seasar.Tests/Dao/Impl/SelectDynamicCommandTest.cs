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
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;
using System.Data;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class SelectDynamicCommandTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestExecuteTx()
        {
            SelectDynamicCommand cmd = new SelectDynamicCommand(DataSource,
                BasicCommandFactory.INSTANCE,
                new BeanMetaDataDataReaderHandler(CreateBeanMetaData(typeof(Employee)), new RowCreatorImpl(), new RelationRowCreatorImpl()),
                BasicDataReaderFactory.INSTANCE);
            cmd.Sql = "SELECT * FROM emp WHERE empno = /*empno*/1234";
            cmd.ArgNames = new string[] { "empno" };
            cmd.ArgTypes = new Type[] { typeof(int) };
            Employee emp = (Employee) cmd.Execute(new object[] { 7788 });
            Trace.WriteLine(emp);
            Assert.IsNotNull(emp, "1");
        }

        /// <summary>
        /// https://www.seasar.org/issues/browse/DAONET-60
        /// </summary>
        [Test, S2]
        public void TestExecute_DataTableTx()
        {
            SelectDynamicCommand cmd = new SelectDynamicCommand(DataSource,
                BasicCommandFactory.INSTANCE,
                new BeanDataTableMetaDataDataReaderHandler(typeof(DataTable)),
                BasicDataReaderFactory.INSTANCE);
            cmd.Sql = "SELECT emp.empno,emp.ename,dept.deptno,dept.dname FROM emp left outer join dept on emp.deptno = dept.deptno where emp.empno = /*employeeNo*/7369";
            cmd.ArgNames = new string[] { "employeeNo" };
            cmd.ArgTypes = new Type[] { typeof(int) };

            const int EMP_NO = 7788;
            object ret = cmd.Execute(new object[] { EMP_NO });
            Assert.IsNotNull(ret, "1");
            DataTable actual = ret as DataTable;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Rows.Count);
            DataRow actualRow = actual.Rows[0];
            Assert.AreEqual(EMP_NO, actualRow["EMPNO"]);

            foreach ( DataColumn col in actual.Columns )
            {
                Trace.Write(col.ColumnName + "=");
                Trace.Write(actualRow[col] == DBNull.Value ? "null" : actualRow[col]);
                Trace.Write(" ");
            }
        }

        /// <summary>
        /// https://www.seasar.org/issues/browse/DAONET-60
        /// </summary>
        [Test, S2]
        public void TestExecute_CustomDataTableTx()
        {
            SelectDynamicCommand cmd = new SelectDynamicCommand(DataSource,
                BasicCommandFactory.INSTANCE,
                new BeanDataTableMetaDataDataReaderHandler(typeof(EmployeeDataSet.EmpAndDeptDataTable)),
                BasicDataReaderFactory.INSTANCE);
            cmd.Sql = "SELECT emp.empno,emp.ename,dept.deptno,dept.dname FROM emp left outer join dept on emp.deptno = dept.deptno where emp.empno = /*employeeNo*/7369";
            cmd.ArgNames = new string[] { "employeeNo" };
            cmd.ArgTypes = new Type[] { typeof(int) };

            const int EMP_NO = 7788;
            object ret = cmd.Execute(new object[] { EMP_NO });
            Assert.IsNotNull(ret, "1");
            EmployeeDataSet.EmpAndDeptDataTable actual = ret as EmployeeDataSet.EmpAndDeptDataTable;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Rows.Count);
            DataRow actualRow = actual.Rows[0];
            Assert.AreEqual(EMP_NO, actualRow["EMPNO"]);

            foreach ( DataColumn col in actual.Columns )
            {
                Trace.Write(col.ColumnName + "=");
                Trace.Write(actualRow[col] == DBNull.Value ? "null" : actualRow[col]);
                Trace.Write(" ");
            }
        }

        /// <summary>
        /// https://www.seasar.org/issues/browse/DAONET-60
        /// </summary>
        [Test, S2]
        public void TestExecute_DataSetTx()
        {
            SelectDynamicCommand cmd = new SelectDynamicCommand(DataSource,
                BasicCommandFactory.INSTANCE,
                new BeanDataSetMetaDataDataReaderHandler(typeof(DataSet)),
                BasicDataReaderFactory.INSTANCE);
            cmd.Sql = "SELECT emp.empno,emp.ename,dept.deptno,dept.dname FROM emp left outer join dept on emp.deptno = dept.deptno where emp.empno = /*employeeNo*/7369";
            cmd.ArgNames = new string[] { "employeeNo" };
            cmd.ArgTypes = new Type[] { typeof(int) };

            const int EMP_NO = 7788;
            object ret = cmd.Execute(new object[] { EMP_NO });
            Assert.IsNotNull(ret, "1");
            DataSet actual = ret as DataSet;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            DataTable actualTable = actual.Tables[0];
            Assert.AreEqual(1, actualTable.Rows.Count);
            DataRow actualRow = actualTable.Rows[0];
            Assert.AreEqual(EMP_NO, actualRow["EMPNO"]);

            foreach ( DataColumn col in actualTable.Columns )
            {
                Trace.Write(col.ColumnName + "=");
                Trace.Write(actualRow[col] == DBNull.Value ? "null" : actualRow[col]);
                Trace.Write(" ");
            }
        }

        /// <summary>
        /// https://www.seasar.org/issues/browse/DAONET-60
        /// </summary>
        [Test, S2]
        public void TestExecute_CustomDataSetTx()
        {
            SelectDynamicCommand cmd = new SelectDynamicCommand(DataSource,
                BasicCommandFactory.INSTANCE,
                new BeanDataSetMetaDataDataReaderHandler(typeof(EmployeeDataSet)),
                BasicDataReaderFactory.INSTANCE);
            cmd.Sql = "SELECT emp.empno,emp.ename,dept.deptno,dept.dname FROM emp left outer join dept on emp.deptno = dept.deptno where emp.empno = /*employeeNo*/7369";
            cmd.ArgNames = new string[] { "employeeNo" };
            cmd.ArgTypes = new Type[] { typeof(int) };

            const int EMP_NO = 7788;
            object ret = cmd.Execute(new object[] { EMP_NO });
            Assert.IsNotNull(ret, "1");
            EmployeeDataSet actual = ret as EmployeeDataSet;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Tables.Count);
            DataTable actualTable = actual.Tables[0];
            Assert.AreEqual(1, actualTable.Rows.Count);
            DataRow actualRow = actualTable.Rows[0];
            Assert.AreEqual(EMP_NO, actualRow["EMPNO"]);

            foreach ( DataColumn col in actualTable.Columns )
            {
                Trace.Write(col.ColumnName + "=");
                Trace.Write(actualRow[col] == DBNull.Value ? "null" : actualRow[col]);
                Trace.Write(" ");
            }
        }
    }
}
