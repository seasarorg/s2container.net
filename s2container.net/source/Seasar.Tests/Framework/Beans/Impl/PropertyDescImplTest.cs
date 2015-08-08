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
using MbUnit.Framework;
using Seasar.Framework.Beans;
using Seasar.Framework.Beans.Impl;
using Seasar.Framework.Util;

namespace Seasar.Tests.Framework.Beans.Impl
{
    [TestFixture]
    public class PropertyDescImplTest
    {
        [Test]
        public void TestReadMethodOk()
        {
            var propertyInfo = typeof(_PropertyDescTest).GetProperty("Both");
            var propertyDescImpl = new PropertyDescImpl(propertyInfo);

            var actual = propertyDescImpl.ReadMethod;
            Assert.IsNotNull(actual);
            Assert.AreEqual("get_Both", actual.Name);
        }

        [Test]
        public void TestReadMethodNoGetter()
        {
            var propertyInfo = typeof(_PropertyDescTest).GetProperty("SetOnly");
            var propertyDescImpl = new PropertyDescImpl(propertyInfo);

            try
            {
                var actual = propertyDescImpl.ReadMethod;
                Assert.Fail("例外が発生するはず", actual.Name);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is MethodNotFoundRuntimeException, ex.GetExType().Name);
                var targetException = (MethodNotFoundRuntimeException) ex;
                Assert.AreEqual(typeof(_PropertyDescTest), targetException.ComponentType);
                Assert.AreEqual("get", targetException.MethodName);
            }
        }

        [Test]
        public void TestWriteMethodOk()
        {
            var propertyInfo = typeof(_PropertyDescTest).GetProperty("Both");
            var propertyDescImpl = new PropertyDescImpl(propertyInfo);

            var actual = propertyDescImpl.WriteMethod;
            Assert.IsNotNull(actual);
            Assert.AreEqual("set_Both", actual.Name);
        }

        [Test]
        public void TestWriteMethodNoSetter()
        {
            var propertyInfo = typeof(_PropertyDescTest).GetProperty("GetOnly");
            var propertyDescImpl = new PropertyDescImpl(propertyInfo);

            try
            {
                var actual = propertyDescImpl.WriteMethod;
                Assert.Fail("例外が発生するはず", actual.Name);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is MethodNotFoundRuntimeException, ex.GetExType().Name);
                var targetException = (MethodNotFoundRuntimeException)ex;
                Assert.AreEqual(typeof(_PropertyDescTest), targetException.ComponentType);
                Assert.AreEqual("set", targetException.MethodName);
            }
        }

        [Test]
        public void TestHasReadMethodTrue()
        {
            var both = typeof(_PropertyDescTest).GetProperty("Both");
            var bothDesc = new PropertyDescImpl(both);
            Assert.IsTrue(bothDesc.HasReadMethod());

            var getOnly = typeof(_PropertyDescTest).GetProperty("GetOnly");
            var getOnlyDesc = new PropertyDescImpl(getOnly);
            Assert.IsTrue(getOnlyDesc.HasReadMethod());
        }

        [Test]
        public void TestHasReadMethodFalse()
        {
            var propertyInfo = typeof(_PropertyDescTest).GetProperty("SetOnly");
            var propertyInfoDesc = new PropertyDescImpl(propertyInfo);
            Assert.IsFalse(propertyInfoDesc.HasReadMethod());
        }

        [Test]
        public void TestHasWriteMethodTrue()
        {
            var both = typeof(_PropertyDescTest).GetProperty("Both");
            var bothDesc = new PropertyDescImpl(both);
            Assert.IsTrue(bothDesc.HasWriteMethod());

            var setOnly = typeof(_PropertyDescTest).GetProperty("SetOnly");
            var setOnlyDesc = new PropertyDescImpl(setOnly);
            Assert.IsTrue(setOnlyDesc.HasWriteMethod());
        }

        [Test]
        public void TestHasWriteMethodFalse()
        {
            var propertyInfo = typeof(_PropertyDescTest).GetProperty("GetOnly");
            var propertyInfoDesc = new PropertyDescImpl(propertyInfo);
            Assert.IsFalse(propertyInfoDesc.HasWriteMethod());
        }

        [Test]
        public void TestGetValueOk()
        {
            var propertyInfo = typeof(_PropertyDescTest).GetProperty("Both");
            var propertyDescImpl = new PropertyDescImpl(propertyInfo);
            var target = new _PropertyDescTest();

            var ret = propertyDescImpl.GetValue(target);
            Assert.AreEqual(typeof (string), ret.GetExType());
            Assert.AreEqual("Hoge", ret);
        }

        [Test]
        public void TestGetValueNg()
        {
            var propertyInfo = typeof(_PropertyDescTest).GetProperty("SetOnly");
            var propertyDescImpl = new PropertyDescImpl(propertyInfo);
            var target = new _PropertyDescTest();

            try
            {
                var ret = propertyDescImpl.GetValue(target);
                Assert.Fail("例外が発生しているはず", ret.ToString());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalPropertyRuntimeException);
                var targetException = (IllegalPropertyRuntimeException) ex;
                Assert.AreEqual(typeof(_PropertyDescTest), targetException.ComponentType);
                Assert.AreEqual("SetOnly", targetException.PropertyName);
                Assert.AreEqual(typeof(MethodNotFoundRuntimeException), targetException.InnerException.GetExType());
            }
        }

        [Test]
        public void TestSetValueOk()
        {
            var propertyInfo = typeof(_PropertyDescTest).GetProperty("Both");
            var propertyDescImpl = new PropertyDescImpl(propertyInfo);
            var target = new _PropertyDescTest();

            propertyDescImpl.SetValue(target, "Updated");
            Assert.AreEqual("Updated", target.Both);
        }

        [Test]
        public void TestSetValueNg()
        {
            var propertyInfo = typeof(_PropertyDescTest).GetProperty("GetOnly");
            var propertyDescImpl = new PropertyDescImpl(propertyInfo);
            var target = new _PropertyDescTest();

            try
            {
                propertyDescImpl.SetValue(target, "Updated");
                Assert.Fail("例外が発生しているはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalPropertyRuntimeException);
                var targetException = (IllegalPropertyRuntimeException)ex;
                Assert.AreEqual(typeof(_PropertyDescTest), targetException.ComponentType);
                Assert.AreEqual("GetOnly", targetException.PropertyName);
                Assert.AreEqual(typeof(MethodNotFoundRuntimeException), targetException.InnerException.GetExType());
            }
        }

        #region テスト用クラス

        private class _PropertyDescTest
        {
            private string _setOnly;
            public string GetOnly => "hoge";
            public string SetOnly { set { _setOnly = value; }}
            public string Both { get; } = "Hoge";
        }

        #endregion
    }
}
