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

using System.Collections.Generic;
using Seasar.S2FormExample.Logics.Dao;
using Seasar.S2FormExample.Logics.Dto;

namespace Seasar.S2FormExample.Logics.Service.Impl
{
    /// <summary>
    /// 基底サービス用実装クラス
    /// </summary>
    /// <remarks>
    /// 多くのフォームで使うマスタのリストボックスなど用のメソッドを用意する
    /// </remarks>
    public class BaseServiceImpl : IBaseService
    {
        /// <summary>
        /// 部門DAO
        /// </summary>
        protected IDepartmentDao daoOfDept;

        /// <summary>
        /// 性別DAO
        /// </summary>
        protected IGenderDao daoOfGender;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseServiceImpl()
        {
            ;
        }

        #region IBaseService Members

        /// <summary>
        /// 部門を一覧で取得する
        /// </summary>
        /// <returns>部門一覧</returns>
        public virtual IList<DepartmentDto> GetDepartmentAll()
        {
            return (daoOfDept.GetAll());
        }

        /// <summary>
        /// 性別を一覧で取得する
        /// </summary>
        /// <returns>性別一覧</returns>
        public IList<GenderDto> GetGenderAll()
        {
            return (daoOfGender.GetAll());
        }

        #endregion
    }
}