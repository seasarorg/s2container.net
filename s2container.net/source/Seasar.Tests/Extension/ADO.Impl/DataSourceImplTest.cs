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
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.ADO.Impl
{
    [TestFixture]
    public class DataSourceImplTest : S2TestCase
    {
        [Test, S2]
        public void ConstructorDataProviderDataSource()
        {
            DataSourceImpl ds = new DataSourceImpl(new DataProvider(), "dummy");
            Assert.IsInstanceOfType(typeof(DataProviderDataSource), ds.Instance);
        }

        [Test, S2]
        public void ConstructorDbProviderFactoryDataSource()
        {
            DataSourceImpl ds = new DataSourceImpl("provider");
            Assert.IsInstanceOfType(typeof(DbProviderFactoryDataSource), ds.Instance);
        }
    }
}
