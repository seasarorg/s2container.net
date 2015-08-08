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

using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Seasar.Framework.Aop;
using Seasar.Framework.Util;
using Seasar.Quill;
using Seasar.Quill.Attrs;
using Seasar.Quill.Exception;

namespace Seasar.Tests.Quill
{
    [TestFixture]
	public class AspectBuilderTest : AspectBuilder
    {
        public AspectBuilderTest()
            : base(null)
        { }
        
        #region GetMethodInterceptorのテスト

        [Test]
        public void TestGetMethodInterceptorQuillIMethodInterceptorに代入できない場合()
        {
            container = new QuillContainer();

            try
            {
                GetMethodInterceptor(typeof(Hoge1));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0012", ex.MessageCode);
            }
        }

        [Test]
        public void TestGetMethodInterceptorQuill正常な場合()
        {
            container = new QuillContainer();

            var interceptor = 
                GetMethodInterceptor(typeof(HogeInterceptor));

            Assert.AreEqual(typeof(HogeInterceptor), interceptor.GetExType());
        }

        //[Test]
        //public void TestGetMethodInterceptor_S2_IMethodInterceptorに代入できない場合()
        //{
        //    S2ContainerImpl container = new S2ContainerImpl();
        //    ComponentDefImpl def = new ComponentDefImpl(typeof(Hoge1), "hoge");
        //    container.Register(def);
        //    SingletonS2ContainerFactory.container = container;

        //    this.container = new QuillContainer();

        //    try
        //    {
        //        this.GetMethodInterceptor("hoge");
        //        Assert.Fail();
        //    }
        //    catch (QuillApplicationException ex)
        //    {
        //        Assert.AreEqual("EQLL0012", ex.MessageCode);
        //    }
        //}

        //[Test]
        //public void TestGetMethodInterceptor_S2_正常な場合()
        //{
        //    S2ContainerImpl container = new S2ContainerImpl();
        //    ComponentDefImpl def = new ComponentDefImpl(typeof(HogeInterceptor), "hoge");
        //    container.Register(def);
        //    SingletonS2ContainerFactory.container = container;

        //    this.container = new QuillContainer();

        //    IMethodInterceptor interceptor = this.GetMethodInterceptor("hoge");

        //    Assert.AreEqual(typeof(HogeInterceptor), interceptor.GetExType());
        //}

        [Test]
        public void TestGetMethodInterceptorAspect属性にパラメータが設定されていない場合()
        {
            container = new QuillContainer();

            Type type = null;
            var attr = new AspectAttribute(type);

            try
            {
                GetMethodInterceptor(attr);

                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0013", ex.MessageCode);
            }
        }

        [Test]
        public void TestGetMethodInterceptorAspect属性にInterceptorTypeが設定されている場合()
        {
            container = new QuillContainer();

            var attr = new AspectAttribute(typeof(HogeInterceptor));

            var interceptor = GetMethodInterceptor(attr);

            Assert.IsTrue(
                typeof(HogeInterceptor).IsAssignableFrom(interceptor.GetExType()));
        }

        //[Test]
        //public void TestGetMethodInterceptor_Aspect属性にcomponentNameが設定されている場合()
        //{
        //    S2ContainerImpl container = new S2ContainerImpl();
        //    ComponentDefImpl def = new ComponentDefImpl(typeof(HogeInterceptor), "hoge");
        //    container.Register(def);
        //    SingletonS2ContainerFactory.container = container;

        //    this.container = new QuillContainer();

        //    AspectAttribute attr = new AspectAttribute("hoge");

        //    IMethodInterceptor interceptor = this.GetMethodInterceptor(attr);

        //    Assert.AreEqual(container.GetComponent("hoge"), interceptor);
        //}

        #endregion

        #region GetMethodInterceptorのテストで使用する内部クラス

        public class Hoge1
        {
        }

