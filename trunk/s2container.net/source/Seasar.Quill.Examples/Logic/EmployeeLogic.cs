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

using Seasar.Quill.Attrs;
using Seasar.Quill.Examples.Entity;
using Seasar.Quill.Examples.Dao;

namespace Seasar.Quill.Examples.Logic
{
    [Implementation]
    public class EmployeeLogic
    {
        protected IEmployeeDao employeeDao;

        [Aspect(typeof(ConsoleWriteInterceptor), 1)]
        [Aspect("LocalRequiredTx", 2)]
        public virtual Employee GetEmployeeByEmpNo(int empNo)
        {
            Employee emp = employeeDao.GetByEmpNo(empNo);

            return emp;
        }
    }
}
