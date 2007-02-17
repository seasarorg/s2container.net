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
using Seasar.Framework.Xml;

namespace Seasar.Framework.Container.Factory
{
	/// <summary>
	/// XmlS2ContainerBuilder の概要の説明です。
	/// </summary>
	public class XmlS2ContainerBuilder : IS2ContainerBuilder
	{
		private static S2ContainerTagHandlerRule rule_ = new S2ContainerTagHandlerRule();
		
		#region IS2ContainerBuilder メンバ

		public IS2Container Build(string path)
		{
			XmlHandlerParser parser = this.CreateXmlHandlerParser(null,path);
			return (IS2Container) parser.Parse(path);
		}

		public IS2Container Include(IS2Container parent, string path)
		{
			XmlHandlerParser parser = this.CreateXmlHandlerParser(parent, path);
			IS2Container child = (IS2Container) parser.Parse(path);
			parent.Include(child);
			return child;
		}

		#endregion

		private XmlHandlerParser CreateXmlHandlerParser(IS2Container parent, string path)
		{
			XmlHandler handler = new XmlHandler(rule_);
			TagHandlerContext ctx = handler.TagHandlerContext;
			ctx.AddParameter("parent",parent);
			ctx.AddParameter("path",path);
			return new XmlHandlerParser(handler);
		}
	}
}
