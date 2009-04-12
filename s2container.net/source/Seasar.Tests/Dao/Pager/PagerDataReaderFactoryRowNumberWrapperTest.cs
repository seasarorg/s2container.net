#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using Seasar.Dao.Pager;
using Seasar.Extension.ADO;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Pager
{
    [TestFixture]
    public class PagerDataReaderFactoryRowNumberWrapperTest : S2TestCase
    {
        private MockDataReaderFactory _original;
        private MockPagerDataReaderFactoryRowNumberWrapper _wrapper;

        [SetUp]
        public void SetUp()
        {
            _original = new MockDataReaderFactory();
            _wrapper = new MockPagerDataReaderFactoryRowNumberWrapper(_original, null);
        }

        [Test]
        public void MakeRowNumberSql()
        {
            Assert.AreEqual(
                "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY empno, ename ASC) AS pagerrownumber,  * FROM emp ) AS a WHERE pagerrownumber BETWEEN 1 AND 2",
                _wrapper.MockMakeRowNumberSql("SELECT * FROM emp ORDER BY empno, ename ASC", 2, 0)
                );
        }

        [Test]
        public void MakeCountSql()
        {
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT"),
                "count(*)で全件数を取得するSQLを生成"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order by id"),
                "count(*)で全件数を取得するSQLを生成(order by 除去)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT ORDER BY id"),
                "count(*)で全件数を取得するSQLを生成(ORDER BY 除去)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT\n) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT\norder by\n    id"),
                "count(*)で全件数を取得するSQLを生成(whitespace付きorder by 除去)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT WHERE name like '%order by%' ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT WHERE name like '%order by%' order by id"),
                "count(*)で全件数を取得するSQLを生成(途中のorder byは除去しない)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT WHERE name='aaa'/*order by*/) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT WHERE name='aaa'/*order by*/order by id"),
                "count(*)で全件数を取得するSQLを生成(途中のorder byは除去しない)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT WHERE\n--order by\nname=1\n) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT WHERE\n--order by\nname=1\norder by id"),
                "count(*)で全件数を取得するSQLを生成(途中のorder byは除去しない)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order by ＮＯ"),
                "count(*)で全件数を取得するSQLを生成(order by除去 UNICODE)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order by 名前, 組織_ID"),
                "count(*)で全件数を取得するSQLを生成(order by除去 UNICODE)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order by 名前 ASC\n, 組織_ID DESC"),
                "count(*)で全件数を取得するSQLを生成(order by除去 ASC,DESC)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order\n\tby\n\n 名前 \n\tASC \n\n\n, 組織_ID \n\tDESC \n"),
                "count(*)で全件数を取得するSQLを生成(order by除去 ASC,DESC+空行)"
                );
        }

        [Test]
        public void ChopOrderByAndMakeCountSql()
        {
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order by id"),
                "count(*)で全件数を取得するSQLを生成(chopOrderBy=true, order by 除去)"
                );
            _wrapper.ChopOrderBy = false;
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT order by id) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order by id"),
                "count(*)で全件数を取得するSQLを生成(chopOrderBy=false, order by 除去)"
                );
        }
    }

    internal class MockPagerDataReaderFactoryRowNumberWrapper : PagerDataReaderFactoryRowNumberWrapper
    {
        public MockPagerDataReaderFactoryRowNumberWrapper(
            IDataReaderFactory dataReaderFactory,
            ICommandFactory commandFactory
            )
            : base(dataReaderFactory, commandFactory)
        {
        }

        public string MockMakeRowNumberSql(string baseSql, int limit, int offset)
        {
            return MakeRowNumberSql(baseSql, limit, offset);
        }

        public string MockMakeCountSql(string baseSql)
        {
            return MakeCountSql(baseSql);
        }
    }
}