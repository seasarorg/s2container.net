#region Copyright

/*
S * Copyright 2005-2008 the Seasar Foundation and the Others.
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

using System.Collections.Generic;
using Seasar.S2FormExample.Logics.Dto;

namespace Seasar.S2FormExample.Logics.Page
{
    /// <summary>
    /// 社員一覧Pageクラス
    /// </summary>
    public class EmployeeListPage
    {
        private string _genderId;
        private string _genderName;
        private IList<EmployeeDto> _list;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmployeeListPage()
        {
            _list = new List<EmployeeDto>();
        }

        /// <summary>
        /// 性別ID
        /// </summary>
        public string GenderId
        {
            get { return _genderId; }
            set { _genderId = value; }
        }

        /// <summary>
        /// 性別名
        /// </summary>
        public string GenderName
        {
            get { return _genderName; }
            set { _genderName = value; }
        }

        /// <summary>
        /// 社員リスト
        /// </summary>
        public IList<EmployeeDto> List
        {
            get { return _list; }
            set { _list = value; }
        }
    }
}