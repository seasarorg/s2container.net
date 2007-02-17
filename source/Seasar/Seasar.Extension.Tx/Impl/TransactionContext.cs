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
using System.Data;
using System.Threading;
using Seasar.Extension.ADO;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Extension.Tx.Impl
{
	/// <summary>
	/// TransactionContext ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class TransactionContext : ITransactionContext, ITransactionStateHandler
	{
		private static Logger logger = Logger.GetLogger(typeof(TransactionContext));
		private LocalDataStoreSlot slot;

		private IDataSource dataSource;
		private IsolationLevel isolationLevel = IsolationLevel.ReadCommitted;

		private ITransactionContext parent;
		private IDbConnection connection;
		private IDbTransaction transaction;

		public TransactionContext()
		{
			slot = Thread.AllocateDataSlot();
		}

		private TransactionContext(LocalDataStoreSlot slot)
		{
			this.slot = slot;
		}

		public void OpenConnection()
		{
            connection = DataSourceUtil.GetConnection(this.DataSouce);
		}

		public void Begin()
		{
			OpenConnection();
			this.transaction = this.Connection.BeginTransaction(this.IsolationLevel);
			logger.Log("DSSR0003", null);
		}

		public void Commit()
		{
			this.transaction.Commit();
			this.transaction.Dispose();
			this.transaction = null;
			logger.Log("DSSR0004", null);
		}

		public void Rollback()
		{
			this.transaction.Rollback();
			this.transaction.Dispose();
			this.transaction = null;
			logger.Log("DSSR0005", null);
		}

		public ITransactionContext Create()
		{
			TransactionContext ctx = new TransactionContext(this.slot);
			ctx.dataSource = this.dataSource;
            ctx.isolationLevel = this.isolationLevel;
			return ctx;
		}

		public ITransactionContext Current
		{
			get
			{
				return (Thread.GetData(slot) as TransactionContext);
			}
			set 
			{
				Thread.SetData(slot, value);
			}
		}

		public ITransactionContext Parent
		{
			get 
			{
				return this.parent;
			}

			set
			{
				this.parent = value;
			}
		}

		public IDbConnection Connection
		{
			get
			{
				return this.connection;
			}
		}

		public bool IsInTransaction
		{
			get
			{
				TransactionContext cur = this.Current as TransactionContext;
				return cur == null ? false : cur.transaction != null;
			}
		}
		public IDataSource DataSouce
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				this.dataSource = value;
			}
		}

		public IsolationLevel IsolationLevel
		{
			get
			{
				return this.isolationLevel;
			}
			set
			{
				this.isolationLevel = value;
			}
		}

        public IDbTransaction Transaction
        {
            get { return this.transaction; }
        }

		#region IDisposable ÉÅÉìÉo

		public void Dispose()
		{
			this.transaction = null;
			try 
			{
                ConnectionUtil.Close(this.Connection);
			} 
			finally
			{
				if(this.connection != null)
				{
					this.Connection.Dispose();
				}
			}
		}

		#endregion
	}
}
