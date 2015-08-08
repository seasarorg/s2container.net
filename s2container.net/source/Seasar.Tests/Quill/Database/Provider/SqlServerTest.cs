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

using System.Data.SqlClient;
using MbUnit.Framework;
using Seasar.Framework.Util;
using Seasar.Quill.Database.Provider;

namespace Seasar.Tests.Quill.Database.Provider
{
    [TestFixture]
    public class SqlServerTest
    {
        [Test]
        public void TestCommandType()
        {
            var provider = new SqlServer();
            var instance = ClassUtil.NewInstance(ClassUtil.ForName(provider.CommandType));
            Assert.IsNotNull(instance);
            Assert.IsTrue(instance is SqlCommand);
        }

        [Test]
        public void TestConnectionType()
        {
            var provider = new SqlServer();
            var instance = ClassUtil.NewInstance(ClassUtil.ForName(provider.ConnectionType));
            Assert.IsNotNull(instance);
            Assert.IsTrue(instance is SqlConnection);
        }

        [Test]
        public void TestDataAdapterType()
        {
            var provider = new SqlServer();
            var instance = ClassUtil.NewInstance(ClassUtil.ForName(provider.DataAdapterType));
            Assert.IsNotNull(instance);
            Assert.IsTrue(instance is SqlDataAdapter);
        }

        [Test]
        public void TestParameterType()
        {
            var provider = new SqlServer();
            var instance = ClassUtil.NewInstance(ClassUtil.ForName(provider.ParameterType));
            Assert.IsNotNull(instance);
            Assert.IsTrue(instance is SqlParameter);
        }
    }
}
