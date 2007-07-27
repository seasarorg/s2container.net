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

using Seasar.Quill.Attrs;
using Seasar.S2FormExample.Logics.Page;
using Seasar.S2FormExample.Logics.Service.Impl;

namespace Seasar.S2FormExample.Logics.Service
{
    /// <summary>
    /// 社員登録用サービス用インターフェイス
    /// </summary>
    [Implementation(typeof (EmployeeEditServiceImpl))]
    public interface IEmployeeEditService : IBaseService
    {
        /// <summary>
        /// 社員データを取得する
        /// </summary>
        /// <param name="id">社員ID</param>
        /// <returns>社員データ</returns>
        EmployeeEditPage GetData(int id);

        /// <summary>
        /// 社員データを登録する
        /// </summary>
        /// <param name="data">登録社員データ</param>
        /// <returns>登録件数</returns>
        int ExecUpdate(EmployeeEditPage data);

        /// <summary>
        /// 社員データを削除する
        /// </summary>
        /// <param name="id">削除社員ID</param>
        /// <returns>削除件数</returns>
        int ExecDelete(int id);
    }
}