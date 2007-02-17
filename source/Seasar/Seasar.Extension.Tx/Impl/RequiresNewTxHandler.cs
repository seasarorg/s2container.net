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

using System;
using System.Transactions;

using Seasar.Framework.Aop;
using Seasar.Framework.Log;

namespace Seasar.Extension.Tx.Impl
{
	public class RequiresNewTxHandler : ITransactionHandler
	{
        private static Logger logger = Logger.GetLogger(typeof(RequiresNewTxHandler));
        #region ITransactionHandler ÉÅÉìÉo

        object ITransactionHandler.Handle(IMethodInvocation invocation, bool alreadyInTransaction)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                logger.Log("DSSR0003", null);
                try
                {
                    object obj = invocation.Proceed();
                    logger.Log("DSSR0004", null);
                    scope.Complete();
                    return obj;
                }
                catch
                {
                    logger.Log("DSSR0005", null);
                    throw;
                }
            }
        }

        #endregion
    }
}
