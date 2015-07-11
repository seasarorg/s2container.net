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
using MbUnit.Framework;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Quill.Attrs;
using Seasar.Quill.Exception;
using Seasar.Quill.Util;

namespace Seasar.Tests.Quill.Util
{
    [TestFixture]
	public class AttributeUtilTest
    {
        #region GetImplementationAttrのテスト

        [Test]
        public void TestGetImplementationAttr_属性が指定されていない場合()
        {
            ImplementationAttribute attr = 
                AttributeUtil.GetImplementationAttr(typeof(Hoge1));

            Assert.IsNull(attr);
        }

        [Test]
        public void TestGetImplementationAttr_クラスの属性に実装クラスが指定されている場合()
        {
            try
            {
                ImplementationAttribute attr =
                    AttributeUtil.GetImplementationAttr(typeof(Hoge2));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0001", ex.MessageCode);
                Console.WriteLine(ex.Message);
            }
        }

        [Test]
        public void TestGetImplementationAttr_属性の実装クラスにインターフェースが指定されている場合()
        {
            try
            {
                ImplementationAttribute attr =
                    AttributeUtil.GetImplementationAttr(typeof(IFuga1));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0002", ex.MessageCode);
                Console.WriteLine(ex.Message);
            }
        }

        [Test]
        public void TestGetImplementationAttr_属性の実装クラスに抽象クラスが指定されている場合()
        {
            try
            {
                ImplementationAttribute attr =
                    AttributeUtil.GetImplementationAttr(typeof(IFuga2));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0003", ex.MessageCode);
                Console.WriteLine(ex.Message);
            }
        }

        [Test]
        public void TestGetImplementationAttr_属性の実装クラスに代入が不可能なクラスが指定されている場合()
        {
            try
            {
                ImplementationAttribute attr =
                    AttributeUtil.GetImplementationAttr(typeof(IFuga3));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0004", ex.MessageCode);
                Console.WriteLine(ex.Message);
            }
        }

        [Test]
        public void TestGetImplementationAttr_正常な場合()
        {
            ImplementationAttribute attr =
                AttributeUtil.GetImplementationAttr(typeof(IFuga4));

            Assert.AreEqual(typeof(Fuga4), attr.ImplementationType);
        }

        #endregion

        #region GetImplementationAttrのテストで使用する内部クラス・インターフェース

        private class Hoge1
        {
        }

        [Implementation(typeof(Hoge1))]
        private class Hoge2
        {
        }

        [Implementation(typeof(IFuga2))]
        private interface IFuga1
        {
        }

        [Implementation(typeof(Hoge3))]
        private interface IFuga2
        {
        }

        private abstract class Hoge3
        {
        }

        [Implementation(typeof(Hoge1))]
        private interface IFuga3
        {
        }

        [Implementation(typeof(Fuga4))]
        private interface IFuga4
        {
        }

        private class Fuga4 : IFuga4
        {
        }

        #endregion

        #region GetAspectAttrsByMemberのテスト

        [Test]
        public void TestGetAspectAttrsByMember_Typeで属性が設定されていない場合()
        {
            AspectAttribute[] aspectAttrs = 
                AttributeUtil.GetAspectAttrsByMember(typeof(AspectHoge1));

            Assert.AreEqual(0, aspectAttrs.Length);
        }

        //[Test]
        //public void TestGetAspectAttrsByMember_Typeで1つ属性が設定されている場合()
        //{
        //    AspectAttribute[] aspectAttrs =
        //        AttributeUtil.GetAspectAttrsByMember(typeof(AspectHoge2));

        //    Assert.AreEqual(1, aspectAttrs.Length);
        //    Assert.AreEqual(typeof(TraceInterceptor), aspectAttrs[0].InterceptorType);
        //    Assert.IsNull(aspectAttrs[0].ComponentName);
        //}

        //[Test]
        //public void TestGetAspectAttrsByMember_Typeで2つ属性が設定されている場合()
        //{
        //    AspectAttribute[] aspectAttrs =
        //        AttributeUtil.GetAspectAttrsByMember(typeof(AspectHoge3));

