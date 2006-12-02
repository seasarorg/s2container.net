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

using System;
using Seasar.WindowsExample.Logics.Dao;
using Seasar.WindowsExample.Logics.Dto;

namespace Seasar.WindowsExample.Logics.Service.Impl
{
    /// <summary>
    /// 部門登録サービス用実装クラス
    /// </summary>
    public class DepartmentEditServiceImpl : BaseServiceImpl, IDepartmentEditService
    {
        /// <summary>
        /// 部門用DAO
        /// </summary>
        private IDepartmentDao _dao;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dao">部門用DAO</param>
        public DepartmentEditServiceImpl(IDepartmentDao dao)
        {
            _dao = dao;
        }

        /// <summary>
        /// 部門データを取得する
        /// </summary>
        /// <param name="id">部門ID</param>
        /// <returns>部門データ</returns>
        public DepartmentDto GetData(int id)
        {
            return ( _dao.GetData(id) );
        }

        /// <summary>
        /// 部門データを登録する
        /// </summary>
        /// <param name="dto">部門データ</param>
        /// <returns>登録件数</returns>
        public int ExecUpdate(DepartmentDto dto)
        {
            if ( dto == null )
                throw new ArgumentNullException("dto");

            if ( dto.Id.HasValue )
            {
                DepartmentDto data = _dao.GetData(dto.Id.Value);
                if ( data != null )
                    return ( _dao.UpdateData(dto) );
                else
                    return ( _dao.InsertData(dto) );
            }
            else
            {
                return ( _dao.InsertData(dto) );
            }            
        }

        /// <summary>
        /// 部門を削除する
        /// </summary>
        /// <param name="id">部門ID</param>
        /// <returns>削除件数</returns>
        public int ExecDelete(int id)
        {
            DepartmentDto data = new DepartmentDto();
            data.Id = id;

            return ( _dao.DeleteData(data) );
        }
    }
}