#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
using System.Reflection;
using Seasar.Framework.Beans.Impl;
using Seasar.Framework.Beans;

namespace Seasar.Tests.Framework.Beans.Impl
{
    [TestFixture]
    public class PropertyDescImplTest
    {
        [Test]
        public void TestReadMethod_OK()
        {
            PropertyInfo propertyInfo = typeof(PropertyDescTest).GetProperty("Both");
            PropertyDescImpl propertyDescImpl = new PropertyDescImpl(propertyInfo);

            IMethodDesc actual = propertyDescImpl.ReadMethod;
            Assert.IsNotNull(actual);
            Assert.AreEqual("get_Both", actual.Name);
        }

        [Test]
        public void TestReadMethod_NoGetter()
        {
            PropertyInfo propertyInfo = typeof(PropertyDescTest).GetProperty("SetOnly");
            PropertyDescImpl propertyDescImpl = new PropertyDescImpl(propertyInfo);

            try
            {
                IMethodDesc actual = propertyDescImpl.ReadMethod;
                Assert.Fail("例外が発生するはず", actual.Name);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is MethodNotFoundRuntimeException, ex.GetType().Name);
                MethodNotFoundRuntimeException targetException = (MethodNotFoundRuntimeException) ex;
                Assert.AreEqual(typeof(PropertyDescTest), targetException.ComponentType);
                Assert.AreEqual("get", targetException.MethodName);
            }
        }

        [Test]
        public void TestWriteMethod_OK()
        {
            PropertyInfo propertyInfo = typeof(PropertyDescTest).GetProperty("Both");
            PropertyDescImpl propertyDescImpl = new PropertyDescImpl(propertyInfo);

            IMethodDesc actual = propertyDescImpl.WriteMethod;
            Assert.IsNotNull(actual);
            Assert.AreEqual("set_Both", actual.Name);
        }

        [Test]
        public void TestWriteMethod_NoSetter()
        {
            PropertyInfo propertyInfo = typeof(PropertyDescTest).GetProperty("GetOnly");
            PropertyDescImpl propertyDescImpl = new PropertyDescImpl(propertyInfo);

            try
            {
                IMethodDesc actual = propertyDescImpl.WriteMethod;
                Assert.Fail("例外が発生するはず", actual.Name);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is MethodNotFoundRuntimeException, ex.GetType().Name);
                MethodNotFoundRuntimeException targetException = (MethodNotFoundRuntimeException)ex;
                Assert.AreEqual(typeof(PropertyDescTest), targetException.ComponentType);
                Assert.AreEqual("set", targetException.MethodName);
            }
        }

        [Test]
        public void TestHasReadMethod_True()
        {
            PropertyInfo both = typeof(PropertyDescTest).GetProperty("Both");
            PropertyDescImpl bothDesc = new PropertyDescImpl(both);
            Assert.IsTrue(bothDesc.HasReadMethod());

            PropertyInfo getOnly = typeof(PropertyDescTest).GetProperty("GetOnly");
            PropertyDescImpl getOnlyDesc = new PropertyDescImpl(getOnly);
            Assert.IsTrue(getOnlyDesc.HasReadMethod());
        }

        [Test]
        public void TestHasReadMethod_False()
        {
            PropertyInfo propertyInfo = typeof(PropertyDescTest).GetProperty("SetOnly");
            PropertyDescImpl propertyInfoDesc = new PropertyDescImpl(propertyInfo);
            Assert.IsFalse(propertyInfoDesc.HasReadMethod());
        }

        [Test]
        public void TestHasWriteMethod_True()
        {
            PropertyInfo both = typeof(PropertyDescTest).GetProperty("Both");
            PropertyDescImpl bothDesc = new PropertyDescImpl(both);
            Assert.IsTrue(bothDesc.HasWriteMethod());

            PropertyInfo setOnly = typeof(PropertyDescTest).GetProperty("SetOnly");
            PropertyDescImpl setOnlyDesc = new PropertyDescImpl(setOnly);
            Assert.IsTrue(setOnlyDesc.HasWriteMethod());
        }

        [Test]
        public void TestHasWriteMethod_False()
        {
            PropertyInfo propertyInfo = typeof(PropertyDescTest).GetProperty("GetOnly");
            PropertyDescImpl propertyInfoDesc = new PropertyDescImpl(propertyInfo);
            Assert.IsFalse(propertyInfoDesc.HasWriteMethod());
        }

        [Test]
        public void TestGetValue_OK()
        {
            PropertyInfo propertyInfo = typeof(PropertyDescTest).GetProperty("Both");
            PropertyDescImpl propertyDescImpl = new PropertyDescImpl(propertyInfo);
            PropertyDescTest target = new PropertyDescTest();

            object ret = propertyDescImpl.GetValue(target);
            Assert.AreEqual(typeof (string), ret.GetType());
            Assert.AreEqual("Hoge", ret);
        }

        [Test]
        public void TestGetValue_NG()
        {
            PropertyInfo propertyInfo = typeof(PropertyDescTest).GetProperty("SetOnly");
            PropertyDescImpl propertyDescImpl = new PropertyDescImpl(propertyInfo);
            PropertyDescTest target = new PropertyDescTest();

            try
            {
                object ret = propertyDescImpl.GetValue(target);
                Assert.Fail("例外が発生しているはず", ret.ToString());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalPropertyRuntimeException);
                IllegalPropertyRuntimeException targetException = (IllegalPropertyRuntimeException) ex;
                Assert.AreEqual(typeof(PropertyDescTest), targetException.ComponentType);
                Assert.AreEqual("SetOnly", targetException.PropertyName);
                Assert.AreEqual(typeof(MethodNotFoundRuntimeException), targetException.InnerException.GetType());
            }
        }

        [Test]
        public void TestSetValue_OK()
        {
            PropertyInfo propertyInfo = typeof(PropertyDescTest).GetProperty("Both");
            PropertyDescImpl propertyDescImpl = new PropertyDescImpl(propertyInfo);
            PropertyDescTest target = new PropertyDescTest();

            propertyDescImpl.SetValue(target, "Updated");
            Assert.AreEqual("Updated", target.Both);
        }

        [Test]
        public void TestSetValue_NG()
        {
            PropertyInfo propertyInfo = typeof(PropertyDescTest).GetProperty("GetOnly");
            PropertyDescImpl propertyDescImpl = new PropertyDescImpl(propertyInfo);
            PropertyDescTest target = new PropertyDescTest();

            try
            {
                propertyDescImpl.SetValue(target, "Updated");
                Assert.Fail("例外が発生しているはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalPropertyRuntimeException);
                IllegalPropertyRuntimeException targetException = (IllegalPropertyRuntimeException)ex;
                Assert.AreEqual(typeof(PropertyDescTest), targetException.ComponentType);
                Assert.AreEqual("GetOnly", targetException.PropertyName);
                Assert.AreEqual(typeof(MethodNotFoundRuntimeException), targetException.InnerException.GetType());
            }
        }

        #region テスト用クラス

        private class PropertyDescTest
        {
            private string _setOnly;
            private string _both = "Hoge";
            public string GetOnly { get { return "hoge"; } }
            public string SetOnly { set { _setOnly = value; }}
            public string Both { get { return _both; } set { _both = value; } }
        }

        #endregion
    }
}