        //    Assert.AreEqual(2, aspectAttrs.Length,"1");
        //    Assert.AreEqual(typeof(TraceInterceptor), aspectAttrs[0].InterceptorType, "2");
        //    Assert.IsNull(aspectAttrs[0].ComponentName, "3");
        //    Assert.IsNull(aspectAttrs[1].InterceptorType, "4");
        //    Assert.AreEqual("Hogeceptor", aspectAttrs[1].ComponentName, "5");
        //}

        [Test]
        public void TestGetAspectAttrsByMember_Methodで属性が設定されていない場合()
        {
            AspectAttribute[] aspectAttrs =AttributeUtil.GetAspectAttrsByMember(
                typeof(AspectHoge1).GetMethod("Hoge"));

            Assert.AreEqual(0, aspectAttrs.Length);
        }

        //[Test]
        //public void TestGetAspectAttrsByMember_Methodで1つ属性が設定されている場合()
        //{
        //    AspectAttribute[] aspectAttrs = AttributeUtil.GetAspectAttrsByMember(
        //        typeof(AspectHoge2).GetMethod("Hoge"));

        //    Assert.AreEqual(1, aspectAttrs.Length);
        //    Assert.AreEqual(typeof(TraceInterceptor), aspectAttrs[0].InterceptorType);
        //    Assert.IsNull(aspectAttrs[0].ComponentName);
        //}

        //[Test]
        //public void TestGetAspectAttrsByMember_Methodで2つ属性が設定されている場合()
        //{
        //    AspectAttribute[] aspectAttrs = AttributeUtil.GetAspectAttrsByMember(
        //        typeof(AspectHoge3).GetMethod("Hoge"));

        //    Assert.AreEqual(2, aspectAttrs.Length);
        //    Assert.IsNull(aspectAttrs[0].InterceptorType);
        //    Assert.AreEqual("Hogeceptor", aspectAttrs[0].ComponentName);
        //    Assert.IsNull(aspectAttrs[1].ComponentName);
        //    Assert.AreEqual(typeof(TraceInterceptor), aspectAttrs[1].InterceptorType);
        //}

        #endregion

        #region GetAspectAttrsByMemberのテストで使用する内部クラス

        private class AspectHoge1
        {
            public void Hoge() { }
        }

        [Aspect(typeof(TraceInterceptor))]
        private class AspectHoge2
        {
            [Aspect(typeof(TraceInterceptor))]
            public void Hoge() { }
        }

        //[Aspect(typeof(TraceInterceptor), 1)]
        //[Aspect("Hogeceptor", 2)]
        //private class AspectHoge3
        //{
        //    [Aspect(typeof(TraceInterceptor), 2)]
        //    [Aspect("Hogeceptor", 1)]
        //    public void Hoge() { }
        //}

        #endregion

        #region GetAspectAttrsのテスト

        [Test]
        public void TestGetAspectAttrs()
        {
            AspectAttribute[] aspectAttrs =
                AttributeUtil.GetAspectAttrs(typeof(AspectAttrsHoge));

            Assert.AreEqual(1, aspectAttrs.Length);
            Assert.AreEqual(typeof(TraceInterceptor), aspectAttrs[0].InterceptorType);
            //Assert.IsNull(aspectAttrs[0].ComponentName);
        }

        [Test]
        public void TestGetAspectAttrs_publicではないクラスに属性が指定された場合()
        {
            try
            {
                AspectAttribute[] aspectAttrs =
                    AttributeUtil.GetAspectAttrs(typeof(AspectAttrsHoge2));
                
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0016", ex.MessageCode);
            }
        }

        #endregion

        #region GetAspectAttrsのテストで使用する内部クラス

        [Aspect(typeof(TraceInterceptor))]
        public class AspectAttrsHoge
        {
        }

        [Aspect(typeof(TraceInterceptor))]
        private class AspectAttrsHoge2
        {
        }

        #endregion

        #region GetAspectAttrsByMethodのテスト

        [Test]
        public void TestGetAspectAttrsByMethod_属性が設定されていない場合()
        {
            AspectAttribute[] aspectAttrs = AttributeUtil.GetAspectAttrsByMethod(
                typeof(AspectAttrMethodHoge1).GetMethod("Hoge"));

            Assert.AreEqual(0, aspectAttrs.Length);
        }

