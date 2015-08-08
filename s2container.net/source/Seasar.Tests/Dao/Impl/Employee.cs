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
using Seasar.Framework.Util;

namespace Seasar.Tests.Dao.Impl
{
    [Table("EMP")]
    public class Employee
    {
        //        private SqlDateTime hiredate;
        //        private SqlDateTime timestamp;

        public long Empno { set; get; }

        public string Ename { set; get; }

        [Column("Job")]
        public string JobName { set; get; }

        public SqlInt16 Mgr { set; get; }

        //        public SqlDateTime Hiredate
        //        {
        //            set { hiredate = value; }
        //            get { return hiredate; }
        //        }

        public SqlSingle Sal { set; get; }

        public SqlSingle Comm { set; get; }

        public int Deptno { set; get; }

        public byte[] Password { set; get; }

        public string Dummy { set; get; }

        [Relno(0)]
        public Department Department { set; get; }

        //        [Column("tstamp")]
        //        public SqlDateTime Timestamp
        //        {
        //            set { timestamp = value; }
        //            get { return timestamp; }
        //        }

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
            buf.Append(JobName).Append(", ");
            buf.Append(Mgr).Append(", ");
            //            buf.Append(hiredate).Append(", ");
            buf.Append(Sal).Append(", ");
            buf.Append(Comm).Append(", ");
            buf.Append(Deptno).Append(", ");
            //            buf.Append(timestamp).Append(", ");
            buf.Append(Department);
            return buf.ToString();
        }
    }
}
