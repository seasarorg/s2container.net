#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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

using Seasar.Dao;
using Seasar.Dao.Dbms;
using Seasar.Extension.Unit;
using Seasar.Dao.Id;
using Seasar.Dao.Attrs;
using MbUnit.Framework;

namespace Seasar.Tests.Dao.Id
{
    [TestFixture]
    public class IdentifierGeneratorFactoryTest : S2TestCase
    {
        [Test, S2]
        public void TestCreateIdentifierGenerator()
        {
            IDbms dbms = new MSSQLServer();
            Hoge hoge = new Hoge();
            hoge.Id = 1;

            IIdentifierGenerator generator = IdentifierGeneratorFactory.CreateIdentifierGenerator(
                "id", dbms, null);
            Assert.AreEqual(typeof(AssignedIdentifierGenerator), generator.GetType(), "1");

            generator = IdentifierGeneratorFactory.CreateIdentifierGenerator(
                "id", dbms, new IDAttribute(IDType.IDENTITY));
            Assert.AreEqual(typeof(IdentityIdentifierGenerator), generator.GetType(), "2");

            generator = IdentifierGeneratorFactory.CreateIdentifierGenerator(
                "id", dbms, new IDAttribute(IDType.SEQUENCE, "myseq"));
            Assert.AreEqual("myseq", ((SequenceIdentifierGenerator) generator).SequenceName, "3");
        }
    }
}