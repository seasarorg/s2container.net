#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Impl
{
    /// <summary>
    /// https://www.seasar.org/issues/browse/DAONET-76
    /// </summary>
    [TestFixture]
    public class ObjectArrayDataReaderHandlerTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestHandleStringArray()
        {
            IDataReaderHandler handler = new ObjectArrayDataReaderHandler(typeof(string));

            string sql = "select emp.ename from emp";
            string[] ret = null;
            using (IDbConnection con = Connection)
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        ret = (string[])handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.Length > 0, "2");
            Assert.IsTrue(ret.GetType().IsArray, "3");
            foreach (string val in ret)
            {
                Console.WriteLine("ename = {0}", val);
            }
        }

        [Test, S2]
        public void TestHandleDateTimeArray()
        {
            IDataReaderHandler handler = new ObjectArrayDataReaderHandler(typeof(DateTime));

            string sql = "select emp.tstamp from emp";
            DateTime[] ret = null;
            using (IDbConnection con = Connection)
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        ret = (DateTime[])handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.Length > 0, "2");
            Assert.IsTrue(ret.GetType().IsArray, "3");
            foreach (DateTime val in ret)
            {
                Console.WriteLine("ename = {0}", val);
            }
        }

        [Test, S2]
        public void TestHandleDecimalArray()
        {
            IDataReaderHandler handler = new ObjectArrayDataReaderHandler(typeof(decimal));

            string sql = "select emp.empno from emp";
            decimal[] ret = null;
            using (IDbConnection con = Connection)
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        ret = (decimal[])handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.Length > 0, "2");
            Assert.IsTrue(ret.GetType().IsArray, "3");
            foreach (decimal val in ret)
            {
                Console.WriteLine("empno = {0}", val);
            }
        }

        [Test, S2]
        public void TestHandleIntegerArray()
        {
            IDataReaderHandler handler = new ObjectArrayDataReaderHandler(typeof(int));

            string sql = "select emp.empno from emp";
            int[] ret = null;
            using (IDbConnection con = Connection)
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        ret = (int[])handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.Length > 0, "2");
            Assert.IsTrue(ret.GetType().IsArray, "3");
            foreach (int val in ret)
            {
                Console.WriteLine("empno = {0}", val);
            }
        }
    }
}
