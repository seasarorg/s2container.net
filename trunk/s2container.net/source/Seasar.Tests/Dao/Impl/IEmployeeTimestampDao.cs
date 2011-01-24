#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
