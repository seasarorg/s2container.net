#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
using Seasar.Framework.Util;

namespace Seasar.Tests.Dao.Pager
{
    [TestFixture]
    public class PagerDataReaderFactoryWrapperTest : S2TestCase
    {
        private readonly ICommandFactory _commandFactory = null;
        private MockDataReaderFactory _original;
        private PagerDataReaderFactoryWrapper _wrapper;

        [SetUp]
        public void SetUp()
        {
            _original = new MockDataReaderFactory();
            _wrapper = new PagerDataReaderFactoryWrapper(_original, _commandFactory);
        }

        [Test]
        public void TestCreateDataReaderNotPagerCondition()
        {
            try
            {
                PagerContext.GetContext().PushArgs(null);
                var reader = _wrapper.CreateDataReader(null, null);
                Assert.AreEqual(1, _original.CreatedDataReaderCount);
                Assert.AreSame(_original.GetCreatedDataReader(0), reader);
            }
            finally
            {
                PagerContext.GetContext().PopArgs();
            }
        }

        [Test]
        public void TestCreateDataReaderPagerCondition()
        {
            try
            {
                PagerContext.GetContext().PushArgs(CreatePagerCondition());
                var reader = _wrapper.CreateDataReader(null, null);
                Assert.AreEqual(1, _original.CreatedDataReaderCount);
                Assert.AreEqual(typeof(PagerDataReaderWrapper), reader.GetExType());
            }
            finally
            {
                PagerContext.GetContext().PopArgs();
            }
        }

        [Test]
        public void TestCreateDataReaderSequence()
        {
            try
            {
                PagerContext.GetContext().PushArgs(CreatePagerCondition());
                PagerContext.GetContext().PushArgs(null);
                var reader = _wrapper.CreateDataReader(null, null);
                Assert.AreEqual(1, _original.CreatedDataReaderCount);
                Assert.AreSame(_original.GetCreatedDataReader(0), reader);
            }
            finally
            {
                PagerContext.GetContext().PopArgs();
                try
                {
                    var reader = _wrapper.CreateDataReader(null, null);
                    Assert.AreEqual(2, _original.CreatedDataReaderCount);
                    Assert.AreEqual(typeof(PagerDataReaderWrapper), reader.GetExType());
                }
                finally
                {
                    PagerContext.GetContext().PopArgs();
                }
            }
        }

        private DefaultPagerCondition CreatePagerCondition()
        {
            return new DefaultPagerCondition();
        }
    }
}
