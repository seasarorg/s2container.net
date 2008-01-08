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

using System.Collections;
using MbUnit.Core.Invokers;
using Seasar.Extension.ADO;
using Seasar.Extension.Tx;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Container;
using Seasar.Framework.Unit;
using Seasar.Framework.Util;

namespace Seasar.Extension.Unit
{
    public class S2TestCaseRunner : S2FrameworkTestCaseRunner
    {
        private static readonly string DATASOURCE_NAME = string.Format("Ado{0}DataSource", ContainerConstants.NS_SEP);

        private S2TestCase _fixture;
        private Tx _tx;
        private ITransactionContext _tc;
        private IDataSource _dataSource;

        public object Run(IRunInvoker invoker, object o, IList args, Tx tx)
        {
            _tx = tx;
            _fixture = o as S2TestCase;
            return Run(invoker, o, args);
        }

        protected override void BeginTransactionContext()
        {
            if (Tx.NotSupported != _tx)
            {
                _tc = (ITransactionContext) Container.GetComponent(typeof(ITransactionContext));
                _tc.Begin();
            }
        }

        protected override void EndTransactionContext()
        {
            if (_tc != null)
            {
                if (Tx.Commit == _tx)
                {
                    _tc.Commit();
                }
                if (Tx.Rollback == _tx)
                {
                    _tc.Rollback();
                }
            }
        }

        protected override void SetUpAfterContainerInit()
        {
            base.SetUpAfterContainerInit();
            SetupDataSource();
        }

        protected override void TearDownBeforeContainerDestroy()
        {
            TearDownDataSource();
            base.TearDownBeforeContainerDestroy();
        }

        protected void SetupDataSource()
        {
            if (Container.HasComponentDef(DATASOURCE_NAME))
            {
                _dataSource = Container.GetComponent(DATASOURCE_NAME) as IDataSource;
            }
            else if (Container.HasComponentDef(typeof(IDataSource)))
            {
                _dataSource = Container.GetComponent(typeof(IDataSource)) as IDataSource;
            }
            if (_fixture != null && _dataSource != null)
            {
                _fixture.SetDataSource(_dataSource);
            }
        }

        protected void TearDownDataSource()
        {
            TxDataSource txDataSource = _dataSource as TxDataSource;
            if (txDataSource != null)
            {
                if (txDataSource.Context.Connection != null)
                {
                    txDataSource.CloseConnection(txDataSource.Context.Connection);
                }
            }
            if (_fixture.HasConnection)
            {
                ConnectionUtil.Close(_fixture.Connection);
                _fixture.SetConnection(null);
            }
            if (_fixture != null)
            {
                _fixture.SetDataSource(null);
            }
            _dataSource = null;
        }
    }
}
