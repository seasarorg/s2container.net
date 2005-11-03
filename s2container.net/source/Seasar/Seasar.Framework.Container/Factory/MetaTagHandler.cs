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
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Xml;
using Seasar.Framework.Util;

namespace Seasar.Framework.Container.Factory
{
	/// <summary>
	/// MetaTagHandler ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class MetaTagHandler : TagHandler
	{

		public override void Start(TagHandlerContext context, IAttributes attributes)
		{
			string name = attributes["name"];
			context.Push(new MetaDefImpl(name));
		}

		public override void End(TagHandlerContext context, string body)
		{
			IMetaDef metaDef = (IMetaDef) context.Pop();
			if(!StringUtil.IsEmpty(body)) metaDef.Expression = body;
			IMetaDefAware metaDefAware = (IMetaDefAware) context.Peek();
			metaDefAware.AddMetaDef(metaDef);
		}

	}
}
