using System;
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
        public Employee GetEmployeeByEmpNo(int empNo)
        {
            Employee emp = employeeDao.GetByEmpNo(empNo);

            return emp;
        }
    }
}
