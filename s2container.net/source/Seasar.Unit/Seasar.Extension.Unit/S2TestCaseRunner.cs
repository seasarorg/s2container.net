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
        private static readonly string DATASOURCE_NAME = "Ado"
            + ContainerConstants.NS_SEP + "DataSource";

        private S2TestCase fixture;
        private Tx tx;
        private ITransactionContext tc;
        private IDataSource dataSource;

        public S2TestCaseRunner()
        {
        }

        public object Run(IRunInvoker invoker, object o, IList args, Tx tx)
        {
            this.tx = tx;
            fixture = o as S2TestCase;
            return this.Run(invoker, o, args);
        }

        protected override void BeginTransactionContext()
        {
            if (Tx.NotSupported != tx)
            {
                tc = (ITransactionContext) this.Container.GetComponent(typeof(ITransactionContext));
                tc.Begin();
            }
        }

        protected override void EndTransactionContext()
        {
            if (tc != null)
            {
                if (Tx.Commit == tx)
                {
                    tc.Commit();
                }
                if (Tx.Rollback == tx)
                {
                    tc.Rollback();
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
            if (this.Container.HasComponentDef(DATASOURCE_NAME))
            {
                dataSource = this.Container.GetComponent(DATASOURCE_NAME) as IDataSource;
            }
            else if (this.Container.HasComponentDef(typeof(IDataSource)))
            {
                dataSource = this.Container.GetComponent(typeof(IDataSource)) as IDataSource;
            }
            if (fixture != null && dataSource != null)
            {
                fixture.SetDataSource(dataSource);
            }
        }

        protected void TearDownDataSource()
        {
            if (dataSource is TxDataSource)
            {
                TxDataSource txDataSource = dataSource as TxDataSource;
                if (txDataSource.Context.Connection != null)
                {
                    DataSourceUtil.CloseConnection(txDataSource, txDataSource.Context.Connection);
                }
            }
            if (fixture.HasConnection)
            {
                ConnectionUtil.Close(fixture.Connection);
                fixture.SetConnection(null);
            }
            if (fixture != null)
            {
                fixture.SetDataSource(null);
            }
            dataSource = null;
        }
    }
}
