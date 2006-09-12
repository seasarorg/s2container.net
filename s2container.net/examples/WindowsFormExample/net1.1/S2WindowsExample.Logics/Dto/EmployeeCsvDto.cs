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

using Nullables;
using Seasar.Dao.Attrs;

namespace Seasar.WindowsExample.Logics.Dto
{
    /// <summary>
    /// CSV用社員DTO
    /// </summary>
    [Table("T_EMP")]
    public class EmployeeCsvDto
    {
        /// <summary>
        /// 社員コード
        /// </summary>
        private string _code;

        /// <summary>
        /// 社員名
        /// </summary>
        private string _name;

        /// <summary>
        /// 性別
        /// </summary>
        private int _gender;

        /// <summary>
        /// 入社日
        /// </summary>
        private NullableDateTime _entryDay;

        /// <summary>
        /// 部門コード
        /// </summary>
        private string _deptCode;

        /// <summary>
        /// 部門名
        /// </summary>
        private string _deptName;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmployeeCsvDto()
        {
            _code = "";
            _name = "";
            _entryDay = null;
            _deptCode = "";
            _deptName = "";
        }

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
        /// 性別
        /// </summary>
        [Column("n_gender")]
        public int Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

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
        /// 部門コード
        /// </summary>
        [Column("s_dept_code")]
        public string DeptCode
        {
            get { return _deptCode; }
            set { _deptCode = value; }
        }

        /// <summary>
        /// 部門名
        /// </summary>
        [Column("s_dept_name")]
        public string DeptName
        {
            get { return _deptName; }
            set { _deptName = value; }
        }
    }
}