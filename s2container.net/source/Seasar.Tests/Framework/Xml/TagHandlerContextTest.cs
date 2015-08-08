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
using System.Collections;
using MbUnit.Framework;
using Seasar.Framework.Xml;

namespace Seasar.Tests.Framework.Xml
{
    [TestFixture]
    public class TagHandlerContextTest
    {
        [Test]
        public void TestStartElementAndEndElement()
        {
            var ctx = new TagHandlerContext();
            ctx.StartElement("aaa");
            Assert.AreEqual("/aaa", ctx.Path);
            Assert.AreEqual("/aaa[1]", ctx.DetailPath);
            Assert.AreEqual("aaa", ctx.QName);

            ctx.StartElement("bbb");
            Assert.AreEqual("/aaa/bbb", ctx.Path);
            Assert.AreEqual("/aaa[1]/bbb[1]", ctx.DetailPath);
            Assert.AreEqual("bbb", ctx.QName);

            ctx.EndElement();
            Assert.AreEqual("/aaa", ctx.Path);
            Assert.AreEqual("/aaa[1]", ctx.DetailPath);
            Assert.AreEqual("aaa", ctx.QName);

            ctx.StartElement("bbb");
            Assert.AreEqual("/aaa/bbb", ctx.Path);
            Assert.AreEqual("/aaa[1]/bbb[2]", ctx.DetailPath);
            Assert.AreEqual("bbb", ctx.QName);
        }

        [Test]
        public void TestPeek()
        {
            var ctx = new TagHandlerContext();
            ctx.Push("aaa");
            ctx.Push(new ArrayList());
            ctx.Push("bbb");
            Assert.IsNotNull(ctx.Peek(typeof(IList)));
            Assert.IsNull(ctx.Peek(typeof(Int32)));
            Assert.AreEqual("bbb", ctx.Peek(typeof(string)));
        }
    }
}
