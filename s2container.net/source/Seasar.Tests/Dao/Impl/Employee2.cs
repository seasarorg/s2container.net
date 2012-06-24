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

using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Table("EMP2")]
    public class Employee2
    {
        private long _empno;
        private string _ename;
        private int _deptnum;
        private Department2 _department2;

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

        public int Deptnum
        {
            set { _deptnum = value; }
            get { return _deptnum; }
        }

        [Relno(0), Relkeys("DEPTNUM:DEPTNO")]
        public Department2 Department2
        {
            set { _department2 = value; }
            get { return _department2; }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(_empno).Append(", ");
            buf.Append(_ename).Append(", ");
            buf.Append(_deptnum).Append(", {");
            buf.Append(_department2).Append("}");
            return buf.ToString();
        }
    }
}
