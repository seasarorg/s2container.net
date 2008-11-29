#region Copyright

/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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

#if NET_1_1
// NET 1.1
using Nullables;
using Seasar.Dao.Attrs;
#else
// NET 2.0
using System;
using Seasar.Dao.Attrs;
#endif

namespace Seasar.WindowsExample.Logics.Dto
{
    /// <summary>
    /// 社員用DTO
    /// </summary>
    [Table("T_EMP")]
    public class EmployeeDto
    {
#if NET_1_1
        // NET 1.1

        /// <summary>
        /// 社員ID
        /// </summary>
        private NullableInt32 _id;
#else
        // NET 2.0
        /// <summary>
        /// 社員ID
        /// </summary>
        private Nullable<int> _id;
#endif

        /// <summary>
        /// 社員コード
        /// </summary>
        private string _code;

        /// <summary>
        /// 社員名
        /// </summary>
        private string _name;

        /// <summary>
        /// 性別ID
        /// </summary>
        private int _genderId;

#if NET_1_1
        // NET 1.1
        /// <summary>
        /// 入社日
        /// </summary>
        private NullableDateTime _entryDay;

        /// <summary>
        /// 部門ID
        /// </summary>
        private NullableInt32 _deptNo;
#else
        // NET 2.0
        /// <summary>
        /// 入社日
        /// </summary>
        private Nullable<DateTime> _entryDay;

        /// <summary>
        /// 部門ID
        /// </summary>
        private Nullable<int> _deptNo;
#endif

        /// <summary>
        /// 部門
        /// </summary>
        private DepartmentDto _department;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmployeeDto()
        {
            _id = null;
            _code = "";
            _name = "";
        }

#if NET_1_1
        // NET 1.1
        /// <summary>
        /// 社員ID
        /// </summary>
        [Column("n_id")]
        public NullableInt32 Id
        {
            get { return _id; }
            set { _id = value; }
        }
#else
        // NET 2.0
        /// <summary>
        /// 社員ID
        /// </summary>
        [Column("n_id")]
        public Nullable<int> Id
        {
            get { return _id; }
            set { _id = value; }
        }
#endif

        /// <summary>
        /// 社員コード
        /// </summary>
        [Column("s_code")]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 社員名
        /// </summary>
        [Column("s_name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 性別ID
        /// </summary>
        [Column("n_gender")]
        public int Gender
        {
            get { return _genderId; }
            set { _genderId = value; }
        }

#if NET_1_1
        // NET1.1

        /// <summary>
        /// 入社日
        /// </summary>
        [Column("d_entry")]
        public NullableDateTime EntryDay
        {
            get { return _entryDay; }
            set { _entryDay = value; }
        }

        /// <summary>
        /// 部門ID
        /// </summary>
        [Column("n_dept_id")]
        public NullableInt32 DeptNo
        {
            get { return _deptNo; }
            set { _deptNo = value; }
        }
#else
        /// <summary>
        /// 入社日
        /// </summary>
        [Column("d_entry")]
        public Nullable<DateTime> EntryDay
        {
            get { return _entryDay; }
            set { _entryDay = value; }
        }

        /// <summary>
        /// 部門ID
        /// </summary>
        [Column("n_dept_id")]
        public Nullable<int> DeptNo
        {
            get { return _deptNo; }
            set { _deptNo = value; }
        }
#endif

        /// <summary>
        /// 部門
        /// </summary>
        [Relno(0), Relkeys("n_dept_id:n_id")]
        public DepartmentDto Department
        {
            get { return _department; }
            set { _department = value; }
        }
        
        /// <summary>
        /// 部門名
        /// </summary>
        public string DeptName
        {
            get { return _department.Name; }
        }
    }
}