        public class HogeInterceptor : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                return 2;
            }
        }

        #endregion

        #region AddMethodNamesForPointcutのテスト

        [Test]
        public void TestAddMethodNamesForPointcut初めてInterceptorにpointcutが設定される場合()
        {
            container = new QuillContainer();

            var attr = new AspectAttribute(typeof(HogeInterceptor2));

            IDictionary<IMethodInterceptor, List<string>> methodNames =
                new Dictionary<IMethodInterceptor, List<string>>();

            AddMethodNamesForPointcut(methodNames, "Hoge", attr);

            Assert.AreEqual(1, methodNames.Count);
            Assert.AreEqual(1, methodNames[GetMethodInterceptor(attr)].Count);
            Assert.AreEqual("Hoge", methodNames[GetMethodInterceptor(attr)][0]);
        }

        [Test]
        public void TestAddMehtodNamesForPointcut既にInterceptorにpointcutが設定されている場合()
        {
            container = new QuillContainer();

            var attr = new AspectAttribute(typeof(HogeInterceptor2));

            IDictionary<IMethodInterceptor, List<string>> methodNames =
                new Dictionary<IMethodInterceptor, List<string>>();

            var interceptor = GetMethodInterceptor(attr);

            var nameList = new List<string>();
            nameList.Add("Hoge");

            methodNames[interceptor] = nameList;

            AddMethodNamesForPointcut(methodNames, "Hoge2", attr);

            Assert.AreEqual(1, methodNames.Count);
            Assert.AreEqual(2, methodNames[interceptor].Count);
            Assert.AreEqual("Hoge", methodNames[interceptor][0]);
            Assert.AreEqual("Hoge2", methodNames[interceptor][1]);
        }

        [Test]
        public void TestAddMethodNamesForPointcut()
        {
            container = new QuillContainer();

            var method = typeof(Hoge2).GetMethod("HogeHoge");

            IDictionary<IMethodInterceptor, List<string>> methodNames =
                new Dictionary<IMethodInterceptor, List<string>>();

            AddMethodNamesForPointcut(methodNames, method);

            var attr = (AspectAttribute)
                Attribute.GetCustomAttribute(method, typeof(AspectAttribute));

            Assert.AreEqual(1, methodNames.Count);
            Assert.AreEqual(1, methodNames[GetMethodInterceptor(attr)].Count);
            Assert.AreEqual("HogeHoge", methodNames[GetMethodInterceptor(attr)][0]);
        }

        #endregion

        #region AddMethodNamesForPointcutのテストで使用する内部クラス

        public class Hoge2
        {
            [Aspect(typeof(HogeInterceptor2))]
            public virtual int HogeHoge()
            {
                return 0;
            }
        }

        public class HogeInterceptor2 : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                return 2;
            }
        }

        #endregion

        #region CreateAspectのテスト

        [Test]
        public void TestCreateAspect全てのメソッドで有効なAspectを作成する場合()
        {
            container = new QuillContainer();
            var attr = new AspectAttribute(typeof(HogeInterceptor3));

            var aspect = CreateAspect(attr);

            var interceptor = (IMethodInterceptor) container.GetComponent(
                typeof(HogeInterceptor3)).GetComponentObject(typeof(HogeInterceptor3));

            Assert.AreEqual(interceptor, aspect.MethodInterceptor);
            Assert.IsNull(aspect.Pointcut);
        }

        [Test]
        public void TestCreateAspectメソッド名を指定して有効なAspectを作成する場合()
        {
            container = new QuillContainer();

            IMethodInterceptor interceptor = new HogeInterceptor3();

            var aspect = CreateAspect(
                interceptor, new[] { "Hoge1", "Hoge2" });

            Assert.AreEqual(interceptor, aspect.MethodInterceptor);
            Assert.IsTrue(aspect.Pointcut.IsApplied("Hoge1"));
            Assert.IsTrue(aspect.Pointcut.IsApplied("Hoge2"));
            Assert.IsFalse(aspect.Pointcut.IsApplied("Hoge3"));
        }

        #endregion

        #region CreateAspectのテストで使用する内部クラス

        public class HogeInterceptor3 : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                return null;
            }
        }

        #endregion

        #region CreateAspectListのテスト

        [Test]
        public void TestCreateAspectListAspectが適用されていない場合()
        {
            container = new QuillContainer();
            var methods = typeof(Hoge3).GetMethods();

            var list = CreateAspectList(methods);

            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void TestCreateAspectListAspectが適用される場合()
        {
            container = new QuillContainer();
            var methods = typeof(Hoge4).GetMethods();

            var list = CreateAspectList(methods);

            Assert.AreEqual(1, list.Count);

            var aspect = list[0];
            var interceptor = (IMethodInterceptor)container.GetComponent(
                typeof(HogeInterceptor4)).GetComponentObject(typeof(HogeInterceptor4));

            Assert.AreEqual(interceptor, aspect.MethodInterceptor);

            var pointcut = aspect.Pointcut;

            Assert.IsTrue(pointcut.IsApplied("Hoge1"));
            Assert.IsTrue(pointcut.IsApplied("Hoge2"));
            Assert.IsFalse(pointcut.IsApplied("Hoge3"));
        }

        #endregion

        #region CreateAspectListのテストで使用する内部クラス

        public class Hoge3
        {
            public void Hoge1()
            {
            }

            public void Hoge2()
            {
            }
        }

        public class Hoge4
        {
            [Aspect(typeof(HogeInterceptor4))]
            public virtual void Hoge1()
            {
            }

            [Aspect(typeof(HogeInterceptor4))]
            public virtual void Hoge2()
            {
            }

            public virtual void Hoge3()
            {
            }
        }

        public class HogeInterceptor4 : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                return null;
            }
        }

        #endregion

        #region CreateAspectsのテスト

        [Test]
        public void TestCreateAspectsAspectが適用されない場合()
        {
            container = new QuillContainer();
            var aspects = CreateAspects(typeof(Hoge5));
            
            Assert.AreEqual(0, aspects.Length);
        }

        [Test]
        public void TestCreateAspectsAspectが適用される場合()
        {
            container = new QuillContainer();
            var aspects = CreateAspects(typeof(Hoge6));

            Assert.AreEqual(2, aspects.Length);
        }

        #endregion

        #region CreateAspectsのテストで使用する内部クラス

        public class Hoge5
        {
            public virtual void Fuga1()
            {
            }

            public virtual void Fuga2()
            {
            }
        }

        [Aspect(typeof(HogeInterceptor5))]
        public class Hoge6
        {
            [Aspect(typeof(HogeInterceptor5))]
            public virtual void Fuga1()
            {
            }

            [Aspect(typeof(HogeInterceptor5))]
            public virtual void Fuga2()
            {
            }

            public virtual void Fuga3()
            {
            }
        }

        public class HogeInterceptor5 : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                return null;
            }
        }

        #endregion    
    }
}
