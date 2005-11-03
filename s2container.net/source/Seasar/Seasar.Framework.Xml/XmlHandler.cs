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
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Seasar.Framework.Container;
using Seasar.Framework.Xml;

namespace Seasar.Framework.Xml
{
	/// <summary>
	/// XmlHandler ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public sealed class XmlHandler
	{

		private TagHandlerRule tagHandlerRule_;
		private TagHandlerContext context_ = new TagHandlerContext();
		
		public XmlHandler(TagHandlerRule tagHandlerRule)
		{
			tagHandlerRule_ = tagHandlerRule;
		}

		public TagHandlerContext TagHandlerContext
		{
			get { return context_; }
		}

		public void StartElement(string qName,IAttributes attributes)
		{
			this.AppendBody();
			context_.StartElement(qName);
			this.Start(attributes);
		}

		public void Characters(string text)
		{
			context_.Characters = text;
			this.AppendBody();
		}

		public void EndElement(string qName)
		{
			this.AppendBody();
			this.End();
			context_.EndElement();
		}

		public object Result
		{
			get { return context_.Result; }
		}

		private TagHandler GetTagHandlerByPath()
		{
			return tagHandlerRule_[context_.Path];
		}

		private TagHandler GetTagHandlerByQName()
		{
			return tagHandlerRule_[context_.QName];
		}

		private void Start(IAttributes attributes)
		{
			TagHandler th = this.GetTagHandlerByPath();
			this.Start(th,attributes);
			th = this.GetTagHandlerByQName();
			this.Start(th,attributes);
		}

		private void Start(TagHandler handler, IAttributes attributes)
		{
			if(handler != null)
			{
				try
				{
					handler.Start(context_,attributes);
				} 
				catch(Exception ex)
				{
					this.ReportDetailPath(ex);
					throw ex;
				}
			}
		}

		private void AppendBody()
		{
			string characters = context_.Characters;
			if(characters.Length > 0)
			{
				TagHandler th = this.GetTagHandlerByPath();
				this.AppendBody(th,characters);
				th = this.GetTagHandlerByQName();
				this.AppendBody(th,characters);
				context_.ClearCharacters();
			}
		}

		private void AppendBody(TagHandler handler, string characters)
		{
			if(handler != null)
			{
				try
				{
					handler.AppendBody(context_, characters);
				}
				catch(Exception ex)
				{
					this.ReportDetailPath(ex);
					throw ex;
				}
			}
		}

		private void End()
		{
			string body = context_.Body;
			TagHandler th = this.GetTagHandlerByPath();
			this.End(th,body);
			th = this.GetTagHandlerByQName();
			this.End(th,body);
		}

		private void End(TagHandler handler, string body)
		{
			if(handler != null)
			{
				try
				{
					handler.End(context_,body);
				}
				catch(Exception ex)
				{
					this.ReportDetailPath(ex);
					throw ex;
				}
			}
		}

		private void ReportDetailPath(Exception cause)
		{
			Console.WriteLine("Exception occured at " + context_.DetailPath);
			Console.WriteLine(cause.Message);
			Console.WriteLine(cause.StackTrace);
		}

	}
}
