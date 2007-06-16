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

namespace Seasar.Dao.Examples.PersistentPropsAttr
{
    public interface IPersistentPropsAttrLogic
    {
        void TestPersistentPropsAttr();
    }

    public class PersistentPropsAttrLogicImpl : IPersistentPropsAttrLogic
    {
        private readonly IEmployeeDao _employeeDao;

        public PersistentPropsAttrLogicImpl(IEmployeeDao employeeDao)
        {
            _employeeDao = employeeDao;
        }

        #region IPersistentPropsAttrLogic メンバ

        public void TestPersistentPropsAttr()
        {
            // 従業員番号7499の従業員を確認
            int empno = 7499;
            Employee emp1 = _employeeDao.GetEmployeeByEmpno(empno);
            Console.WriteLine("従業員番号[" + empno + "]の従業員：" + emp1.ToString());

            // 従業員番号7499の部署番号を更新
            emp1.Ename = "Sugimoto";
            emp1.Deptnum = 99;
            int ret = _employeeDao.UpdateDeptnum(emp1);
            Console.WriteLine("UpdateEmployeeメソッドの戻り値:" + ret);

            // 従業員番号7499の従業員を確認
            Employee emp2 = _employeeDao.GetEmployeeByEmpno(empno);
            Console.WriteLine("従業員番号[" + empno + "]の従業員：" + emp2.ToString());

            throw new ForCleanupException();
        }

        #endregion
    }
}
