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

using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Interceptors;

namespace Seasar.Extension.Tx
{
    /// <summary>
    /// トランザクション処理を行うInterceptor
    /// </summary>
    public class TransactionInterceptor : AbstractInterceptor
    {
        private readonly ITransactionHandler transactionhandler;
        private ITransactionStateHandler tansactionstatehandler;

        public TransactionInterceptor(ITransactionHandler transactionhandler)
        {
            this.transactionhandler = transactionhandler;
        }

        public override object Invoke(IMethodInvocation invocation)
        {
            return transactionhandler.Handle(invocation, TransactionStateHandler.IsInTransaction);
        }

        public ITransactionStateHandler TransactionStateHandler
        {
            get { return tansactionstatehandler; }
            set { tansactionstatehandler = value; }
        }
    }
}
