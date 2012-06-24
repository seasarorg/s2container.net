#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO;
using Seasar.Extension.Unit;
using MbUnit.Framework;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class BeanGenericListMetaDataDataReaderHandlerTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestHandle()
        {
            IDataReaderHandler handler = new BeanGenericListMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), new RowCreatorImpl(), new RelationRowCreatorImpl());

            string sql = "select * from emp";
            using (IDbConnection con = Connection)
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    IList<Employee> ret;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        ret = (IList<Employee>) handler.Handle(reader);
                    }

                    Assert.IsNotNull(ret, "1");
                    foreach (Employee emp in ret)
                    {
                        Trace.WriteLine(emp.Empno + "," + emp.Ename);
                    }
                }
            }
        }

        [Test, S2]
        public void TestHandle2()
        {
            IDataReaderHandler handler = new BeanGenericListMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), new RowCreatorImpl(), new RelationRowCreatorImpl());

            string sql = "select emp.*, dept.dname as dname_0 from emp, dept where emp.deptno = dept.deptno and emp.deptno = 20";
            using (IDbConnection con = Connection)
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    IList<Employee> ret;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        ret = (IList<Employee>) handler.Handle(reader);
                    }

                    Assert.IsNotNull(ret, "1");
                    foreach (Employee emp in ret)
                    {
                        Trace.WriteLine(emp);
                        Department dept = emp.Department;
                        Assert.IsNotNull(dept, "2");
                        Assert.AreEqual(emp.Deptno, dept.Deptno, "3");
                        Assert.IsNotNull(dept.Dname, "4");
                    }
                }
            }
        }

        [Test, S2]
        public void TestHandle3()
        {
            IDataReaderHandler handler = new BeanGenericListMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), new RowCreatorImpl(), new RelationRowCreatorImpl());

            string sql = "select emp.*, dept.deptno as deptno_0, dept.dname as dname_0 from emp, dept where dept.deptno = 20 and emp.deptno = dept.deptno";
            using (IDbConnection con = Connection)
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    IList<Employee> ret;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        ret = (IList<Employee>) handler.Handle(reader);
                    }

                    IEnumerator<Employee> employees = ret.GetEnumerator();
                    employees.MoveNext();
                    Employee emp = employees.Current;
                    employees.MoveNext();
                    Employee emp2 = employees.Current;
                    Assert.AreSame(emp.Department, emp2.Department, "1");
                }
            }
        }
    }
}
