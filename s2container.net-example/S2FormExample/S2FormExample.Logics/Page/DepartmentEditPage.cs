#region Copyright

/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
    /// ����ҏWPage�N���X
    /// </summary>
    public class DepartmentEditPage
    {
        private string _code;
        private int? _id;
        private string _name;
        private string _order;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public DepartmentEditPage()
        {
            ;
        }

        /// <summary>
        /// ����ID
        /// </summary>
        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// ����R�[�h
        /// </summary>
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// ���喼
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// �\����
        /// </summary>
        public string Order
        {
            get { return _order; }
            set { _order = value; }
        }
    }
}