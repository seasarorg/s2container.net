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
using Seasar.Quill.Util;
using Seasar.Quill;

namespace Seasar.Tests.Quill.Util
{
    [TestFixture]
	public class MessageUtilTest
    {
        #region GetSimpleMessageのテスト

        [Test]
        public void TestGetSimpleMessage_メッセージが見つからない場合()
        {
            try
            {
                string message = MessageUtil.GetSimpleMessage("hoge", null);
                Assert.Fail();
            }
            catch (QuillApplicationException ex)
            {
                Assert.AreEqual("[EQLL0000]message not found.", ex.Message);
            }
        }

        [Test]
        public void TestGetSimpleMessage_メッセージ中に埋め込む値がnullの場合()
        {
            string message = MessageUtil.GetSimpleMessage("EQLL0009", null);

            Assert.AreEqual("S2Containerが作成されていません。" +
                "SingletonS2ContainerFactoryからS2Containerが作成してください", message);
        }

        [Test]
        public void TestGetSimpleMessage_メッセージ中に埋め込む値がnullでないの場合()
        {
            string message = MessageUtil.GetSimpleMessage("EQLL0010",
                new object[] { "hoge" });

            Assert.AreEqual(
                "S2Containerにコンポーネント\"hoge\"が存在しません", message);
        }

        #endregion

        #region GetMessageのテスト

        [Test]
        public void TestGetMessage()
        {
            string message = MessageUtil.GetMessage("EQLL0010",
                new object[] { "hoge" });

            Assert.AreEqual(
                "[EQLL0010]S2Containerにコンポーネント\"hoge\"が存在しません", message);
        }

        #endregion
    }
}
