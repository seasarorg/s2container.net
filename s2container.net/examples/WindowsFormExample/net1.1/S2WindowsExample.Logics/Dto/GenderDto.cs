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

namespace Seasar.WindowsExample.Logics.Dto
{
    /// <summary>
    /// 性別用DTO
    /// </summary>
    public class GenderDto
    {
        /// <summary>
        /// 性別ID
        /// </summary>
        private int _id;

        /// <summary>
        /// 性別名
        /// </summary>
        private string _name;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GenderDto()
        {
            _name = "";
        }

        /// <summary>
        /// 性別ID
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 性別名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}