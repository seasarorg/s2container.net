#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
using System.Reflection;
using System.IO;
using NUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using log4net;
using log4net.Config;
using log4net.Util;

namespace Seasar.Tests.Framework.Container.Factory
{
	/// <summary>
	/// AspectTagHandlerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class AspectTagHandlerTest
	{
		private const string PATH 
            = "Seasar/Tests/Framework/Container/Factory/AspectTagHandlerTest.dicon";

		[SetUp]
		public void SetUp()
		{
			FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
				Assembly.GetExecutingAssembly()) + ".config");
			XmlConfigurator.Configure(LogManager.GetRepository(), info);
		}

		[Test]
		public void TestAspect()
		{
			IS2Container container = S2ContainerFactory.Create(PATH);
			
			IList list = (IList) container.GetComponent(typeof(IList));
			int count = list.Count;

			IFoo foo = (IFoo) container.GetComponent(typeof(IFoo));
			int time = foo.Time;
			foo.ToString();
			int hashCode = foo.GetHashCode();
		}

		public interface IFoo
		{
			int Time { get; }
			int GetHashCode();
			string ToString();
		}

		public class FooImpl : IFoo
		{
			private int time_ = 3;

			public FooImpl()
			{
			}

			#region IFoo ÉÅÉìÉo

			public int Time
			{
				get
				{
					return time_;
				}
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			public override string ToString()
			{
				return time_.ToString();
			}

			#endregion
		}
	}
}
