#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
    [Bean(typeof(Employee))]
    public interface IEmployeeAutoDao
    {
        [Query("deptno=/*deptno*/")]
        IList GetEmployeeByDeptno(int deptno);

        [Query("sal BETWEEN /*minSal*/ AND /*maxSal*/ ORDER BY empno")]
        IList GetEmployeesBySal(float minSal, float maxSal);

        [Query("ename IN /*enames*/('SCOTT','MARY') AND job IN /*jobs*/('ANALYST', 'FREE')")]
        IList GetEmployeesByEnameJob(IList enames, IList jobs);

        IList GetEmployeesBySearchCondition(EmployeeSearchCondition dto);

        [Query("department.dname = /*dto.Department.Dname*/'RESEARCH'")]
        IList GetEmployeesBySearchCondition2(EmployeeSearchCondition dto);

        IList GetEmployeesByEmployee(Employee dto);

        [Query("empno=/*empno*/")]
        Employee GetEmployee(int empno);

        void Insert(Employee employee);

        [NoPersistentProps("job, mgr, hiredate, sal, comm, deptno")]
        void Insert2(Employee employee);

        [PersistentProps("deptno")]
        void Insert3(Employee employee);

        void Update(Employee employee);

        [NoPersistentProps("job, mgr, hiredate, sal, comm, deptno")]
        void Update2(Employee employee);

        [PersistentProps("deptno")]
        void Update3(Employee employee);

        void UpdateUnlessNull(Employee employee);

        void Delete(Employee employee);
    }
}
