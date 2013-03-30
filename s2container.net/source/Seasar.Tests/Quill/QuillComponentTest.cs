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
using System.Collections;
using MbUnit.Framework;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Quill;
using Seasar.Quill.Exception;
using Seasar.Quill.Attrs;
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

        [Test]
        public void TestGetComponentObject_Destroy済みの場合()
        {
            QuillComponent component = new QuillComponent(
                typeof(ArrayList), typeof(ArrayList), new IAspect[] { });

            component.Destroy();

            try
            {
                component.GetComponentObject(typeof(ArrayList));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0018", ex.MessageCode);
            }

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

        [Test]
        public void TestQuillComponent_コンストラクタで例外が発生するコンポーネントの場合()
        {
            try
            {
                QuillComponent component = new QuillComponent(
                    typeof(ExceptionHoge), typeof(ExceptionHoge), new IAspect[0]);
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0036", ex.MessageCode);
            }
        }

        [Test]
        public void TestQuillComponent_アスペクトがかけられないコンポーネントの場合()
        {
            try
            {
                QuillComponent component = new QuillComponent(
                    typeof(UnableCreateProxyHoge), typeof(UnableCreateProxyHoge), 
                    new IAspect[] { new AspectImpl(new TraceInterceptor())});
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0037", ex.MessageCode);
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

        public class ExceptionHoge
        {
            public ExceptionHoge()
            {
                throw new ApplicationException("テストのためにわざと発生させた例外");
            }
        }

        /// <summary>
        /// sealedクラス→プロキシオブジェクトを作れないはず
        /// </summary>
        public sealed class UnableCreateProxyHoge
        {
            public object Hoge()
            {
                return null;
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

        [Test]
        public void TestDispose_Destroy済みの場合()
        {
            Type type = typeof(DisposableClass);

            QuillComponent component = new QuillComponent(type, type, new IAspect[0]);

            component.Destroy();

            try
            {
                component.Dispose();
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

        #region Destoryのテスト

        [Test]
        public void TestDestroy()
        {
            QuillComponent component = new QuillComponent(
                typeof(ArrayList), typeof(ArrayList), new IAspect[] { });

            component.Destroy();

            Assert.IsNull(component.ComponentType);
            Assert.IsNull(component.ReceiptType);

            try
            {
                component.GetComponentObject(typeof(ArrayList));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0018", ex.MessageCode);
            }

            try
            {
                component.Dispose();
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0018", ex.MessageCode);
            }

            component.Destroy();
        }

        #endregion
    }
}
