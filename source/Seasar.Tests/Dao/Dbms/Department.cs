#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Dbms
{
    [Table("DEPT")]
    [Serializable()]
    public class Department
    {
        private int _deptno;
        private string _dname;
        private string _loc;
        private int _versionNo;

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

        public string Loc
        {
            set { _loc = value; }
            get { return _loc; }
        }

        public int VersionNo
        {
            set { _versionNo = value; }
            get { return _versionNo; }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(_deptno).Append(", ");
            buf.Append(_dname).Append(", ");
            buf.Append(_loc).Append(", ");
            buf.Append(_versionNo);
            return buf.ToString();
        }
    }
}
