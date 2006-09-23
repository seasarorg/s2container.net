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
#endif
using Seasar.Dao.Attrs;
using Seasar.WindowsExample.Logics.Dto;

namespace Seasar.WindowsExample.Logics.Dao
{
    /// <summary>
    /// 部門用DAO
    /// </summary>
    [Bean(typeof (DepartmentDto))]
    public interface IDepartmentDao
    {
#if NET_1_1
        // NET 1.1
        /// <summary>
        /// 部門一覧を取得する
        /// </summary>
        /// <returns>部門リスト</returns>
        [Query("order by n_show_order")]
        IList GetAll();
#else
        // NET 2.0
        /// <summary>
        /// 部門一覧を取得する
        /// </summary>
        /// <returns>部門リスト</returns>
        [Query("order by n_show_order")]
        IList<DepartmentDto> GetAll();
#endif

        /// <summary>
        /// 部門データを取得する
        /// </summary>
        /// <param name="id">部門ID</param>
        /// <returns>部門データ</returns>
        [Query("n_id = /*id*/1")]
        DepartmentDto GetData(int id);

        /// <summary>
        /// 部門IDを取得する
        /// </summary>
        /// <param name="code">部門コード</param>
        /// <returns>部門ID</returns>
        [Sql("select n_id from t_dept where s_code = /*code*/'0002'")]
        int GetId(string code);

        /// <summary>
        /// 部門を挿入する
        /// </summary>
        /// <param name="dto">挿入するデータ</param>
        /// <returns>挿入件数</returns>
        [NoPersistentProps("Id")]
        int InsertData(DepartmentDto dto);

        /// <summary>
        /// 部門を更新する
        /// </summary>
        /// <param name="dto">更新データ</param>
        /// <returns>更新件数</returns>
        int UpdateData(DepartmentDto dto);

        /// <summary>
        /// 部門を削除する
        /// </summary>
        /// <param name="dto">削除データ</param>
        /// <returns>削除件数</returns>
        int DeleteData(DepartmentDto dto);
    }
}