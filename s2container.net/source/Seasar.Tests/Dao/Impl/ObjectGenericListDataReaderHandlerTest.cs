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
using System.Collections.Generic;
using MbUnit.Framework;
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Dao.Impl
{
    /// <summary>
    /// https://www.seasar.org/issues/browse/DAONET-76
    /// </summary>
    [TestFixture]
    public class ObjectGenericListDataReaderHandlerTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestHandleStringGenericList()
        {
            IDataReaderHandler handler = new ObjectGenericListDataReaderHandler(typeof(string));

            var sql = "select emp.ename from emp";
            object ret;
            using (var con = Connection)
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        ret = handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.GetExType().IsGenericType, "2");
            var resultList = (IList<string>)ret;
            Assert.IsTrue(resultList.Count > 0, "3");
            foreach (object val in resultList)
            {
                Assert.IsTrue(val is string, "4");
                Console.WriteLine($"ename = {val}");
            }
        }

        [Test, S2]
        public void TestHandleDateTimeGenericList()
        {
            IDataReaderHandler handler = new ObjectGenericListDataReaderHandler(typeof(DateTime));

            var sql = "select emp.tstamp from emp";
            object ret;
            using (var con = Connection)
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        ret = handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.GetExType().IsGenericType, "2");
            var resultList = (IList<DateTime>)ret;
            Assert.IsTrue(resultList.Count > 0, "3");
            foreach (object val in resultList)
            {
                Assert.IsTrue(val is DateTime, "4");
                Console.WriteLine($"ename = {val}");
            }
        }

        [Test, S2]
        public void TestHandleDecimalGenericList()
        {
            IDataReaderHandler handler = new ObjectGenericListDataReaderHandler(typeof(decimal));

            var sql = "select emp.empno from emp";
            object ret;
            using (var con = Connection)
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        ret = handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.GetExType().IsGenericType, "2");
            var resultList = (IList<decimal>)ret;
            Assert.IsTrue(resultList.Count > 0, "3");
            foreach (object val in resultList)
            {
                Assert.IsTrue(val is decimal, "4");
                Console.WriteLine($"empno = {val}");
            }
        }

        [Test, S2]
        public void TestHandleIntegerGenericList()
        {
            IDataReaderHandler handler = new ObjectGenericListDataReaderHandler(typeof(int));

            var sql = "select emp.empno from emp";
            object ret;
            using (var con = Connection)
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        ret = handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.GetExType().IsGenericType, "2");
            var resultList = (IList<int>)ret;
            Assert.IsTrue(resultList.Count > 0, "3");
            foreach (object val in resultList)
            {
                Assert.IsTrue(val is int, "4");
                Console.WriteLine($"empno = {val}");
            }
        }
    }
}
