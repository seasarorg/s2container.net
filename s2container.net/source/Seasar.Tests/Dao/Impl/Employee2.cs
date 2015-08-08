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

using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Table("EMP2")]
    public class Employee2
    {
        public long Empno { set; get; }

        public string Ename { set; get; }

        public int Deptnum { set; get; }

        [Relno(0), Relkeys("DEPTNUM:DEPTNO")]
        public Department2 Department2 { set; get; }

        public override string ToString()
        {
            var buf = new StringBuilder();
            buf.Append(Empno).Append(", ");
            buf.Append(Ename).Append(", ");
            buf.Append(Deptnum).Append(", {");
            buf.Append(Department2).Append("}");
            return buf.ToString();
        }
    }
}
