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
using System.EnterpriseServices;

using Seasar.Framework.Aop;
using Seasar.Framework.Log;


namespace Seasar.Extension.Tx.Impl
{
	/// <summary>
	/// RequiresNewTxHandler ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[Transaction(TransactionOption.RequiresNew)]
	public class DTCRequiresNewTxHandler : AbstractDTCTransactionHandler
	{
		private static Logger logger = Logger.GetLogger(typeof(DTCRequiresNewTxHandler));

		#region ITransactionHandler ÉÅÉìÉo

		[AutoComplete]
		public override object Handle(IMethodInvocation invocation, bool alreadyInTransaction)
		{
			logger.Log("DSSR0003", null);
			try
			{
				object obj = invocation.Proceed();
				logger.Log("DSSR0004", null);
				return obj;
			}
			catch
			{
				logger.Log("DSSR0005", null);
				throw;
			}
		}

		#endregion
	}
}
