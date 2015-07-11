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

using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Log;

namespace Seasar.Extension.Tx.Impl
{
    public class TxDataSource : DataSourceImpl, IDataSource
    {
        //  Å¶í≤ç∏ópÇ…ÉçÉOí«â¡
        private readonly Logger _logger = Logger.GetLogger(typeof(TxDataSource));

        private ITransactionContext _context;

        public TxDataSource()
        {
        }

        public TxDataSource(DataProvider provider, string connectionString)
            : base(provider, connectionString)
        {
        }

        public TxDataSource(string providerInvariantName)
            : base(providerInvariantName)
        {
        }

        public TxDataSource(IDataSource instance)
            : base(instance)
        {
        }

        public override IDbConnection GetConnection()
        {
            IDbConnection con;
            ITransactionContext tc = Context.Current;

            if (tc != null && tc.Connection != null)
            {
                con = tc.Connection;
            }
            else
            {
                con = Instance.GetConnection();
            }
            return con;
        }

        public override void CloseConnection(IDbConnection connection)
        {
            if (_context.IsInTransaction)
            {
                return;
            }
            base.CloseConnection(connection);
        }

        public ITransactionContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        public override IDbTransaction GetTransaction()
        {
            return _context.Current.Transaction;
        }

        public override void SetTransaction(IDbCommand cmd)
        {
            if (Context.IsInTransaction)
            {
                cmd.Transaction = Context.Current.Transaction;
            }
        }
    }
}
