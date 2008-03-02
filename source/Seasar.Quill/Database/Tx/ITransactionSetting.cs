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

using Seasar.Extension.ADO;
using Seasar.Extension.Tx;
using Seasar.Framework.Aop;

namespace Seasar.Quill.Database.Tx
{
    /// <summary>
    /// トランザクションの設定用インターフェース
    /// </summary>
    public interface ITransactionSetting
    {
        ITransactionContext TransactionContext { get; }
        IMethodInterceptor TransactionInterceptor { get; }

        /// <summary>
        /// トランザクション関係の設定を行います
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
