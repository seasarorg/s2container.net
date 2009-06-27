#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using Seasar.Framework.Aop;
using Seasar.Quill.Database.DataSource.Impl;

namespace Seasar.Quill.Database.Tx.Impl
{
    /// <summary>
    /// 標準的なトランザクションの設定クラス
    /// </summary>
    public class TypicalTransactionSetting : AbstractTransactionSetting
    {
        /// <summary>
        /// トランザクション分離レベル
        /// </summary>
        protected virtual IsolationLevel IsolationLevel
        {
            get { return System.Data.IsolationLevel.ReadCommitted; }
        }

        /// <summary>
        /// トランザクション設定のセットアップ
        /// </summary>
        /// <param name="dataSource"></param>
        protected override void SetupTransaction(IDataSource dataSource)
        {
            Console.WriteLine("★★★★★★★★　new tx setting !!");
            //  TransactionContext
            _transactionContext = CreateTransactionContext();
            TransactionContext txContext = (TransactionContext)_transactionContext;
            txContext.DataSouce = dataSource;
            txContext.IsolationLevel = this.IsolationLevel;

            //  TransactionContextを使用するデータソースにも設定
            Type dataSourceType = dataSource.GetType();
            if (typeof(SelectableDataSourceProxyWithDictionary).IsAssignableFrom(dataSourceType))
            {
                SelectableDataSourceProxyWithDictionary dataSourceProxyWithDictionary
                    = (SelectableDataSourceProxyWithDictionary)dataSource;
                if (!string.IsNullOrEmpty(DataSourceName))
                {
                    IDataSource usingDataSource = dataSourceProxyWithDictionary.GetDataSource(DataSourceName);
                    if (usingDataSource is TxDataSource)
                    {
                        ((TxDataSource)usingDataSource).Context = txContext;
                    }
                }
                else
                {
                    //  DataSourceName無指定の場合はDataSourceは一つだけと見なす
                    dataSourceProxyWithDictionary.SetTransactionContext(txContext);
                }
            }
            else if (typeof(TxDataSource).IsAssignableFrom(dataSourceType))
            {
                ((TxDataSource)dataSource).Context = txContext;
            }

            ITransactionHandler handler = CreateTransactionHandler();
            if(handler is AbstractLocalTxHandler)
            {
                ((AbstractLocalTxHandler)handler).Context = txContext;
            }

            _transactionInterceptor = CreateTransactionInterceptor(handler);
            if(_transactionInterceptor is TransactionInterceptor)
            {
                ((TransactionInterceptor)_transactionInterceptor).TransactionStateHandler
                = txContext;
            }
        }

        /// <summary>
        /// TransactionContextインスタンスの生成
        /// </summary>
        /// <returns></returns>
        protected virtual ITransactionContext CreateTransactionContext()
        {
            return new TransactionContext();
        }

        /// <summary>
        /// Transactionハンドラ生成
        /// </summary>
        /// <returns></returns>
        protected virtual ITransactionHandler CreateTransactionHandler()
        {
            return new LocalRequiredTxHandler();
        }

        /// <summary>
        /// TransactionInterceptorインスタンスの生成
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        protected virtual IMethodInterceptor CreateTransactionInterceptor(ITransactionHandler handler)
        {
            return new TransactionInterceptor(handler);
        }
    }
}
