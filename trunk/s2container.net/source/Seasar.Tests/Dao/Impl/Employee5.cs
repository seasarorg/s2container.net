#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
using System.Data.SqlTypes;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Table("EMP")]
    public class Employee5
    {
        private long _empno;
        private string _ename;
        private string _job;
        private SqlInt16 _mgr;
        private DateTime _hiredate;
        private SqlSingle _sal;
        private SqlSingle _comm;
        private int _deptno;
        private byte[] _password;
        private string _dummy;
        private Department _department;
        private DateTime _timestamp;

        public Employee5()
        {
        }

        public Employee5(long empno)
        {
            _empno = empno;
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

        public SqlInt16 Mgr
        {
            set { _mgr = value; }
            get { return _mgr; }
        }

        public DateTime Hiredate
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

        [Column("tstamp")]
        public DateTime Timestamp
        {
            set { _timestamp = value; }
            get { return _timestamp; }
        }

        [Relno(0), Relkeys("deptno:deptno, ename:dname")]
        public Department Department
        {
            set { _department = value; }
            get { return _department; }
        }
    }
}
