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
using Seasar.Quill.Attrs;
using Seasar.S2FormExample.Logics.Dao;
using Seasar.S2FormExample.Logics.Dto;
using Seasar.S2FormExample.Logics.Page;

namespace Seasar.S2FormExample.Logics.Service.Impl
{
    /// <summary>
    /// 部門登録サービス用実装クラス
    /// </summary>
    public class DepartmentEditServiceImpl : BaseServiceImpl, IDepartmentEditService
    {
        /// <summary>
        /// 部門用DAO
        /// </summary>
        protected IDepartmentDao dao;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DepartmentEditServiceImpl()
        {
            ;
        }


        /// <summary>
        /// 部門用DAO
        /// </summary>
        /// <remarks>S2Unitでテストするために追加(Injection用)</remarks>
        public IDepartmentDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        #region IDepartmentEditService Members

        /// <summary>
        /// 部門編集Pageを取得する
        /// </summary>
        /// <param name="id">部門ID</param>
        /// <returns>部門編集Page</returns>
        public DepartmentEditPage GetData(int id)
        {
            DepartmentEditPage page = new DepartmentEditPage();

            DepartmentDto dto = dao.GetData(id);
            if (dto != null)
            {
                page.Code = dto.Code;
                page.Id = dto.Id;
                page.Name = dto.Name;
                page.Order = Convert.ToString(dto.ShowOrder);
            }
            else
            {
                page = null;
            }
            return page;
        }

        /// <summary>
        /// 部門を登録する
        /// </summary>
        /// <param name="dto">登録部門編集Page</param>
        /// <returns>登録件数</returns>
        [Aspect("LocalRequiredTx")]
        public virtual int ExecUpdate(DepartmentEditPage dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            DepartmentDto data = new DepartmentDto();
            data.Code = dto.Code;
            data.Id = dto.Id;
            data.Name = dto.Name;
            data.ShowOrder = Convert.ToInt32(dto.Order);

            if (dto.Id.HasValue)
            {
                DepartmentDto departmentDto = dao.GetData(dto.Id.Value);
                if (departmentDto != null)
                    return (dao.UpdateData(data));
                else
                    return (dao.InsertData(data));
            }
            else
            {
                return (dao.InsertData(data));
            }
        }

        /// <summary>
        /// 部門を削除する
        /// </summary>
        /// <param name="id">部門ID</param>
        /// <returns>削除件数</returns>
        [Aspect("LocalRequiredTx")]
        public virtual int ExecDelete(int id)
        {
            DepartmentDto data = new DepartmentDto();
            data.Id = id;

            return (dao.DeleteData(data));
        }

        #endregion
    }
}