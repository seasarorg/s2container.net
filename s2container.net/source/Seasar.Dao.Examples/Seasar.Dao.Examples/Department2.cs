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

using Seasar.Dao.Attrs;

namespace Seasar.Dao.Examples
{
    [Table("DEPT2")]
    public class Department2
    {
        private int? _deptno;
        private string _dname;
        private short? _active;

        public int? Deptno
        {
            set { _deptno = value; }
            get { return _deptno; }
        }

        public string Dname
        {
            set { _dname = value; }
            get { return _dname; }
        }

        public short? Active
        {
            set { _active = value; }
            get { return _active; }
        }

        public override string ToString()
        {
            return "deptno=" + (!_deptno.HasValue ? "null" : _deptno.Value.ToString())
                + ", dname=" + (_dname == null ? "null" : _dname)
                + ", active=" + (!_active.HasValue ? "null" : _active.Value.ToString());
        }
    }
}
