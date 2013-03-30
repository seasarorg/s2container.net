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

using MbUnit.Framework;
using Seasar.Dao.Pager;
using Seasar.Extension.ADO;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Pager
{
    [TestFixture]
    public class PagerDataReaderFactoryTopWrapperTest : S2TestCase
    {
        private MockDataReaderFactory _original;
        private MockPagerDataReaderFactoryTopWrapper _wrapper;

        [SetUp]
        public void SetUp()
        {
            _original = new MockDataReaderFactory();
            _wrapper = new MockPagerDataReaderFactoryTopWrapper(_original, null);
        }

        [Test]
        public void MakeTopSql()
        {
            Assert.AreEqual(
                "SELECT TOP 65 * FROM DEPARTMENT",
                _wrapper.MockMakeTopSql("SELECT * FROM DEPARTMENT", 10, 55)
                );
        }
    }

    internal class MockPagerDataReaderFactoryTopWrapper : PagerDataReaderFactoryTopWrapper
    {
        public MockPagerDataReaderFactoryTopWrapper(
            IDataReaderFactory dataReaderFactory,
            ICommandFactory commandFactory
            )
            : base(dataReaderFactory, commandFactory)
        {
        }

        public string MockMakeTopSql(string baseSql, int limit, int offset)
        {
            return MakeTopSql(baseSql, limit, offset);
        }
    }
}
