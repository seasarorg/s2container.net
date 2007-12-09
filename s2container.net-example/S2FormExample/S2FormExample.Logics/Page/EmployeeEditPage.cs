#region Copyright

/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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

namespace Seasar.S2FormExample.Logics.Page
{
    /// <summary>
    /// 社員編集Pageクラス
    /// </summary>
    public class EmployeeEditPage
    {
        private int? _id;
        private string _code;
        private string _name;
        private int _gender;
        private DateTime? _entry;
        private int? _depart;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmployeeEditPage()
        {
            _entry = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }

        /// <summary>
        /// 社員ID
        /// </summary>
        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 社員コード
        /// </summary>
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 社員名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 性別ID
        /// </summary>
        public int Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        /// <summary>
        /// 入社日
        /// </summary>
        public DateTime? Entry
        {
            get { return _entry; }
            set { _entry = value; }
        }

        /// <summary>
        /// 部門ID
        /// </summary>
        public int? Depart
        {
            get { return _depart; }
            set { _depart = value; }
        }
    }
}