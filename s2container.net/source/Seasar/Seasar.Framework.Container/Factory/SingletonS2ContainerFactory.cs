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
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;
using Seasar.Framework.Xml;

namespace Seasar.Framework.Container.Factory
{
	/// <summary>
	/// SingletonS2ContainerFactory ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public sealed class SingletonS2ContainerFactory
	{
		private static string configPath_ = "app.dicon";
		private static IS2Container container_;

		private SingletonS2ContainerFactory()
		{
		}

        static SingletonS2ContainerFactory()
        {
            S2Section config = S2SectionHandler.GetS2Section();
            if (config != null)
            {
                string configPath = config.ConfigPath;
                if (!StringUtil.IsEmpty(configPath)) configPath_ = configPath;
            }
        }

		public static string ConfigPath
		{
			set { configPath_ = value; }
			get { return configPath_; }
		}

		public static void Init()
		{
			container_ = S2ContainerFactory.Create(configPath_);
			container_.Init();
		}

		public static void Destroy()
		{
			container_.Destroy();
			container_ = null;
		}

		public static IS2Container Container
		{
			get 
			{
				if(container_ == null)
					throw new EmptyRuntimeException("S2Container");
				return container_;
			}
			set
			{
				container_ = value;
			}
		}

		public static bool HasContainer
		{
			get { return container_ != null; }
		}

	}
}
