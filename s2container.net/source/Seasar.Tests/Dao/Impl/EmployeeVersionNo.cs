using System;
using Seasar.Dao.Attrs;
using Nullables;

namespace Seasar.Tests.Dao.Impl
{
    [VersionNoProperty("VersionNo")]
    [Table("DECIMAL_VERSION_NO")]
    public class EmployeeNullableDecimalVersionNo
    {
        private int _empNo;
        public int EmpNo
        {
            get { return _empNo; }
            set { _empNo = value; }
        }

        private string _empName;
        public string EmpName
        {
            get { return _empName; }
            set { _empName = value; }
        }

        private NullableDecimal _versionNo;
        public NullableDecimal VersionNo
        {
            get { return _versionNo; }
            set { _versionNo = value; }
        }
    }

    [VersionNoProperty("VersionNo")]
    [Table("INT_VERSION_NO")]
    public class EmployeeNullableIntVersionNo
    {
        private int _empNo;
        public int EmpNo
        {
            get { return _empNo; }
            set { _empNo = value; }
        }

        private string _empName;
        public string EmpName
        {
            get { return _empName; }
            set { _empName = value; }
        }

        private NullableInt32 _versionNo;
        public NullableInt32 VersionNo
        {
            get { return _versionNo; }
            set { _versionNo = value; }
        }
    }
#if !NET_1_1
    [VersionNoProperty("VersionNo")]
    [Table("DECIMAL_VERSION_NO")]
    public class EmployeeGenericNullableDecimalVersionNo
    {
        private int _empNo;
        public int EmpNo
        {
            get { return _empNo; }
            set { _empNo = value; }
        }

        private string _empName;
        public string EmpName
        {
            get { return _empName; }
            set { _empName = value; }
        }

        private Nullable<Decimal> _versionNo;
        public Nullable<Decimal> VersionNo
        {
            get { return _versionNo; }
            set { _versionNo = value; }
        }
    }

    [VersionNoProperty("VersionNo")]
    [Table("INT_VERSION_NO")]
    public class EmployeeGenericNullableIntVersionNo
    {
        private int _empNo;
        public int EmpNo
        {
            get { return _empNo; }
            set { _empNo = value; }
        }

        private string _empName;
        public string EmpName
        {
            get { return _empName; }
            set { _empName = value; }
        }

        private Nullable<int> _versionNo;
        public Nullable<int> VersionNo
        {
            get { return _versionNo; }
            set { _versionNo = value; }
        }
    }
#endif
    [VersionNoProperty("VersionNo")]
    [Table("DECIMAL_VERSION_NO")]
    public class EmployeeDecimalVersionNo
    {
        private int _empNo;
        public int EmpNo
        {
            get { return _empNo; }
            set { _empNo = value; }
        }

        private string _empName;
        public string EmpName
        {
            get { return _empName; }
            set { _empName = value; }
        }

        private Decimal _versionNo;
        public Decimal VersionNo
        {
            get { return _versionNo; }
            set { _versionNo = value; }
        }
    }

    [VersionNoProperty("VersionNo")]
    [Table("INT_VERSION_NO")]
    public class EmployeeIntVersionNo
    {
        private int _empNo;
        public int EmpNo
        {
            get { return _empNo; }
            set { _empNo = value; }
        }

        private string _empName;
        public string EmpName
        {
            get { return _empName; }
            set { _empName = value; }
        }

        private int _versionNo;
        public int VersionNo
        {
            get { return _versionNo; }
            set { _versionNo = value; }
        }
    }
}
