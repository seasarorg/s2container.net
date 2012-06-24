#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
using System.Reflection;
using MbUnit.Framework;
using Seasar.Framework.Beans;
using Seasar.Framework.Beans.Impl;
using System.Text;

namespace Seasar.Tests.Framework.Beans.Impl
{
    /// <summary>
    /// BeanDescImplのテスト
    /// ※Field/Method/PropertyDescFactoryのテストも兼ねています
    /// </summary>
    [TestFixture]
    public class BeanDescImplTest
    {
        #region PropertyDesc系
        [Test]
        public void TestHasPropertyDesc_Default_True()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsTrue(actual.HasProperty("HogeProp"));
        }

        [Test]
        public void TestPropertyDesc_Default_False_NonPublic()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsFalse(actual.HasProperty("HideProp"));
        }

        [Test]
        public void TestPropertyDesc_Default_False_NotExists()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsFalse(actual.HasProperty("NotExists"));
        }

        [Test]
        public void TestPropertyDesc_BindFlag_True()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsTrue(actual.HasProperty("HideProp", BindingFlags.NonPublic | BindingFlags.Instance));
        }

        [Test]
        public void TestPropertyDesc_BindFlag_False()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsFalse(actual.HasProperty("NotExists", BindingFlags.NonPublic | BindingFlags.Instance));
        }

        [Test]
        public void TestGetProperty_Default_OK()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            IPropertyDesc result = actual.GetPropertyDesc("HogeProp");
            Assert.IsNotNull(result);
            Assert.AreEqual("HogeProp", result.Name);
        }

        [Test]
        public void TestGetProperty_Default_NG_NonPublic()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IPropertyDesc result = actual.GetPropertyDesc("HideProp");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyNotFoundRuntimeException);
                PropertyNotFoundRuntimeException targetException = (PropertyNotFoundRuntimeException) ex;
                Assert.AreEqual(typeof(BeanDescTest), targetException.ComponentType);
                Assert.AreEqual("HideProp", targetException.PropertyName);
            }
        }

        [Test]
        public void TestGetProperty_Default_NG_NotExists()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IPropertyDesc result = actual.GetPropertyDesc("NotExists");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyNotFoundRuntimeException);
                PropertyNotFoundRuntimeException targetException = (PropertyNotFoundRuntimeException)ex;
                Assert.AreEqual(typeof(BeanDescTest), targetException.ComponentType);
                Assert.AreEqual("NotExists", targetException.PropertyName);
            }
        }

        [Test]
        public void TestGetProperty_BindFlags_OK()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            IPropertyDesc result = actual.GetPropertyDesc("HideProp", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(result);
            Assert.AreEqual("HideProp", result.Name);
        }

        [Test]
        public void TestGetProperty_BindFlags_NG_NotExists()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IPropertyDesc result = actual.GetPropertyDesc("NotExists", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyNotFoundRuntimeException);
                PropertyNotFoundRuntimeException targetException = (PropertyNotFoundRuntimeException)ex;
                Assert.AreEqual(typeof(BeanDescTest), targetException.ComponentType);
                Assert.AreEqual("NotExists", targetException.PropertyName);
            }
        }

        [Test]
        public void TestGetProperty_Index_Default_OK()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            IPropertyDesc result = actual.GetPropertyDesc(actual.GetPropertyDescs().Length - 1);
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestGetProperty_Index_Default_NG()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IPropertyDesc result = actual.GetPropertyDesc(actual.GetPropertyDescs().Length);
                Assert.Fail("例外が発生するはず");
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        [Test]
        public void TestGetProperty_Index_BindFlags_OK()
        {
            const BindingFlags b = (BindingFlags.NonPublic | BindingFlags.Instance);
            BeanDescImpl actual = CreateTestBeanDescImpl();
            IPropertyDesc result = actual.GetPropertyDesc(actual.GetPropertyDescs(b).Length - 1, b);
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestGetProperty_Index_BindingFlags_NG()
        {
            const BindingFlags b = (BindingFlags.NonPublic | BindingFlags.Instance);
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IPropertyDesc result = actual.GetPropertyDesc(actual.GetPropertyDescs(b).Length, b);
                Assert.Fail("例外が発生するはず");
            }
            catch (IndexOutOfRangeException)
            {
            }
        }
        #endregion

        #region FieldDesc系
        [Test]
        public void TestHasField_Default_True()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsTrue(actual.HasField("_hogeField"));
        }

        [Test]
        public void TestHasField_Default_False_NonPublic()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsFalse(actual.HasField("_hideField"));
        }

        [Test]
        public void TestHasField_Default_False_NotExists()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsFalse(actual.HasField("NotExists"));
        }

        [Test]
        public void TestHasField_BindFlag_True()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsTrue(actual.HasField("_hideField", BindingFlags.NonPublic | BindingFlags.Instance));
        }

        [Test]
        public void TestHasField_BindFlag_False()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsFalse(actual.HasField("NotExists", BindingFlags.NonPublic | BindingFlags.Instance));
        }

        [Test]
        public void TestGetField_Default_OK()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            IFieldDesc result = actual.GetFieldDesc("_hogeField");
            Assert.IsNotNull(result);
            Assert.AreEqual("_hogeField", result.Name);
        }

        [Test]
        public void TestGetField_Default_NG_NonPublic()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IFieldDesc result = actual.GetFieldDesc("_hideField");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is FieldNotFoundRuntimeException);
                FieldNotFoundRuntimeException targetException = (FieldNotFoundRuntimeException)ex;
                Assert.AreEqual(typeof(BeanDescTest), targetException.ComponentType);
                Assert.AreEqual("_hideField", targetException.FieldName);
            }
        }

        [Test]
        public void TestGetField_Default_NG_NotExists()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IFieldDesc result = actual.GetFieldDesc("NotExists");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is FieldNotFoundRuntimeException);
                FieldNotFoundRuntimeException targetException = (FieldNotFoundRuntimeException)ex;
                Assert.AreEqual(typeof(BeanDescTest), targetException.ComponentType);
                Assert.AreEqual("NotExists", targetException.FieldName);
            }
        }

        [Test]
        public void TestGetField_BindFlags_OK()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            IFieldDesc result = actual.GetFieldDesc("_hideField", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(result);
            Assert.AreEqual("_hideField", result.Name);
        }

        [Test]
        public void TestGetField_BindFlags_NG_NotExists()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IFieldDesc result = actual.GetFieldDesc("NotExists", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is FieldNotFoundRuntimeException);
                FieldNotFoundRuntimeException targetException = (FieldNotFoundRuntimeException)ex;
                Assert.AreEqual(typeof(BeanDescTest), targetException.ComponentType);
                Assert.AreEqual("NotExists", targetException.FieldName);
            }
        }

        [Test]
        public void TestGetField_Index_Default_OK()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            IFieldDesc result = actual.GetFieldDesc(actual.GetFieldDescs().Length - 1);
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestGetField_Index_Default_NG()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IFieldDesc result = actual.GetFieldDesc(actual.GetFieldDescs().Length);
                Assert.Fail("例外が発生するはず");
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        [Test]
        public void TestGetField_Index_BindFlags_OK()
        {
            const BindingFlags b = (BindingFlags.NonPublic | BindingFlags.Instance);
            BeanDescImpl actual = CreateTestBeanDescImpl();
            IFieldDesc result = actual.GetFieldDesc(actual.GetFieldDescs(b).Length - 1, b);
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestGetField_Index_BindingFlags_NG()
        {
            const BindingFlags b = (BindingFlags.NonPublic | BindingFlags.Instance);
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IFieldDesc result = actual.GetFieldDesc(actual.GetFieldDescs(b).Length, b);
                Assert.Fail("例外が発生するはず");
            }
            catch (IndexOutOfRangeException)
            {
            }
        }
        #endregion

        #region MethodDesc系
        [Test]
        public void TestHasMethod_Default_True()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsTrue(actual.HasMethod("HogeMethod"));
        }

        [Test]
        public void TestHasMethod_Default_False_NonPublic()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsFalse(actual.HasMethod("HodeMethod"));
        }

        [Test]
        public void TestHasMethod_Overload_True()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Type[] parameterTypes = new Type[] { typeof(int) };

            Assert.IsTrue(actual.HasMethod("HogeMethod", parameterTypes));
        }

        [Test]
        public void TestHasMethod_Overload_False()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Type[] notExistParamTypes = new Type[] { typeof(int), typeof(DateTime) };

            Assert.IsFalse(actual.HasMethod("HogeMethod", notExistParamTypes));
        }

        [Test]
        public void TestHasMethod_Default_False_NotExists()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsFalse(actual.HasMethod("NotExists"));
        }

        [Test]
        public void TestHasMethod_BindFlag_True()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsTrue(actual.HasMethod("HideMethod", BindingFlags.NonPublic | BindingFlags.Instance));
        }

        [Test]
        public void TestHasMethod_BindFlag_False()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Assert.IsFalse(actual.HasMethod("NotExists", BindingFlags.NonPublic | BindingFlags.Instance));
        }

        [Test]
        public void TestHasMethod_Overload_BindFlag_True()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            //  引数なし
            Type[] notExistParamTypes = new Type[] { };

            Assert.IsTrue(actual.HasMethod("HideMethod", notExistParamTypes, 
                BindingFlags.NonPublic | BindingFlags.Instance));
        }

        [Test]
        public void TestHasMethod_Overload_BindFlag_False()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Type[] notExistParamTypes = new Type[] { typeof(int), typeof(DateTime) };

            Assert.IsFalse(actual.HasMethod("HideMethod", notExistParamTypes, 
                BindingFlags.NonPublic | BindingFlags.Instance));
        }

        [Test]
        public void TestGetMethodDesc_Default_OK()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            IMethodDesc result = actual.GetMethodDesc("HogeMethod");
            Assert.IsNotNull(result);
            Assert.AreEqual("HogeMethod", result.Name);
            Assert.AreEqual(0, result.GetParameterInfos().Length, 
                "引数型を指定しない場合は引数なしのメソッドを取得しに行く");
        }

        [Test]
        public void TestGetMethodDesc_Default_NG_NonPublic()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IMethodDesc result = actual.GetMethodDesc("HodeMethod");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is MethodNotFoundRuntimeException);
                MethodNotFoundRuntimeException targetException = (MethodNotFoundRuntimeException)ex;
                Assert.AreEqual(typeof(BeanDescTest), targetException.ComponentType);
                Assert.AreEqual("HodeMethod", targetException.MethodName);
            }
        }

        [Test]
        public void TestGetMethodDesc_Default_NG_NotExists()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IMethodDesc result = actual.GetMethodDesc("NotExists");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is MethodNotFoundRuntimeException);
                MethodNotFoundRuntimeException targetException = (MethodNotFoundRuntimeException)ex;
                Assert.AreEqual(typeof(BeanDescTest), targetException.ComponentType);
                Assert.AreEqual("NotExists", targetException.MethodName);
            }
        }

        [Test]
        public void TestGetMethodDesc_Overload_OK()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Type[] paramTypes = new Type[] { typeof(int) };
            IMethodDesc result = actual.GetMethodDesc("HogeMethod", paramTypes);
            Assert.IsNotNull(result);
            Assert.AreEqual("HogeMethod", result.Name);
            Assert.AreEqual(paramTypes.Length, result.GetParameterInfos().Length);
        }

        [Test]
        public void TestGetMethodDesc_Overload_NG_NotExists()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Type[] notExistParamTypes = new Type[] { typeof(Array) };
            try
            {
                IMethodDesc result = actual.GetMethodDesc("HogeMethod", notExistParamTypes);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is MethodNotFoundRuntimeException);
                MethodNotFoundRuntimeException targetException = (MethodNotFoundRuntimeException)ex;
                Assert.AreEqual(typeof(BeanDescTest), targetException.ComponentType);
                Assert.AreEqual("HogeMethod", targetException.MethodName);
                Assert.AreEqual(notExistParamTypes.Length, targetException.MethodArgTypes.Length);
                for(int i = 0; i < notExistParamTypes.Length; i++)
                {
                    Assert.AreEqual(notExistParamTypes[i].Name, targetException.MethodArgTypes[i].Name);
                }
            }
        }

        [Test]
        public void TestGetMethodDesc_BindFlags_OK()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            IMethodDesc result = actual.GetMethodDesc("HideMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(result);
            Assert.AreEqual("HideMethod", result.Name);
            Assert.AreEqual(0, result.GetParameterInfos().Length,
                "引数型を指定しない場合は引数なしのメソッドを取得しに行く");
        }

        [Test]
        public void TestGetMethodDesc_BindFlags_NG_NotExists()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            try
            {
                IMethodDesc result = actual.GetMethodDesc("NotExists", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is MethodNotFoundRuntimeException);
                MethodNotFoundRuntimeException targetException = (MethodNotFoundRuntimeException)ex;
                Assert.AreEqual(typeof(BeanDescTest), targetException.ComponentType);
                Assert.AreEqual("NotExists", targetException.MethodName);
            }
        }

        [Test]
        public void TestGetMethodDesc_Overload_BindFlags_OK()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Type[] paramTypes = new Type[] { typeof(string) };
            IMethodDesc result = actual.GetMethodDesc("HideMethod", paramTypes, 
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(result);
            Assert.AreEqual("HideMethod", result.Name);
            Assert.AreEqual(paramTypes.Length, result.GetParameterInfos().Length);
        }

        [Test]
        public void TestGetMethodDesc_Overload_BindFlags_NG_NotExists()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            Type[] notExistParamTypes = new Type[] { typeof(Array) };
            try
            {
                IMethodDesc result = actual.GetMethodDesc("HideMethod", notExistParamTypes,
                    BindingFlags.NonPublic | BindingFlags.Instance);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is MethodNotFoundRuntimeException);
                MethodNotFoundRuntimeException targetException = (MethodNotFoundRuntimeException)ex;
                Assert.AreEqual(typeof(BeanDescTest), targetException.ComponentType);
                Assert.AreEqual("HideMethod", targetException.MethodName);
                Assert.AreEqual(notExistParamTypes.Length, targetException.MethodArgTypes.Length);
                for (int i = 0; i < notExistParamTypes.Length; i++)
                {
                    Assert.AreEqual(notExistParamTypes[i].Name, targetException.MethodArgTypes[i].Name);
                }
            }
        }

        [Test]
        public void TestGetMethodDescs_Default()
        {
            MethodInfo[] expectedInfos = typeof (BeanDescTest).GetMethods();
            BeanDescImpl actual = CreateTestBeanDescImpl();

            IMethodDesc[] actualInfos = actual.GetMethodDescs();
            Assert.AreEqual(expectedInfos.Length, actualInfos.Length);
            for(int i = 0; i < expectedInfos.Length; i++)
            {
                Assert.AreEqual(expectedInfos[i].Name, actualInfos[i].Name);
            }
        }

        [Test]
        public void TestGetMethodDescs_BindFlags()
        {
            const BindingFlags testFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            MethodInfo[] expectedInfos = typeof(BeanDescTest).GetMethods(testFlags);
            BeanDescImpl actual = CreateTestBeanDescImpl();

            IMethodDesc[] actualInfos = actual.GetMethodDescs(testFlags);
            Assert.AreEqual(expectedInfos.Length, actualInfos.Length);
            for (int i = 0; i < expectedInfos.Length; i++)
            {
                Assert.AreEqual(expectedInfos[i].Name, actualInfos[i].Name);
            }
        }

        [Test]
        public void TestGetMethodDescs_MethodName_Default()
        {
            const string TEST_NAME = "HogeMethod";
            MethodInfo[] expectedInfosBase = typeof(BeanDescTest).GetMethods();
            List<MethodInfo> expectedInfos = new List<MethodInfo>();
            //  テスト用リスト作成
            foreach (MethodInfo info in expectedInfosBase)
            {
                if(info.Name.Equals(TEST_NAME))
                {
                    expectedInfos.Add(info);
                }
            }

            BeanDescImpl actual = CreateTestBeanDescImpl();

            IMethodDesc[] actualInfos = actual.GetMethodDescs(TEST_NAME);
            Assert.AreEqual(expectedInfos.Count, actualInfos.Length);
            for (int i = 0; i < expectedInfos.Count; i++)
            {
                Assert.AreEqual(expectedInfos[i].Name, actualInfos[i].Name);
            }
        }

        [Test]
        public void TestGetMethodDescs_MethodName_BindFlags()
        {
            const string TEST_NAME = "HideMethod";
            const BindingFlags TEST_FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;
            MethodInfo[] expectedInfosBase = typeof(BeanDescTest).GetMethods(TEST_FLAGS);
            List<MethodInfo> expectedInfos = new List<MethodInfo>();
            //  テスト用リスト作成
            foreach (MethodInfo info in expectedInfosBase)
            {
                if (info.Name.Equals(TEST_NAME))
                {
                    expectedInfos.Add(info);
                }
            }

            BeanDescImpl actual = CreateTestBeanDescImpl();

            IMethodDesc[] actualInfos = actual.GetMethodDescs(TEST_NAME, TEST_FLAGS);
            Assert.AreEqual(expectedInfos.Count, actualInfos.Length);
            for (int i = 0; i < expectedInfos.Count; i++)
            {
                Assert.AreEqual(expectedInfos[i].Name, actualInfos[i].Name);
            }
        }

        [Test]
        public void TestGetMethodDescs_NotFound()
        {
            BeanDescImpl actual = CreateTestBeanDescImpl();
            IMethodDesc[] result = actual.GetMethodDescs("NotFound");
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        #endregion

        [Test]
        public void TestIsNullable_True()
        {
            BeanDescImpl actual = new BeanDescImpl(typeof(int?));
            Assert.IsTrue(actual.IsNullable());
        }

        [Test]
        public void TestIsNullable_False()
        {
            BeanDescImpl actual = new BeanDescImpl(typeof(int));
            Assert.IsFalse(actual.IsNullable());
        }

        [Test]
        public void TestIsAssignableFrom_Type_True()
        {
            BeanDescImpl actual = new BeanDescImpl(typeof(IBeanDesc));
            Assert.IsTrue(actual.IsAssignableFrom(typeof(BeanDescImpl)));
        }

        [Test]
        public void TestIsAssignableFrom_Type_False()
        {
            BeanDescImpl actual = new BeanDescImpl(typeof(IPropertyDesc));
            Assert.IsFalse(actual.IsAssignableFrom(typeof(BeanDescImpl)));
        }

        [Test]
        public void TestIsAssignableFrom_BeanDesc_True()
        {
            BeanDescImpl actual = new BeanDescImpl(typeof(IBeanDesc));
            Assert.IsTrue(actual.IsAssignableFrom(new BeanDescImpl(typeof(BeanDescImpl))));
        }

        [Test]
        public void TestIsAssignableFrom_BeanDesc_False()
        {
            BeanDescImpl actual = new BeanDescImpl(typeof(IPropertyDesc));
            Assert.IsFalse(actual.IsAssignableFrom(new BeanDescImpl(typeof(BeanDescImpl))));
        }

        [Test]
        public void TestGetConstructor_OK_NoParam()
        {
            Type[] testParamTypes = new Type[] { };
            BeanDescImpl actual = CreateTestBeanDescImpl();
            ConstructorInfo expectedInfo = typeof(BeanDescTest).GetConstructor(testParamTypes);

            ConstructorInfo actualInfo = actual.GetConstructor(testParamTypes);
            Assert.AreEqual(expectedInfo.DeclaringType, actualInfo.DeclaringType);
            Assert.AreEqual(expectedInfo.GetParameters().Length, actualInfo.GetParameters().Length);
        }

        [Test]
        public void TestGetConstructor_OK_ExistsParam()
        {
            Type[] testParamTypes = new Type[] { typeof(DateTime) };
            BeanDescImpl actual = CreateTestBeanDescImpl();
            ConstructorInfo expectedInfo = typeof(BeanDescTest).GetConstructor(testParamTypes);

            ConstructorInfo actualInfo = actual.GetConstructor(testParamTypes);
            Assert.AreEqual(expectedInfo.DeclaringType, actualInfo.DeclaringType);

            ParameterInfo[] expectedParamInfos = expectedInfo.GetParameters();
            ParameterInfo[] actualParamInfos = actualInfo.GetParameters();
            Assert.AreEqual(expectedParamInfos.Length, actualParamInfos.Length);
            for (int i = 0; i < expectedParamInfos.Length; i++ )
            {
                Assert.AreEqual(expectedParamInfos[i].Name, actualParamInfos[i].Name);
            }
        }

        [Test]
        public void TestGetConstructor_NG()
        {
            Type[] testTypes = new Type[] { typeof(Exception) };
            BeanDescImpl actual = CreateTestBeanDescImpl();

            try
            {
                ConstructorInfo result = actual.GetConstructor(testTypes);
                Assert.Fail("存在しないパラメータなので例外が発生するはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ConstructorNotFoundRuntimeException);
                ConstructorNotFoundRuntimeException targetException = (ConstructorNotFoundRuntimeException) ex;
                Assert.AreEqual(typeof(BeanDescTest), targetException.ComponentType);
                Assert.AreEqual(testTypes.Length, targetException.MethodArgs.Length);
            }
        }

        #region テスト補助メソッド

        private static BeanDescImpl CreateTestBeanDescImpl()
        {
            return new BeanDescImpl(typeof(BeanDescTest));
        }

        #endregion

        #region テスト用クラス

        /// <summary>
        /// BeanDescImplテスト用クラス
        /// </summary>
        private class BeanDescTest
        {
            public string _hogeField = "hoge";
            public DateTime _dateField = DateTime.Now;
            private int _hideField = 555;

            public string HogeProp { get { return "hoge"; } }
            public DateTime DateProp { get { return DateTime.Now; } }
            private int HideProp { get { return 777; } }

            public BeanDescTest()
            {}

            public BeanDescTest(DateTime currentDate)
            {
                _dateField = currentDate;
            }

            public void HogeMethod() { return; }
            public string HogeMethod(int a) { return "hoge"; }
            public string HogeMethodX() { return "huga"; }
            private int HideMethod() { return 999; }
            private int HideMethod(string huga) { return 794; }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(_hogeField);
                builder.AppendLine(_dateField.ToString());
                builder.AppendLine(_hideField.ToString());
                return builder.ToString();
            }
        }

        #endregion
    }
}
