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
using System.Data.SqlTypes;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    // [TimestampProperty("NullableNextRestDate")]
    [Table("EMP_NULLABLE")]
    public class EmployeeNullable
    {
        private long _empno;
        private string _ename;
        private string _job;
        private SqlInt16 _mgr;
        private DateTime _hiredate = DateTime.Now;
        private SqlSingle _sal;
        private SqlSingle _comm;
        private int _deptno;
        private DateTime _tstamp = DateTime.Now;
        private DateTime? _nullableNextRestDate;

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

        public SqlInt16 Mgr
        {
            set { _mgr = value; }
            get { return _mgr; }
        }

        public DateTime HireDate
        {
            set { _hiredate = value; }
            get { return _hiredate; }
        }

        public SqlSingle Sal
        {
            set { _sal = value; }
            get { return _sal; }
        }

        public SqlSingle Comm
        {
            set { _comm = value; }
            get { return _comm; }
        }

        public int Deptno
        {
            set { _deptno = value; }
            get { return _deptno; }
        }

        public DateTime TStamp
        {
            set { _tstamp = value; }
            get { return _tstamp; }
        }

        public DateTime? NullableNextRestDate
        {
            set { _nullableNextRestDate = value; }
            get { return _nullableNextRestDate; }
        }

        public bool equals(object other)
        {
            if (!(other.GetType() == typeof(EmployeeNullable))) return false;
            EmployeeNullable castOther = (EmployeeNullable) other;
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
            buf.Append(_tstamp).Append(", ");
            buf.Append(_nullableNextRestDate);
            return buf.ToString();
        }
    }
}
