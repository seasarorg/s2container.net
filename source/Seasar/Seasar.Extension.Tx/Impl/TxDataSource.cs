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

using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Extension.Tx.Impl
{
	/// <summary>
	/// TxDataSource ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class TxDataSource : DataSourceImpl, IDataSource
	{
		private ITransactionContext context;

		public TxDataSource()
		{
		}

		public TxDataSource(DataProvider provider, string connectionString) : base(provider, connectionString)
		{
		}

		public new IDbConnection GetConnection()
		{
			IDbConnection con = null;
			ITransactionContext tc = this.Context.Current;
			if(tc != null && tc.Connection != null)
			{
				con = tc.Connection;
			} 
			else 
			{
				con = base.GetConnection();
			}
			return con;
		}

		public ITransactionContext Context
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

        public override IDbTransaction GetTransaction()
        {
            return this.context.Current.Transaction;
        }
	}
}
