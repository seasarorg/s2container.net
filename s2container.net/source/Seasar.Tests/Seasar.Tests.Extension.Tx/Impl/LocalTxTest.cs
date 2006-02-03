#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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

using Seasar.Extension.Tx;
using Seasar.Extension.Tx.Impl;

using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;

namespace TestSeasar.Extension.Tx.Impl
{
	public interface ILocalTxTest
	{
		bool IsInTransaction();
		IDbConnection GetConnection();
		void throwException();
	}

	public class LocalTxTest : ILocalTxTest
	{
		private ITransactionContext context;

		public bool IsInTransaction()
		{
			return context.IsInTransaction;
		}

		public IDbConnection GetConnection()
		{
			return context.Connection;
		}

		public void throwException()
		{
			throw new NotSupportedException();
		}

		public ITransactionContext TC
		{
			get
			{
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

	}
}
