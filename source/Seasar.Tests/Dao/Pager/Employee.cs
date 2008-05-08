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
using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Pager
{
    [Table("EMP")]
    public class Employee
    {
        private long _empno;

        private string _ename;

        private string _job;

        private short _mgr;

        private DateTime _hiredate;

        private float _sal;

        private float _comm;

        private short _deptno;

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

        public short Mgr
        {
            set { _mgr = value; }
            get { return _mgr; }
        }

        public DateTime Hiredate
        {
            set { _hiredate = value; }
            get { return _hiredate; }
        }

        public float Sal
        {
            set { _sal = value; }
            get { return _sal; }
        }

        public float Comm
        {
            set { _comm = value; }
            get { return _comm; }
        }

        public short Deptno
        {
            set { _deptno = value; }
            get { return _deptno; }
        }

        public bool equals(object other)
        {
            if (!(other.GetType() == typeof(Employee))) return false;
            Employee castOther = (Employee) other;
            return Empno == castOther.Empno;
        }

        public int hashCode()
        {
            return (int) Empno;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder(50);
            buf.Append(_empno).Append(", ");
            buf.Append(_ename).Append(", ");
            buf.Append(_job).Append(", ");
            buf.Append(_mgr).Append(", ");
            buf.Append(_hiredate).Append(", ");
            buf.Append(_sal).Append(", ");
            buf.Append(_comm).Append(", ");
            buf.Append(_deptno).Append(", ");
            return buf.ToString();
        }
    }
}
