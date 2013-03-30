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
