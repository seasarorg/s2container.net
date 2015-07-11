#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
using Seasar.Dao;
using Seasar.Extension.ADO;
using Seasar.Framework.Aop;

namespace Seasar.Quill.Dao
{
    /// <summary>
    /// S2Dao機能利用時に使用するクラスのインスタンスを保持する
    /// インターフェース
    /// </summary>
    public interface IDaoSetting
    {
        [Obsolete("内部で使用していないため、削除予定です。")]
        IDaoMetaDataFactory DaoMetaDataFactory { get; }
        IMethodInterceptor DaoInterceptor { get; }

        /// <summary>
        /// 使用するデータソース登録名
        /// </summary>
        string DataSourceName { get; }

        /// <summary>
        /// Dao関係の設定を行います
        /// </summary>
        /// <param name="dataSource"></param>
        void Setup(IDataSource dataSource);

        /// <summary>
        /// Setupメソッドの実行が必要かどうか判定
        /// </summary>
        /// <returns></returns>
        bool IsNeedSetup();
    }
}
