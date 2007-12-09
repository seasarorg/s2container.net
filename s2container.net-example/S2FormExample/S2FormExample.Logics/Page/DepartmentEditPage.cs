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

namespace Seasar.S2FormExample.Logics.Page
{
    /// <summary>
    /// 部門編集Pageクラス
    /// </summary>
    public class DepartmentEditPage
    {
        private string _code;
        private int? _id;
        private string _name;
        private string _order;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DepartmentEditPage()
        {
            ;
        }

        /// <summary>
        /// 部門ID
        /// </summary>
        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 部門コード
        /// </summary>
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 部門名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 表示順
        /// </summary>
        public string Order
        {
            get { return _order; }
            set { _order = value; }
        }
    }
}