        //[Test]
        //public void TestGetAspectAttrsByMethod_クラスがpublicではない場合()
        //{
        //    try
        //    {
        //        AspectAttribute[] aspectAttrs = AttributeUtil.GetAspectAttrsByMethod(
        //            typeof(AspectAttrMethodHoge6).GetMethod("Hoge"));

        //        Assert.Fail();
        //    }
        //    catch (QuillApplicationException ex)
        //    {
        //        Assert.AreEqual("EQLL0016", ex.MessageCode);
        //    }
        //}

        //[Test]
        //public void TestGetAspectAttrsByMethod_メソッドがstaticの場合()
        //{
        //    try
        //    {
        //        AspectAttribute[] aspectAttrs = AttributeUtil.GetAspectAttrsByMethod(
        //            typeof(AspectAttrMethodHoge2).GetMethod("Hoge"));

        //        Assert.Fail();
        //    }
        //    catch (QuillApplicationException ex)
        //    {
        //        Assert.AreEqual("EQLL0005", ex.MessageCode);
        //    }
        //}

        //[Test]
        //public void TestGetAspectAttrsByMethod_メソッドがprivateの場合()
        //{
        //    try
        //    {
        //        AspectAttribute[] aspectAttrs = AttributeUtil.GetAspectAttrsByMethod(
        //            typeof(AspectAttrMethodHoge3).GetMethod(
        //            "Hoge", BindingFlags.NonPublic | BindingFlags.Instance));

        //        Assert.Fail();
        //    }
        //    catch (QuillApplicationException ex)
        //    {
        //        Assert.AreEqual("EQLL0006", ex.MessageCode);
        //    }
        //}


        //[Test]
        //public void TestGetAspectAttrsByMethod_メソッドがvirtualではない場合()
        //{
        //    try
        //    {
        //        AspectAttribute[] aspectAttrs = AttributeUtil.GetAspectAttrsByMethod(
        //            typeof(AspectAttrMethodHoge4).GetMethod("Hoge"));

        //        Assert.Fail();
        //    }
        //    catch (QuillApplicationException ex)
        //    {
        //        Assert.AreEqual("EQLL0007", ex.MessageCode);
        //    }
        //}

        //[Test]
        //public void TestGetAspectAttrsByMethod_正常な場合()
        //{
        //    AspectAttribute[] aspectAttrs = AttributeUtil.GetAspectAttrsByMethod(
        //        typeof(AspectAttrMethodHoge5).GetMethod("Hoge"));

        //    Assert.AreEqual(1, aspectAttrs.Length);
        //    Assert.IsNull(aspectAttrs[0].InterceptorType);
        //    Assert.AreEqual("Hogeceptor", aspectAttrs[0].ComponentName);
        //}

        #endregion

        #region GetAspectAttrsByMethodのテストで使用する内部クラス

        public class AspectAttrMethodHoge1
        {
            public void Hoge() { }
        }

        //public class AspectAttrMethodHoge2
        //{
        //    [Aspect("Hogeceptor")]
        //    public static void Hoge() { }
        //}

        //public class AspectAttrMethodHoge3
        //{
        //    [Aspect("Hogeceptor")]
        //    private void Hoge() { }
        //}

        //public class AspectAttrMethodHoge4
        //{
        //    [Aspect("Hogeceptor")]
        //    public void Hoge() { }
        //}

        //public class AspectAttrMethodHoge5
        //{
        //    [Aspect("Hogeceptor")]
        //    public virtual void Hoge() { }
        //}

        //private class AspectAttrMethodHoge6
        //{
        //    [Aspect("Hogeceptor")]
        //    public virtual void Hoge() { }
        //}

        #endregion

        #region GetBindingAttrのテスト

        [Test]
        public void TestGetBindingAttr_staticフィールドに属性が設定された場合()
        {
            try
            {
                BindingAttribute attr = AttributeUtil.GetBindingAttr(
                    typeof(BindingHoge1).GetField("hoge",
                    BindingFlags.Public | BindingFlags.Static));

                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0015", ex.MessageCode);
            }
        }

