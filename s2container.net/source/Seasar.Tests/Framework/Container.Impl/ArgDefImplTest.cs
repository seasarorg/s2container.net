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
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;

namespace Seasar.Tests.Framework.Container.Impl
{
    [TestFixture]
    public class ArgDefImplTest
    {
        [Test]
        public void TestSetExpression()
        {
            IS2Container container = new S2ContainerImpl();
            var cd = new ComponentDefImpl(typeof(A));
            IArgDef ad = new ArgDefImpl();
            ad.Expression = "hoge";
            cd.AddArgDef(ad);
            container.Register(cd);
            var cd2 = new ComponentDefImpl(typeof(B), "hoge");
            container.Register(cd2);
            container.Register(typeof(C));
            var a = (A) container.GetComponent(typeof(A));
            Assert.AreEqual("B", a.HogeName);
        }

        [Test]
        public void TestSetChildComponentDef()
        {
            IS2Container container = new S2ContainerImpl();
            var cd = new ComponentDefImpl(typeof(A));
            IArgDef ad = new ArgDefImpl();
            var cd2 = new ComponentDefImpl(typeof(B));
            ad.ChildComponentDef = cd2;
            cd.AddArgDef(ad);
            container.Register(cd);
            container.Register(typeof(C));
            var a = (A) container.GetComponent(typeof(A));
            Assert.AreEqual("B", a.HogeName);
        }

        [Test]
        public void TestPrototype()
        {
            IS2Container container = new S2ContainerImpl();
            var cd = new ComponentDefImpl(typeof(StrHolderImpl), "foo");
            cd.InstanceMode = "prototype";
            var cd2 = new ComponentDefImpl(typeof(StrFacadeImpl));
            cd2.InstanceMode = "prototype";
            IArgDef ad = new ArgDefImpl();
            ad.Expression = "foo";
            cd2.AddArgDef(ad);
            container.Register(cd);
            container.Register(cd2);
            var facade1 = (IStrFacade) container.GetComponent(typeof(IStrFacade));
            var facade2 = (IStrFacade) container.GetComponent(typeof(IStrFacade));
            facade1.Str = "aaa";
            facade2.Str = "bbb";
            Assert.AreEqual("aaa", facade1.Str);
            Assert.AreEqual("bbb", facade2.Str);
        }

        [Test]
        public void TestValueEnumType()
        {
            IS2Container container = new S2ContainerImpl();
            var cd = new ComponentDefImpl(typeof(A));
            IArgDef ad = new ArgDefImpl();
            ad.Expression = "System.Data.DbType.String";
            cd.AddArgDef(ad);
            container.Register(cd);
            Assert.AreEqual(System.Data.DbType.String, ad.Value);
            ad.Expression = "System.Data.DbType.Decimal";
            Assert.AreEqual(System.Data.DbType.Decimal, ad.Value);
        }

        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestValueEnumTypeInvalidExpression()
        {
            // 列挙定数の名前の解析失敗時、
            // Seasar.Framework.Exceptions.JScriptEvaluateRuntimeException
            // を返すようにしたほうがいいかも
            IS2Container container = new S2ContainerImpl();
            var cd = new ComponentDefImpl(typeof(A));
            IArgDef ad = new ArgDefImpl();
            ad.Expression = "System.Data.DbType.Zzz";
            cd.AddArgDef(ad);
            container.Register(cd);
            var value = ad.Value;
        }

        [Test]
        public void TestValueType()
        {
            IS2Container container = new S2ContainerImpl();
            var cd = new ComponentDefImpl(typeof(A));
            IArgDef ad = new ArgDefImpl();
            ad.Expression = "System.Data.UniqueConstraint";
            cd.AddArgDef(ad);
            container.Register(cd);
            Assert.AreSame(typeof(System.Data.UniqueConstraint), ad.Value);
        }

        [Test]
        [ExpectedException(typeof(Seasar.Framework.Exceptions.JScriptEvaluateRuntimeException))]
        public void TestValueTypeInvalidExpression()
        {
            IS2Container container = new S2ContainerImpl();
            var cd = new ComponentDefImpl(typeof(A));
            IArgDef ad = new ArgDefImpl();
            ad.Expression = "System.Data.Zzz";
            cd.AddArgDef(ad);
            container.Register(cd);
            var value = ad.Value;
        }

        public class A
        {
            private readonly IHoge _hoge;

            public A(IHoge hoge)
            {
                _hoge = hoge;
            }

            public string HogeName
            {
                get { return _hoge.Name; }
            }
        }

        public class A2
        {
            private IHoge _hoge;

            public IHoge Hoge
            {
                set { _hoge = value; }
            }

            public string HogeName
            {
                get { return _hoge.Name; }
            }
        }

        public interface IHoge
        {
            string Name { get; }
        }

        public class B : IHoge
        {
            public string Name
            {
                get { return "B"; }
            }
        }

        public class C : IHoge
        {
            private A2 _a2;

            public A2 A2
            {
                set { _a2 = value; }
            }

            public string Name
            {
                get { return "C"; }
            }

            public string HogeName
            {
                get { return _a2.HogeName; }
            }
        }

        public interface IStrHolder
        {
            string Str { set; get; }
        }

        public class StrHolderImpl : IStrHolder
        {
            public string Str { set; get; }
        }

        public interface IStrFacade
        {
            string Str { set; get; }
        }

        public class StrFacadeImpl : IStrFacade
        {
            private readonly IStrHolder _strHolder;

            public StrFacadeImpl(IStrHolder strHolder)
            {
                _strHolder = strHolder;
            }

            public string Str
            {
                set { _strHolder.Str = value; }
                get { return _strHolder.Str; }
            }
        }
    }
}
