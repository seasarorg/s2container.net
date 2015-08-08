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
    public class Employee5
    {
        public Employee5()
        {
        }

        public Employee5(long empno)
        {
            Empno = empno;
        }

        public long Empno { set; get; }

        public string Ename { set; get; }

        public string Job { set; get; }

        public SqlInt16 Mgr { set; get; }

        public DateTime Hiredate { set; get; }

        public SqlSingle Sal { set; get; }

        public SqlSingle Comm { set; get; }

        public int Deptno { set; get; }

        public byte[] Password { set; get; }

        public string Dummy { set; get; }

        [Column("tstamp")]
        public DateTime Timestamp { set; get; }

        [Relno(0), Relkeys("deptno:deptno, ename:dname")]
        public Department Department { set; get; }
    }
}
