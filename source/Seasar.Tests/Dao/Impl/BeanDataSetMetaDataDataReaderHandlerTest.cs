#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using System.Collections.Generic;
using System.Text;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO;
using Seasar.Dao.Impl;
using Seasar.Extension.Unit;
using MbUnit.Framework;
using System.Diagnostics;

namespace Seasar.Tests.Dao.Impl
{
    /// <summary>
    /// https://www.seasar.org/issues/browse/DAONET-60
    /// </summary>
    [TestFixture]
    public class BeanDataSetMetaDataDataReaderHandlerTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestHandleDataSet()
        {
            IDataReaderHandler handler = new BeanDataSetMetaDataDataReaderHandler(typeof(DataSet));

            string sql = "select emp.empno as uid, emp.*, dept.deptno as deptno_0, dept.dname as dname_0 " +
                "from emp, dept where empno = 7788 and emp.deptno = dept.deptno";
            DataSet ret;
            using ( IDbConnection con = Connection )
            {
                using ( IDbCommand cmd = con.CreateCommand() )
                {
                    cmd.CommandText = sql;

                    using ( IDataReader reader = cmd.ExecuteReader() )
                    {
                        ret = (DataSet)handler.Handle(reader);
                    }
                }
            }

            Assert.IsTrue(ret.Tables.Count > 0, "1");
            foreach ( DataTable table in ret.Tables )
            {
                Assert.IsTrue(table.Columns.Count > 0, "2");
                Assert.IsTrue(table.Rows.Count > 0, "3");
                Assert.IsTrue(table.Columns.Contains("EMPNO"), "empno");
                Assert.IsTrue(table.Columns.Contains("uid"), "uid");
                Assert.IsTrue(table.Columns.Contains("deptno_0"), "deptno_0");
                foreach ( DataRow row in table.Rows )
                {
                    foreach ( DataColumn col in table.Columns )
                    {
                        Console.WriteLine("{0} = {1}", col.ColumnName, row[col.ColumnName]);
                    }
                }
            }
        }

        [Test, S2]
        public void TestHandleCustomDataSet()
        {
            IDataReaderHandler handler = new BeanDataSetMetaDataDataReaderHandler(typeof(EmployeeDataSet));

            string sql = "select DEPT.DEPTNO, DEPT.DNAME, DEPT.LOC, EMP.EMPNO, " +
                "EMP.ENAME, EMP.JOB, EMP.MGR, EMP.HIREDATE, EMP.SAL, EMP.COMM " +
                "FROM DEPT INNER JOIN EMP ON DEPT.DEPTNO = EMP.DEPTNO " +
                "WHERE DEPT.DEPTNO=20 AND EMP.EMPNO=7566";
            EmployeeDataSet ret;
            using ( IDbConnection con = Connection )
            {
                using ( IDbCommand cmd = con.CreateCommand() )
                {
                    cmd.CommandText = sql;

                    using ( IDataReader reader = cmd.ExecuteReader() )
                    {
                        ret = (EmployeeDataSet)handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.Tables.Count > 0, "1");
            EmployeeDataSet.EmpAndDeptDataTable table = ret.EmpAndDept;
            Assert.IsNotNull(table, "2");
            Assert.IsTrue(table.Rows.Count > 0, "3");
            Assert.AreEqual("EmpAndDept", table.TableName);
            Assert.AreEqual("DEPTNO", table.DEPTNOColumn.ColumnName);
            Assert.AreEqual("DNAME", table.DNAMEColumn.ColumnName);
            Assert.AreEqual("ENAME", table.ENAMEColumn.ColumnName);
            foreach ( DataRow row in table.Rows )
            {
                EmployeeDataSet.EmpAndDeptRow customRow = row as EmployeeDataSet.EmpAndDeptRow;
                Assert.IsNotNull(customRow);
                Assert.AreEqual(20, customRow.DEPTNO);
                Assert.AreEqual(7566, customRow.EMPNO);
            }
        }
    }
}
