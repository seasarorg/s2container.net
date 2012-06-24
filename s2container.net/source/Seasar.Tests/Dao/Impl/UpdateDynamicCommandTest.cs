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

using MbUnit.Framework;
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class UpdateDynamicCommandTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteTx()
        {
            UpdateDynamicCommand cmd = new UpdateDynamicCommand(DataSource,
                BasicCommandFactory.INSTANCE);
            cmd.Sql = "UPDATE emp SET ename = /*employee.Ename*/'HOGE' WHERE empno = /*employee.Empno*/1234";
            cmd.ArgNames = new string[] { "employee" };

            Employee emp = new Employee();
            emp.Empno = 7788;
            emp.Ename = "SCOTT";
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }
    }
}
