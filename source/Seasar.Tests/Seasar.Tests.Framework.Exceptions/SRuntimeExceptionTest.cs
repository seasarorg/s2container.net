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
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Framework.Exceptions;

namespace Seasar.Tests.Framework.Exceptions
{
	/// <summary>
	/// SRuntimeExceptionTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class SRuntimeExceptionTest
	{
		[Test]
		public void TestSRuntimeException()
		{
			SRuntimeException ex = new SRuntimeException("ESSR0001", new object[] { "hoge" });
			Assert.AreEqual("ESSR0001", ex.MessageCode);
			Assert.AreEqual(1, ex.Args.Length);
			Assert.AreEqual("hoge", ex.Args[0]);
			Trace.WriteLine(ex.Message);
		}

		[Test]
		public void TestGetCause()
		{
			Exception ex = new NullReferenceException("test");
			SRuntimeException sre = new SRuntimeException("ESSR0017",
				new object[] { ex }, ex);
			Assert.AreEqual(ex, sre.InnerException);
			Trace.WriteLine(sre.Message);
			Trace.WriteLine(sre.StackTrace);
		}
	}
}
