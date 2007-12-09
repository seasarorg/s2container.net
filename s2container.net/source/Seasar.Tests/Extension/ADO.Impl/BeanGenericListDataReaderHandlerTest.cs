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

using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Extension.ADO.Impl
{
    [TestFixture]
    public class BeanGenericListDataReaderHandlerTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Ado.dicon";

        public void SetUpHandle()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void Handle()
        {
            IDataReaderHandler handler = new BeanGenericListDataReaderHandler(typeof(Employee));
            string sql = "select * from emp";
            IDbConnection con = Connection;
            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = sql;
            IList<Employee> ret;
            DataSource.SetTransaction(cmd);
            IDataReader reader = cmd.ExecuteReader();
            try
            {
                ret = (IList<Employee>) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }
            Assert.IsNotNull(ret, "1");
            foreach (Employee emp in ret)
            {
                Trace.WriteLine(emp.Empno + "," + emp.Ename);
            }
        }
    }
}
