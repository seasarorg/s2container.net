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
using System.Collections;
#else
// NET 2.0
using System.Collections.Generic;
using Seasar.WindowsExample.Logics.Dto;
#endif
using Seasar.WindowsExample.Logics.Dao;

namespace Seasar.WindowsExample.Logics.Service.Impl
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
        private IDepartmentDao _daoOfDept;

        /// <summary>
        /// 性別DAO
        /// </summary>
        private IGenderDao _daoOfGender;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseServiceImpl()
        {
            ;
        }

        /// <summary>
        /// 部門DAO
        /// </summary>
        public IDepartmentDao DaoOfDept
        {
            get { return _daoOfDept; }
            set { _daoOfDept = value; }
        }

        /// <summary>
        /// 性別DAO
        /// </summary>
        public IGenderDao DaoOfGender
        {
            get { return _daoOfGender; }
            set { _daoOfGender = value; }
        }

#if NET_1_1
        // NET 1.1

        /// <summary>
        /// 部門を一覧で取得する
        /// </summary>
        /// <returns>部門一覧</returns>
        public virtual IList GetDepartmentAll()
        {
            return ( _daoOfDept.GetAll() );
        }

        /// <summary>
        /// 性別を一覧で取得する
        /// </summary>
        /// <returns>性別一覧</returns>
        public virtual IList GetGenderAll()
        {
            return ( _daoOfGender.GetAll() );
        }
#else
        // NET 2.0

        /// <summary>
        /// 部門を一覧で取得する
        /// </summary>
        /// <returns>部門一覧</returns>
        public virtual IList<DepartmentDto> GetDepartmentAll()
        {
            return ( _daoOfDept.GetAll() );
        }

        /// <summary>
        /// 性別を一覧で取得する
        /// </summary>
        /// <returns>性別一覧</returns>
        public IList<GenderDto> GetGenderAll()
        {
            return ( _daoOfGender.GetAll() );
        }
#endif
    }
}