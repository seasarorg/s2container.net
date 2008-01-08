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
using Seasar.Dao.Pager;

namespace Seasar.Tests.Dao.Pager
{
    [Bean(typeof(Employee))]
    public interface IEmployeeDao
    {
        [Query("ORDER BY EMPNO")]
        IList GetEmployees(IPagerCondition condition);

        [Query("ORDER BY EMPNO")]
        [Pager]
        IList GetEmployeesPager(int limit, int offset, out int count);

        [Query("ORDER BY EMPNO")]
        [Pager("l", "o", "c")]
        IList GetEmployeesPager2(int l, int o, out int c);

        [Query("DEPTNO = /*deptNo*/ ORDER BY EMPNO")]
        [Pager]
        IList GetEmployeesPager3(int deptNo, int limit, int offset, out int count);
    }
}
