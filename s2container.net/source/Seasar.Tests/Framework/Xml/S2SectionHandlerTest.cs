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
using Seasar.Framework.Xml;

namespace Seasar.Tests.Framework.Xml
{
    [TestFixture]
    public class S2SectionHandlerTest
    {
        [Test]
        public void Test()
        {
            S2Section section = S2SectionHandler.GetS2Section();

            Assert.AreEqual("Seasar.Tests.test.dicon", section.ConfigPath);
            
            //  SQLServer以外を使う場合は設定上６以上になるためコメントアウト
            //Assert.AreEqual(5, section.Assemblys.Count);

            string[] expects = new string[] { "Seasar.Tests", "Seasar", "Seasar.DynamicProxy", "Seasar.Dao", "Seasar.Dxo" };
            for(int i = 0; i < expects.Length; i++ ){
                Assert.AreEqual(expects[i], section.Assemblys[i].ToString());
            }
        }
    }
}
