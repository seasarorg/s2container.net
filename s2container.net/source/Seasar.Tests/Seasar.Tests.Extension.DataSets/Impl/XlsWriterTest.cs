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

using System.Data;
using System.IO;
using System.Reflection;
using MbUnit.Framework;
using Seasar.Extension.DataSets;
using Seasar.Extension.DataSets.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Extension.DataSets.Impl
{
	[TestFixture]
	public class XlsWriterTest : S2TestCase
	{
		private const string PATH = "Seasar.Tests.Extension.DataSets.Impl.XlsReaderImplTest.xls";

		private string path2_;

		private DataSet dataSet_;

		private IDataWriter writer_;

		[SetUp]
		public void SetUp() 
		{
			using (Stream stream = ResourceUtil.GetResourceAsStream(PATH, Assembly.GetExecutingAssembly()))  
			{
				dataSet_ = new XlsReader(stream).Read();
			}
			path2_ = Path.Combine(Path.GetTempPath(), "XlsWriterImplTest.xls");
			writer_ = new XlsWriter(path2_);
		}

		[TearDown]
		public void TearDown() 
		{
			if (File.Exists(path2_)) 
			{
				File.Delete(path2_);
			}
		}

		[Test]
		public void TestWrite() 
		{
			writer_.Write(dataSet_);
		}
	}
}
