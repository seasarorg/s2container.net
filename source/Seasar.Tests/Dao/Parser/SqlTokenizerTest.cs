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

using Seasar.Dao;
using Seasar.Dao.Parser;
using MbUnit.Framework;

namespace Seasar.Tests.Dao.Parser
{
    [TestFixture]
    public class SqlTokenizerTest
    {
        [Test]
        public void TestNext()
        {
            string sql = "SELECT * FROM emp";
            ISqlTokenizer tokenizer = new SqlTokenizerImpl(sql);
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "1");
            Assert.AreEqual(sql, tokenizer.Token, "2");
            Assert.AreEqual(TokenType.EOF, tokenizer.Next(), "3");
            Assert.AreEqual(null, tokenizer.Token, "4");
        }

        [Test, ExpectedException(typeof(TokenNotClosedRuntimeException))]
        public void TestCommentEndNotFound()
        {
            string sql = "SELECT * FROM emp/*hoge";
            ISqlTokenizer tokenizer = new SqlTokenizerImpl(sql);
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "1");
            Assert.AreEqual("SELECT * FROM emp", tokenizer.Token, "2");
            tokenizer.Next();
            Assert.Fail("3");
        }

        [Test]
        public void TestBindVariable()
        {
            string sql = "SELECT * FROM emp WHERE job = /*job*/'CLER K' AND deptno = /*deptno*/20";
            ISqlTokenizer tokenizer = new SqlTokenizerImpl(sql);
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "1");
            Assert.AreEqual("SELECT * FROM emp WHERE job = ", tokenizer.Token, "2");
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "3");
            Assert.AreEqual("job", tokenizer.Token, "4");
            tokenizer.SkipToken();
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "5");
            Assert.AreEqual(" AND deptno = ", tokenizer.Token, "6");
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "7");
            Assert.AreEqual("deptno", tokenizer.Token, "8");
            tokenizer.SkipToken();
            Assert.AreEqual(TokenType.EOF, tokenizer.Next(), "9");
        }

        [Test]
        public void TestParseBindVariable2()
        {
            string sql = "SELECT * FROM emp WHERE job = /*job*/'CLERK'/**/";
            ISqlTokenizer tokenizer = new SqlTokenizerImpl(sql);
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "1");
            Assert.AreEqual("SELECT * FROM emp WHERE job = ", tokenizer.Token, "2");
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "3");
            Assert.AreEqual("job", tokenizer.Token, "4");
            tokenizer.SkipToken();
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "5");
            Assert.AreEqual(string.Empty, tokenizer.Token, "6");
            Assert.AreEqual(TokenType.EOF, tokenizer.Next(), "7");
        }

        [Test]
        public void TestParseBindVariable3()
        {
            string sql = "/*job*/'CLERK',";
            ISqlTokenizer tokenizer = new SqlTokenizerImpl(sql);
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "1");
            Assert.AreEqual("job", tokenizer.Token, "2");
            tokenizer.SkipToken();
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "3");
            Assert.AreEqual(",", tokenizer.Token, "4");
            Assert.AreEqual(TokenType.EOF, tokenizer.Next(), "5");
        }

        [Test]
        public void TestParseElse()
        {
            string sql = "SELECT * FROM emp WHERE /*IF job != null*/job = /*job*/'CLERK'-- ELSE job is null/*END*/";
            ISqlTokenizer tokenizer = new SqlTokenizerImpl(sql);
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "1");
            Assert.AreEqual("SELECT * FROM emp WHERE ", tokenizer.Token, "2");
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "3");
            Assert.AreEqual("IF job != null", tokenizer.Token, "4");
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "5");
            Assert.AreEqual("job = ", tokenizer.Token, "6");
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "7");
            Assert.AreEqual("job", tokenizer.Token, "8");
            tokenizer.SkipToken();
            Assert.AreEqual(TokenType.ELSE, tokenizer.Next(), "9");
            tokenizer.SkipWhitespace();
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "10");
            Assert.AreEqual("job is null", tokenizer.Token, "11");
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "12");
            Assert.AreEqual("END", tokenizer.Token, "13");
            Assert.AreEqual(TokenType.EOF, tokenizer.Next(), "14");
        }

        [Test]
        public void TestParseElse2()
        {
            string sql = "/*IF false*/aaa -- ELSE bbb = /*bbb*/123/*END*/";
            ISqlTokenizer tokenizer = new SqlTokenizerImpl(sql);
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "1");
            Assert.AreEqual("IF false", tokenizer.Token, "2");
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "3");
            Assert.AreEqual("aaa ", tokenizer.Token, "4");
            Assert.AreEqual(TokenType.ELSE, tokenizer.Next(), "5");
            tokenizer.SkipWhitespace();
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "6");
            Assert.AreEqual("bbb = ", tokenizer.Token, "7");
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "8");
            Assert.AreEqual("bbb", tokenizer.Token, "9");
            tokenizer.SkipToken();
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "10");
            Assert.AreEqual("END", tokenizer.Token, "11");
            Assert.AreEqual(TokenType.EOF, tokenizer.Next(), "12");
        }

        [Test]
        public void TestAnd()
        {
            string sql = " AND bbb";
            ISqlTokenizer tokenizer = new SqlTokenizerImpl(sql);
            Assert.AreEqual(" ", tokenizer.SkipWhitespace(), "1");
            Assert.AreEqual("AND", tokenizer.SkipToken(), "2");
            Assert.AreEqual(" AND", tokenizer.Before, "3");
            Assert.AreEqual(" bbb", tokenizer.After, "3");
        }

        [Test]
        public void TestBindVariable2()
        {
            string sql = "? abc ? def ?";
            ISqlTokenizer tokenizer = new SqlTokenizerImpl(sql);
            Assert.AreEqual(TokenType.BIND_VARIABLE, tokenizer.Next(), "1");
            Assert.AreEqual("$1", tokenizer.Token, "2");
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "3");
            Assert.AreEqual(" abc ", tokenizer.Token, "4");
            Assert.AreEqual(TokenType.BIND_VARIABLE, tokenizer.Next(), "5");
            Assert.AreEqual("$2", tokenizer.Token, "6");
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "7");
            Assert.AreEqual(" def ", tokenizer.Token, "8");
            Assert.AreEqual(TokenType.BIND_VARIABLE, tokenizer.Next(), "9");
            Assert.AreEqual("$3", tokenizer.Token, "10");
            Assert.AreEqual(TokenType.EOF, tokenizer.Next(), "11");
        }

        [Test]
        public void TestBindVariable3()
        {
            string sql = "abc ? def";
            ISqlTokenizer tokenizer = new SqlTokenizerImpl(sql);
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "1");
            Assert.AreEqual("abc ", tokenizer.Token, "2");
            Assert.AreEqual(TokenType.BIND_VARIABLE, tokenizer.Next(), "3");
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "4");
            Assert.AreEqual(" def", tokenizer.Token, "5");
            Assert.AreEqual(TokenType.EOF, tokenizer.Next(), "6");
        }

        [Test]
        public void TestBindVariable4()
        {
            string sql = "/*IF false*/aaa--ELSE bbb = /*bbb*/123/*END*/";
            ISqlTokenizer tokenizer = new SqlTokenizerImpl(sql);
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "1");
            Assert.AreEqual("IF false", tokenizer.Token, "2");
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "3");
            Assert.AreEqual("aaa", tokenizer.Token, "4");
            Assert.AreEqual(TokenType.ELSE, tokenizer.Next(), "5");
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "6");
            Assert.AreEqual(" bbb = ", tokenizer.Token, "7");
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "8");
            Assert.AreEqual("bbb", tokenizer.Token, "9");
        }

        [Test]
        public void TestSkipTokenForParent()
        {
            string sql = "INSERT INTO TABLE_NAME (ID) VALUES (/*id*/20)";
            ISqlTokenizer tokenizer = new SqlTokenizerImpl(sql);
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "1");
            Assert.AreEqual(TokenType.COMMENT, tokenizer.Next(), "2");
            Assert.AreEqual("20", tokenizer.SkipToken(), "3");
            Assert.AreEqual(TokenType.SQL, tokenizer.Next(), "4");
            Assert.AreEqual(")", tokenizer.Token, "5");
            Assert.AreEqual(TokenType.EOF, tokenizer.Next(), "6");
        }
    }
}
