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

using Seasar.Extension.ADO;
using Seasar.Extension.Tx;
using Seasar.Framework.Aop;
using Seasar.Framework.Util;
using Seasar.Quill.Exception;

namespace Seasar.Quill.Database.Tx.Impl
{
    /// <summary>
    /// トランザクション設定抽象クラス
    /// </summary>
    public abstract class AbstractTransactionSetting : ITransactionSetting
    {
        protected bool isNeedSetup = true;

        protected ITransactionContext transactionContext = null;

        protected IMethodInterceptor transactionInterceptor = null;

        #region ITransactionSetting メンバ

        public virtual string DataSourceName => null;

        public ITransactionContext TransactionContext => transactionContext;

        public IMethodInterceptor TransactionInterceptor => transactionInterceptor;

        public void Setup(IDataSource dataSource)
        {
            SetupTransaction(dataSource);
            if (transactionContext == null)
            {
                throw new QuillApplicationException("EQLL0028", new [] { this.GetExType().Name });
            }

            isNeedSetup = false;
        }

        public virtual bool IsNeedSetup() => isNeedSetup;

        #endregion

        /// <summary>
        /// トランザクションクラスの設定を行う
        /// </summary>
        /// <param name="dataSource"></param>
        protected abstract void SetupTransaction(IDataSource dataSource);
    }
}
