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

using System;
using MbUnit.Framework;
using Seasar.Quill;
using Seasar.Framework.Aop;
using Seasar.Quill.Attrs;

namespace Seasar.Tests.Quill
{
    [TestFixture]
	public class QuillContainerTest
    {
        #region GetComponentのテスト

        [Test]
        public void TestGetComponent_インターフェースにAspectが適用されていない場合()
        {
            QuillContainer container = new QuillContainer();

            try
            {
                container.GetComponent(typeof(Hoge1), typeof(Hoge1));
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
            QuillComponent component = container.GetComponent(typeof(Hoge2));
            QuillComponent component2 = container.GetComponent(typeof(Hoge2));
            Assert.AreEqual(typeof(Hoge2), component.ReceiptType);
            Assert.AreEqual(component.GetComponentObject(typeof(Hoge2)),
                component2.GetComponentObject(typeof(Hoge2)));
        }

        [Test]
        public void TestGetComponent_Destroy済みの場合()
        {
            QuillContainer container = new QuillContainer();
            container.GetComponent(typeof(Hoge2));

            container.Destroy();

            try
            {
                container.GetComponent(typeof(Hoge2));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0018", ex.MessageCode);
            }
        }

        #endregion

        #region GetComponentのテストで使用する内部クラス・インターフェース

        public interface Hoge1
        {
            void Fuga();
        }

        [Aspect(typeof(HogeInterceptor1))]
        public interface Hoge2
        {
            void Fuga();
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
    }
}
