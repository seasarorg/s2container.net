#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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

using MbUnit.Framework;
using Seasar.Framework.Message;
using Seasar.Windows.AOP.Interceptors;

namespace Seasar.Tests.Windows.Resources
{
    /// <summary>
    /// リソースが正常に設定されているか確認するテストクラス
    /// </summary>
    [TestFixture]
    public class ResourceEnableTest
    {
        /// <summary>
        /// メッセージリソースが正常に取得できているか確認するためのテスト
        /// </summary>
        [Test]
        public void TestGetMessage()
        {
            const string EXPECTED_MESSAGE = "引数 testが0より小さいです。";

            //  ここでは「既定の名前空間」が設定されている場合のテストのみ行います。
            string message = MessageFormatter.GetSimpleMessage(
                "FSWF0002", new object[] { "test" }, typeof(FormInterceptor).Assembly, "Seasar.Windows");
            Assert.AreEqual(EXPECTED_MESSAGE, message, "メッセージリソースを取得出来る事");
            message = MessageFormatter.GetSimpleMessage(
                "FSWF0002", new object[] { "test" }, typeof(FormInterceptor).Assembly);
            Assert.AreEqual(EXPECTED_MESSAGE, message, "メッセージリソースを取得出来る事");
        }
    }
}