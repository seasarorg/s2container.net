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

using MbUnit.Framework;
using Seasar.Dao.Id;
using Seasar.Dao.Unit;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Id
{
    [TestFixture]
    public class SequenceIdentifierGeneratorTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2(Tx.Rollback)]
        public void TestGenerate()
        {
            if (Dbms.GetSequenceNextValString("SEQ_IDTABLE") == null)
            {
                Assert.Ignore("シーケンスをサポートしていないDBMS。");
            }

            SequenceIdentifierGenerator generator = new SequenceIdentifierGenerator("Id", Dbms);
            generator.SequenceName = "\"SEQ_IDTABLE\"";
            Hoge hoge = new Hoge();
            generator.SetIdentifier(hoge, DataSource);
            Assert.IsTrue(hoge.Id > 0);
        }
    }
}
