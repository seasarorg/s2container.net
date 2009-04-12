#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using Seasar.Dao.Attrs;

#if NHIBERNATE_NULLABLES
using Nullables;
#endif

namespace Seasar.Tests.Dao.Impl
{
#if NHIBERNATE_NULLABLES
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
#endif

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
