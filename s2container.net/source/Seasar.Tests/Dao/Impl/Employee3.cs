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

using System.Data.SqlTypes;
using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Table("EMP")]
    public class Employee3
    {
        public Employee3()
        {
        }

        public Employee3(SqlInt64 empno)
        {
            Empno = empno;
        }

        public SqlInt64 Empno { set; get; }

        public string Ename { set; get; }

        public string Job { set; get; }

        public SqlInt16 Mgr { set; get; }

        public SqlDateTime Hiredate { set; get; }

        public SqlSingle Sal { set; get; }

        public SqlSingle Comm { set; get; }

        public SqlInt32 Deptno { set; get; }

        public Department Department { set; get; }

        public override string ToString()
        {
            var buf = new StringBuilder();
            buf.Append(Empno).Append(", ");
            buf.Append(Ename).Append(", ");
            buf.Append(Job).Append(", ");
            buf.Append(Mgr).Append(", ");
            buf.Append(Hiredate).Append(", ");
            buf.Append(Sal).Append(", ");
            buf.Append(Comm).Append(", ");
            buf.Append(Deptno).Append(", {");
            buf.Append(Department).Append("}");
            return buf.ToString();
        }
    }
}
