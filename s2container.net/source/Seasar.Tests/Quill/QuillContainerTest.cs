#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using Seasar.Quill;
using Seasar.Framework.Aop;
using Seasar.Quill.Attrs;

namespace Seasar.Tests.Quill
{
    [TestFixture]
	public class QuillContainerTest
    {
        #region GetComponentのテスト

        [Test]
        public void TestGetComponent_インターフェースにAspectが適用されていない場合()
        {
            QuillContainer container = new QuillContainer();

            try
            {
                container.GetComponent(typeof(Hoge1), typeof(Hoge1));
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("EQLL0008", ex.MessageCode);
            }
        }

        [Test]
        public void TestGetComponent_正常な場合()
        {
            QuillContainer container = new QuillContainer();
            QuillComponent component = container.GetComponent(typeof(Hoge2));
            QuillComponent component2 = container.GetComponent(typeof(Hoge2));
            Assert.AreEqual(typeof(Hoge2), component.ReceiptType);
            Assert.AreEqual(component.GetComponentObject(typeof(Hoge2)),
                component2.GetComponentObject(typeof(Hoge2)));
        }

        #endregion

        #region GetComponentのテストで使用する内部クラス・インターフェース

        public interface Hoge1
        {
            void Fuga();
        }

        [Aspect(typeof(HogeInterceptor1))]
        public interface Hoge2
        {
            void Fuga();
        }

        public class HogeInterceptor1 : IMethodInterceptor
        {
            public object Invoke(IMethodInvocation invocation)
            {
                return null;
            }
        }
        
        #endregion

    }
}
