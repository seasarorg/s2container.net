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
using MbUnit.Framework;
using System.Reflection;
using Seasar.Framework.Beans.Impl;
using Seasar.Framework.Beans;

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
            FieldInfo readonlyField = typeof (ReadableTest).GetField("ReadOnlyField");
            FieldDescImpl readonlyFieldDesc = new FieldDescImpl(readonlyField);
            FieldInfo reaableField = typeof (ReadableTest).GetField("ReadableField");
            FieldDescImpl readableFieldDesc = new FieldDescImpl(reaableField);

            Assert.IsTrue(readonlyFieldDesc.IsReadOnly(), "読み取り専用のはず");
            Assert.IsFalse(readableFieldDesc.IsReadOnly(), "読み書きOKのはず");
        }

        /// <summary>
        /// リフレクションの形で値を取得できるか
        /// </summary>
        [Test]
        public void TestGetValue()
        {
            FieldInfo fieldInfo = typeof (GetValueTest).GetField("TestField");
            FieldDescImpl fieldDescImpl = new FieldDescImpl(fieldInfo);
            GetValueTest target = new GetValueTest();

            object result = fieldDescImpl.GetValue(target);
            Assert.IsTrue(result is string, "文字列のはず");
            Assert.AreEqual(target.TestField, result, "targetから直接取得した場合と同じ値のはず");
        }

        /// <summary>
        /// 値を取得できなかったときの例外が想定通りか（targetの型が違う場合）
        /// </summary>
        [Test]
        public void TestGetValue_Exception_NotSameClass()
        {
            FieldInfo fieldInfo = typeof(GetValueTest).GetField("TestField");
            FieldDescImpl fieldDescImpl = new FieldDescImpl(fieldInfo);
            ReadableTest target = new ReadableTest();

            try
            {
                fieldDescImpl.GetValue(target);
                Assert.Fail("例外が発生しているはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalFieldRuntimeException);
                IllegalFieldRuntimeException targetException = (IllegalFieldRuntimeException)ex;
                Assert.AreEqual(typeof(GetValueTest), targetException.ComponentType);
                Assert.AreEqual("TestField", targetException.FieldName);
                Assert.AreEqual(typeof(ArgumentException), targetException.InnerException.GetType());
            } 
        }

        /// <summary>
        /// 値を取得できなかったときの例外が想定通りか(targetがnullだった場合）
        /// </summary>
        [Test]
        public void TestGetValue_Exception_TargetNull()
        {
            FieldInfo fieldInfo = typeof(GetValueTest).GetField("TestField");
            FieldDescImpl fieldDescImpl = new FieldDescImpl(fieldInfo);

            try
            {
                fieldDescImpl.GetValue(null);
                Assert.Fail("例外が発生しているはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalFieldRuntimeException);
                IllegalFieldRuntimeException targetException = (IllegalFieldRuntimeException)ex;
                Assert.AreEqual(typeof(GetValueTest), targetException.ComponentType);
                Assert.AreEqual("TestField", targetException.FieldName);
                Assert.AreEqual(typeof(TargetException), targetException.InnerException.GetType());
            }
        }

        /// <summary>
        /// リフレクションの形で値を設定できるか
        /// </summary>
        [Test]
        public void TestSetValue()
        {
            FieldInfo fieldInfo = typeof(GetValueTest).GetField("TestField");
            FieldDescImpl fieldDescImpl = new FieldDescImpl(fieldInfo);
            GetValueTest target = new GetValueTest();

            fieldDescImpl.SetValue(target, "Huga");
            Assert.AreEqual("Huga", target.TestField, "値が設定されているはず");
        }

        /// <summary>
        /// 値設定：例外パターン（読み取り専用だった場合）
        /// </summary>
        [Test]
        public void TestSetValue_Exception_ReadOnly()
        {
            FieldInfo fieldInfo = typeof(ReadableTest).GetField("ReadOnlyField");
            FieldDescImpl fieldDescImpl = new FieldDescImpl(fieldInfo);
            ReadableTest target = new ReadableTest();

            try
            {
                fieldDescImpl.SetValue(target, "HugaHo");
                Assert.Fail("例外が発生しているはず:");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalFieldRuntimeException);
                IllegalFieldRuntimeException targetException = (IllegalFieldRuntimeException)ex;
                Assert.AreEqual(typeof(ReadableTest), targetException.ComponentType);
                Assert.AreEqual("ReadOnlyField", targetException.FieldName);
                Assert.AreEqual(typeof(FieldAccessException), targetException.InnerException.GetType());
            }
        }

        /// <summary>
        /// 値設定：例外パターン（設定できない型だった場合）
        /// </summary>
        [Test]
        public void TestSetValue_Exception_NotAssignable()
        {
            FieldInfo fieldInfo = typeof(GetValueTest).GetField("TestField");
            FieldDescImpl fieldDescImpl = new FieldDescImpl(fieldInfo);
            GetValueTest target = new GetValueTest();

            try
            {
                fieldDescImpl.SetValue(target, 999);
                Assert.Fail("例外が発生しているはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalFieldRuntimeException);
                IllegalFieldRuntimeException targetException = (IllegalFieldRuntimeException)ex;
                Assert.AreEqual(typeof(GetValueTest), targetException.ComponentType);
                Assert.AreEqual("TestField", targetException.FieldName);
                Assert.AreEqual(typeof(ArgumentException), targetException.InnerException.GetType());
            }
        }

        /// <summary>
        /// 値設定：例外パターン（targetの型が違った場合）
        /// </summary>
        [Test]
        public void TestSetValue_Exception_NotSameClass()
        {
            FieldInfo fieldInfo = typeof(GetValueTest).GetField("TestField");
            FieldDescImpl fieldDescImpl = new FieldDescImpl(fieldInfo);
            ReadableTest target = new ReadableTest();

            try
            {
                fieldDescImpl.SetValue(target, "Hoge");
                Assert.Fail("例外が発生しているはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalFieldRuntimeException);
                IllegalFieldRuntimeException targetException = (IllegalFieldRuntimeException)ex;
                Assert.AreEqual(typeof(GetValueTest), targetException.ComponentType);
                Assert.AreEqual("TestField", targetException.FieldName);
                Assert.AreEqual(typeof(ArgumentException), targetException.InnerException.GetType());
            }
        }

        /// <summary>
        /// 値設定：例外パターン（targetがnullだった場合）
        /// </summary>
        [Test]
        public void TestSetValue_Exception_TargetNull()
        {
            FieldInfo fieldInfo = typeof(GetValueTest).GetField("TestField");
            FieldDescImpl fieldDescImpl = new FieldDescImpl(fieldInfo);

            try
            {
                fieldDescImpl.SetValue(null, "Hoge");
                Assert.Fail("例外が発生しているはず");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is IllegalFieldRuntimeException);
                IllegalFieldRuntimeException targetException = (IllegalFieldRuntimeException)ex;
                Assert.AreEqual(typeof(GetValueTest), targetException.ComponentType);
                Assert.AreEqual("TestField", targetException.FieldName);
                Assert.AreEqual(typeof(TargetException), targetException.InnerException.GetType());
            }
        }

        #region テスト用クラス

        /// <summary>
        /// 読み取り専用判定テストクラス
        /// </summary>
        private class ReadableTest
        {
            public const string ReadOnlyField = "Test";
            public string ReadableField = "Test";
        }

        /// <summary>
        /// 値取得テスト用クラス
        /// </summary>
        private class GetValueTest
        {
            public string TestField = "Hoge";
        }

        #endregion
    }
}
