#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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

using Seasar.Dao.Dbms;
using Seasar.Extension.ADO;
using Seasar.Extension.Unit;
using MbUnit.Framework;

namespace Seasar.Tests.Dao.Dbms
{
    [TestFixture]
    public class DbmsManagerTest : S2TestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestCreateAutoSelectList()
        {
            IDataSource dataSource = (IDataSource) GetComponent(typeof(IDataSource));
            Assert.IsNotNull(DbmsManager.GetDbms(dataSource), "1");
        }

        [Test, S2]
        public void TestGetStandard()
        {
            Assert.IsTrue(DbmsManager.GetDbms("OleDbConnection_xxx") is Standard);
            Assert.IsTrue(DbmsManager.GetDbms("OdbcConnection_xxx") is Standard);
            Assert.IsTrue(DbmsManager.GetDbms("xxx") is Standard);
        }
    }
}
