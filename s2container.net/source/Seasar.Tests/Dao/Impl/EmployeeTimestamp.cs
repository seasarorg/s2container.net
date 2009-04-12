using System;
using System.Data.SqlTypes;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [TimestampProperty("Timestamp")]
    [Table("EMP")]
    public class EmployeeGenericNullableTimestamp
    {
        private int _empNo;

        public int EmpNo
        {
            get { return _empNo; }
            set { _empNo = value; }
        }

        private DateTime? _timestamp;

        [Column("TSTAMP")]
        public DateTime? Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
    }

    [TimestampProperty("Timestamp")]
    [Table("EMP")]
    public class EmployeeTimestamp
    {
        private int _empNo;

        public int EmpNo
        {
            get { return _empNo; }
            set { _empNo = value; }
        }

        private DateTime _timestamp;

        [Column("TSTAMP")]
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
    }

    [TimestampProperty("Timestamp")]
    [Table("EMP")]
    public class EmployeeSqlTimestamp
    {
        private int _empNo;

        public int EmpNo
        {
            get { return _empNo; }
            set { _empNo = value; }
        }

        private SqlDateTime _timestamp;

        [Column("TSTAMP")]
        public SqlDateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
    }
}
