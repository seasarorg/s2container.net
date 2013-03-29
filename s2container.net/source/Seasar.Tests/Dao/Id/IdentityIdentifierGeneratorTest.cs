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

using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Dao.Id;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Id
{
    [TestFixture]
    public class IdentityIdentifierGeneratorTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2(Tx.Rollback)]
        public void TestGetGeneratedValue()
        {
            if (Dbms.IdentitySelectString == null)
            {
                //Assert.Ignore("IDENTITYをサポートしていないDBMS。");
            }

            BasicUpdateHandler updateHandler = new BasicUpdateHandler(
                DataSource, "insert into idtable(id_name) values('hoge')");
            updateHandler.Execute(null);

            IdentityIdentifierGenerator generator = new IdentityIdentifierGenerator("Id", Dbms);
            Hoge hoge = new Hoge();
            generator.SetIdentifier(hoge, DataSource);
            Assert.IsTrue(hoge.Id > 0);
        }

#if NHIBERNATE_NULLABLES
        [Test, S2(Tx.Rollback)]
        public void TestGetGeneratedNullableValue()
        {
            if (Dbms.IdentitySelectString == null)
            {
                Assert.Ignore("IDENTITYをサポートしていないDBMS。");
            }

            BasicUpdateHandler updateHandler = new BasicUpdateHandler(
                DataSource, "insert into idtable(id_name) values('hoge')");
            updateHandler.Execute(null);

            IdentityIdentifierGenerator generator = new IdentityIdentifierGenerator("Id", Dbms);
            {
                HogeNullableDecimal hoge = new HogeNullableDecimal();
                generator.SetIdentifier(hoge, DataSource);
                Assert.IsTrue(hoge.Id.Value > 0);
                Trace.WriteLine(hoge.Id);
            }
            {
                HogeNullableInt16 hoge = new HogeNullableInt16();
                generator.SetIdentifier(hoge, DataSource);
                Assert.IsTrue(hoge.Id.Value > 0);
                Trace.WriteLine(hoge.Id);
            }
            {
                HogeNullableInt32 hoge = new HogeNullableInt32();
                generator.SetIdentifier(hoge, DataSource);
                Assert.IsTrue(hoge.Id.Value > 0);
                Trace.WriteLine(hoge.Id);
            }
            {
                HogeNullableInt64 hoge = new HogeNullableInt64();
                generator.SetIdentifier(hoge, DataSource);
                Assert.IsTrue(hoge.Id.Value > 0);
                Trace.WriteLine(hoge.Id);
            }
            {
                HogeNullableSingle hoge = new HogeNullableSingle();
                generator.SetIdentifier(hoge, DataSource);
                Assert.IsTrue(hoge.Id.Value > 0);
                Trace.WriteLine(hoge.Id);
            }
            {
                HogeNullableDouble hoge = new HogeNullableDouble();
                generator.SetIdentifier(hoge, DataSource);
                Assert.IsTrue(hoge.Id.Value > 0);
                Trace.WriteLine(hoge.Id);
            }
        }
#endif

#if !NET_1_1
        [Test, S2(Tx.Rollback)]
        public void TestGetGeneratedSystemNullableValue()
        {
            if (Dbms.IdentitySelectString == null)
            {
                //Assert.Ignore("IDENTITYをサポートしていないDBMS。");
            }

            BasicUpdateHandler updateHandler = new BasicUpdateHandler(
                DataSource, "insert into idtable(id_name) values('hoge')");
            updateHandler.Execute(null);

            IdentityIdentifierGenerator generator = new IdentityIdentifierGenerator("Id", Dbms);
            {
                HogeSystemNullableDecimal hoge = new HogeSystemNullableDecimal();
                generator.SetIdentifier(hoge, DataSource);
                Assert.IsTrue(hoge.Id.Value > 0);
                Trace.WriteLine(hoge.Id);
            }
            {
                HogeSystemNullableInt hoge = new HogeSystemNullableInt();
                generator.SetIdentifier(hoge, DataSource);
                Assert.IsTrue(hoge.Id.Value > 0);
                Trace.WriteLine(hoge.Id);
            }
            {
                HogeSystemNullableShort hoge = new HogeSystemNullableShort();
                generator.SetIdentifier(hoge, DataSource);
                Assert.IsTrue(hoge.Id.Value > 0);
                Trace.WriteLine(hoge.Id);
            }
            {
                HogeSystemNullableLong hoge = new HogeSystemNullableLong();
                generator.SetIdentifier(hoge, DataSource);
                Assert.IsTrue(hoge.Id.Value > 0);
                Trace.WriteLine(hoge.Id);
            }
            {
                HogeSystemNullableFloat hoge = new HogeSystemNullableFloat();
                generator.SetIdentifier(hoge, DataSource);
                Assert.IsTrue(hoge.Id.Value > 0);
                Trace.WriteLine(hoge.Id);
            }
            {
                HogeSystemNullableDouble hoge = new HogeSystemNullableDouble();
                generator.SetIdentifier(hoge, DataSource);
                Assert.IsTrue(hoge.Id.Value > 0);
                Trace.WriteLine(hoge.Id);
            }
        }
#endif
    }
}
