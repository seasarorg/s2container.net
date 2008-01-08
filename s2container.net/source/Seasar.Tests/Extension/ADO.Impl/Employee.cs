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

using System;
using Nullables;

namespace Seasar.Tests.Extension.ADO.Impl
{
    [Serializable]
    public class Employee
    {
        private long _empno;

        private string _ename;

        private string _job;

        private NullableInt16 _mgr;

        private NullableDateTime _hiredate;

        private NullableSingle _sal;

        private NullableSingle _comm;

        private NullableInt32 _deptno;

        private NullableDateTime _tstamp;

        private Department _department;

        public Employee()
        {
        }

        public Employee(
            long empno,
            string ename,
            string job,
            NullableInt16 mgr,
            NullableDateTime hiredate,
            NullableSingle sal,
            NullableSingle comm,
            NullableInt32 deptno,
            NullableDateTime tstamp
            )
        {
            _empno = empno;
            _ename = ename;
            _job = job;
            _mgr = mgr;
            _hiredate = hiredate;
            _sal = sal;
            _comm = comm;
            _deptno = deptno;
            _tstamp = tstamp;
        }

        public long Empno
        {
            set { _empno = value; }
            get { return _empno; }
        }

        public string Ename
        {
            set { _ename = value; }
            get { return _ename; }
        }

        public string Job
        {
            set { _job = value; }
            get { return _job; }
        }

        public NullableInt16 Mgr
        {
            set { _mgr = value; }
            get { return _mgr; }
        }

        public NullableDateTime Hiredate
        {
            set { _hiredate = value; }
            get { return _hiredate; }
        }

        public NullableSingle Sal
        {
            set { _sal = value; }
            get { return _sal; }
        }

        public NullableSingle Comm
        {
            set { _comm = value; }
            get { return _comm; }
        }

        public NullableInt32 Deptno
        {
            set { _deptno = value; }
            get { return _deptno; }
        }

        public NullableDateTime Tstamp
        {
            set { _tstamp = value; }
            get { return _tstamp; }
        }

        public Department Department
        {
            set { _department = value; }
            get { return _department; }
        }
    }
}
