#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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

using MbUnit.Framework;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;

namespace Seasar.Tests.Extension.ADO.Impl
{
    [TestFixture]
    public class SelectableDataSourceProxyWithContainerTest
    {
        [Test]
        public void TestSetAndGetDataSourceName()
        {
            //  ## Arrange ##
            const string DS_NAME = "HOGE_DATASOURCE";
            SelectableDataSourceProxyWithContainer ds = new SelectableDataSourceProxyWithContainer();

            //  ## Act ##
            ds.SetDataSourceName(DS_NAME);

            //  ## Assert ##
            Assert.AreEqual(DS_NAME, ds.GetDataSourceName());
        }

        [Test]
        public void TestSelectDataSource()
        {
            //  ## Arrange ##
            const string DS_NAME = "DataSource1";
            const string SUB_NAME = "DataSource2";
            const string NO_NAME = "NotExist";

            //  データソースを二つ扱う関係上、別枠でConainerを初期化
            S2ContainerImpl container = new S2ContainerImpl();
            container.Register(typeof(TxDataSource), DS_NAME);
            container.Register(typeof(DataSourceImpl), SUB_NAME);

            SelectableDataSourceProxyWithContainer ds = new SelectableDataSourceProxyWithContainer();
            ds.Container = container;
            Assert.IsNotNull(ds.Container);

            {
                //  ## Act ##
                ds.SetDataSourceName(DS_NAME);
                //  ## Assert ##
                IDataSource actual = ds.GetDataSource();
                Assert.IsNotNull(actual, "1");
                Assert.IsTrue(actual is TxDataSource, "2");
                TxDataSource txDs = (TxDataSource)actual;
            }
            {
                //  ## Act ##
                ds.SetDataSourceName(SUB_NAME);
                //  ## Assert ##
                IDataSource actual = ds.GetDataSource();
                Assert.IsNotNull(actual, "11");
                Assert.IsTrue(actual is DataSourceImpl, "12");
                DataSourceImpl implDs = (DataSourceImpl)actual;
            }
            {
                //  ## Act ##
                ds.SetDataSourceName(NO_NAME);
                //  ## Assert##
                try
                {
                    ds.GetDataSource();
                    Assert.Fail("21");
                }
                catch (ComponentNotFoundRuntimeException)
                {
                }
            }
            container.Destroy();
        }
    }
}
