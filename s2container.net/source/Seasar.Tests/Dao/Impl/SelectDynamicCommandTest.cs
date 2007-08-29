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
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class SelectDynamicCommandTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestExecuteTx()
        {
            SelectDynamicCommand cmd = new SelectDynamicCommand(DataSource,
                BasicCommandFactory.INSTANCE,
                new BeanMetaDataDataReaderHandler(CreateBeanMetaData(typeof(Employee)), new RowCreatorImpl(), new RelationRowCreatorImpl()),
                BasicDataReaderFactory.INSTANCE);
            cmd.Sql = "SELECT * FROM emp WHERE empno = /*empno*/1234";
            cmd.ArgNames = new string[] { "empno" };
            cmd.ArgTypes = new Type[] { typeof(int) };
            Employee emp = (Employee) cmd.Execute(new object[] { 7788 });
            Trace.WriteLine(emp);
            Assert.IsNotNull(emp, "1");
        }
    }
}
