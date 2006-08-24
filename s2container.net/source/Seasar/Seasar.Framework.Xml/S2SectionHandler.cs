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
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using Seasar.Framework.Container;

namespace Seasar.Framework.Xml
{
	/// <summary>
	/// S2.NETの構成セクションハンドラクラスです。
	/// </summary>
	public class S2SectionHandler : IConfigurationSectionHandler
	{
		public S2SectionHandler()
		{
		}

		public static S2Section GetS2Section()
		{
#if NET_1_1
            return (S2Section) ConfigurationSettings.GetConfig(
                ContainerConstants.SEASAR_CONFIG);
#else
			return (S2Section) ConfigurationManager.GetSection(
				ContainerConstants.SEASAR_CONFIG);
#endif
		}

		#region IConfigurationSectionHandler メンバ

		public object Create(object parent, object configContext,
			System.Xml.XmlNode section)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(S2Section));
			return serializer.Deserialize(new XmlNodeReader(section));
		}

		#endregion

	}
}
