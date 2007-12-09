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
using Seasar.Quill.Unit;
using Seasar.Quill;
using Seasar.Quill.Attrs;
using System.Reflection;

namespace Seasar.Tests.Quill.Unit
{
    [TestFixture]
    public class MockInjectorTest : MockInjector
    {
        public MockInjectorTest()
            : base()
        {
        }

        public override void Dispose()
        {
            // MbUnitがDisposeを呼び出すのでDisposeをoverrideしておく
        }

        #region GetInstanceのテスト

        [Test]
        public void TestGetInstance()
        {
            container = new QuillContainer();
            MockInjector injector1 = MockInjector.GetInstance();
            MockInjector injector2 = MockInjector.GetInstance();

            Assert.AreSame(injector1, injector2);
        }

        [Test]
        public void TestGetInstance_Destroy済みの場合()
        {
            MockInjector injector1 = MockInjector.GetInstance();
            MockInjector.GetInstance().Destroy();
            MockInjector injector2 = MockInjector.GetInstance();

            Assert.IsNotNull(injector2);
            Assert.AreNotSame(injector1, injector2);
        }

        #endregion

        #region Destroyのテスト

        [Test]
        public void TestDestroy()
        {
            container = new QuillContainer();
            Destroy();

            try
            {
                Inject(new DestroyHoge());
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0018", ex.MessageCode);
            }

            Destroy();
        }

        #endregion

        #region Destroyのテストで使用する内部クラス

        private class DestroyHoge
        {
        }

        #endregion

        #region InjectFieldのテスト

        [Test]
        public void TestInjectField_Mock属性が設定されている場合()
        {
            container = new QuillContainer();
            FieldInfo field = typeof(Target1).GetField("Hoge1");
            Target1 target = new Target1();

            this.InjectField(target, field);

            Assert.AreEqual("HogeMock", target.Hoge1.HogeHoge());
        }

        [Test]
        public void TestInjectField_Mock属性が指定されていない場合()
        {
            container = new QuillContainer();
            FieldInfo field = typeof(Target2).GetField("Hoge2");
            Target2 target = new Target2();

            this.InjectField(target, field);

            Assert.AreEqual("HogeHoge", target.Hoge2.HogeHoge());
        }

        #endregion

        #region InjectFieldのテストで使用する内部クラス

        public class Target1
        {
            public IHoge1 Hoge1;
        }

        [Mock(typeof(HogeMock1))]
        public interface IHoge1
        {
            string HogeHoge();
        }

        public class Hoge1 : IHoge1
        {
            #region IHoge1 メンバ

            public string HogeHoge()
            {
                return "HogeHoge";
            }

            #endregion
        }

        public class HogeMock1 : IHoge1

        {
            #region IHoge1 メンバ

            public string HogeHoge()
            {
                return "HogeMock";
            }

            #endregion
        }

        public class Target2
        {
            public IHoge2 Hoge2;
        }

        [Implementation(typeof(Hoge2))]
        public interface IHoge2
        {
            string HogeHoge();
        }

        public class Hoge2 : IHoge2
        {
            #region IHoge1 メンバ

            public string HogeHoge()
            {
                return "HogeHoge";
            }

            #endregion
        }

        public class HogeMock2 : IHoge2
        {
            #region IHoge1 メンバ

            public string HogeHoge()
            {
                return "HogeMock";
            }

            #endregion
        }

        #endregion
    }
}
