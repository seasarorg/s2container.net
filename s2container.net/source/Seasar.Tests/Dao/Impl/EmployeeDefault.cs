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
using System.Text;
using Seasar.Dao.Attrs;
using System;

namespace Seasar.Tests.Dao.Impl
{
    [Table("EMP_DEFAULT")]
    [TimestampProperty("Timestamp")]
    public class EmployeeDefaultTimestamp
    {
        private long _empno;
        private string _ename;
        private string _job;
        private SqlInt16 _mgr;
        private SqlSingle _sal;
        private SqlSingle _comm;
        private int _deptno;
        private byte[] _password;
        private string _dummy;
        private Department _department;
        private DateTime _timestamp;

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
            set { _timestamp = value; }
            get { return _timestamp; }
        }

        public bool equals(object other)
        {
            if ( !( other.GetType() == typeof(EmployeeDefaultTimestamp) ) ) return false;
            EmployeeDefaultTimestamp castOther = (EmployeeDefaultTimestamp)other;
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
            buf.Append(_sal).Append(", ");
            buf.Append(_comm).Append(", ");
            buf.Append(_deptno).Append(", ");
            buf.Append(_timestamp).Append(", ");
            buf.Append(_department);
            return buf.ToString();
        }
    }

    [Table("EMP_DEFAULT")]
    [VersionNoProperty("Version")]
    public class EmployeeDefaultVersionNo
    {
        private long _empno;
        private string _ename;
        private string _job;
        private SqlInt16 _mgr;
        private SqlSingle _sal;
        private SqlSingle _comm;
        private int _deptno;
        private byte[] _password;
        private string _dummy;
        private Department _department;
        private int _version;

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

        public int Version
        {
            set { _version = value; }
            get { return _version; }
        }

        public bool equals(object other)
        {
            if ( !( other.GetType() == typeof(EmployeeDefaultVersionNo) ) ) return false;
            EmployeeDefaultVersionNo castOther = (EmployeeDefaultVersionNo)other;
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
            buf.Append(_sal).Append(", ");
            buf.Append(_comm).Append(", ");
            buf.Append(_deptno).Append(", ");
            buf.Append(_version).Append(", ");
            buf.Append(_department);
            return buf.ToString();
        }
    }
}
