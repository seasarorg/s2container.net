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
using System.Data.SqlTypes;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Table("EMP")]
    public class Employee4
    {
        public long Empno { set; get; }

        public string Ename { set; get; }

        public string Job { set; get; }

        public SqlInt16 Mgr { set; get; }

        public DateTime Hiredate { set; get; }

        public float Sal { set; get; }

        public float Comm { set; get; }

        public int Deptno { set; get; }

        [Relno(0), Relkeys("mgr:empno")]
        public Employee4 Parent { set; get; }
    }
}
