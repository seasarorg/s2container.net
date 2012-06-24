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
    public class BeanMetaDataDataReaderHandlerTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestHandle()
        {
            IDataReaderHandler handler = new BeanMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), new RowCreatorImpl(), new RelationRowCreatorImpl());
            string sql = "select emp.*, dept.deptno as deptno_0, dept.dname as dname_0 " +
                "from emp, dept where empno = 7788 and emp.deptno = dept.deptno";
            Employee ret;
            using (IDbConnection con = Connection)
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        ret = (Employee) handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Trace.WriteLine(ret.Empno + "," + ret.Ename);
            Department dept = ret.Department;
            Assert.IsNotNull(dept, "2");
            Assert.AreEqual(20, dept.Deptno, "3");
            Assert.AreEqual("RESEARCH", dept.Dname, "4");
        }

        [Test, S2]
        public void TestHandle2()
        {
            IDataReaderHandler handler = new BeanMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), new RowCreatorImpl(), new RelationRowCreatorImpl());
            string sql = "select ename, job from emp where empno = 7788";
            Employee ret;
            using (IDbConnection con = Connection)
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        ret = (Employee) handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Trace.WriteLine(ret.Empno + "," + ret.Ename);
            Department dept = ret.Department;
            Assert.IsNull(dept, "2");
        }

        [Test, S2]
        public void TestHandle3()
        {
            IDataReaderHandler handler = new BeanMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), new RowCreatorImpl(), new RelationRowCreatorImpl());
            string sql = "select ename, dept.dname as dname_0 " +
                "from emp, dept where empno = 7788 and emp.deptno = dept.deptno";
            Employee ret;
            using (IDbConnection con = Connection)
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        ret = (Employee) handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Trace.WriteLine(ret.Empno + "," + ret.Ename);
            Department dept = ret.Department;
            Assert.IsNotNull(dept, "2");
            Assert.AreEqual("RESEARCH", dept.Dname, "3");
        }

        /// <summary>
        /// https://www.seasar.org/issues/browse/DAONET-24
        /// </summary>
        [Test, S2]
        public void TestMappingByPropertyName()
        {
            IDataReaderHandler handler = new BeanMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), new RowCreatorImpl(), new RelationRowCreatorImpl());
            string sql = "select job as jobname from emp where empno = 7788";
            Employee ret;
            using (IDbConnection con = Connection)
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        ret = (Employee) handler.Handle(reader);
                    }
                }
            }
            Assert.AreEqual("ANALYST", ret.JobName);
        }
    }
}
