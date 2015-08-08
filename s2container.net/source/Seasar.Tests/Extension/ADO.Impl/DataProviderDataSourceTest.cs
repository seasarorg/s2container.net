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

using System.Data;
using MbUnit.Framework;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.ADO.Impl
{
    [TestFixture]
    public class DataProviderDataSourceTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Ado.dicon";

        public void SetUpGetCommand()
        {
            Include(PATH);
        }

        [Test, S2]
        public void GetCommand()
        {
            using (var cmd = DataSource.GetCommand())
            {
                Assert.IsNotNull(cmd);
            }
        }

        public void SetUpGetParameter()
        {
            Include(PATH);
        }

        [Test, S2]
        public void GetParameter()
        {
            var param = DataSource.GetParameter();
            Assert.IsNotNull(param);
        }

        public void SetUpGetDataAdapter()
        {
            Include(PATH);
        }

        [Test, S2]
        public void GetDataAdapter()
        {
            var da = DataSource.GetDataAdapter();
            Assert.IsNotNull(da);
        }
    }
}
