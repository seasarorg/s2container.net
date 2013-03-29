#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
    [Ignore("Microsoft.Jet.OLEDB4.0は32bitOSでしか動作しないため動かせず。保留。")]
    [TestFixture]
    public class XlsWriterTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Extension.DataSets.Impl.XlsReaderImplTest.xls";
        private string _path2;
        private DataSet _dataSet;
        private IDataWriter _writer;

        [SetUp]
        public void SetUp()
        {
            using (Stream stream = ResourceUtil.GetResourceAsStream(PATH, Assembly.GetExecutingAssembly()))
            {
                _dataSet = new XlsReader(stream).Read();
            }
            _path2 = Path.Combine(Path.GetTempPath(), "XlsWriterImplTest.xls");
            _writer = new XlsWriter(_path2);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_path2))
            {
                File.Delete(_path2);
            }
        }

        [Ignore("Microsoft.Jet.OLEDB4.0は32bitOSでしか動作しないため動かせず。保留。")]
        [Test]
        public void TestWrite()
        {
            _writer.Write(_dataSet);
        }
    }
}
