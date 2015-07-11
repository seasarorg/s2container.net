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
using Seasar.Dao;
using Seasar.Dao.Dbms;
using Seasar.Dao.Unit;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Dbms
{
    [TestFixture]
    public class OracleTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestCreateAutoSelectList()
        {
            IDbms dbms = new Seasar.Dao.Dbms.Oracle();
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Employee), dbms);
            string sql = dbms.GetAutoSelectSql(bmd);
            // #.NET4.0 取得されるプロパティの順番が逆になっているため修正
#if NET_4_0
            Assert.AreEqual("SELECT EMP2.EMPNO, EMP2.ENAME, EMP2.DEPTNUM FROM EMP2", sql);
            
#else
#region NET2.0
            Assert.AreEqual("SELECT EMP2.DEPTNUM, EMP2.ENAME, EMP2.EMPNO FROM EMP2", sql);
#endregion
#endif
        }

        [Test, S2]
        public void TestCreateAutoSelectList2()
        {
            IDbms dbms = new Seasar.Dao.Dbms.Oracle();
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Department), dbms);
            string sql = dbms.GetAutoSelectSql(bmd);
#if NET_4_0
            Assert.AreEqual("SELECT DEPT.LOC, DEPT.DNAME, DEPT.VERSIONNO, DEPT.DEPTNO FROM DEPT", sql);
#else
#region NET2.0
            Assert.AreEqual("SELECT DEPT.LOC, DEPT.DNAME, DEPT.DEPTNO, DEPT.VERSIONNO FROM DEPT", sql);
#endregion
#endif
        }
    }
}
