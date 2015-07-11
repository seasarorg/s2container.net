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

using System.Collections;
using System.Diagnostics;
using Seasar.Extension.Unit;
using MbUnit.Framework;

namespace Seasar.Tests.Dao.Interceptors
{
    [TestFixture]
    public class S2DaoInterceptor2Test : S2TestCase
    {
        private IEmployeeAutoDao _dao = null;

        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Tests.dicon");
        }

        [Test, S2(Tx.Rollback)]
        public void TestInsert()
        {
            Employee emp = new Employee();
            emp.Empno = 99;
            emp.Ename = "hoge";
            Assert.AreEqual(1, _dao.Insert(emp));
        }

        [Test, S2()]
        public void TestSelect()
        {
            Employee emp = _dao.GetEmployee(7788);
            Trace.WriteLine(emp);
            Assert.AreEqual(7788, emp.Empno);
        }

        [Test, S2]
        public void TestSelectQuery()
        {
            IList employees = _dao.GetEmployeesBySal(0, 1000);
            Trace.WriteLine(employees);
            Assert.AreEqual(2, employees.Count);
        }

        [Test, S2(Tx.Rollback)]
        public void TestFullWidthTilda()
        {
            Employee emp = new Employee();
            emp.Empno = 99;
            emp.Ename = "Å`";
            _dao.Insert(emp);
            Employee emp2 = _dao.GetEmployee(99);
            Assert.AreEqual(emp.Ename, emp2.Ename);
        }
    }
}
