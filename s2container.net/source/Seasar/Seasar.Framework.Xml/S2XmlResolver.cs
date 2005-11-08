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
using System.Collections;
using System.IO;
using System.Xml;
using System.Reflection;
using Seasar.Framework.Util;

namespace Seasar.Framework.Xml
{
	/// <summary>
	/// S2XmlResolver �̊T�v�̐����ł��B
	/// </summary>
	public class S2XmlResolver : XmlResolver
	{
		public const string PUBLIC_ID = "-/SEASAR/DTD S2Container/EN";
		public const string PUBLIC_ID21 = "-/SEASAR2.1/DTD S2Container/EN";

		public const string COMPONENTS_URI = "http://www.seasar.org/dtd/components.dtd";
		public const string COMPONENTS_URI21 = "http://www.seasar.org/dtd/components21.dtd";

		public const string COMPONENTS_PATH = "components.dtd";
		public const string COMPONENTS_PATH21 = "components21.dtd";

		public S2XmlResolver()
		{
		}

		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			Stream stream = null;

			if(absoluteUri.AbsoluteUri.EndsWith(PUBLIC_ID) ||
				COMPONENTS_URI.Equals(absoluteUri.AbsoluteUri))
			{
				stream = ResourceUtil.GetResourceAsStream(COMPONENTS_PATH, Assembly.GetExecutingAssembly());
			}
			else if(absoluteUri.AbsoluteUri.EndsWith(PUBLIC_ID21) ||
				COMPONENTS_URI21.Equals(absoluteUri.AbsoluteUri))
			{
				stream = ResourceUtil.GetResourceAsStream(COMPONENTS_PATH21, Assembly.GetExecutingAssembly());
			}
			
			return stream;
		}

		public override System.Net.ICredentials Credentials
		{
			set
			{
			}
		}


	}
}