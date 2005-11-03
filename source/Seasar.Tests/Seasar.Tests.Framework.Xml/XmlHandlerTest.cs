#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
using System.Reflection;
using System.IO;
using System.Text;
using NUnit.Framework;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Xml;
using Seasar.Framework.Util;

namespace TestSeasar.Framework.Xml
{
	/// <summary>
	/// XmlHandlerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class XmlHandlerTest
	{
		private const string XML_FILE_NAME = 
			"Seasar.Tests.Framework.Xml.test1.xml";

		private TagHandlerRule rule_;
		private Assembly asm_;

		[SetUp]
		public void SetUp()
		{
			rule_ = new TagHandlerRule();
			asm_ = Assembly.GetExecutingAssembly();
		}

		[Test]
		public void TestStart()
		{
			rule_["/tag1"] = new TagHandler1();
			XmlHandler handler = new XmlHandler(rule_);
			XmlHandlerParser parser = new XmlHandlerParser(handler);
			StreamReader stream = null;
			try
			{
				stream = ResourceUtil.GetResourceAsStreamReader(XML_FILE_NAME,asm_);
				Assert.AreEqual("aaa", parser.Parse(stream));
			}
			finally
			{
				if(stream != null) stream.Close();
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
			StringBuilder buf = new StringBuilder();
			TagHandler2 th2 = new TagHandler2();
			th2.Buf = buf;
			rule_["/tag1"] = th2;
			XmlHandler handler = new XmlHandler(rule_);
			XmlHandlerParser parser = new XmlHandlerParser(handler);
			StreamReader stream = null;
			try
			{
				stream = ResourceUtil.GetResourceAsStreamReader(XML_FILE_NAME,asm_);
				parser.Parse(stream);
			}
			finally
			{
				if(stream != null) stream.Close();
			}
			Console.WriteLine(buf);
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
			StringBuilder buf = new StringBuilder();
			TagHandler2 th2 = new TagHandler2();
			th2.Buf = buf;
			rule_["tag1"] = th2;
			XmlHandler handler = new XmlHandler(rule_);
			XmlHandlerParser parser = new XmlHandlerParser(handler);
			StreamReader stream = null;
			try
			{
				stream = ResourceUtil.GetResourceAsStreamReader(XML_FILE_NAME,asm_);
				parser.Parse(stream);
			}
			finally
			{
				if(stream != null) stream.Close();
			}
			Console.WriteLine(buf);
			Assert.AreEqual("[111 222][333]", buf.ToString());
		}

		[Test]
		public void TestAppendBody3()
		{
			StringBuilder buf = new StringBuilder();
			TagHandler2 th2 = new TagHandler2();
			th2.Buf = buf;
			rule_["/tag1/tag3/tag4"] = th2;
			XmlHandler handler = new XmlHandler(rule_);
			XmlHandlerParser parser = new XmlHandlerParser(handler);
			StreamReader stream = null;
			try
			{
				stream = ResourceUtil.GetResourceAsStreamReader(XML_FILE_NAME,asm_);
				parser.Parse(stream);
			}
			finally
			{
				if(stream != null) stream.Close();
			}
			Console.WriteLine(buf);
			Assert.AreEqual("[eee]", buf.ToString());
		}

		[Test]
		public void TestEnd()
		{
			rule_["/tag1/tag2"] = new TagHandler3();
			XmlHandler handler = new XmlHandler(rule_);
			XmlHandlerParser parser = new XmlHandlerParser(handler);
			object result = parser.Parse(XML_FILE_NAME);
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
			rule_["/tag1/tag3"] = new TagHandler4();
			XmlHandler handler = new XmlHandler(rule_);
			XmlHandlerParser parser = new XmlHandlerParser(handler);
			StreamReader stream = ResourceUtil.GetResourceAsStreamReader(XML_FILE_NAME,asm_);
			try
			{
				parser.Parse(stream);
				Assert.Fail();
			}
			catch(ApplicationException ex)
			{
				Console.WriteLine(ex);
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
			rule_["tag1"] = th;
			rule_["tag2"] = th;
			rule_["tag3"] = th;
			rule_["tag4"] = th;
			rule_["tag5"] = th;
			Console.WriteLine("tagMatching");
			XmlHandler handler = new XmlHandler(rule_);
			XmlHandlerParser parser = new XmlHandlerParser(handler);
			StreamReader stream = ResourceUtil.GetResourceAsStreamReader(XML_FILE_NAME,asm_);
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
				Console.WriteLine(context.DetailPath);
			}
		}
	}
}
