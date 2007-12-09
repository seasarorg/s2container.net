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

using Seasar.S2FormExample.Logics.Page;

namespace Seasar.S2FormExample.Logics.Service.Impl
{
    /// <summary>
    /// 部門リストサービス用実装クラス
    /// </summary>
    public class DepartmentListServiceImpl : BaseServiceImpl, IDepartmentListService
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DepartmentListServiceImpl()
        {
            ;
        }

        #region IDepartmentListService Members

        /// <summary>
        /// 部門を一覧で取得する
        /// </summary>
        /// <returns>部門一覧Page</returns>
        public DepartmentListPage GetAll()
        {
            DepartmentListPage page = new DepartmentListPage();
            page.List = base.GetDepartmentAll();

            return page;
        }

        #endregion
    }
}