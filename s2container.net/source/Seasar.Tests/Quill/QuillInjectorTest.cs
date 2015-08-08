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
using System.Reflection;
using System.Threading;
using MbUnit.Framework;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Quill;
using Seasar.Quill.Attrs;
using Seasar.Quill.Exception;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Util;

namespace Seasar.Tests.Quill
{
    [TestFixture]
	public class QuillInjectorTest : QuillInjector
    {
        public QuillInjectorTest()
            : base()
        {
        }

        public override void Dispose()
        {
            // MbUnitがDisposeを呼び出すのでDisposeをoverrideしておく
        }

        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
            //  MbUnitはテスト間でstaticな変数を保持してしまうようなので
            //  毎回InjectionMapをクリアしておく
            InjectionMap.GetInstance().Clear();
            //  InjectionMapは毎回クリアしておく
            this.InjectionMap = null;

        }

        #region GetInstanceのテスト

        [Test]
        public void TestGetInstance()
        {
            container = new QuillContainer();
            var injector1 = QuillInjector.GetInstance();
            var injector2 = QuillInjector.GetInstance();

            Assert.AreSame(injector1, injector2);
            Assert.IsNotNull(injector1.Container);
        }

        [Test]
        public void TestGetInstanceDestroy済みの場合()
        {
            var injector1 = QuillInjector.GetInstance();
            QuillInjector.GetInstance().Destroy();
            var injector2 = QuillInjector.GetInstance();

            Assert.IsNotNull(injector2);
            Assert.AreNotSame(injector1, injector2);
        }

        #endregion

        #region InjectFieldのテスト

        [Test]
        public void TestGetFields()
        {
            var actual = new BaseClass4GetFieldTest();
            QuillInjector.GetInstance().Inject(actual);

            Assert.IsNotNull(actual.IF, "TestGetFields01");
            Assert.AreEqual(Impl4GetFieldTest.EXPECT_VALUE, actual.IF.TargetMethod(), "TestGetFields02");

            Assert.IsNotNull(actual.IF.GetEntity(), "TestGetFields11");
            Assert.AreEqual(Hoge1.EXPECT_VALUE, actual.IF.GetEntity().AspectedMethod(), "TestGetFields12");
        }

        [Test]
        public void TestInjectFieldQuill型がクラスの場合()
        {
            container = new QuillContainer();
            var attr = new ImplementationAttribute();
            var field = typeof(Target1).GetField("Hoge1");
            var target = new Target1();

            this.InjectField(target, field, attr);

            Assert.IsNotNull(target.Hoge1);
        }

        [Test]
        public void TestInjectFieldQuill型がインターフェースで実装クラスが指定されていない場合()
        {
            container = new QuillContainer();
            var attr = new ImplementationAttribute();
            var field = typeof(Target2).GetField("Hoge2");
            var target = new Target2();

            this.InjectField(target, field, attr);

            Assert.IsNotNull(target.Hoge2);
        }

        [Test]
        public void TestInjectFieldQuill型がインターフェースで実装クラスが指定されている場合()
        {
            container = new QuillContainer();
            var attr = new ImplementationAttribute(typeof(Hoge3));
            var field = typeof(Target3).GetField("Hoge3");
            var target = new Target3();

            this.InjectField(target, field, attr);
            Console.WriteLine(target.Hoge3.GetEmpName(0));
            Assert.IsNotNull(target.Hoge3);
        }

        [Test]
        public void TestInjectFieldType()
        {
            container = new QuillContainer();
            var field = typeof(Target3).GetField("Hoge3");
            var target = new Target3();

            this.InjectField(target, field, typeof(Hoge3));

            Assert.IsNotNull(target.Hoge3);
        }

        [Test]
        public void TestInjectFieldS2代入不可能な場合()
        {
            container = new QuillContainer();
            IS2Container s2Container = new S2ContainerImpl();
            IComponentDef def = new ComponentDefImpl(typeof(Hoge1), "hoge3");
            s2Container.Register(def);
            SingletonS2ContainerFactory.Container = s2Container;

            var attr = new BindingAttribute("hoge3");
            var target = new Target4();
            var field = typeof(Target4).GetField("Hoge3");

            try
            {
                this.InjectField(target, field, attr);
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0014", ex.MessageCode);
            }
        }

        [Test]
        public void TestInjectFieldS2正常な場合()
        {
            container = new QuillContainer();
            IS2Container s2Container = new S2ContainerImpl();
            IComponentDef def = new ComponentDefImpl(typeof(Hoge3), "hoge3");
            s2Container.Register(def);
            SingletonS2ContainerFactory.Container = s2Container;

            var attr = new BindingAttribute("hoge3");
            var target = new Target5();
            var field = typeof(Target5).GetField("Hoge3");

            this.InjectField(target, field, attr);

            Assert.AreSame(s2Container.GetComponent("hoge3"), target.Hoge3);
        }

