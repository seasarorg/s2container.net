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

using MbUnit.Framework;
using Seasar.Framework.Message;
using Seasar.Quill;

namespace Seasar.Tests.Quill.Resouces
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
            const string EXPECTED_MESSAGE = "S2Containerにコンポーネント\"test\"が存在しません";

            //  ここでは「既定の名前空間」が設定されている場合のテストのみ行います。
            string message = MessageFormatter.GetSimpleMessage(
                "EQLL0010", new object[] { "test" }, typeof(QuillContainer).Assembly, "Seasar.Quill");
            Assert.AreEqual(EXPECTED_MESSAGE, message, "メッセージリソースを取得出来る事");
            message = MessageFormatter.GetSimpleMessage(
                "EQLL0010", new object[] { "test" }, typeof(QuillContainer).Assembly);
            Assert.AreEqual(EXPECTED_MESSAGE, message, "メッセージリソースを取得出来る事");
        }

        [Test]
        public void TestImageResource()
        {
            QuillControl control = new QuillControl();
            Assert.AreEqual("QuillControl", control.Name, 
                "例外が発生することなく画像リソースを使ったコントロールを生成できているか");
        }

        /// <summary>
        /// QuillControlを積んだフォームを表示、インジェクションできるか
        /// </summary>
        [Test]
        public void TestShowDialogWithQuillControl()
        {
            using (FormForQuillControlTest f = new FormForQuillControlTest())
            {
                f.Show();
                f.Close();
                Assert.IsNotNull(f.Prop, "インジェクションが行われているか");
                Assert.AreEqual("QuillControlProp", f.Prop.GetInfo());
            }
        }
    }
}
