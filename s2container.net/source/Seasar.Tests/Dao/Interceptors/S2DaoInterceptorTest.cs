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
    public class S2DaoInterceptorTest : S2TestCase
    {
        private IEmployeeDao _employeeDao = null;

        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Tests.dicon");
        }

        [Test, S2()]
        public void TestSelectBeanList()
        {
            IList employees = _employeeDao.GetAllEmployees();
            for (int i = 0; i < employees.Count; ++i)
            {
                Trace.WriteLine(employees[i].ToString());
            }
            Assert.AreEqual(true, employees.Count > 0, "1");
        }

        [Test, S2()]
        public void TestSelectBean()
        {
            Employee employee = _employeeDao.GetEmployee(7788);
            Trace.WriteLine(employee.ToString());
            Assert.AreEqual("SCOTT", employee.Ename, "1");
        }

        [Test, S2()]
        public void TestSelectObject()
        {
            int count = _employeeDao.GetCount();
            Trace.WriteLine("count:" + count);
            Assert.AreEqual(true, count > 0, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestUpdate()
        {
            Employee employee = _employeeDao.GetEmployee(7788);
            Assert.AreEqual(1, _employeeDao.Update(employee), "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestInsert()
        {
            Assert.AreEqual(1, _employeeDao.Insert(9999, "hoge"));
        }

        [Test, S2()]
        public void TestSelectNullables()
        {
            Employee emp = new Employee();
            emp.Ename = "SCOTT";
            Assert.AreEqual(7788, _employeeDao.GetEmpnoByEmp(emp).Value);
            Assert.IsTrue(_employeeDao.GetEmpnoByEmp(null).HasValue);
            emp.Ename = "Kazuya";
            Assert.IsFalse(_employeeDao.GetEmpnoByEmp(emp).HasValue);
        }

        [Test, S2()]
        public void TestSelectSqlTypes()
        {
            Hoge hoge = new Hoge();
            Hoge hoge2 = new Hoge();
            hoge2.Val = "SCOTT";
            hoge.Parent = hoge2;
            Assert.AreEqual(7788, _employeeDao.GetEmpnoByHoge(hoge).Value);
            Assert.IsFalse(_employeeDao.GetEmpnoByHoge(null).IsNull);
            hoge2.Val = "Kazuya";
            Assert.IsTrue(_employeeDao.GetEmpnoByHoge(hoge).IsNull);
            hoge2.Val = null;
            Assert.IsTrue(_employeeDao.GetEmpnoByHoge2(hoge).IsNull);
        }
    }
}
