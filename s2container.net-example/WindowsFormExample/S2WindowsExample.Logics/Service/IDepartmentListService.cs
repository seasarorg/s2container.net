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

namespace Seasar.WindowsExample.Logics.Service
{
    /// <summary>
    /// 部門リストサービス用インターフェイス
    /// </summary>
    public interface IDepartmentListService : IBaseService
    {
        // サンプルなので、次のメソッドを用意しているが、IBaseServiceで用意しているので本当は不要
#if NET_1_1
        // NET 1.1
        
        /// <summary>
        /// 部門を一覧で取得する
        /// </summary>
        /// <returns>部門一覧</returns>
        IList GetAll();
#else
        // NET 2.0
        /// <summary>
        /// 部門を一覧で取得する
        /// </summary>
        /// <returns>部門一覧</returns>
        IList<DepartmentDto> GetAll();
#endif
    }
}