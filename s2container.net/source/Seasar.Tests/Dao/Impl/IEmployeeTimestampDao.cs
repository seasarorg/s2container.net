using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Bean(typeof(EmployeeGenericNullableTimestamp))]
    public interface IEmployeeGenericNullableTimestampDao
    {
        [Query("empno=/*empno*/")]
        EmployeeGenericNullableTimestamp GetEmployee(int empno);

        void Insert(EmployeeGenericNullableTimestamp employee);
        void Update(EmployeeGenericNullableTimestamp employee);
        void Delete(EmployeeGenericNullableTimestamp employee);
    }

    [Bean(typeof(EmployeeTimestamp))]
    public interface IEmployeeTimestampDao
    {
        [Query("empno=/*empno*/")]
        EmployeeTimestamp GetEmployee(int empno);

        void Insert(EmployeeTimestamp employee);
        void Update(EmployeeTimestamp employee);
        void Delete(EmployeeTimestamp employee);
    }

    [Bean(typeof(EmployeeSqlTimestamp))]
    public interface IEmployeeSqlTimestampDao
    {
        [Query("empno=/*empno*/")]
        EmployeeSqlTimestamp GetEmployee(int empno);

        void Insert(EmployeeSqlTimestamp employee);
        void Update(EmployeeSqlTimestamp employee);
        void Delete(EmployeeSqlTimestamp employee);
    }
}
