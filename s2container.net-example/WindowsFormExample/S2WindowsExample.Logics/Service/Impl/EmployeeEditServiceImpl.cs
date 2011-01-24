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
    /// 社員登録用サービス用インターフェイス
    /// </summary>
    public class EmployeeEditServiceImpl : BaseServiceImpl, IEmployeeEditService
    {
        /// <summary>
        /// 社員DAO
        /// </summary>
        private IEmployeeDao _dao;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmployeeEditServiceImpl()
        {
            ;
        }

        /// <summary>
        /// 社員DAO
        /// </summary>
        public IEmployeeDao DaoOfEmp
        {
            get { return _dao; }
            set { _dao = value; }
        }

        /// <summary>
        /// 社員データを取得する
        /// </summary>
        /// <param name="id">社員ID</param>
        /// <returns>社員データ</returns>
        public EmployeeDto GetData(int id)
        {
            return ( _dao.GetData(id) );
        }

        /// <summary>
        /// 社員データを登録する
        /// </summary>
        /// <param name="data">登録社員データ</param>
        /// <returns>登録件数</returns>
        public int ExecUpdate(EmployeeDto data)
        {
            if ( data == null )
                throw new ArgumentNullException("data");

            if ( data.Id.HasValue )
            {
                EmployeeDto e1 = _dao.GetData(data.Id.Value);
                if ( e1 != null )
                    return ( _dao.UpdateData(data) );
                else
                    return ( _dao.InsertData(data) );
            }
            else
            {
                return ( _dao.InsertData(data) );
            }
        }

        /// <summary>
        /// 社員データを削除する
        /// </summary>
        /// <param name="id">削除社員ID</param>
        /// <returns>削除件数</returns>
        public int ExecDelete(int id)
        {
            EmployeeDto data = new EmployeeDto();
            data.Id = id;

            return ( _dao.DeleteData(data) );
        }
    }
}