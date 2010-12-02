
using System;
using Seasar.Extension.Tx;
using Seasar.Extension.Unit;
using Seasar.Extension.ADO;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Util;

namespace Seasar.Unit.Core
{
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
