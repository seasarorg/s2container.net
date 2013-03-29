#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using MbUnit.Framework;
using Seasar.Extension.Unit;
using Seasar.Dao.Pager;

namespace Seasar.Tests.Dao.Pager
{
    [TestFixture]
    public class PagerDataReaderWrapperTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Dao.Pager.PagerDataReaderWrapperTest.dicon";

        private IEmployeeDao _employeeDao = null;

        public void SetUpPageLimit()
        {
            System.Console.WriteLine("SetUpPageLimit");
            Include(PATH);
        }

        [Test, S2]
        public void PageLimit()
        {
            IPagerCondition condition = new DefaultPagerCondition();
            condition.Limit = 2;
            Assert.AreEqual(0, condition.Offset);
            Assert.AreEqual(0, condition.Count);
            IList employees = _employeeDao.GetEmployees(condition);
            Assert.AreEqual(14, condition.Count);
            Assert.AreEqual(2, employees.Count);
            Assert.AreEqual(7369, GetEmployee(employees, 0).Empno);
            Assert.AreEqual(7499, GetEmployee(employees, 1).Empno);
        }

        public void SetUpOffset()
        {
            Include(PATH);
        }

        [Test, S2]
        public void Offset()
        {
            IPagerCondition condition = new DefaultPagerCondition();
            condition.Limit = 2;
            condition.Offset = 1;
            IList employees = _employeeDao.GetEmployees(condition);
            Assert.AreEqual(14, condition.Count);
            Assert.AreEqual(2, employees.Count);
            Assert.AreEqual(7499, GetEmployee(employees, 0).Empno);
            Assert.AreEqual(7521, GetEmployee(employees, 1).Empno);
        }

        public void SetUpLastPage()
        {
            Include(PATH);
        }

        [Test, S2]
        public void LastPage()
        {
            IPagerCondition condition = new DefaultPagerCondition();
            condition.Limit = 5;
            condition.Offset = 10;
            System.Console.WriteLine(_employeeDao == null ? "null!!" : _employeeDao.ToString());
            IList employees = _employeeDao.GetEmployees(condition);
            Assert.AreEqual(14, condition.Count);
            Assert.AreEqual(4, employees.Count);
            Assert.AreEqual(7876, GetEmployee(employees, 0).Empno);
            Assert.AreEqual(7900, GetEmployee(employees, 1).Empno);
            Assert.AreEqual(7902, GetEmployee(employees, 2).Empno);
            Assert.AreEqual(7934, GetEmployee(employees, 3).Empno);
        }

        public void SetUpPagerAttribute()
        {
            Include(PATH);
        }

        [Test, S2]
        public void PagerAttribute()
        {
            int count;
            IList employees = _employeeDao.GetEmployeesPager(2, 1, out count);
            Assert.AreEqual(14, count);
            Assert.AreEqual(2, employees.Count);
            Assert.AreEqual(7499, GetEmployee(employees, 0).Empno);
            Assert.AreEqual(7521, GetEmployee(employees, 1).Empno);
        }

        public void SetUpPagerAttributeRenameParameter()
        {
            Include(PATH);
        }

        [Test, S2]
        public void PagerAttributeRenameParameter()
        {
            int count;
            IList employees = _employeeDao.GetEmployeesPager2(2, 1, out count);
            Assert.AreEqual(14, count);
            Assert.AreEqual(2, employees.Count);
            Assert.AreEqual(7499, GetEmployee(employees, 0).Empno);
            Assert.AreEqual(7521, GetEmployee(employees, 1).Empno);
        }

        public void SetUpPagerAttributeWithParameter()
        {
            Include(PATH);
        }

        [Test, S2]
        public void PagerAttributeWithParameter()
        {
            int count;
            IList employees = _employeeDao.GetEmployeesPager3(20, 2, 1, out count);
            Assert.AreEqual(5, count);
            Assert.AreEqual(2, employees.Count);
            Assert.AreEqual(7566, GetEmployee(employees, 0).Empno);
            Assert.AreEqual(7788, GetEmployee(employees, 1).Empno);
        }

        private Employee GetEmployee(IList employees, int i)
        {
            return (Employee) employees[i];
        }
    }
}