        [Test]
        public void TestInjectFieldBinding属性が設定されている場合()
        {
            container = new QuillContainer();
            IS2Container s2Container = new S2ContainerImpl();
            IComponentDef def = new ComponentDefImpl(typeof(Hoge3), "hoge3");
            s2Container.Register(def);
            SingletonS2ContainerFactory.Container = s2Container;

            var target = new Target5();
            var field = typeof(Target5).GetField("Hoge3");

            this.InjectField(target, field);

            Assert.AreSame(s2Container.GetComponent("hoge3"), target.Hoge3);
        }

        [Test]
        public void TestInjectFieldImplementation属性が設定されている場合()
        {
            container = new QuillContainer();
            var field = typeof(Target3).GetField("Hoge3");
            var target = new Target3();

            this.InjectField(target, field);

            Assert.IsNotNull(target.Hoge3);
        }

        #endregion

        #region InjectFiledのテストで使用する内部クラス

        public class Target1
        {
            public Hoge1 Hoge1;
        }

        [Implementation()]
        public class Hoge1
        {
            public const string EXPECT_VALUE = "Hoge1EXPECT_VALUE";
            public string GetResult()
            {
                return EXPECT_VALUE;
            }
        }

        public class Target2
        {
            public IHoge2 Hoge2;
        }

        [Implementation()]
        [Aspect(typeof(TraceInterceptor))]
        public interface IHoge2
        {
        }

        public class Target3
        {

            public IHoge3 Hoge3;
        }

        [Implementation(typeof(Hoge3))]
        public interface IHoge3
        {
            string GetEmpName(int empNo);
        }

        public class Hoge3 : IHoge3
        {
            [Aspect(typeof(TraceInterceptor))]
            public virtual string GetEmpName(int empNo)
            {
                return "targetHoge3";
            }
        }

        public class Target4
        {
            [Binding("hoge3")]
            public IHoge3 Hoge3;
        }

        public class Target5
        {
            [Binding("hoge3")]
            public IHoge3 Hoge3;
        }


        #endregion

        #region Injectのテスト

        [Test]
        public void TestInject()
        {
            container = new QuillContainer();
            var target = new Target10();
            Inject(target);

            Assert.IsNotNull(target.Hoge10, "1");
            Assert.IsNotNull(target.Hoge11, "2");
            Assert.IsNotNull(target.Hoge12, "3");
        }

        [Test]
        public void TestInjectDestroy済みの場合()
        {
            container = new QuillContainer();
            var target = new Target10();
            Destroy();
            try
            {
                Inject(target);
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0018", ex.MessageCode);
            }
        }

        /// <summary>
        /// 相互参照がある型のオブジェクトに対してInjectを行った場合のテスト
        /// </summary>
        [Test]
        public void TestInjectEachInjection()
        {
            var actual = new EachReferenceA();
            Assert.IsNull(actual.B);

            GetInstance().Inject(actual);

            Assert.IsNotNull(actual.B);
            Assert.IsNotNull(actual.B.A);
        }

        /// <summary>
        /// 循環参照がある型のオブジェクトに対してInjectを行った場合のテスト
        /// </summary>
        [Test]
        public void TestInjectRoopInjection()
        {
            var actual = new RoopReferenceA();
            Assert.IsNull(actual.B);
            Assert.IsNull(actual.C);

            GetInstance().Inject(actual);

            Assert.IsNotNull(actual.B);
            Assert.IsNotNull(actual.B.A);
            Assert.IsNotNull(actual.B.C);
            Assert.IsNotNull(actual.C);
            Assert.IsNotNull(actual.C.A);
            Assert.IsNotNull(actual.C.B);
        }

        [Test]
        public void TestInjectTwoTimesInjectionNormal()
        {
            var injector = QuillInjector.GetInstance();
            {
                var actual = new Target10();
                Assert.IsNull(actual.Hoge11);
                Assert.IsNull(actual.Hoge12);
                injector.Inject(actual);
                Assert.IsNotNull(actual.Hoge11);
                Assert.IsNotNull(actual.Hoge12);
            }
            {
                var actual = new Target10();
                Assert.IsNull(actual.Hoge11);
                Assert.IsNull(actual.Hoge12);
                injector.Inject(actual);
                Assert.IsNotNull(actual.Hoge11);
                Assert.IsNotNull(actual.Hoge12);
            }
        }

