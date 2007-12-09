#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using System.Text;

namespace Seasar.Examples.Reference.S2Unit
{
    [Serializable]
    public class Employee
    {
        private long _empno;
        private string _ename;
        private short _deptno;
        private string _dname;

        public Employee() { }

        public Employee(long empno)
        {
            _empno = empno;
        }

        public Employee(
            long empno,
            string ename,
            short deptno,
            string dname
        )
        {
            _empno = empno;
            _ename = ename;
            _deptno = deptno;
            _dname = dname;
        }

        public long Empno
        {
            get { return _empno; }
            set { _empno = value; }
        }

        public string Ename
        {
            get { return _ename; }
            set { _ename = value; }
        }

        public short Deptno
        {
            get { return _deptno; }
            set { _deptno = value; }
        }

        public string Dname
        {
            get { return _dname; }
            set { _dname = value; }
        }

        public override bool Equals(object other)
        {
            if (!(other is Employee)) return false;
            Employee castOther = (Employee) other;
            return Empno == castOther.Empno;
        }

        public override int GetHashCode()
        {
            return (int) Empno;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(_empno).Append(", ");
            buf.Append(_ename).Append(", ");
            buf.Append(_deptno).Append(", ");
            buf.Append(_dname);
            return buf.ToString();
        }
    }
}
