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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MbUnit.Framework;
using Seasar.Framework.Util;

namespace Seasar.Tests.Framework.Util
{
	[TestFixture]
	public class ResourceUtilTest
	{
		[Test]
		public void TestGetExtension()
		{
			Assert.AreEqual("xml", ResourceUtil.GetExtension("aaa.bbb.xml"));
			Assert.AreEqual(null, ResourceUtil.GetExtension("aaa"));
		}

		[Test]
		public void TestGetResourceAsStream()
		{
			StreamReader stream = ResourceUtil.GetResourceAsStreamReader(
				"Seasar.Tests.Framework.Util.test1.xml", Assembly.GetExecutingAssembly());
			Trace.WriteLine(stream.ReadToEnd());
			stream.Close();
		}

		[Test]
		public void TestResourceNotFound()
		{
			StreamReader stream = null;
			try
			{
				stream = ResourceUtil.GetResourceAsStreamReader(
					"Seasar.Tests.Framework.Util.test2.xml", Assembly.GetExecutingAssembly());
				Assert.Fail();
			}
			catch(ResourceNotFoundRuntimeException ex)
			{
				Trace.WriteLine(ex.Message);
			}
			finally
			{
				if(stream != null) stream.Close();
			}
		}
	}
}
