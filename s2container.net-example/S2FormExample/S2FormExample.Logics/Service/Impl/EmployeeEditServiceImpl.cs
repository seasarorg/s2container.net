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

using System;
using Seasar.Quill.Attrs;
using Seasar.S2FormExample.Logics.Dao;
using Seasar.S2FormExample.Logics.Dto;
using Seasar.S2FormExample.Logics.Page;

namespace Seasar.S2FormExample.Logics.Service.Impl
{
    /// <summary>
    /// 社員登録用サービス用実装クラス
    /// </summary>
    public class EmployeeEditServiceImpl : BaseServiceImpl, IEmployeeEditService
    {
        /// <summary>
        /// 社員DAO
        /// </summary>
        protected IEmployeeDao dao;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmployeeEditServiceImpl()
        {
            ;
        }

        #region IEmployeeEditService Members

        /// <summary>
        /// 社員データを取得する
        /// </summary>
        /// <param name="id">社員ID</param>
        /// <returns>社員データ</returns>
        public EmployeeEditPage GetData(int id)
        {
            EmployeeEditPage page = new EmployeeEditPage();

            EmployeeDto dto = dao.GetData(id);
            if (dto != null)
            {
                page.Code = dto.Code;
                page.Depart = dto.DeptNo;
                page.Entry = dto.EntryDay;
                page.Gender = dto.Gender;
                page.Id = dto.Id;
                page.Name = dto.Name;
            }
            else
            {
                page = null;
            }
            return page;
        }

        /// <summary>
        /// 社員データを登録する
        /// </summary>
        /// <param name="data">登録社員データ</param>
        /// <returns>登録件数</returns>
        [Transaction]
        public virtual int ExecUpdate(EmployeeEditPage data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            EmployeeDto dto = new EmployeeDto();
            dto.Code = data.Code;
            dto.DeptNo = data.Depart;
            dto.EntryDay = data.Entry;
            dto.Gender = data.Gender;
            dto.Id = data.Id;
            dto.Name = data.Name;

            if (data.Id.HasValue)
            {
                EmployeeDto e1 = dao.GetData(data.Id.Value);
                if (e1 != null)
                    return (dao.UpdateData(dto));
                else
                    return (dao.InsertData(dto));
            }
            else
            {
                return (dao.InsertData(dto));
            }
        }

        /// <summary>
        /// 社員データを削除する
        /// </summary>
        /// <param name="id">削除社員ID</param>
        /// <returns>削除件数</returns>
        [Transaction]
        public virtual int ExecDelete(int id)
        {
            EmployeeDto data = new EmployeeDto();
            data.Id = id;

            return (dao.DeleteData(data));
        }

        #endregion
    }
}