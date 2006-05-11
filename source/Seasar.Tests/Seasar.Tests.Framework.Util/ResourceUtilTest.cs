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
using System.Reflection;
using NUnit.Framework;
using Seasar.Framework.Util;

namespace Seasar.Tests.Framework.Util
{
	/// <summary>
	/// ResourceUtilTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
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
			Console.WriteLine(stream.ReadToEnd());
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
				Console.WriteLine(ex.Message);
			}
			finally
			{
				if(stream != null) stream.Close();
			}
		}
	}
}
