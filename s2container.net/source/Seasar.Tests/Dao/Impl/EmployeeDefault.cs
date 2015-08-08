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
using System;
using Seasar.Framework.Util;

namespace Seasar.Tests.Dao.Impl
{
    [Table("EMP_DEFAULT")]
    [TimestampProperty("Timestamp")]
    public class EmployeeDefaultTimestamp
    {
        public long Empno { set; get; }

        public string Ename { set; get; }

        [Column("Job")]
        public string JobName { set; get; }

        public SqlInt16 Mgr { set; get; }

        public SqlSingle Sal { set; get; }

        public SqlSingle Comm { set; get; }

        public int Deptno { set; get; }

        public byte[] Password { set; get; }

        public string Dummy { set; get; }

        [Relno(0)]
        public Department Department { set; get; }

        [Column("tstamp")]
        public DateTime Timestamp { set; get; }

        public bool equals(object other)
        {
            if ( !( other.GetExType() == typeof(EmployeeDefaultTimestamp) ) ) return false;
            var castOther = (EmployeeDefaultTimestamp)other;
            return Empno == castOther.Empno;
        }

        public int HashCode()
        {
            return (int)Empno;
        }

        public override string ToString()
        {
            var buf = new StringBuilder(50);
            buf.Append(Empno).Append(", ");
            buf.Append(Ename).Append(", ");
            buf.Append(JobName).Append(", ");
            buf.Append(Mgr).Append(", ");
            buf.Append(Sal).Append(", ");
            buf.Append(Comm).Append(", ");
            buf.Append(Deptno).Append(", ");
            buf.Append(Timestamp).Append(", ");
            buf.Append(Department);
            return buf.ToString();
        }
    }

    [Table("EMP_DEFAULT")]
    [VersionNoProperty("Version")]
    public class EmployeeDefaultVersionNo
    {
        public long Empno { set; get; }

        public string Ename { set; get; }

        [Column("Job")]
        public string JobName { set; get; }

        public SqlInt16 Mgr { set; get; }

        public SqlSingle Sal { set; get; }

        public SqlSingle Comm { set; get; }

        public int Deptno { set; get; }

        public byte[] Password { set; get; }

        public string Dummy { set; get; }

        [Relno(0)]
        public Department Department { set; get; }

        public int Version { set; get; }

        public bool equals(object other)
        {
            if (!(other.GetExType() == typeof(EmployeeDefaultVersionNo))) return false;
            var castOther = (EmployeeDefaultVersionNo)other;
            return Empno == castOther.Empno;
        }

        public int HashCode()
        {
            return (int)Empno;
        }

        public override string ToString()
        {
            var buf = new StringBuilder(50);
            buf.Append(Empno).Append(", ");
            buf.Append(Ename).Append(", ");
            buf.Append(JobName).Append(", ");
            buf.Append(Mgr).Append(", ");
            buf.Append(Sal).Append(", ");
            buf.Append(Comm).Append(", ");
            buf.Append(Deptno).Append(", ");
            buf.Append(Version).Append(", ");
            buf.Append(Department);
            return buf.ToString();
        }
    }

    [Table("EMP_DEFAULT")]
    [VersionNoProperty("Version")]
    public class EmployeeDefaultVersionNoIgnoreCase
    {
        public long Empno { set; get; }

        public string Ename { set; get; }

        [Column("Job")]
        public string JobName { set; get; }

        public SqlInt16 Mgr { set; get; }

        public SqlSingle Sal { set; get; }

        public SqlSingle Comm { set; get; }

        public int Deptno { set; get; }

        public byte[] Password { set; get; }

        public string Dummy { set; get; }

        [Relno(0)]
        public Department Department { set; get; }

        // 大文字小文字を無視して名前比較をしているか確認
        public int vERSION { set; get; } = 10;

        public bool equals(object other)
        {
            if ( !( other.GetExType() == typeof(EmployeeDefaultVersionNo) ) ) return false;
            var castOther = (EmployeeDefaultVersionNo)other;
            return Empno == castOther.Empno;
        }

        public int HashCode()
        {
            return (int)Empno;
        }

        public override string ToString()
        {
            var buf = new StringBuilder(50);
            buf.Append(Empno).Append(", ");
            buf.Append(Ename).Append(", ");
            buf.Append(JobName).Append(", ");
            buf.Append(Mgr).Append(", ");
            buf.Append(Sal).Append(", ");
            buf.Append(Comm).Append(", ");
            buf.Append(Deptno).Append(", ");
            buf.Append(vERSION).Append(", ");
            buf.Append(Department);
            return buf.ToString();
        }
    }
}