        [Test]
        public void TestInjectTwoTimesInjectionEachReference()
        {
            var injector = QuillInjector.GetInstance();
            {
                var actual = new EachReferenceA();
                Assert.IsNull(actual.B);
                injector.Inject(actual);
                Assert.IsNotNull(actual.B);
            }
            {
                var actual = new EachReferenceA();
                Assert.IsNull(actual.B);
                injector.Inject(actual);
                Assert.IsNotNull(actual.B);
            }
        }

        [Test]
        public void TestInjectTwoTimesInjectionLoopReference()
        {
            var injector = QuillInjector.GetInstance();
            {
                var actual = new LoopReferenceC();
                Assert.IsNull(actual.A);
                Assert.IsNull(actual.B);
                injector.Inject(actual);
                Assert.IsNotNull(actual.A);
                Assert.IsNotNull(actual.B);
            }
            {
                var actual = new LoopReferenceC();
                Assert.IsNull(actual.A);
                Assert.IsNull(actual.B);
                injector.Inject(actual);
                Assert.IsNotNull(actual.A);
                Assert.IsNotNull(actual.B);
            }
        }

        [Test]
        public void TestInjectマルチスレッドでインジェクション()
        {
            var injector = QuillInjector.GetInstance();
            var actual1 = new Target10();
            Assert.IsNull(actual1.Hoge11);
            Assert.IsNull(actual1.Hoge12);
            var t1 = new Thread(ExecuteMultiThreadInjection);

            var actual2 = new Target10();
            Assert.IsNull(actual2.Hoge11);
            Assert.IsNull(actual2.Hoge12);
            var t2 = new Thread(ExecuteMultiThreadInjection);

            var parameter1 = new object[] { injector, actual1 };
            var parameter2 = new object[] { injector, actual2 };
            t1.Start(parameter1);
            t2.Start(parameter2);

            t1.Join();
            t2.Join();

            Assert.IsNotNull(actual1.Hoge11, "1");
            Assert.IsNotNull(actual1.Hoge12, "2");

            Assert.IsNotNull(actual2.Hoge11, "3");
            Assert.IsNotNull(actual2.Hoge12, "4");
        }

        #region マルチスレッドでインジェクション用メソッド

        /// <summary>
        /// マルチスレッド実行用インジェクション呼び出し
        /// </summary>
        /// <param name="parameter"></param>
        public void ExecuteMultiThreadInjection(object parameter)
        {
            var parameters = parameter as object[];
            if (parameters == null)
            {
                return;
            }

            var injector = parameters[0] as QuillInjector;
            if (injector == null)
            {
                return;
            }

            var target = parameters[1] as Target10;
            if (target == null)
            {
                return;
            }

            injector.Inject(target);
        }

        #endregion

        #endregion

        #region Injectのテストで使用する内部クラス

        public class Target10
        {
            public IHoge10 Hoge10;
            protected Hoge11 hoge11;

            public Hoge11 Hoge11
            {
                get { return hoge11; }
            }

            public Hoge12 Hoge12 { get; } = null;
        }

        [Implementation(typeof(Hoge10))]
        public interface IHoge10
        {
        }

        public class Hoge10 : IHoge10
        {
        }

        [Implementation()]
        public class Hoge11
        {
        }

        [Implementation()]
        public class Hoge12
        {
        }

        #region TestInject_RoopInjectionで使う内部クラス

        /// <summary>
        /// 相互参照クラスA
        /// </summary>
        [Implementation]
        public class EachReferenceA
        {
            public EachReferenceB B { get; set; }
        }

        /// <summary>
        /// 相互参照クラスB
        /// </summary>
        [Implementation]
        public class EachReferenceB
        {
            public EachReferenceA A { get; set; }
        }

        /// <summary>
        /// 循環参照クラスA
        /// </summary>
        [Implementation]
        public class RoopReferenceA
        {
            public RoopReferenceB B { get; set; }

            public LoopReferenceC C { get; set; }
        }

        /// <summary>
        /// 循環参照クラスB
        /// </summary>
        [Implementation]
        public class RoopReferenceB
        {
            public EachReferenceA A { get; set; }

            public LoopReferenceC C { get; set; }
        }

        /// <summary>
        /// 循環参照クラスC
        /// </summary>
        [Implementation]
        public class LoopReferenceC
        {
            public EachReferenceA A { get; set; }

            public RoopReferenceB B { get; set; }
        }

        #endregion

        #endregion

        #region InjectionMap（プロパティ）のテスト

