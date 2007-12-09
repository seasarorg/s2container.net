#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Examples
{
    [Table("DEPT2")]
    public class Department1
    {
        private SqlInt32 _deptno;
        private SqlString _dname;
        private SqlInt16 _active;

        public SqlInt32 Deptno
        {
            set { _deptno = value; }
            get { return _deptno; }
        }

        public SqlString Dname
        {
            set { _dname = value; }
            get { return _dname; }
        }

        public SqlInt16 Active
        {
            set { _active = value; }
            get { return _active; }
        }

        public override string ToString()
        {
            return "deptno=" + (_deptno.IsNull ? "null" : _deptno.Value.ToString())
                + ", dname=" + (_dname.IsNull ? "null" : _dname.Value)
                + ", active=" + (_active.IsNull ? "null" : _active.Value.ToString());
        }
    }
}
