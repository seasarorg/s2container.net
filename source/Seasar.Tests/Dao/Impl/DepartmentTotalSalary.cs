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

using System.Data.SqlTypes;

namespace Seasar.Tests.Dao.Impl
{
    public class DepartmentTotalSalary
    {
        private SqlInt32 _deptno;

        private decimal _totalSalary;

        public SqlInt32 Deptno
        {
            set { _deptno = value; }
            get { return _deptno; }
        }

        public decimal TotalSalary
        {
            set { _totalSalary = value; }
            get { return _totalSalary; }
        }
    }
}
