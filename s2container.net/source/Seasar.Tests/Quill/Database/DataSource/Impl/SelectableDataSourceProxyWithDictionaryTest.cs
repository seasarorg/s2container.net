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

using MbUnit.Framework;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Container;
using Seasar.Quill.Database.DataSource.Impl;
using System.Threading;
using System;

namespace Seasar.Tests.Quill.Database.DataSource.Impl
{
    [TestFixture]
    public class SelectableDataSourceProxyWithDictionaryTest
    {
        [Test]
        public void TestSetAndGetDataSourceName()
        {
            //  ## Arrange ##
            const string DS_NAME = "HOGE_DATASOURCE";
            SelectableDataSourceProxyWithDictionary ds = new SelectableDataSourceProxyWithDictionary();
            
            //  ## Act ##
            ds.SetDataSourceName(DS_NAME);

            //  ## Assert ##
            Assert.AreEqual(DS_NAME, ds.GetDataSourceName());
        }

        [Test]
        public void TestSelectDataSource()
        {
            //  ## Arrange ##
            const string DS_NAME = "provider";
            const string SUB_NAME = "providerEx";
            const string NO_NAME = "NotExist";

            SelectableDataSourceProxyWithDictionary ds = new SelectableDataSourceProxyWithDictionary();
            ds.RegistDataSource(DS_NAME, new DataSourceImpl1(DS_NAME));
            ds.RegistDataSource(SUB_NAME, new TxDataSource(SUB_NAME));
            Assert.AreEqual(2, ds.DataSourceCollection.Count, "0");

            {
                //  ## Act ##
                ds.SetDataSourceName(DS_NAME);
                //  ## Assert ##
                IDataSource actual = ds.GetDataSource();
                Assert.IsNotNull(actual, "1");
                Assert.IsTrue(actual is DataSourceImpl1, "2");
            }
            {
                //  ## Act ##
                ds.SetDataSourceName(SUB_NAME);
                //  ## Assert ##
                IDataSource actual = ds.GetDataSource();
                Assert.IsNotNull(actual, "11");
                Assert.IsTrue(actual is TxDataSource, "12");

                TransactionContext1 txc = new TransactionContext1();
                ds.SetTransactionContext(txc);

                Assert.IsTrue(((TxDataSource)actual).Context is TransactionContext1, "13");
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
        }

        /// <summary>
        /// 別スレッドで設定したデータソース名を参照できるかテスト
        /// </summary>
        [Test]
        public void TestMultiThread()
        {
            Thread t1 = new Thread(Invoke_SetDSName);
            Thread t2 = new Thread(Invoke_GetDSName);

            SelectableDataSourceProxyWithDictionary ds = new SelectableDataSourceProxyWithDictionary();
            t1.Start(ds);
            t1.Join();
            t2.Start(ds);
            t2.Join();
            Assert.IsFalse(string.IsNullOrEmpty(ds.GetDataSourceName()));
        }

        #region マルチスレッド実行用デリゲートメソッド

        private void Invoke_SetDSName(object ds)
        {
            SelectableDataSourceProxyWithDictionary proxy = (SelectableDataSourceProxyWithDictionary)ds;
            proxy.SetDataSourceName("Hoge");
            Console.WriteLine("dataSourceName={0}, ThreadId={1}",
                proxy.GetDataSourceName(), Thread.CurrentThread.ManagedThreadId);
        }

        private void Invoke_GetDSName(object ds)
        {
            SelectableDataSourceProxyWithDictionary proxy = (SelectableDataSourceProxyWithDictionary)ds;
            Console.WriteLine("dataSourceName={0}, ThreadId={1}",
                proxy.GetDataSourceName(), Thread.CurrentThread.ManagedThreadId);
        }

        #endregion

        #region テスト用IDataSource実装クラス

        public class DataSourceImpl1 : DataSourceImpl
        {
            public DataSourceImpl1(string providerInvariantName)
                : base(providerInvariantName)
            {
            }
        }

        public class TransactionContext1 : TransactionContext
        {
        }

        #endregion
    }
}
