using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
#if NHIBERNATE_NULLABLES
    [Bean(typeof(EmployeeNullableDecimalVersionNo))]
    public interface IEmployeeNullableDecimalVersionNoDao
    {
        [Query("empno=/*empno*/")]
        EmployeeNullableDecimalVersionNo GetEmployee(int empno);

        void Insert(EmployeeNullableDecimalVersionNo employee);
        void Update(EmployeeNullableDecimalVersionNo employee);
        void Delete(EmployeeNullableDecimalVersionNo employee);
    }

    [Bean(typeof(EmployeeNullableIntVersionNo))]
    public interface IEmployeeNullableIntVersionNoDao
    {
        [Query("empno=/*empno*/")]
        EmployeeNullableIntVersionNo GetEmployee(int empno);

        void Insert(EmployeeNullableIntVersionNo employee);
        void Update(EmployeeNullableIntVersionNo employee);
        void Delete(EmployeeNullableIntVersionNo employee);
    }
#endif

#if !NET_1_1
    [Bean(typeof(EmployeeGenericNullableDecimalVersionNo))]
    public interface IEmployeeGenericNullableDecimalVersionNoDao
    {
        [Query("empno=/*empno*/")]
        EmployeeGenericNullableDecimalVersionNo GetEmployee(int empno);

        void Insert(EmployeeGenericNullableDecimalVersionNo employee);
        void Update(EmployeeGenericNullableDecimalVersionNo employee);
        void Delete(EmployeeGenericNullableDecimalVersionNo employee);
    }

    [Bean(typeof(EmployeeGenericNullableIntVersionNo))]
    public interface IEmployeeGenericNullableIntVersionNoDao
    {
        [Query("empno=/*empno*/")]
        EmployeeGenericNullableIntVersionNo GetEmployee(int empno);

        void Insert(EmployeeGenericNullableIntVersionNo employee);
        void Update(EmployeeGenericNullableIntVersionNo employee);
        void Delete(EmployeeGenericNullableIntVersionNo employee);
    }
#endif
    [Bean(typeof(EmployeeDecimalVersionNo))]
    public interface IEmployeeDecimalVersionNoDao
    {
        [Query("empno=/*empno*/")]
        EmployeeDecimalVersionNo GetEmployee(int empno);

        void Insert(EmployeeDecimalVersionNo employee);
        void Update(EmployeeDecimalVersionNo employee);
        void Delete(EmployeeDecimalVersionNo employee);
    }

    [Bean(typeof(EmployeeIntVersionNo))]
    public interface IEmployeeIntVersionNoDao
    {
        [Query("empno=/*empno*/")]
        EmployeeIntVersionNo GetEmployee(int empno);

        void Insert(EmployeeIntVersionNo employee);
        void Update(EmployeeIntVersionNo employee);
        void Delete(EmployeeIntVersionNo employee);
    }
}
