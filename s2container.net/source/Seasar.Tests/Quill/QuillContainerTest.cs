#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using MbUnit.Framework;
using Seasar.Extension.ADO;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Aop;
using Seasar.Quill;
using Seasar.Quill.Attrs;
using Seasar.Quill.Database.DataSource.Impl;
using Seasar.Quill.Exception;
using Seasar.Quill.Unit;

namespace Seasar.Tests.Quill
{
    [TestFixture]
	public class QuillContainerTest : QuillTestCase
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region GetComponentのテスト

        [Test]
        public void TestGetComponent_インターフェースにAspectが適用されていない場合()
        {
            QuillContainer container = new QuillContainer();

            try
            {
                container.GetComponent(typeof(IHoge1), typeof(IHoge1));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0008", ex.MessageCode);
            }
        }

        [Test]
        public void TestGetComponent_正常な場合()
        {
            QuillContainer container = new QuillContainer();
            QuillComponent component = container.GetComponent(typeof(IHoge2));
            QuillComponent component2 = container.GetComponent(typeof(IHoge2));
            Assert.AreEqual(typeof(IHoge2), component.ReceiptType);
            Assert.AreEqual(component.GetComponentObject(typeof(IHoge2)),
                component2.GetComponentObject(typeof(IHoge2)));
        }

        [Test]
        public void TestGetComponent_Destroy済みの場合()
        {
            QuillContainer container = new QuillContainer();
            container.GetComponent(typeof(IHoge2));

            container.Destroy();

            try
            {
                container.GetComponent(typeof(IHoge2));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0018", ex.MessageCode);
            }
        }

        [Test]
        public void TestGetComponent_Interfaceで受け取る場合()
        {
            QuillContainer container = new QuillContainer();
            QuillComponent component1 = container.GetComponent(typeof(IHoge3), typeof(Hoge3));
            QuillComponent component2 = container.GetComponent(typeof(IHoge3), typeof(Hoge3));
            Assert.AreEqual(component1.GetComponentObject(typeof(IHoge3)),
                component2.GetComponentObject(typeof(IHoge3)));
        }

        #endregion

        #region GetComponentのテストで使用する内部クラス・インターフェース

        public interface IHoge1
        {
            void Fuga();
        }

        [Aspect(typeof(HogeInterceptor1))]
        public interface IHoge2
        {
            void Fuga();
        }

        [Implementation(typeof(Hoge3))]
        public interface IHoge3
        {
        }

        public class Hoge3 : IHoge3
        {
        }

        public class HogeInterceptor1 : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                return null;
            }
        }

        

        #endregion

        #region Disposeのテスト

        [Test]
        public void TestDispose()
        {
            QuillContainer container = new QuillContainer();
            QuillComponent component = 
                container.GetComponent(typeof(NotDisposableClass));
            QuillComponent component2 = container.GetComponent(typeof(DisposableClass));

            container.Dispose();

            DisposableClass disposable = 
                (DisposableClass) component2.GetComponentObject(typeof(DisposableClass));

            Assert.IsTrue(disposable.Disposed);
        }

        [Test]
        public void TestDispose_Destroy済みの場合()
        {
            QuillContainer container = new QuillContainer();
            container.GetComponent(typeof(DisposableClass));

            container.Destroy();

            try
            {
                container.GetComponent(typeof(DisposableClass));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0018", ex.MessageCode);
            }
        }

        #endregion

        #region Disposeのテストで使用する内部クラス

        public class NotDisposableClass
        {
        }

        public class DisposableClass : IDisposable
        {
            public bool Disposed = false;

            #region IDisposable メンバ

            public void Dispose()
            {
                Disposed = true;
            }

            #endregion
        }

        #endregion

        #region Destroyのテスト

        [Test]
        public void TestDestroy()
        {

            QuillContainer container = new QuillContainer();
            container.GetComponent(typeof(HogeDestroy));

            container.Destroy();

            try
            {
                container.GetComponent(typeof(HogeDestroy));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0018", ex.MessageCode);
            }

            try
            {
                container.Dispose();
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0018", ex.MessageCode);
            }

            container.Destroy();
        }

        #endregion

        #region Destroyのテストで使用する内部クラス

        [Aspect(typeof(HogeDestoryInterceptor))]
        public interface HogeDestroy
        {
            void Fuga();
        }

        public class HogeDestoryInterceptor : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                return null;
            }
        }

        #endregion 

        #region RegistDataSource データソース登録テスト

        /// <summary>
        /// データソース登録テスト
        /// </summary>
        /// <remarks>
        /// Quill設定が関わるテストに関してはQuillTestCaseを利用
        /// </remarks>
        [Test, Quill]
        public void TestRegistDataSource()
        {
            QuillContainer container = new QuillContainer();

            QuillComponent qc = container.GetComponent(typeof(SelectableDataSourceProxyWithDictionary));
            Assert.AreEqual(typeof(SelectableDataSourceProxyWithDictionary), qc.ComponentType, "1");
            SelectableDataSourceProxyWithDictionary ds = (SelectableDataSourceProxyWithDictionary)qc.GetComponentObject(
                typeof(SelectableDataSourceProxyWithDictionary));
            Assert.IsNotNull(ds, "2");
            Assert.GreaterEqualThan(ds.DataSourceCollection.Count, 7);

            foreach (string key in ds.DataSourceCollection.Keys)
            {
                IDataSource part = ds.DataSourceCollection[key];
                if (part is TxDataSource)
                {
                    //  全てのTxDataSource系のデータソースに
                    //  TransactionContextが設定されているか確認
                    Assert.IsNotNull(((TxDataSource)part).Context,
                        "全てのTxDataSource系のデータソースにTransactionContextが設定されていない");
                }
            }
        }

        #endregion
    }
}
