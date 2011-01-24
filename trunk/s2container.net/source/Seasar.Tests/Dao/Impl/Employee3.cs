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

using System.Data.SqlTypes;
using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Table("EMP")]
    public class Employee3
    {
        private SqlInt64 _empno;
        private string _ename;
        private string _job;
        private SqlInt16 _mgr;
        private SqlDateTime _hiredate;
        private SqlSingle _sal;
        private SqlSingle _comm;
        private SqlInt32 _deptno;
        private Department _department;

        public Employee3()
        {
        }

        public Employee3(SqlInt64 empno)
        {
            _empno = empno;
        }

        public SqlInt64 Empno
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

        public SqlDateTime Hiredate
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

        public SqlInt32 Deptno
        {
            set { _deptno = value; }
            get { return _deptno; }
        }

        public Department Department
        {
            set { _department = value; }
            get { return _department; }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(_empno).Append(", ");
            buf.Append(_ename).Append(", ");
            buf.Append(_job).Append(", ");
            buf.Append(_mgr).Append(", ");
            buf.Append(_hiredate).Append(", ");
            buf.Append(_sal).Append(", ");
            buf.Append(_comm).Append(", ");
            buf.Append(_deptno).Append(", {");
            buf.Append(_department).Append("}");
            return buf.ToString();
        }
    }
}
