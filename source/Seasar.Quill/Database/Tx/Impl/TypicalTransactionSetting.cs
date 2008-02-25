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
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.Tx;
using Seasar.Extension.Tx.Impl;
using Seasar.Quill.Database.DataSource.Impl;

namespace Seasar.Quill.Database.Tx.Impl
{
    /// <summary>
    /// 標準的なトランザクションの設定クラス
    /// </summary>
    public class TypicalTransactionSetting : AbstractTransactionSetting
    {
        protected override void SetupTransaction(IDataSource dataSource)
        {
            //  TransactionContext
            _transactionContext = new TransactionContext();
            TransactionContext txContext = (TransactionContext)_transactionContext;
            txContext.DataSouce = dataSource;
            txContext.IsolationLevel = IsolationLevel.ReadCommitted;

            //  TransactionContextを使用するデータソースにも設定
            Type dataSourceType = dataSource.GetType();
            if (typeof(SelectableDataSourceProxyWithDictionary).IsAssignableFrom(dataSourceType))
            {
                ((SelectableDataSourceProxyWithDictionary)dataSource).SetTransactionContext(
                    txContext);
            }
            else if (typeof(TxDataSource).IsAssignableFrom(dataSourceType))
            {
                ((TxDataSource)dataSource).Context = txContext;
            }

            //  TransactionInterceptor
            LocalRequiredTxHandler handler = new LocalRequiredTxHandler();
            handler.Context = txContext;
            //RequiredTxHandler handler = new RequiredTxHandler();
            _transactionInterceptor = new TransactionInterceptor(handler);
            ((TransactionInterceptor)_transactionInterceptor).TransactionStateHandler
                = txContext;
        }
    }
}