        [Test]
        public void TestGetBindingAttr_属性が設定されていない場合()
        {
            BindingAttribute attr = AttributeUtil.GetBindingAttr(
                typeof(BindingHoge2).GetField("hoge", 
                BindingFlags.Public | BindingFlags.Instance));

            Assert.IsNull(attr);
        }

        [Test]
        public void TestGetBindingAttr_コンポーネント名が設定されていない場合()
        {
            BindingAttribute attr = AttributeUtil.GetBindingAttr(
                typeof(BindingHoge3).GetField("hoge",
                BindingFlags.Public | BindingFlags.Instance));

            Assert.IsNull(attr);
        }

        [Test]
        public void TestGetBindingAttr_正常な場合()
        {
            BindingAttribute attr = AttributeUtil.GetBindingAttr(
                typeof(BindingHoge4).GetField("hoge",
                BindingFlags.Public | BindingFlags.Instance));

            Assert.AreEqual("HogeComponent", attr.ComponentName);
        }

        #endregion

        #region GetBindingAttrのテストで使用する内部クラス

        private class BindingHoge1
        {
            public static string hoge = null;
        }

        private class BindingHoge2
        {
            public string hoge = null;
        }

        private class BindingHoge3
        {
            [Binding(null)]
            public string hoge = null;
        }

        private class BindingHoge4
        {
            [Binding("HogeComponent")]
            public string hoge = null;
        }

        #endregion

        #region GetMockAttrのテスト

        [Test]
        public void TestGetMockAttr_Mock属性指定無し()
        {
            MockAttribute attr = AttributeUtil.GetMockAttr(typeof(TestGetMockAttrNon));
            Assert.IsNull(attr);
        }

        [Test]
        public void TestGetMockAttr_mockTypeがnull()
        {
            try
            {
                MockAttribute attr =
                    AttributeUtil.GetMockAttr(typeof(TestGetMockAttrNull));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.AreEqual("EQLL0019", ex.MessageCode);
            }
        }

        [Test]
        public void TestGetMockAttr_mockTypeがインターフェース()
        {
            try
            {
                MockAttribute attr =
                    AttributeUtil.GetMockAttr(typeof(TestGetMockAttrInterface));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.AreEqual("EQLL0020", ex.MessageCode);
            }
        }

        [Test]
        public void TestGetMockAttr_mockTypeが抽象クラス()
        {
            try
            {
                MockAttribute attr =
                    AttributeUtil.GetMockAttr(typeof(TestGetMockAttrAbstract));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.AreEqual("EQLL0021", ex.MessageCode);
            }
        }

        [Test]
        public void TestGetMockAttr_mockTypeが代入不可能()
        {
            try
            {
                MockAttribute attr =
                    AttributeUtil.GetMockAttr(typeof(TestGetMockAttrIsNotAssign));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.AreEqual("EQLL0022", ex.MessageCode);
            }
        }

        [Test]
        public void TestGetMockAttr_正常な属性()
        {
            MockAttribute attr =
                AttributeUtil.GetMockAttr(typeof(TestGetMockAttrIsAssign));

            Assert.AreEqual(typeof(TestGetMockAttrClass), attr.MockType);
        }

        #endregion

        #region GetMockAttrのテストで使用する内部クラス

        private interface TestGetMockAttrNon
        {
        }
        
        [Mock(null)]
        private interface TestGetMockAttrNull
        {
        }

        [Mock(typeof(TestGetMockAttrNon))]
        private interface TestGetMockAttrInterface
        {
        }

        private abstract class TestGetMockAttrAbstractClass
        {
        }
        
        [Mock(typeof(TestGetMockAttrAbstractClass))]
        private interface TestGetMockAttrAbstract
        {
        }

        private class TestGetMockAttrNotInterfaceClass
        {
        }

        [Mock(typeof(TestGetMockAttrNotInterfaceClass))]
        private interface TestGetMockAttrIsNotAssign
        {
        }

        private class TestGetMockAttrClass : TestGetMockAttrIsAssign
        {
        }

        [Mock(typeof(TestGetMockAttrClass))]
        private interface TestGetMockAttrIsAssign
        {
        }

        #endregion
    }
}
