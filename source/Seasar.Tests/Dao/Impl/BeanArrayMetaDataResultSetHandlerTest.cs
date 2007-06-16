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
    public class BeanArrayMetaDataDataReaderHandlerTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestHandle()
        {
            IDataReaderHandler handler = new BeanArrayMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)));

            string sql = "select * from emp";
            using (IDbConnection con = Connection)
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;

                    Employee[] ret;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        ret = (Employee[]) handler.Handle(reader);
                    }

                    Assert.IsNotNull(ret, "1");
                    for (int i = 0; i < ret.Length; ++i)
                    {
                        Employee emp = ret[i];
                        Trace.WriteLine(emp.Empno + "," + emp.Ename);
                    }
                }
            }
        }
    }
}
