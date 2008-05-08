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
    /// 部門用DTO
    /// </summary>
    [Table("T_DEPT")]
    public class DepartmentDto
    {
#if NET_1_1
        // NET 1.1

        /// <summary>
        /// 部門ID
        /// </summary>
        private NullableInt32 _id;
#else
        // NET 2.0
        /// <summary>
        /// 部門ID
        /// </summary>
        private Nullable<int> _id;
#endif

        /// <summary>
        /// 部門コード
        /// </summary>
        private string _code;

        /// <summary>
        /// 部門名
        /// </summary>
        private string _name;

        /// <summary>
        /// 表示順番
        /// </summary>
        private int _showOrder;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DepartmentDto()
        {
            _id = null;
            _code = "";
            _name = "";
        }

#if NET_1_1
        // NET 1.1

        /// <summary>
        /// 部門ID
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
        /// 部門ID
        /// </summary>
        [Column("n_id")]
        public Nullable<int> Id
        {
            get { return _id; }
            set { _id = value; }
        }
#endif

        /// <summary>
        /// 部門コード
        /// </summary>
        [Column("s_code")]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 部門名
        /// </summary>
        [Column("s_name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 表示順番
        /// </summary>
        [Column("n_show_order")]
        public int ShowOrder
        {
            get { return _showOrder; }
            set { _showOrder = value; }
        }
    }
}