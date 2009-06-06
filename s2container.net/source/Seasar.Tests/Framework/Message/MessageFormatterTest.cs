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

using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using MbUnit.Framework;
using Seasar.Framework.Message;

namespace Seasar.Tests.Framework.Message
{
    [TestFixture]
    public class MessageFormatterTest
    {
        [Test]
        public void TestGetMessage()
        {
            string message = MessageFormatter.GetMessage("ESSR0001", new object[] { "test" });
            Assert.AreEqual("[ESSR0001] testが見つかりません", message, "メッセージリソースを取得出来る事");
        }

        [Test]
        public void TestGetMessage2()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string message = MessageFormatter.GetMessage("ETST0001", new object[] { "test" }, asm, "Seasar.Tests");
            ResourceManager rm = new ResourceManager("TSTMessages", asm);
            try
            {
                Trace.WriteLine(rm.GetString("ETST0001"));
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
            Assert.AreEqual("[ETST0001] test message", message, "メッセージリソースを取得出来る事");
        }

        [Test]
        public void TestGetMessage3()
        {
            //  ここでは「既定の名前空間」が設定されている場合のテストのみ行います。
            string message = MessageFormatter.GetMessage("ESSR0001", new object[] { "test" }, "Seasar.Tests");
            Assert.AreEqual("[ESSR0001] testが見つかりません", message, "メッセージリソースを取得出来る事");
            message = MessageFormatter.GetMessage("ESSR0001", new object[] { "test" });
            Assert.AreEqual("[ESSR0001] testが見つかりません", message, "メッセージリソースを取得出来る事");
        }
    }
}
