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
using Seasar.Quill.Exception;
using Seasar.Framework.Log;

namespace Seasar.Quill.Database.Tx.Impl
{
    /// <summary>
    /// トランザクション設定抽象クラス
    /// </summary>
    public abstract class AbstractTransactionSetting : ITransactionSetting
    {
        //  ※調査用にログ追加
        private readonly Logger _logger = Logger.GetLogger(typeof(AbstractTransactionSetting));

        protected bool _isNeedSetup = true;

        protected ITransactionContext _transactionContext = null;

        protected IMethodInterceptor _transactionInterceptor = null;

        #region ITransactionSetting メンバ

        public virtual string DataSourceName
        {
            get { return null; }
        }

        public ITransactionContext TransactionContext
        {
            get { return _transactionContext; }
        }

        public IMethodInterceptor TransactionInterceptor
        {
            get { return _transactionInterceptor; }
        }

        public void Setup(IDataSource dataSource)
        {
            SetupTransaction(dataSource);
            if (_transactionContext == null)
            {
                throw new QuillApplicationException("EQLL0028", new object[] { this.GetType().Name });
            }
            //  現在のTransactionContextのインスタンスを明示的に設定しておく
            //  （Currentがnullだと毎回Connectionが新たに作られてしまうため）
            _transactionContext.Current = _transactionContext;
            if(_logger.IsDebugEnabled)
            {
                _logger.Debug(
                    string.Format("TransactionContext.Current({0})が設定されました。",
                                  _transactionContext.Current == null
                                      ? "null"
                                      : _transactionContext.Current.GetHashCode().ToString()));
            }
            _isNeedSetup = false;
        }

        public virtual bool IsNeedSetup()
        {
            return _isNeedSetup;
        }

        #endregion

        /// <summary>
        /// トランザクションクラスの設定を行う
        /// </summary>
        /// <param name="dataSource"></param>
        protected abstract void SetupTransaction(IDataSource dataSource);
    }
}
