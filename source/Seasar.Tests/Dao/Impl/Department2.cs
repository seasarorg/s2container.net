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

using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Table("DEPT2")]
    public class Department2
    {
        private int _deptno;
        private string _dname;
        private bool _active;

        public int Deptno
        {
            set { _deptno = value; }
            get { return _deptno; }
        }

        public string Dname
        {
            set { _dname = value; }
            get { return _dname; }
        }

        public bool IsActive
        {
            set { _active = value; }
            get { return _active; }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(_deptno).Append(", ");
            buf.Append(_dname).Append(", ");
            return buf.ToString();
        }
    }
}
