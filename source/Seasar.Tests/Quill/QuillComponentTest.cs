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
using MbUnit.Framework;
using Seasar.Quill;
using Seasar.Framework.Aop;
using System.Collections;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Interceptors;

namespace Seasar.Tests.Quill
{
    [TestFixture]
	public class QuillComponentTest
    {
        #region GetComponentObjectのテスト

        [Test]
        public void TestGetComponentObject_コンポーネントのオブジェクトが存在する場合()
        {
            QuillComponent component = new QuillComponent(
                typeof(ArrayList), typeof(ArrayList), new IAspect[] { });

            object ret = component.GetComponentObject(typeof(ArrayList));

            Assert.IsNotNull(ret);
        }

        [Test]
        public void TestGetComponentObject_コンポーネントのオブジェクトが存在しない場合()
        {
            QuillComponent component = new QuillComponent(
                typeof(ArrayList), typeof(ArrayList), new IAspect[] { });

            object ret = component.GetComponentObject(typeof(string));

            Assert.IsNull(ret);
        }

        #endregion

        #region QuillComponentのテスト

        [Test]
        public void TestQuillComponent_Aspectが適用されている場合()
        {
            IAspect aspect = new AspectImpl(new HogeInterceptor());

            QuillComponent component = new QuillComponent(
                typeof(Hoge1), typeof(Hoge1), new IAspect[] { aspect });

            Hoge1 hoge = (Hoge1)component.GetComponentObject(typeof(Hoge1));

            Assert.AreEqual(2, hoge.HogeHoge());
        }

        [Test]
        public void TestQuillComponent_Aspectが適用されており受け側の型が異なる場合()
        {
            IAspect aspect = new AspectImpl(new HogeInterceptor());

            QuillComponent component = new QuillComponent(
                typeof(Hoge1), typeof(IHoge1), new IAspect[] { aspect });

            IHoge1 hoge = (IHoge1)component.GetComponentObject(typeof(IHoge1));

            Assert.AreEqual(2, hoge.HogeHoge());
        }

        [Test]
        public void TestQuillComponent_Aspectが適用されていない場合()
        {
            QuillComponent component = new QuillComponent(
                typeof(Hoge1), typeof(Hoge1), new IAspect[0]);

            Hoge1 hoge = (Hoge1)component.GetComponentObject(typeof(Hoge1));

            Assert.AreEqual(1, hoge.HogeHoge());
        }

        [Test]
        public void TestQuillComponent_Aspectを適用でパラメータ無しのコンストラクタがない場合()
        {
            try
            {
                QuillComponent component = new QuillComponent(
                    typeof(Hoge2), typeof(Hoge2), new IAspect[0]);

                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0017", ex.MessageCode);
            }
        }

        [Test]
        public void TestQuillComponent_Aspectを適用でpublicなコンストラクタがない場合()
        {
            try
            {
                QuillComponent component = new QuillComponent(
                    typeof(Hoge3), typeof(Hoge3), new IAspect[0]);

                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0017", ex.MessageCode);
            }
        }

        #endregion

        #region QuillComponentのテストで使用する内部クラス

        public class Hoge1 : IHoge1
        {
            public virtual int HogeHoge()
            {
                return 1;
            }
        }

        public interface IHoge1
        {
            int HogeHoge();
        }

        private class HogeInterceptor : IMethodInterceptor
        {
            
            public object Invoke(IMethodInvocation invocation)
            {
                return 2;
            }
        }

        public class Hoge2
        {
            public Hoge2(string param)
            {
            }
        }

        public class Hoge3
        {
            private Hoge3()
            {
            }
        }

        #endregion

        #region Disposeのテスト

        [Test]
        public void TestDispose_コンポーネントがIDisposableを実装している場合()
        {
            Type type = typeof(DisposableClass);

            QuillComponent component = new QuillComponent(type, type, new IAspect[0]);

            component.Dispose();

            DisposableClass disposable =
                (DisposableClass)component.GetComponentObject(type);

            Assert.IsTrue(disposable.Disposed);
        }

        [Test]
        public void TestDispose_コンポーネントがIDisposableを実装していない場合()
        {
            Type type = typeof(NotDisposableClass);

            QuillComponent component = new QuillComponent(type, type, new IAspect[0]);

            component.Dispose();
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
    }
}
