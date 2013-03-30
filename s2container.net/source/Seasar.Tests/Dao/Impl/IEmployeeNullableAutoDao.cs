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
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Bean(typeof(EmployeeNullable))]
    public interface IEmployeeNullableAutoDao
    {
        [Query("deptno=/*deptno*/")]
        IList GetEmployeeNullableByDeptno(int deptno);

        [Query("sal BETWEEN /*minSal*/ AND /*maxSal*/ ORDER BY empno")]
        IList GetEmployeesNullableBySal(float minSal, float maxSal);

        [Query("ename IN /*enames*/('SCOTT','MARY') AND job IN /*jobs*/('ANALYST', 'FREE')")]
        IList GetEmployeesNullableByEnameJob(IList enames, IList jobs);

        IList GetEmployeesNullableBySearchCondition(EmployeeSearchCondition dto);

        [Query("department.dname = /*dto.Department.Dname*/'RESEARCH'")]
        IList GetEmployeesNullableBySearchCondition2(EmployeeSearchCondition dto);

        IList GetEmployeesNullableByEmployee(EmployeeNullable dto);

        [Query("empno=/*empno*/")]
        EmployeeNullable GetEmployeeNullable(int empno);

        void Insert(EmployeeNullable employee);

        [NoPersistentProps("job, mgr, hiredate, sal, comm, deptno")]
        void Insert2(EmployeeNullable employee);

        [PersistentProps("deptno")]
        void Insert3(EmployeeNullable employee);

        void Update(EmployeeNullable employee);

        [NoPersistentProps("job, mgr, hiredate, sal, comm, deptno")]
        void Update2(EmployeeNullable employee);

        [PersistentProps("deptno")]
        void Update3(EmployeeNullable employee);

        [PersistentProps("nullableNextRestDate")]
        void UpdateNullable(EmployeeNullable employee);

        void Delete(EmployeeNullable employee);
    }
}
