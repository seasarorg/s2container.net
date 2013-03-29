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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MbUnit.Framework;
using Seasar.Extension.DataSets.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Extension.DataSets.Impl
{
    [Ignore("Microsoft.Jet.OLEDB4.0は32bitOSでしか動作しないため動かせず。保留。")]
    [TestFixture]
    public class XlsReaderSortTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Extension.DataSets.Impl.XlsReaderImplSortTest.xls";

        private DataSet dataSet;

        [SetUp]
        public void SetUp()
        {
            using (Stream stream = ResourceUtil.GetResourceAsStream(PATH, Assembly.GetExecutingAssembly()))
            {
                dataSet = new XlsReader(stream).Read();
            }
        }

        [Ignore("Microsoft.Jet.OLEDB4.0は32bitOSでしか動作しないため動かせず。保留。")]
        [Test]
        public void TestCreateTable()
        {
            Assert.AreEqual(4, dataSet.Tables.Count, "1");
            Trace.WriteLine(ToStringUtil.ToString(dataSet));
        }

        [Ignore("Microsoft.Jet.OLEDB4.0は32bitOSでしか動作しないため動かせず。保留。")]
        [Test]
        public void TestSort()
        {
            Assert.AreEqual(dataSet.Tables[0].TableName, "TEST_TABLE", "1");
            Assert.AreEqual(dataSet.Tables[1].TableName, "SECOND_TABLE", "2");
            Assert.AreEqual(dataSet.Tables[2].TableName, "あ", "3");
            Assert.AreEqual(dataSet.Tables[3].TableName, "EMPTY_TABLE", "4");
        }
    }
}
