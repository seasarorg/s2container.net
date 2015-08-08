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
using System.Reflection;
using Seasar.Framework.Beans.Impl;
using Seasar.Framework.Beans;
using Seasar.Framework.Util;

namespace Seasar.Tests.Framework.Beans.Impl
{
    [TestFixture]
    public class FieldDescImplTest
    {
        /// <summary>
        /// 読み取り専用判定テスト
        /// </summary>
        [Test]
        public void TestIsReadOnly()
        {
            var readonlyField = typeof (_ReadableTest).GetField("ReadOnlyField");
            var readonlyFieldDesc = new FieldDescImpl(readonlyField);
            var reaableField = typeof (_ReadableTest).GetField("ReadableField");
            var readableFieldDesc = new FieldDescImpl(reaableField);

            Assert.IsTrue(readonlyFieldDesc.IsReadOnly(), "読み取り専用のはず");
            Assert.IsFalse(readableFieldDesc.IsReadOnly(), "読み書きOKのはず");
        }

        /// <summary>
        /// リフレクションの形で値を取得できるか
        /// </summary>
        [Test]
        public void TestGetValue()
        {
            var fieldInfo = typeof (_GetValueTest).GetField("TestField");
            var fieldDescImpl = new FieldDescImpl(fieldInfo);
            var target = new _GetValueTest();

            var result = fieldDescImpl.GetValue(target);
            Assert.IsTrue(result is string, "文字列のはず");
            Assert.AreEqual(target.TestField, result, "targetから直接取得した場合と同じ値のはず");
        }

        /// <summary>
        /// 値を取得できなかったときの例外が想定通りか（targetの型が違う場合）
        /// </summary>
        [Test]
        public void TestGetValueExceptionNotSameClass()
        {
            var fieldInfo = typeof(_GetValueTest).GetField("TestField");
            var fieldDescImpl = new FieldDescImpl(fieldInfo);
            var target = new _ReadableTest();

            try
            {
                fieldDescImpl.GetValue(target);
                Assert.Fail("例外が発生しているはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalFieldRuntimeException);
                var targetException = (IllegalFieldRuntimeException)ex;
                Assert.AreEqual(typeof(_GetValueTest), targetException.ComponentType);
                Assert.AreEqual("TestField", targetException.FieldName);
                Assert.AreEqual(typeof(ArgumentException), targetException.InnerException.GetExType());
            } 
        }

        /// <summary>
        /// 値を取得できなかったときの例外が想定通りか(targetがnullだった場合）
        /// </summary>
        [Test]
        public void TestGetValueExceptionTargetNull()
        {
            var fieldInfo = typeof(_GetValueTest).GetField("TestField");
            var fieldDescImpl = new FieldDescImpl(fieldInfo);

            try
            {
                fieldDescImpl.GetValue(null);
                Assert.Fail("例外が発生しているはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalFieldRuntimeException);
                var targetException = (IllegalFieldRuntimeException)ex;
                Assert.AreEqual(typeof(_GetValueTest), targetException.ComponentType);
                Assert.AreEqual("TestField", targetException.FieldName);
                Assert.AreEqual(typeof(TargetException), targetException.InnerException.GetExType());
            }
        }

        /// <summary>
        /// リフレクションの形で値を設定できるか
        /// </summary>
        [Test]
        public void TestSetValue()
        {
            var fieldInfo = typeof(_GetValueTest).GetField("TestField");
            var fieldDescImpl = new FieldDescImpl(fieldInfo);
            var target = new _GetValueTest();

            fieldDescImpl.SetValue(target, "Huga");
            Assert.AreEqual("Huga", target.TestField, "値が設定されているはず");
        }

        /// <summary>
        /// 値設定：例外パターン（読み取り専用だった場合）
        /// </summary>
        [Test]
        public void TestSetValueExceptionReadOnly()
        {
            var fieldInfo = typeof(_ReadableTest).GetField("ReadOnlyField");
            var fieldDescImpl = new FieldDescImpl(fieldInfo);
            var target = new _ReadableTest();

            try
            {
                fieldDescImpl.SetValue(target, "HugaHo");
                Assert.Fail("例外が発生しているはず:");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalFieldRuntimeException);
                var targetException = (IllegalFieldRuntimeException)ex;
                Assert.AreEqual(typeof(_ReadableTest), targetException.ComponentType);
                Assert.AreEqual("ReadOnlyField", targetException.FieldName);
                Assert.AreEqual(typeof(FieldAccessException), targetException.InnerException.GetExType());
            }
        }

        /// <summary>
        /// 値設定：例外パターン（設定できない型だった場合）
        /// </summary>
        [Test]
        public void TestSetValueExceptionNotAssignable()
        {
            var fieldInfo = typeof(_GetValueTest).GetField("TestField");
            var fieldDescImpl = new FieldDescImpl(fieldInfo);
            var target = new _GetValueTest();

            try
            {
                fieldDescImpl.SetValue(target, 999);
                Assert.Fail("例外が発生しているはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalFieldRuntimeException);
                var targetException = (IllegalFieldRuntimeException)ex;
                Assert.AreEqual(typeof(_GetValueTest), targetException.ComponentType);
                Assert.AreEqual("TestField", targetException.FieldName);
                Assert.AreEqual(typeof(ArgumentException), targetException.InnerException.GetExType());
            }
        }

        /// <summary>
        /// 値設定：例外パターン（targetの型が違った場合）
        /// </summary>
        [Test]
        public void TestSetValueExceptionNotSameClass()
        {
            var fieldInfo = typeof(_GetValueTest).GetField("TestField");
            var fieldDescImpl = new FieldDescImpl(fieldInfo);
            var target = new _ReadableTest();

            try
            {
                fieldDescImpl.SetValue(target, "Hoge");
                Assert.Fail("例外が発生しているはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalFieldRuntimeException);
                var targetException = (IllegalFieldRuntimeException)ex;
                Assert.AreEqual(typeof(_GetValueTest), targetException.ComponentType);
                Assert.AreEqual("TestField", targetException.FieldName);
                Assert.AreEqual(typeof(ArgumentException), targetException.InnerException.GetExType());
            }
        }

        /// <summary>
        /// 値設定：例外パターン（targetがnullだった場合）
        /// </summary>
        [Test]
        public void TestSetValueExceptionTargetNull()
        {
            var fieldInfo = typeof(_GetValueTest).GetField("TestField");
            var fieldDescImpl = new FieldDescImpl(fieldInfo);

            try
            {
                fieldDescImpl.SetValue(null, "Hoge");
                Assert.Fail("例外が発生しているはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalFieldRuntimeException);
                var targetException = (IllegalFieldRuntimeException)ex;
                Assert.AreEqual(typeof(_GetValueTest), targetException.ComponentType);
                Assert.AreEqual("TestField", targetException.FieldName);
                Assert.AreEqual(typeof(TargetException), targetException.InnerException.GetExType());
            }
        }

        #region テスト用クラス

        /// <summary>
        /// 読み取り専用判定テストクラス
        /// </summary>
        private class _ReadableTest
        {
            public const string READ_ONLY_FIELD = "Test";
            public string ReadableField = "Test";
        }

        /// <summary>
        /// 値取得テスト用クラス
        /// </summary>
        private class _GetValueTest
        {
            public string TestField = "Hoge";
        }

        #endregion
    }
}
