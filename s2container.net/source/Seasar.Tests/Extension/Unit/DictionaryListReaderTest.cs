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

using System.Collections;
using System.Data;
using MbUnit.Framework;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.Unit
{
    [TestFixture]
    public class DictionaryListReaderTest
    {
        [Test]
        public void TestRead()
        {
            IDictionary emp = new Hashtable();
            emp.Add("empno", 7788);
            emp.Add("ename", "SCOTT");
            IList list = new ArrayList();
            list.Add(emp);
            DictionaryReader reader = new DictionaryListReader(list);
            DataSet ds = reader.Read();
            DataTable table = ds.Tables[0];
            DataRow row = table.Rows[0];
            Assert.AreEqual(7788, row["empno"], "1");
            Assert.AreEqual("SCOTT", row["ename"], "2");
            Assert.AreEqual(DataRowState.Unchanged, row.RowState, "3");
        }
    }
}
