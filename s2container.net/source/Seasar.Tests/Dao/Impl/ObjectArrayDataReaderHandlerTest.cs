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

            var sql = "select emp.ename from emp";
            string[] ret;
            using (var con = Connection)
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        ret = (string[])handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.Length > 0, "2");
            Assert.IsTrue(ret.GetExType().IsArray, "3");
            foreach (var val in ret)
            {
                Console.WriteLine($"ename = {val}");
            }
        }

        [Test, S2]
        public void TestHandleDateTimeArray()
        {
            IDataReaderHandler handler = new ObjectArrayDataReaderHandler(typeof(DateTime));

            var sql = "select emp.tstamp from emp";
            DateTime[] ret;
            using (var con = Connection)
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        ret = (DateTime[])handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.Length > 0, "2");
            Assert.IsTrue(ret.GetExType().IsArray, "3");
            foreach (var val in ret)
            {
                Console.WriteLine($"ename = {val}");
            }
        }

        [Test, S2]
        public void TestHandleDecimalArray()
        {
            IDataReaderHandler handler = new ObjectArrayDataReaderHandler(typeof(decimal));

            var sql = "select emp.empno from emp";
            decimal[] ret;
            using (var con = Connection)
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        ret = (decimal[])handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.Length > 0, "2");
            Assert.IsTrue(ret.GetExType().IsArray, "3");
            foreach (var val in ret)
            {
                Console.WriteLine($"empno = {val}");
            }
        }

        [Test, S2]
        public void TestHandleIntegerArray()
        {
            IDataReaderHandler handler = new ObjectArrayDataReaderHandler(typeof(int));

            var sql = "select emp.empno from emp";
            int[] ret;
            using (var con = Connection)
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        ret = (int[])handler.Handle(reader);
                    }
                }
            }
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret.Length > 0, "2");
            Assert.IsTrue(ret.GetExType().IsArray, "3");
            foreach (var val in ret)
            {
                Console.WriteLine($"empno = {val}");
            }
        }
    }
}
