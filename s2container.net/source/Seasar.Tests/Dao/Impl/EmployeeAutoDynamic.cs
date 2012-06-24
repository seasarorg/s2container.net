#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

namespace Seasar.Tests.Dao.Impl
{
    [Table("EMP")]
    public class EmployeeAutoDynamic
    {
        private long _empno;
        private string _ename;
        private string _job;
        private int? _mgr;
        private DateTime? hiredate;
        private Nullable<int> _sal;
        private byte[] _password;
        private string _dummy;

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

        [Column("Job")]
        public string JobName
        {
            set { _job = value; }
            get { return _job; }
        }

        public int? Mgr
        {
            set { _mgr = value; }
            get { return _mgr; }
        }

        public DateTime? Hiredate
        {
            set { hiredate = value; }
            get { return hiredate; }
        }

        public Nullable<int> Sal
        {
            set { _sal = value; }
            get { return _sal; }
        }

        public byte[] Password
        {
            set { _password = value; }
            get { return _password; }
        }

        public string Dummy
        {
            set { _dummy = value; }
            get { return _dummy; }
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
            buf.Append(hiredate).Append(", ");
            buf.Append(_sal).Append(", ");
            return buf.ToString();
        }
    }

    [Table("EMP")]
    public class EmployeeAutoDynamicTimestamp
    {
        private long _empno;
        private string _ename;
        private string _job;
        private int? _mgr;
        private DateTime? hiredate;
        private Nullable<int> _sal;
        private byte[] _password;
        private string _dummy;
        private Department _department;
        private DateTime timestamp;

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

        [Column("Job")]
        public string JobName
        {
            set { _job = value; }
            get { return _job; }
        }

        public int? Mgr
        {
            set { _mgr = value; }
            get { return _mgr; }
        }

        public DateTime? Hiredate
        {
            set { hiredate = value; }
            get { return hiredate; }
        }

        public Nullable<int> Sal
        {
            set { _sal = value; }
            get { return _sal; }
        }

        public byte[] Password
        {
            set { _password = value; }
            get { return _password; }
        }

        public string Dummy
        {
            set { _dummy = value; }
            get { return _dummy; }
        }

        [Relno(0)]
        public Department Department
        {
            set { _department = value; }
            get { return _department; }
        }

        [Column("tstamp")]
        public DateTime Timestamp
        {
            set { timestamp = value; }
            get { return timestamp; }
        }

        public bool equals(object other)
        {
            if ( !( other.GetType() == typeof(Employee) ) ) return false;
            Employee castOther = (Employee)other;
            return Empno == castOther.Empno;
        }

        public int hashCode()
        {
            return (int)Empno;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder(50);
            buf.Append(_empno).Append(", ");
            buf.Append(_ename).Append(", ");
            buf.Append(_job).Append(", ");
            buf.Append(_mgr).Append(", ");
            buf.Append(hiredate).Append(", ");
            buf.Append(_sal).Append(", ");
            buf.Append(timestamp).Append(", ");
            buf.Append(_department);
            return buf.ToString();
        }
    }
}
