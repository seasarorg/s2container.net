using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Bean(typeof(EmployeeDefaultTimestamp))]
    public interface IEmployeeDefaultTimestampDao
    {
        void Insert(EmployeeDefaultTimestamp employee);

        [Query("empno=/*empno*/")]
        EmployeeDefaultTimestamp GetEmployee(int empno);
    }

    [Bean(typeof(EmployeeDefaultVersionNo))]
    public interface IEmployeeDefaultVersionNoDao
    {
        void Insert(EmployeeDefaultVersionNo employee);

        [Query("empno=/*empno*/")]
        EmployeeDefaultVersionNo GetEmployee(int empno);
    }
}
