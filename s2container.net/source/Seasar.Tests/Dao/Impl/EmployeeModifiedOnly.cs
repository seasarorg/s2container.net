#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
using System.Collections;
using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Table("EMP")]
    public class EmployeeModifiedOnly
    {
        private long _empno;
        private string _ename;
        private string _job;
        private int? _mgr;
        private DateTime? _hiredate;
        private Nullable<int> _sal;
        private string _dummy;
        private DepartmentModifiedOnly _department;
        private IDictionary _modifiedPropertyNames = new Hashtable();

        [Relno(0)]
        public DepartmentModifiedOnly Department
        {
            set { _department = value; }
            get { return _department; }
        }

        public long Empno
        {
            set { _empno = value; }
            get { return _empno; }
        }

        public string Ename
        {
            set 
            {
                _ename = value;
                _modifiedPropertyNames["EName"] = _ename;
            }
            get { return _ename; }
        }

        [Column("Job")]
        public string JobName
        {
            set
            {
                _job = value;
                _modifiedPropertyNames["JobName"] = _job;
            }
            get { return _job; }
        }

        public int? Mgr
        {
            set 
            {
                _mgr = value;
                _modifiedPropertyNames["Mgr"] = _mgr;
            }
            get { return _mgr; }
        }

        public DateTime? Hiredate
        {
            set 
            {
                _hiredate = value;
                _modifiedPropertyNames["Hiredate"] = _hiredate;
            }
            get { return _hiredate; }
        }

        public Nullable<int> Sal
        {
            set
            {
                _sal = value;
                _modifiedPropertyNames["Sal"] = _sal;
            }
            get { return _sal; }
        }

        public string Dummy
        {
            set
            {
                _dummy = value;
                _modifiedPropertyNames["Dummy"] = _dummy;
            }
            get { return _dummy; }
        }

        public IDictionary ModifiedPropertyNames
        {
            get { return _modifiedPropertyNames; }
        }

        public void ClearModifiedPropertyNames()
        {
            _modifiedPropertyNames.Clear();
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
            buf.Append(_hiredate).Append(", ");
            buf.Append(_sal).Append(", ");
            return buf.ToString();
        }
    }

    [Table("EMP")]
    public class EmployeeModifiedOnlyWithoutClearMethod
    {
        private long _empno;
        private string _ename;
        private string _job;
        private int? _mgr;
        private DateTime? _hiredate;
        private Nullable<int> _sal;
        private string _dummy;
        private IDictionary _modifiedPropertyNames = new Hashtable();

        public long Empno
        {
            set { _empno = value; }
            get { return _empno; }
        }

        public string Ename
        {
            set
            {
                _ename = value;
                _modifiedPropertyNames["EName"] = _ename;
            }
            get { return _ename; }
        }

        [Column("Job")]
        public string JobName
        {
            set
            {
                _job = value;
                _modifiedPropertyNames["JobName"] = _job;
            }
            get { return _job; }
        }

        public int? Mgr
        {
            set
            {
                _mgr = value;
                _modifiedPropertyNames["Mgr"] = _mgr;
            }
            get { return _mgr; }
        }

        public DateTime? Hiredate
        {
            set
            {
                _hiredate = value;
                _modifiedPropertyNames["Hiredate"] = _hiredate;
            }
            get { return _hiredate; }
        }

        public Nullable<int> Sal
        {
            set
            {
                _sal = value;
                _modifiedPropertyNames["Sal"] = _sal;
            }
            get { return _sal; }
        }

        public string Dummy
        {
            set
            {
                _dummy = value;
                _modifiedPropertyNames["Dummy"] = _dummy;
            }
            get { return _dummy; }
        }

        public IDictionary ModifiedPropertyNames
        {
            get { return _modifiedPropertyNames; }
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
            buf.Append(_hiredate).Append(", ");
            buf.Append(_sal).Append(", ");
            return buf.ToString();
        }
    }

    [Table("EMP")]
    public class EmployeeClearModifiedMethodOnly
    {
        private long _empno;
        private string _ename;
        private string _job;
        private int? _mgr;
        private DateTime? hiredate;
        private Nullable<int> _sal;
        private byte[] _password;
        private string _dummy;
        private bool _isClearMethodCalled = false;

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

        public bool IsClearMethodCalled
        {
            get { return _isClearMethodCalled; }
            set { _isClearMethodCalled = value; }
        }

        public void ClearModifiedPropertyNames()
        {
            _isClearMethodCalled = true;
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
            return buf.ToString();
        }
    }

    [Table("EMP")]
    public class EmployeeNoModifiedPropertyNamesAndMethod
    {
        private long _empno;
        private string _ename;
        private string _job;
        private int? _mgr;
        private DateTime? hiredate;
        private Nullable<int> _sal;
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

        public string Dummy
        {
            set { _dummy = value; }
            get { return _dummy; }
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
            return buf.ToString();
        }
    }
}