        [Test]
        public void TestInjectInjectionMapあり()
        {
            container = new QuillContainer();
            var map = Seasar.Quill.InjectionMap.GetInstance();
            map.Add(typeof(IInjectTest), typeof(InjectTestImpl4InjectionMap));
            map.Add(typeof(InjectTestAnother4InjectionMap));
            this.InjectionMap = map;

            var actual = new InjectTestTarget();

            Inject(actual);

            Assert.IsNotNull(actual.InjectTest, "11");
            Assert.AreEqual(typeof(InjectTestImpl4InjectionMap),
                actual.InjectTest.GetExType(), "InjectionMapで指定された方優先でInjectされているはず");
            Assert.IsNotNull(actual.InjectionTestInjectMapOnly, "21");
            Assert.AreEqual(typeof(InjectTestAnother4InjectionMap),
                actual.InjectionTestInjectMapOnly.GetExType(), "22");
            Assert.IsNotNull(actual.InjectTestImplementationOnly,
                "InjectionMapに含まれていなくてもImplementationで指定されていればInjectされているはず");
        }

        [Test]
        public void TestInjectInjectionMapなし()
        {
            container = new QuillContainer();
            var actual = new InjectTestTarget();

            Assert.IsNull(this.InjectionMap, "00");
            Inject(actual);

            Assert.IsNull(actual.InjectionTestInjectMapOnly,
                "Implementationで指定されていないクラスはInjectされないはず");
            Assert.IsNotNull(actual.InjectTest, "11");
            Assert.AreEqual(typeof(InjectTestImpl4Implementation),
                actual.InjectTest.GetExType(),
                "InjectionMapがないのでImplementation指定されている型を適用");
            Assert.IsNotNull(actual.InjectTestImplementationOnly, "21");
            Assert.AreEqual(typeof(InjectTestAnother4Implementation),
                actual.InjectTestImplementationOnly.GetExType(), "22");
        }

        #region テスト用クラス

        private class InjectTestTarget
        {
            public IInjectTest InjectTest = null;
            public InjectTestAnother4Implementation InjectTestImplementationOnly = null;
            public InjectTestAnother4InjectionMap InjectionTestInjectMapOnly = null;
        }

        [Implementation(typeof(InjectTestImpl4Implementation))]
        public interface IInjectTest
        { }

        /// <summary>
        /// Implementation属性で指定されているクラス
        /// </summary>
        public class InjectTestImpl4Implementation : IInjectTest
        { }

        /// <summary>
        /// InjectionMapで指定されているクラス
        /// </summary>
        public class InjectTestImpl4InjectionMap : IInjectTest
        { }

        /// <summary>
        /// Implementation属性でだけ指定されているクラス
        /// </summary>
        [Implementation]
        public class InjectTestAnother4Implementation
        { }

        /// <summary>
        /// InjectionMapでだけ指定されているクラス
        /// </summary>
        public class InjectTestAnother4InjectionMap
        { }

        #endregion
        #endregion

        #region Disposeのテスト

        [Test]
        public void TestDispose()
        {
            container = new QuillContainer();
            var target = new DisposableTarget();
            Inject(target);

            Assert.IsFalse(target.DisposableClass.Disposed);

            base.Dispose();

            Assert.IsTrue(target.DisposableClass.Disposed);
        }

        #endregion

        #region Disposeのテストで使用する内部クラス

        public class DisposableTarget
        {
            public DisposableClass DisposableClass;
        }

        [Implementation]
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

    }

    public class BaseClass4GetFieldTest
    {
        public IF4GetFieldTest IF;
    }

    [Implementation(typeof(Impl4GetFieldTest))]
    public interface IF4GetFieldTest
    {
        string TargetMethod();
        IFieldsTestEntity GetEntity();
    }

    public class Impl4GetFieldTest : IF4GetFieldTest
    {
        public const string EXPECT_VALUE = "TargetMethod4GetFieldTest";

        // このフィールドがnullになっていないかテスト
        public IFieldsTestEntity FT;

        [Aspect(typeof(TraceInterceptor))]
        public virtual string TargetMethod()
        {
            return EXPECT_VALUE;
        }

        public IFieldsTestEntity GetEntity()
        {
            return FT;
        }
    }

    [Implementation(typeof(GetFieldsTestEntity))]
    public interface IFieldsTestEntity
    {
        string AspectedMethod();
    }

    public class GetFieldsTestEntity : IFieldsTestEntity
    {
        // このフィールドがnullになっていないかテスト
        protected Seasar.Tests.Quill.QuillInjectorTest.Hoge1 field;

        [Aspect(typeof(TraceInterceptor))]
        public virtual string AspectedMethod()
        {
            return field.GetResult();
        }
    }
}
