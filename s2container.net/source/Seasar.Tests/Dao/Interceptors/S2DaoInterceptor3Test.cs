#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

using Seasar.Extension.Unit;
using MbUnit.Framework;

namespace Seasar.Tests.Dao.Interceptors
{
    [TestFixture]
    public class S2DaoInterceptor3Test : S2TestCase
    {
        private IDepartmentAutoDao _dao = null;

        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Tests.dicon");
        }

        [Test, S2(Tx.Rollback)]
        public void TestUpdate()
        {
            Department department = new Department();
            department.Deptno = 10;
            Assert.AreEqual(1, _dao.Update(department));
        }

        [Test, S2(Tx.Rollback)]
        public void TestDelete()
        {
            Department department = new Department();
            department.Deptno = 10;
            Assert.AreEqual(1, _dao.Delete(department));
        }
    }
}
