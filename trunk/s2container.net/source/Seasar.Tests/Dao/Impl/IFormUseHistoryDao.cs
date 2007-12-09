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

using System.Collections;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Bean(typeof(FormUseHistory))]
    public interface IFormUseHistoryDao
    {
        /// <summary>
        /// インサート
        /// </summary>
        /// <param name="formUseHistory">WEB 画面利用履歴</param>
        /// <returns>登録した数</returns>
        int Insert(FormUseHistory formUseHistory);

        /// <summary>
        /// エンティティ取得
        /// </summary>
        /// <param name="webUserCode"></param>
        /// <param name="webFormId"></param>
        /// <returns>WEB 画面利用履歴</returns>
        [Query("W_USER_CD=/*webUserCode*/ and W_FORM_ID=/*webFormId*/")]
        FormUseHistory GetEntity(string webUserCode, string webFormId);

        /// <summary>
        /// リスト取得
        /// </summary>
        /// <returns>WEB 画面利用履歴のリスト</returns>
        IList GetList();
    }
}
