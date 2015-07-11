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

using System;
using System.Data;
using System.Threading;
using Seasar.Extension.ADO;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Extension.Tx.Impl
{
    public class TransactionContext : ITransactionContext, ITransactionStateHandler
    {
        private static readonly Logger _logger = Logger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly LocalDataStoreSlot _slot;
        private IDataSource _dataSource;
        private IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;
        private ITransactionContext _parent;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public TransactionContext()
        {
            _slot = Thread.AllocateDataSlot();
        }

        private TransactionContext(LocalDataStoreSlot slot)
        {
            _slot = slot;
        }

        public void OpenConnection()
        {
            _connection = DataSourceUtil.GetConnection(_dataSource);
        }

        public void Begin()
        {
            OpenConnection();
            _transaction = Connection.BeginTransaction(IsolationLevel);
            _logger.Log("DSSR0003", null);
        }

        public void Commit()
        {
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
            _logger.Log("DSSR0004", null);
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
            _logger.Log("DSSR0005", null);
        }

        public ITransactionContext Create()
        {
            TransactionContext ctx = new TransactionContext(_slot);
            ctx._dataSource = _dataSource;
            ctx._isolationLevel = _isolationLevel;
            return ctx;
        }

        public ITransactionContext Current
        {
            get { return (Thread.GetData(_slot) as TransactionContext); }
            set { Thread.SetData(_slot, value); }
        }

        public ITransactionContext Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public IDbConnection Connection
        {
            get { return _connection; }
        }

        public bool IsInTransaction
        {
            get
            {
                TransactionContext cur = Current as TransactionContext;
                return cur == null ? false : cur._transaction != null;
            }
        }
        public IDataSource DataSouce
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        public IsolationLevel IsolationLevel
        {
            get { return _isolationLevel; }
            set { _isolationLevel = value; }
        }

        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }

        #region IDisposable ÉÅÉìÉo

        public void Dispose()
        {
            _transaction = null;
            try
            {
                ConnectionUtil.Close(Connection);
            }
            finally
            {
                if (_connection != null)
                {
                    Connection.Dispose();
                }
            }
        }

        #endregion
    }
}
