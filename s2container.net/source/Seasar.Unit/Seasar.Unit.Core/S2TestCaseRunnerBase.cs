#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

#if NET_4_0
using System;
using Seasar.Extension.ADO;
using Seasar.Extension.Tx;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Util;

namespace Seasar.Unit.Core
{
    /// <summary>
    /// テストフレームワーク依存処理デリゲート
    /// </summary>
    /// <typeparam name="T">テストフレームワーク依存情報</typeparam>
    /// <param name="argument"></param>
    public delegate void DelegateS2TestMethod<T>(T argument);

    /// <summary>
    /// テスト実行基底クラス
    /// </summary>
    public class S2TestCaseRunnerBase
    {
        protected readonly Seasar.Extension.Unit.Tx _txTreatment;
        protected ITransactionContext _transactionContext;
        protected IDataSource _dataSource;

        public S2TestCaseRunnerBase(Seasar.Extension.Unit.Tx txTreatment)
        {
            _txTreatment = txTreatment;
        }

        public void SetUp<T>(object fixtureInstance, DelegateS2TestMethod<T> invoker, T argument)
        {
            SetUpContainer(fixtureInstance);
            invoker(argument);
            SetUpAfterContainerInit(fixtureInstance);
        }

        public void Execute<T>(DelegateS2TestMethod<T> invoker, T argument)
        {
            try
            {
                BeginTransaction();
                invoker(argument);
                EndTransaction();
            }
            catch (System.Exception)
            {
                RollbackTransaction();
                throw;
            }
        }

        public void TearDown<T>(object fixtureInstance, DelegateS2TestMethod<T> invoker, T argument)
        {
            try
            {
                try
                {
                    TearDownBeforeContainerDestroy(fixtureInstance);
                }
                finally
                {
                    try
                    {
                        invoker(argument);
                    }
                    finally
                    {
                        TearDownContainer(fixtureInstance);
                    }
                }
            }
            finally
            {
                for (int i = 0; i < 3; ++i)
                {
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }
        }

        protected virtual void SetUpContainer(object fixtureInstance)
        {
            //  継承クラスで拡張
        }

        protected virtual void TearDownContainer(object fixtureInstance)
        {
            //  継承クラスで拡張
        }

        protected virtual ITransactionContext GetTransactionContext()
        {
            //  継承クラスで拡張
            return null;
        }

        protected virtual void SetUpAfterContainerInit(object fixtureInstance)
        {
            //  継承クラスで拡張
        }

        protected virtual void TearDownBeforeContainerDestroy(object fixtureInstance)
        {
            //  継承クラスで拡張
        }

        protected virtual IDataSource GetDataSource(object fixtureInstance)
        {
            //  継承クラスで拡張
            return null;
        }

        protected virtual void SetUpDataSource(object fixtureInstance)
        {
            _dataSource = GetDataSource(fixtureInstance);
            if (_dataSource != null &&
                typeof(S2TestCaseBase).IsAssignableFrom(fixtureInstance.GetType()))
            {
                ((S2TestCaseBase)fixtureInstance).DataSource = _dataSource;
            }
        }

        protected virtual void TearDownDataSource(object fixtureInstance)
        {
            if (_dataSource == null)
            {
                return;
            }

            var txDataSource = _dataSource as TxDataSource;
            if (txDataSource != null)
            {
                if (txDataSource.Context.Connection != null)
                {
                    txDataSource.CloseConnection(txDataSource.Context.Connection);
                }
            }

            if (typeof(S2TestCaseBase).IsAssignableFrom(fixtureInstance.GetType()))
            {
                var fixture = (S2TestCaseBase)fixtureInstance;
                if (fixture.HasConnection)
                {
                    ConnectionUtil.Close(fixture.Connection);
                    fixture.Connection = null;
                }
                fixture.DataSource = null;
            }

            if (_dataSource != null && _transactionContext != null)
            {
                _dataSource.CloseConnection(_transactionContext.Connection);
            }
            _transactionContext = null;
            _dataSource = null;
        }

        protected virtual void BeginTransaction()
        {
            if (_txTreatment != Seasar.Extension.Unit.Tx.NotSupported)
            {
                _transactionContext = GetTransactionContext();
                if (_transactionContext == null)
                {
                    throw new ArgumentException();
                }
                _transactionContext = _transactionContext.Create();
                _transactionContext.Current = _transactionContext;
                _transactionContext.Begin();
            }
        }

        protected virtual void EndTransaction()
        {
            if (_txTreatment == Seasar.Extension.Unit.Tx.Commit)
            {
                _transactionContext.Commit();
            }

            if (_txTreatment == Seasar.Extension.Unit.Tx.Rollback)
            {
                _transactionContext.Rollback();
            }
        }

        protected virtual void RollbackTransaction()
        {
            if (_transactionContext != null)
            {
                _transactionContext.Rollback();
            }
        }
    }
}
#endif