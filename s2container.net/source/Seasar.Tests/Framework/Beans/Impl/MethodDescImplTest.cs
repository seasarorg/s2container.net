#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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

namespace Seasar.Tests.Framework.Beans.Impl
{
    [TestFixture]
    public class MethodDescImplTest
    {
        [Test]
        public void TestName()
        {
            MethodInfo methodInfo = typeof(MethodDescTest).GetMethod("Hoge");
            MethodDescImpl methodDescImpl = new MethodDescImpl(methodInfo);

            Assert.AreEqual(methodInfo.Name, methodDescImpl.Name);
        }

        [Test]
        public void TestGetParameterInfos()
        {
            MethodInfo methodInfo = typeof (MethodDescTest).GetMethod("Hoge");
            MethodDescImpl methodDescImpl = new MethodDescImpl(methodInfo);

            ParameterInfo[] expectedParamInfos = methodInfo.GetParameters();
            ParameterInfo[] actualParamInfos = methodDescImpl.GetParameterInfos();
            Assert.AreEqual(expectedParamInfos.Length, actualParamInfos.Length, "パラメータの数");
            for(int i = 0; i < expectedParamInfos.Length; i++)
            {
                Assert.AreEqual(expectedParamInfos[i], actualParamInfos[i], string.Format("[{0}]番目の引数", i));
            }
        }

        [Test]
        public void TestGetReturnType()
        {
            MethodInfo methodInfo = typeof(MethodDescTest).GetMethod("Hoge");
            MethodDescImpl methodDescImpl = new MethodDescImpl(methodInfo);

            Assert.AreEqual(methodInfo.ReturnType, methodDescImpl.GetReturnType());
        }

        [Test]
        public void TestCanOverride_virtual()
        {
            MethodInfo methodInfo = typeof(MethodDescTest).GetMethod("HogeOverridable");
            MethodDescImpl methodDescImpl = new MethodDescImpl(methodInfo);

            Assert.IsTrue(methodDescImpl.CanOverride());
        }

        [Test]
        public void TestCanOverride_abstract()
        {
            MethodInfo methodInfo = typeof(AbstractMethodDescTest).GetMethod("Hoge");
            MethodDescImpl methodDescImpl = new MethodDescImpl(methodInfo);

            Assert.IsTrue(methodDescImpl.CanOverride());
        }

        [Test]
        public void TestCanOverride_interface()
        {
            MethodInfo methodInfo = typeof(IMethodDescTest).GetMethod("Hoge");
            MethodDescImpl methodDescImpl = new MethodDescImpl(methodInfo);

            Assert.IsTrue(methodDescImpl.CanOverride());
        }

        [Test]
        public void TestCanOverride_UnableOverride()
        {
            MethodInfo methodInfo = typeof(MethodDescTest).GetMethod("Hoge");
            MethodDescImpl methodDescImpl = new MethodDescImpl(methodInfo);

            Assert.IsFalse(methodDescImpl.CanOverride());
        }

        [Test]
        public void TestInvoke_HasParameter()
        {
            MethodInfo methodInfo = typeof(MethodDescTest).GetMethod("Hoge");
            MethodDescImpl methodDescImpl = new MethodDescImpl(methodInfo);
            MethodDescTest target = new MethodDescTest();

            object ret = methodDescImpl.Invoke(target, 1, "aiueo", DateTime.Now);
            Assert.IsNotNull(ret);
            Assert.AreEqual(typeof(string), ret.GetType());
            Assert.AreEqual(MethodDescTest.RETURN_VAL, ret);
        }

        [Test]
        public void TestInvoke_NoParameter()
        {
            MethodInfo methodInfo = typeof(MethodDescTest).GetMethod("HogeNoParam");
            MethodDescImpl methodDescImpl = new MethodDescImpl(methodInfo);
            MethodDescTest target = new MethodDescTest();

            object ret = methodDescImpl.Invoke(target);
            Assert.IsNotNull(ret);
            Assert.AreEqual(typeof(int), ret.GetType());
            Assert.AreEqual(MethodDescTest.RETURN_VAL_NOPARAM, ret);
        }

        #region テスト用クラス

        private class MethodDescTest
        {
            public const string RETURN_VAL = "Huga";
            public const int RETURN_VAL_NOPARAM = 39;

            public string Hoge(int no, string name, DateTime? date)
            {
                return RETURN_VAL;
            }

            public int HogeNoParam()
            {
                return RETURN_VAL_NOPARAM;
            }

            public virtual string HogeOverridable(int no, string name, DateTime? date)
            {
                return RETURN_VAL;
            }
        }

        private interface IMethodDescTest
        {
            string Hoge(int no, string name, DateTime? date);
        }

        private abstract class AbstractMethodDescTest
        {
            public abstract string Hoge(int no, string name, DateTime? date);
        }

        #endregion
    }


}
