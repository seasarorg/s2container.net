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
using System.Collections;
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Dao;
using Seasar.Dao.Dbms;
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class DaoMetaDataImplTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Tests.dicon");
        }

        [Test, S2]
        public void TestSelectBeanList()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetAllEmployees");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual("SELECT * FROM emp", cmd.Sql, "2");
            var rsh = (BeanListMetaDataDataReaderHandler) cmd.DataReaderHandler;
            Assert.AreEqual(typeof(Employee), rsh.BeanMetaData.BeanType, "3");
        }

        [Test, S2]
        public void TestSelectGenericList1()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetAllEmployeesToGenericList1");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual("SELECT * FROM emp", cmd.Sql, "2");
            var rsh = (BeanGenericListMetaDataDataReaderHandler) cmd.DataReaderHandler;
            Assert.AreEqual(typeof(Employee), rsh.BeanMetaData.BeanType, "3");
        }

        [Test, S2]
        public void TestSelectGenericList2()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetAllEmployeesToGenericList2");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual("SELECT * FROM emp", cmd.Sql, "2");
            var rsh = (BeanGenericListMetaDataDataReaderHandler) cmd.DataReaderHandler;
            Assert.AreEqual(typeof(Employee), rsh.BeanMetaData.BeanType, "3");
        }

        [Test, S2]
        public void TestSelectBeanArray()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetAllEmployeeArray");
            Assert.IsNotNull(cmd, "1");
            var rsh = (BeanListMetaDataDataReaderHandler) cmd.DataReaderHandler;
            Assert.AreEqual(typeof(Employee), rsh.BeanMetaData.BeanType, "2");
        }

        [Test, S2]
        public void TestSelectObjectArray()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeObjectListDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeeNoArray");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual(typeof(ObjectArrayDataReaderHandler), cmd.DataReaderHandler.GetExType(), "2");
            Assert.AreEqual("empno", cmd.ArgNames[0], "3");
        }

        [Test, S2]
        public void TestSelectObjectGenericList()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeObjectListDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeeNoGenericList");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual(typeof(ObjectGenericListDataReaderHandler), cmd.DataReaderHandler.GetExType(), "2");
            Assert.AreEqual("empno", cmd.ArgNames[0], "3");
        }

        [Test, S2]
        public void TestSelectObjectList()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeObjectListDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeeNoList");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual(typeof(ObjectListDataReaderHandler), cmd.DataReaderHandler.GetExType(), "2");
            Assert.AreEqual("empno", cmd.ArgNames[0], "3");
        }

        [Test, S2]
        public void TestSelectBean()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual(typeof(BeanMetaDataDataReaderHandler), cmd.DataReaderHandler.GetExType(), "2");
            Assert.AreEqual("empno", cmd.ArgNames[0], "3");
        }

        [Test, S2]
        public void TestSelectCountBySqlFile1()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetCount");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual(typeof(ObjectDataReaderHandler), cmd.DataReaderHandler.GetExType(), "2");
            Assert.AreEqual("SELECT count(*) FROM emp", cmd.Sql, "3");
            var ret = cmd.Execute(new object[] { });
            //  OracleではCOUNTの結果がDecimalで返ってくるため
            if (Dbms.Dbms == KindOfDbms.Oracle)
            {
                try
                {
                    var ret4oracle = ConversionUtil.ConvertTargetType(ret, typeof(int));
                    Assert.IsTrue(ret4oracle is int, "4-1");
                }
                catch (InvalidCastException)
                {
                    Assert.Fail("4-2");
                }
            }
            else
            {
                Assert.IsTrue(ret is int, "4-3");
            }
            Assert.AreEqual(14, ret, "5");
        }

        [Test, S2]
        public void TestSelectCountBySqlFile2()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetCount2");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual(typeof(ObjectDataReaderHandler), cmd.DataReaderHandler.GetExType(), "2");
            Assert.AreEqual("SELECT count(*) FROM emp", cmd.Sql, "3");
            var ret = cmd.Execute(new object[] { });
            //  OracleではCOUNTの結果がDecimalで返ってくるため
            if (Dbms.Dbms == KindOfDbms.Oracle)
            {
                try
                {
                    var ret4oracle = ConversionUtil.ConvertTargetType(ret, typeof(int));
                    Assert.IsTrue(ret4oracle is int, "4-1");
                }
                catch (InvalidCastException)
                {
                    Assert.Fail("4-2");
                }
            }
            else
            {
                Assert.IsTrue(ret is int, "4-3");
            }
            Assert.AreEqual(14, ret, "5");
        }

        [Test, S2]
        public void TestUpdate()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            var cmd = (UpdateDynamicCommand) dmd.GetSqlCommand("Update");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual("employee", cmd.ArgNames[0], "2");
        }

        [Test, S2(Tx.Rollback)]
        public void TestInsertAutoTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.GetSqlCommand("Insert");
            Assert.IsNotNull(cmd, "1");
            var emp = new Employee();
            emp.Empno = 99;
            emp.Ename = "hoge";
            cmd.Execute(new object[] { emp });
        }

        [Test, S2(Tx.Rollback)]
        public void TestUpdateAutoTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.GetSqlCommand("Update");
            Assert.IsNotNull(cmd, "1");
            var cmd2 = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            var emp = (Employee) cmd2.Execute(new object[] { 7788 });
            emp.Ename = "hoge2";
            cmd.Execute(new object[] { emp });
        }

        [Test, S2(Tx.Rollback)]
        public void TestDeleteAutoTx()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.GetSqlCommand("Delete");
            Assert.IsNotNull(cmd, "1");
            var cmd2 = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            var emp = (Employee) cmd2.Execute(new object[] { 7788 });
            cmd.Execute(new object[] { emp });
        }

        [Test, S2, ExpectedException(typeof(IllegalSignatureRuntimeException))]
        public void TestIllegalAutoUpdateMethod()
        {
            var dmd = CreateDaoMetaData(typeof(IIllegalEmployeeAutoDao));
        }

        [Test, S2]
        public void TestSelectAuto()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeeByDeptno");
            Trace.WriteLine(cmd.Sql);
        }

        [Test, S2]
        public void TestCreateFindCommand()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.CreateFindCommand(null);
            var employees = (IList) cmd.Execute(null);
            Trace.WriteLine(employees);
            Assert.IsTrue(employees.Count > 0, "1");
        }

        [Test, S2]
        public void TestCreateFindCommand2()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.CreateFindCommand(null);
            var employees = (IList) cmd.Execute(null);
            Trace.WriteLine(employees);
            Assert.IsTrue(employees.Count > 0, "1");
        }

        [Test, S2]
        public void TestCreateFindCommand3()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.CreateFindCommand("select * from emp");
            var employees = (IList) cmd.Execute(null);
            Trace.WriteLine(employees);
            Assert.IsTrue(employees.Count > 0, "1");
        }

        [Test, S2]
        public void TestCreateFindCommand4()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.CreateFindCommand("order by empno");
            var employees = (IList) cmd.Execute(null);
            Trace.WriteLine(employees);
            Assert.IsTrue(employees.Count > 0, "1");
        }

        [Test, S2]
        public void TestCreateFindCommand5()
        {
            var dmd = (DaoMetaDataImpl) CreateDaoMetaData(typeof(IEmployeeAutoDao));
            dmd.Dbms = new Seasar.Dao.Dbms.Oracle();
            var cmd = (SelectDynamicCommand) dmd.CreateFindCommand("EMPNO = ?");
            Trace.WriteLine(cmd.Sql);
            Assert.IsTrue(cmd.Sql.EndsWith(" EMPNO = ?"), "1");
        }

        [Test, S2]
        public void TestCreateFindBeanCommand()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.CreateFindBeanCommand("empno = ?");
            var employee = (Employee) cmd.Execute(new object[] { 7788 });
            Trace.WriteLine(employee);
            Assert.IsNotNull(employee, "1");
        }

        [Test, S2]
        public void TestCreateObjectBeanCommand()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.CreateFindObjectCommand("select count(*) from emp");
            // Dbms == SQL Server の場合、System.Int32
            // Dbms == Oracle の場合、System.Decimal
            // の戻り値の型になる。
            var count = cmd.Execute(null);
            Assert.AreEqual("14", count.ToString(), "1");
        }

        [Test, S2]
        public void TestSelectAutoByQuery()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.GetSqlCommand("GetEmployeesBySal");
            var employees = (IList) cmd.Execute(new object[] { 0, 1000 });
            Trace.WriteLine(employees);
            Assert.AreEqual(2, employees.Count, "1");
        }

        [Test, S2]
        public void TestSelectAutoByQueryMultiIn()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesByEnameJob");
            Trace.WriteLine(cmd.Sql);
            IList enames = new ArrayList();
            enames.Add("SCOTT");
            enames.Add("MARY");
            IList jobs = new ArrayList();
            jobs.Add("ANALYST");
            jobs.Add("FREE");
            var employees = (IList) cmd.Execute(new object[] { enames, jobs });
            Trace.WriteLine(employees);
            Assert.AreEqual(1, employees.Count, "MARYはいないので");
        }

        /// <summary>
        /// 外部結合にWHERE句が使われるDBを想定したテスト
        /// </summary>
        [Test, S2]
        public void TestSelectAutoByQueryOuterJoinByWhereClause()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeeByDeptName");
            Trace.WriteLine(cmd.Sql);
            {
                //  Query内条件あり
                const string DNAME_CONDITION = "SALES";
                var actualList = (IList) cmd.Execute(new object[] { DNAME_CONDITION });
                Trace.WriteLine(actualList);
                Assert.IsTrue(actualList.Count > 0, "少なくとも一人はいる");
                Assert.IsTrue(actualList[0] is Employee, "Employee型で返ってくる");
                foreach (Employee emp in actualList)
                {
                    Assert.IsNotNull(emp.Department);
                    Assert.AreEqual(DNAME_CONDITION, emp.Department.Dname, "検索条件に該当している");
                }
            }
            {
                //  Query内条件なし
                var actualList = (IList) cmd.Execute(new object[] { null });
                Trace.WriteLine(actualList);
                Assert.IsTrue(actualList.Count > 0, "少なくとも一人はいる");
                Assert.IsTrue(actualList[0] is Employee, "Employee型で返ってくる");
                foreach (Employee emp in actualList)
                {
                    Assert.IsNotNull(emp.Department);
                }
            }
        }

        [Test, S2]
        public void TestRelation1()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployee2Dao));
            var cmd = dmd.GetSqlCommand("GetAllEmployees");
            var emps = (IList) cmd.Execute(null);
            Trace.WriteLine(emps);
            Assert.IsTrue(emps.Count > 0, "1");
            foreach (Employee2 emp in emps)
            {
                Assert.IsNotNull(emp.Department2);
            }
        }

        [Test, S2]
        public void TestRelation2()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployee2Dao));
            var cmd = dmd.GetSqlCommand("GetAllEmployeesOnly");
            var emps = (IList) cmd.Execute(null);
            Trace.WriteLine(emps);
            Assert.IsTrue(emps.Count > 0, "1");
            foreach (Employee2 emp in emps)
            {
                // [DAONET-56]のPerformance改善のため、
                // 空Entityは作成しないようにした。
                // これでJava版S2Daoと同じ仕様になる。[DAO-7]
                // Assert.IsNotNull(emp.Department2);
                Assert.IsNull(emp.Department2);
            }
        }

        [Test, S2(Tx.Rollback)]
        public void TestRelation3()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = dmd.GetSqlCommand("Insert");
            var emp = new Employee();
            emp.Empno = 9999;
            emp.Ename = "test";
            // Department:50 does not exist.
            emp.Deptno = 50;
            cmd.Execute(new object[] { emp });

            var cmd2 = dmd.GetSqlCommand("GetEmployee");
            var emp2 = (Employee) cmd2.Execute(new object[] { 7369 });
            Trace.WriteLine(emp2);
            Assert.IsNotNull(emp2.Department);

            var emp3 = (Employee) cmd2.Execute(new object[] { 9999 });
            Trace.WriteLine(emp3);
            Assert.IsNotNull(emp2.Department);
        }

        [Test, S2]
        public void TestGetDaoInterface()
        {
            Assert.AreEqual(typeof(IEmployeeDao), DaoMetaDataImpl.GetDaoInterface(typeof(IEmployeeDao)), "1");
            //Assert.AreEqual(typeof(IEmployeeDao), DaoMetaDataImpl.getDaoInterface(typeof(EmployeeDaoImpl)), "2");
        }

        [Test, S2]
        public void TestAutoSelectSqlByDto()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesBySearchCondition");
            Assert.IsNotNull(cmd, "1");
            Trace.WriteLine(cmd.Sql);
            var dto = new EmployeeSearchCondition();
            dto.Dname = "RESEARCH";
            var employees = (IList) cmd.Execute(new object[] { dto });
            Assert.IsTrue(employees.Count > 0, "2");
        }

        [Test, S2]
        public void TestAutoSelectSqlByDto2()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesByEmployee");
            Assert.IsNotNull(cmd, "1");
            Trace.WriteLine(cmd.Sql);
            var dto = new Employee();
            dto.JobName = "MANAGER";
            var employees = (IList) cmd.Execute(new object[] { dto });
            Trace.WriteLine(employees);
            //Assert.IsTrue(employees.Count > 0, "2");
        }

        [Test, S2]
        public void TestAutoSelectSqlByDto3()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployee3Dao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployees");
            Assert.IsNotNull(cmd, "1");
            Trace.WriteLine(cmd.Sql);
            var dto = new Employee3();
            dto.Mgr = 7902;
            var employees = (IList) cmd.Execute(new object[] { dto });
            Trace.WriteLine(employees);
            Assert.IsTrue(employees.Count > 0, "2");
        }

        [Test, S2]
        public void TestAutoSelectSqlByDto4()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployee3Dao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployees2");
            Assert.IsNotNull(cmd, "1");
            Trace.WriteLine(cmd.Sql);
            Assert.IsTrue(cmd.Sql.EndsWith(" ORDER BY empno"));
        }

        [Test, S2]
        public void TestAutoSelectSqlByDto5()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesBySearchCondition2");
            Assert.IsNotNull(cmd, "1");
            Trace.WriteLine(cmd.Sql);
            var dto = new EmployeeSearchCondition();
            var department = new Department();
            department.Dname = "RESEARCH";
            dto.Department = department;
            var employees = (IList) cmd.Execute(new object[] { dto });
            Assert.IsTrue(employees.Count > 0, "2");
        }

        [Test, S2]
        public void TestAutoSelectSqlByDto6()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesBySearchCondition2");
            Assert.IsNotNull(cmd, "1");
            Trace.WriteLine(cmd.Sql);
            var dto = new EmployeeSearchCondition();
            dto.Department = null;
            var employees = (IList) cmd.Execute(new object[] { dto });
            Assert.AreEqual(0, employees.Count, "2");
        }

        [Test, S2]
        public void TestSelfReference()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployee4Dao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Assert.IsNotNull(cmd, "1");
            Trace.WriteLine(cmd.Sql);
            var employee = (Employee4) cmd.Execute(new object[] { 7788 });
            Trace.WriteLine(employee);
            Assert.AreEqual(7566, employee.Parent.Empno, "2");
        }

        [Test, S2]
        public void TestSelfMultiPk()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployee5Dao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Assert.IsNotNull(cmd, "1");
            Trace.WriteLine(cmd.Sql);
        }

        [Test, S2]
        public void TestNotHavePrimaryKey()
        {
            var dmd = CreateDaoMetaData(typeof(IDepartmentTotalSalaryDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetTotalSalaries");
            Assert.IsNotNull(cmd, "1");
            Trace.WriteLine(cmd.Sql);
            var result = (IList) cmd.Execute(null);
            Trace.WriteLine(result);
        }

        [Test, S2]
        public void TestSelectAutoFullColumnName()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Trace.WriteLine(cmd.Sql);
        }

        [Test, S2]
        public void TestStartsWithOrderBy()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployee6Dao));
            var condition = new EmployeeSearchCondition();
            condition.Dname = "RESEARCH";
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployees");
            Trace.WriteLine(cmd.Sql);
            condition.OrderByString = "ENAME";
            var results = (Employee[]) cmd.Execute(new object[] { condition });
            Assert.AreEqual(7876, results[0].Empno, "1");
        }

        [Test, S2]
        public void TestStartsWithBeginComment()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployee8Dao));
            var cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployees");
            Trace.WriteLine(cmd.Sql);
            {
                var emp = new Employee();
                var results = (IList) cmd.Execute(new object[] { emp });
                Assert.AreEqual(14, results.Count);
            }
            {
                var emp = new Employee();
                emp.JobName = "SALESMAN";
                var results = (IList) cmd.Execute(new object[] { emp });
                Assert.AreEqual(4, results.Count);
            }
            {
                var emp = new Employee();
                emp.Ename = "SMITH";
                emp.JobName = "CLERK";
                var results = (IList) cmd.Execute(new object[] { emp });
                Assert.AreEqual(1, results.Count);
            }
            {
                var emp = new Employee();
                emp.Ename = "a";
                emp.JobName = "b";
                var results = (IList) cmd.Execute(new object[] { emp });
                Assert.AreEqual(0, results.Count);
            }
        }

        [Test, S2(Tx.Rollback)]
        public void TestQueryAnnotation()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployee7Dao));
            var cmd1 = (SelectDynamicCommand) dmd.GetSqlCommand("GetCount");
            var cmd2 = (UpdateDynamicCommand) dmd.GetSqlCommand("DeleteEmployee");
            Trace.WriteLine(cmd1.Sql);
            Trace.WriteLine(cmd2.Sql);
            Assert.AreEqual(14, cmd1.Execute(null));
            Assert.AreEqual(1, cmd2.Execute(new object[] { 7369 }));
            Assert.AreEqual(13, cmd1.Execute(null));
        }

        // TODO AbstractDaoとEntityManagerが未実装のため、テストケース無し。
        //public void TestDaoExtend1()

        // TODO AbstractDaoとEntityManagerが未実装のため、テストケース無し。
        //public void TestDaoExtend2()

        // TODO InsertAutoDynamibCommandが未実装のため、テストケース無し。
        //public void TestUsingColumnAnnotationForSql_Insert() 

        // TODO InsertAutoDynamibCommandが未実装のため、テストケース無し。
        //public void TestUsingColumnAnnotationForSql_Update()

        // TODO InsertAutoDynamibCommandが未実装のため、テストケース無し。
        //public void TestUsingColumnAnnotationForSql_Select()

        // TODO InsertAutoDynamibCommandが未実装のため、テストケース無し。
        //public void TestUsingColumnAnnotationForSql_SelectDto()

        [Test, S2(Tx.Rollback)]
        public void TestSqlFileEncodingDefault()
        {
            var dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            var cmd = dmd.GetSqlCommand("UpdateSqlFileEncodingDefault");
            var emp = new Employee();
            emp.Empno = 7788;
            cmd.Execute(new object[] { emp });

            var cmd2 = dmd.GetSqlCommand("GetEmployee");
            var ret = (Employee) cmd2.Execute(new object[] { 7788 });
            Assert.AreEqual("日本語", ret.Ename);
        }

        [Test, S2(Tx.Rollback)]
        public void TestSqlFileEncodingUTF8()
        {
            var dmd = (DaoMetaDataImpl) CreateDaoMetaData(typeof(IEmployeeDao));
            dmd.SqlFileEncoding = "utf-8";
            var cmd = dmd.GetSqlCommand("UpdateSqlFileEncodingUTF8");
            var emp = new Employee();
            emp.Empno = 7788;
            cmd.Execute(new object[] { emp });

            var cmd2 = dmd.GetSqlCommand("GetEmployee");
            var ret = (Employee) cmd2.Execute(new object[] { 7788 });
            Assert.AreEqual("日本語", ret.Ename);
        }

        private IStoredProcedureTestDao _procedureDao = null;

        [Test, S2]
        public void TestFunction()
        {
            if (Dbms.Dbms != KindOfDbms.MSSQLServer && Dbms.Dbms != KindOfDbms.Oracle)
            {
                //Assert.Ignore("ProcedureのテストはSQL ServerとOracleしか用意していない。");
            }
            var sales = 0.4;
            var ret = _procedureDao.GetSalesTax2(sales);
            Assert.AreEqual(0.08, Math.Round(ret, 2), "1");
        }

        [Test, S2]
        public void TestProcedureOutParameter()
        {
            if (Dbms.Dbms != KindOfDbms.MSSQLServer && Dbms.Dbms != KindOfDbms.Oracle)
            {
                //Assert.Ignore("ProcedureのテストはSQL ServerとOracleしか用意していない。");
            }
            var sales = 0.4;
            double tax;
            _procedureDao.GetSalesTax(sales, out tax);
            Assert.AreEqual(0.08, Math.Round(tax, 2), "1");
        }

        [Test, S2]
        public void TestProcedureInOutParameter()
        {
            if (Dbms.Dbms != KindOfDbms.MSSQLServer && Dbms.Dbms != KindOfDbms.Oracle)
            {
                //Assert.Ignore("ProcedureのテストはSQL ServerとOracleしか用意していない。");
            }
            var sales = 0.4;
            _procedureDao.GetSalesTax3(ref sales);
            Assert.AreEqual(0.08, Math.Round(sales, 2), "1");
        }

        [Test, S2]
        public void TestProcedureMultiParameter()
        {
            if (Dbms.Dbms != KindOfDbms.MSSQLServer && Dbms.Dbms != KindOfDbms.Oracle)
            {
                //Assert.Ignore("ProcedureのテストはSQL ServerとOracleしか用意していない。");
            }
            var sales = 0.4;
            double tax;
            double total;
            _procedureDao.GetSalesTax4(sales, out tax, out total);
            Assert.AreEqual(0.08, Math.Round(tax, 2), "1");
            Assert.AreEqual(0.48, Math.Round(total, 2), "1");
        }
    }
}
