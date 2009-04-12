using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Bean(typeof(EmployeeModifiedOnly))]
    public interface IEmployeeModifiedOnlyDao
    {
        [Query("empno=/*empno*/")]
        EmployeeModifiedOnly GetEmployee(int empno);

        void Update(EmployeeModifiedOnly employee);

        void UpdateModifiedOnly(EmployeeModifiedOnly employee);
    }

    [Bean(typeof(EmployeeModifiedOnlyWithoutClearMethod))]
    public interface IEmployeeModifiedOnlyWithoutClearMethodDao
    {
        [Query("empno=/*empno*/")]
        EmployeeModifiedOnlyWithoutClearMethod GetEmployee(int empno);

        void Update(EmployeeModifiedOnlyWithoutClearMethod employee);

        void UpdateModifiedOnly(EmployeeModifiedOnlyWithoutClearMethod employee);
    }

    [Bean(typeof(EmployeeClearModifiedMethodOnly))]
    public interface IEmployeeClearModifiedMethodOnlyDao
    {
        [Query("empno=/*empno*/")]
        EmployeeClearModifiedMethodOnly GetEmployee(int empno);

        void Update(EmployeeClearModifiedMethodOnly employee);

        void UpdateModifiedOnly(EmployeeClearModifiedMethodOnly employee);
    }

    [Bean(typeof(EmployeeNoModifiedPropertyNamesAndMethod))]
    public interface IEmployeeNoModifiedPropertyNamesAndMethodDao
    {
        [Query("empno=/*empno*/")]
        EmployeeNoModifiedPropertyNamesAndMethod GetEmployee(int empno);

        void Update(EmployeeNoModifiedPropertyNamesAndMethod employee);

        void UpdateModifiedOnly(EmployeeNoModifiedPropertyNamesAndMethod employee);
    }
}
