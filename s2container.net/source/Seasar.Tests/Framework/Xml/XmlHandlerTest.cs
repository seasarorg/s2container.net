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
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Text;
using MbUnit.Framework;
using Seasar.Framework.Xml;
using Seasar.Framework.Util;

namespace Seasar.Tests.Framework.Xml
{
    [TestFixture]
    public class XmlHandlerTest
    {
        private const string XML_FILE_NAME = "Seasar.Tests.Framework.Xml.test1.xml";
        private TagHandlerRule _rule;
        private Assembly _asm;

        [SetUp]
        public void SetUp()
        {
            _rule = new TagHandlerRule();
            _asm = Assembly.GetExecutingAssembly();
        }

        [Test]
        public void TestStart()
        {
            _rule["/tag1"] = new TagHandler1();
            var handler = new XmlHandler(_rule);
            var parser = new XmlHandlerParser(handler);
            StreamReader stream = null;
            try
            {
                stream = ResourceUtil.GetResourceAsStreamReader(XML_FILE_NAME, _asm);
                Assert.AreEqual("aaa", parser.Parse(stream));
            }
            finally
            {
                if (stream != null) stream.Close();
            }
        }

        public class TagHandler1 : TagHandler
        {
            public override void Start(TagHandlerContext ctx,
                IAttributes attributes)
            {
                ctx.Push(attributes["attr1"]);
            }
        }

        [Test]
        public void TestAppendBody()
        {
            var buf = new StringBuilder();
            var th2 = new TagHandler2();
            th2.Buf = buf;
            _rule["/tag1"] = th2;
            var handler = new XmlHandler(_rule);
            var parser = new XmlHandlerParser(handler);
            StreamReader stream = null;
            try
            {
                stream = ResourceUtil.GetResourceAsStreamReader(XML_FILE_NAME, _asm);
                parser.Parse(stream);
            }
            finally
            {
                if (stream != null) stream.Close();
            }
            Trace.WriteLine(buf);
            Assert.AreEqual("[111 222][333]", buf.ToString());
        }

        public class TagHandler2 : TagHandler
        {
            private StringBuilder buf_;

            public override void AppendBody(TagHandlerContext context, string body)
            {
                buf_.Append("[" + body + "]");
            }

            public StringBuilder Buf
            {
                set { buf_ = value; }
            }
        }

        [Test]
        public void TestAppendBody2()
        {
            var buf = new StringBuilder();
            var th2 = new TagHandler2();
            th2.Buf = buf;
            _rule["tag1"] = th2;
            var handler = new XmlHandler(_rule);
            var parser = new XmlHandlerParser(handler);
            StreamReader stream = null;
            try
            {
                stream = ResourceUtil.GetResourceAsStreamReader(XML_FILE_NAME, _asm);
                parser.Parse(stream);
            }
            finally
            {
                if (stream != null) stream.Close();
            }
            Trace.WriteLine(buf);
            Assert.AreEqual("[111 222][333]", buf.ToString());
        }

        [Test]
        public void TestAppendBody3()
        {
            var buf = new StringBuilder();
            var th2 = new TagHandler2();
            th2.Buf = buf;
            _rule["/tag1/tag3/tag4"] = th2;
            var handler = new XmlHandler(_rule);
            var parser = new XmlHandlerParser(handler);
            StreamReader stream = null;
            try
            {
                stream = ResourceUtil.GetResourceAsStreamReader(XML_FILE_NAME, _asm);
                parser.Parse(stream);
            }
            finally
            {
                if (stream != null) stream.Close();
            }
            Trace.WriteLine(buf);
            Assert.AreEqual("[eee]", buf.ToString());
        }

        [Test]
        public void TestEnd()
        {
            _rule["/tag1/tag2"] = new TagHandler3();
            var handler = new XmlHandler(_rule);
            var parser = new XmlHandlerParser(handler);
            var result = parser.Parse(XML_FILE_NAME);
            Assert.AreEqual("c c", result);
        }

        public class TagHandler3 : TagHandler
        {
            public override void End(TagHandlerContext context, string body)
            {
                context.Push(body);
            }
        }

        [Test]
        public void TestException()
        {
            _rule["/tag1/tag3"] = new TagHandler4();
            var handler = new XmlHandler(_rule);
            var parser = new XmlHandlerParser(handler);
            var stream = ResourceUtil.GetResourceAsStreamReader(XML_FILE_NAME, _asm);
            try
            {
                parser.Parse(stream);
                Assert.Fail();
            }
            catch (ApplicationException ex)
            {
                Trace.WriteLine(ex);
            }
            finally
            {
                stream.Close();
            }
        }

        public class TagHandler4 : TagHandler
        {
            public override void Start(TagHandlerContext context, IAttributes attributes)
            {
                throw new ApplicationException("testException");
            }
        }

        [Test]
        public void TestTagMatching()
        {
            TagHandler th = new TagHandler5();
            _rule["tag1"] = th;
            _rule["tag2"] = th;
            _rule["tag3"] = th;
            _rule["tag4"] = th;
            _rule["tag5"] = th;
            Trace.WriteLine("tagMatching");
            var handler = new XmlHandler(_rule);
            var parser = new XmlHandlerParser(handler);
            var stream = ResourceUtil.GetResourceAsStreamReader(XML_FILE_NAME, _asm);
            try
            {
                parser.Parse(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        public class TagHandler5 : TagHandler
        {
            public override void Start(TagHandlerContext context, IAttributes attributes)
            {
                Trace.WriteLine(context.DetailPath);
            }
        }
    }
}
