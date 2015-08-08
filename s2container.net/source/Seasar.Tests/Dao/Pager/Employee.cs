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
using System.Text;
using Seasar.Dao.Attrs;
using Seasar.Framework.Util;

namespace Seasar.Tests.Dao.Pager
{
    [Table("EMP")]
    public class Employee
    {
        public long Empno { set; get; }

        public string Ename { set; get; }

        public string Job { set; get; }

        public short Mgr { set; get; }

        public DateTime Hiredate { set; get; }

        public float Sal { set; get; }

        public float Comm { set; get; }

        public short Deptno { set; get; }

        public bool equals(object other)
        {
            if (!(other.GetExType() == typeof(Employee))) return false;
            var castOther = (Employee) other;
            return Empno == castOther.Empno;
        }

        public int HashCode()
        {
            return (int) Empno;
        }

        public override string ToString()
        {
            var buf = new StringBuilder(50);
            buf.Append(Empno).Append(", ");
            buf.Append(Ename).Append(", ");
            buf.Append(Job).Append(", ");
            buf.Append(Mgr).Append(", ");
            buf.Append(Hiredate).Append(", ");
            buf.Append(Sal).Append(", ");
            buf.Append(Comm).Append(", ");
            buf.Append(Deptno).Append(", ");
            return buf.ToString();
        }
    }
}
