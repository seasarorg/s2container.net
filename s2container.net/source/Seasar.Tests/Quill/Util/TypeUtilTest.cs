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
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Quill.Util;

namespace Seasar.Tests.Quill.Util
{
    [TestFixture]
	public class TypeUtilTest
    {
        #region GetTypeのテスト

        [Test]
        public void TestGetType_透過プロキシの場合()
        {
            Hoge hoge = new Hoge();
            IAspect aspect = new AspectImpl(new TraceInterceptor());

            AopProxy aopProxy = new AopProxy(typeof(Hoge),
                new IAspect[] { aspect }, null, hoge);
            object ret = aopProxy.Create();

            Type type = TypeUtil.GetType(ret);

            Assert.AreEqual(typeof(Hoge), type);
        }

        [Test]
        public void TestGetType_透過プロキシでない場合()
        {
            Hoge hoge = new Hoge();

            Type type = TypeUtil.GetType(hoge);

            Assert.AreEqual(typeof(Hoge), type);
        }

        #endregion

        #region GetTypeのテストで使用する内部クラス

        private class Hoge : MarshalByRefObject
        {
        }

        #endregion
    }
}
