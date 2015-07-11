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

using MbUnit.Framework;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Container.Impl;
using Seasar.Quill;
using Seasar.Quill.Exception;

namespace Seasar.Tests.Quill
{
    [TestFixture]
	public class SingletonS2ContainerConnectorTest
    {
        #region GetComponentのテスト

        [Test]
        public void TestGetComponent_S2Containerが作成されていない場合()
        {
            SingletonS2ContainerFactory.Container = null;

            try
            {
                SingletonS2ContainerConnector.GetComponent("hoge", typeof(string));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0009", ex.MessageCode);
            }
        }

        [Test]
        public void TestGetComponent_コンポーネントが登録されていない場合()
        {
            S2ContainerImpl container = new S2ContainerImpl();
            SingletonS2ContainerFactory.Container = container;

            try
            {
                SingletonS2ContainerConnector.GetComponent("hoge", typeof(string));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0010", ex.MessageCode);
            }
            finally
            {
                SingletonS2ContainerFactory.Container = null;
            }
        }

        [Test]
        public void TestGetComponent_コンポーネントの取得でS2Containerから例外がスローされた場合()
        {
            S2ContainerImpl container = new S2ContainerImpl();
            SingletonS2ContainerFactory.Container = container;
            ComponentDefImpl def = new ComponentDefImpl(typeof(Hoge), "hoge");
            PropertyDefImpl propertyDef = new PropertyDefImpl("HogeHoge", "test");
            def.AddPropertyDef(propertyDef);
            container.Register(def);

            try
            {
                SingletonS2ContainerConnector.GetComponent("hoge", typeof(string));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0011", ex.MessageCode);
            }
            finally
            {
                SingletonS2ContainerFactory.Container = null;
            }
        }

        [Test]
        public void TestGetComponent_受け側の型を指定しないでコンポーネントを受け取る場合1()
        {
            S2ContainerImpl container = new S2ContainerImpl();
            SingletonS2ContainerFactory.Container = container;
            ComponentDefImpl def = new ComponentDefImpl(typeof(Hoge), "hoge");
            container.Register(def);

            Hoge hoge = (Hoge) SingletonS2ContainerConnector.GetComponent("hoge", null);
            Assert.IsNotNull(hoge);
            SingletonS2ContainerFactory.Container = null;
        }

        [Test]
        public void TestGetComponent_受け側の型を指定しないでコンポーネントを受け取る場合2()
        {
            S2ContainerImpl container = new S2ContainerImpl();
            SingletonS2ContainerFactory.Container = container;
            ComponentDefImpl def = new ComponentDefImpl(typeof(Hoge), "hoge");
            container.Register(def);

            Hoge hoge = (Hoge)SingletonS2ContainerConnector.GetComponent("hoge");
            Assert.IsNotNull(hoge);
            SingletonS2ContainerFactory.Container = null;
        }

        [Test]
        public void TestGetComponent_受け側の型を指定してコンポーネントを受け取る場合()
        {
            S2ContainerImpl container = new S2ContainerImpl();
            SingletonS2ContainerFactory.Container = container;
            ComponentDefImpl def = new ComponentDefImpl(typeof(Hoge), "hoge");
            container.Register(def);

            IHoge hoge = (IHoge)SingletonS2ContainerConnector.GetComponent(
                "hoge", typeof(IHoge));
            Assert.IsNotNull(hoge);
            SingletonS2ContainerFactory.Container = null;
        }

        #endregion

        #region GetComponentのテストで使用する内部クラス

        private class Hoge : IHoge
        {
            public int HogeHoge
            {
                get { return 0; }
            }
        }

        private interface IHoge
        {
        }

        #endregion
    }
}
