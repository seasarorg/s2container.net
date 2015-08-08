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
using Seasar.Framework.Util;

namespace Seasar.Tests.Dao.Impl
{
    [Table("EMP")]
    public class EmployeeModifiedOnly
    {
        private string _ename;
        private string _job;
        private int? _mgr;
        private DateTime? _hiredate;
        private int? _sal;
        private string _dummy;

        [Relno(0)]
        public DepartmentModifiedOnly Department { set; get; }

        public long Empno { set; get; }

        public string Ename
        {
            set 
            {
                _ename = value;
                ModifiedPropertyNames["EName"] = _ename;
            }
            get { return _ename; }
        }

        [Column("Job")]
        public string JobName
        {
            set
            {
                _job = value;
                ModifiedPropertyNames["JobName"] = _job;
            }
            get { return _job; }
        }

        public int? Mgr
        {
            set 
            {
                _mgr = value;
                ModifiedPropertyNames["Mgr"] = _mgr;
            }
            get { return _mgr; }
        }

        public DateTime? Hiredate
        {
            set 
            {
                _hiredate = value;
                ModifiedPropertyNames["Hiredate"] = _hiredate;
            }
            get { return _hiredate; }
        }

        public int? Sal
        {
            set
            {
                _sal = value;
                ModifiedPropertyNames["Sal"] = _sal;
            }
            get { return _sal; }
        }

        public string Dummy
        {
            set
            {
                _dummy = value;
                ModifiedPropertyNames["Dummy"] = _dummy;
            }
            get { return _dummy; }
        }

        public IDictionary ModifiedPropertyNames { get; } = new Hashtable();

        public void ClearModifiedPropertyNames()
        {
            ModifiedPropertyNames.Clear();
        }

        public bool equals(object other)
        {
            if ( !( other.GetExType() == typeof(Employee) ) ) return false;
            var castOther = (Employee)other;
            return Empno == castOther.Empno;
        }

        public int HashCode()
        {
            return (int)Empno;
        }

        public override string ToString()
        {
            var buf = new StringBuilder(50);
            buf.Append(Empno).Append(", ");
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
        private string _ename;
        private string _job;
        private int? _mgr;
        private DateTime? _hiredate;
        private int? _sal;
        private string _dummy;

        public long Empno { set; get; }

        public string Ename
        {
            set
            {
                _ename = value;
                ModifiedPropertyNames["EName"] = _ename;
            }
            get { return _ename; }
        }

        [Column("Job")]
        public string JobName
        {
            set
            {
                _job = value;
                ModifiedPropertyNames["JobName"] = _job;
            }
            get { return _job; }
        }

        public int? Mgr
        {
            set
            {
                _mgr = value;
                ModifiedPropertyNames["Mgr"] = _mgr;
            }
            get { return _mgr; }
        }

        public DateTime? Hiredate
        {
            set
            {
                _hiredate = value;
                ModifiedPropertyNames["Hiredate"] = _hiredate;
            }
            get { return _hiredate; }
        }

        public int? Sal
        {
            set
            {
                _sal = value;
                ModifiedPropertyNames["Sal"] = _sal;
            }
            get { return _sal; }
        }

        public string Dummy
        {
            set
            {
                _dummy = value;
                ModifiedPropertyNames["Dummy"] = _dummy;
            }
            get { return _dummy; }
        }

        public IDictionary ModifiedPropertyNames { get; } = new Hashtable();

        public bool equals(object other)
        {
            if ( !( other.GetExType() == typeof(Employee) ) ) return false;
            var castOther = (Employee)other;
            return Empno == castOther.Empno;
        }

        public int HashCode()
        {
            return (int)Empno;
        }

        public override string ToString()
        {
            var buf = new StringBuilder(50);
            buf.Append(Empno).Append(", ");
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
        public long Empno { set; get; }

        public string Ename { set; get; }

        [Column("Job")]
        public string JobName { set; get; }

        public int? Mgr { set; get; }

        public DateTime? Hiredate { set; get; }

        public int? Sal { set; get; }

        public byte[] Password { set; get; }

        public string Dummy { set; get; }

        public bool IsClearMethodCalled { get; set; } = false;

        public void ClearModifiedPropertyNames()
        {
            IsClearMethodCalled = true;
        }

        public bool equals(object other)
        {
            if ( !( other.GetExType() == typeof(Employee) ) ) return false;
            var castOther = (Employee)other;
            return Empno == castOther.Empno;
        }

        public int HashCode()
        {
            return (int)Empno;
        }

        public override string ToString()
        {
            var buf = new StringBuilder(50);
            buf.Append(Empno).Append(", ");
            buf.Append(Ename).Append(", ");
            buf.Append(JobName).Append(", ");
            buf.Append(Mgr).Append(", ");
            buf.Append(Hiredate).Append(", ");
            buf.Append(Sal).Append(", ");
            return buf.ToString();
        }
    }

    [Table("EMP")]
    public class EmployeeNoModifiedPropertyNamesAndMethod
    {
        public long Empno { set; get; }

        public string Ename { set; get; }

        [Column("Job")]
        public string JobName { set; get; }

        public int? Mgr { set; get; }

        public DateTime? Hiredate { set; get; }

        public int? Sal { set; get; }

        public string Dummy { set; get; }

        public bool equals(object other)
        {
            if ( !( other.GetExType() == typeof(Employee) ) ) return false;
            var castOther = (Employee)other;
            return Empno == castOther.Empno;
        }

        public int HashCode()
        {
            return (int)Empno;
        }

        public override string ToString()
        {
            var buf = new StringBuilder(50);
            buf.Append(Empno).Append(", ");
            buf.Append(Ename).Append(", ");
            buf.Append(JobName).Append(", ");
            buf.Append(Mgr).Append(", ");
            buf.Append(Hiredate).Append(", ");
            buf.Append(Sal).Append(", ");
            return buf.ToString();
        }
    }
}
