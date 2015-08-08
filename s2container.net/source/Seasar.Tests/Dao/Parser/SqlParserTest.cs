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

using System.Collections;
using Seasar.Dao;
using Seasar.Dao.Parser;
using Seasar.Dao.Node;
using Seasar.Dao.Context;
using MbUnit.Framework;
using Seasar.Framework.Util;

namespace Seasar.Tests.Dao.Parser
{
    [TestFixture]
    public class SqlParserTest
    {
        [Test]
        public void TestParse()
        {
            var sql = "SELECT * FROM emp";
            ISqlParser parser = new SqlParserImpl(sql);
            ICommandContext ctx = new CommandContextImpl();
            var node = parser.Parse();
            node.Accept(ctx);
            Assert.AreEqual(sql, ctx.Sql, "1");
        }

        [Test]
        public void TestParseEndSemicolon()
        {
            ParseEndSemicolon(";");
            ParseEndSemicolon(";\t");
            ParseEndSemicolon("; ");
        }

        private void ParseEndSemicolon(string endChar)
        {
            var sql = "SELECT * FROM emp";
            ISqlParser parser = new SqlParserImpl(sql + endChar);
            ICommandContext ctx = new CommandContextImpl();
            var node = parser.Parse();
            node.Accept(ctx);
            Assert.AreEqual(sql, ctx.Sql, "1");
        }

        [Test, ExpectedException(typeof(TokenNotClosedRuntimeException))]
        public void TestCommentEndNotFound()
        {
            var sql = "SELECT * FROM emp/*hoge";
            ISqlParser parser = new SqlParserImpl(sql);
            parser.Parse();
            Assert.Fail("1");
        }

        [Test]
        public void TestParseBindVariable()
        {
            var sql = "SELECT * FROM emp WHERE job = /*job*/'CLERK' AND deptno = /*deptno*/20";
            var sql2 = "SELECT * FROM emp WHERE job = ? AND deptno = ?";
            var sql3 = "SELECT * FROM emp WHERE job = ";
            var sql4 = " AND deptno = ";
            ISqlParser parser = new SqlParserImpl(sql);
            ICommandContext ctx = new CommandContextImpl();
            var job = "CLERK";
            var deptno = 20;
            ctx.AddArg("job", job, job.GetExType());
            ctx.AddArg("deptno", deptno, deptno.GetExType());
            var root = parser.Parse();
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
            var vars = ctx.BindVariables;
            Assert.AreEqual(2, vars.Length, "2");
            Assert.AreEqual(job, vars[0], "3");
            Assert.AreEqual(deptno, vars[1], "4");
            Assert.AreEqual(4, root.ChildSize, "5");
            var sqlNode = (SqlNode) root.GetChild(0);
            Assert.AreEqual(sql3, sqlNode.Sql, "6");
            var varNode = (BindVariableNode) root.GetChild(1);
            Assert.AreEqual("job", varNode.Expression, "7");
            var sqlNode2 = (SqlNode) root.GetChild(2);
            Assert.AreEqual(sql4, sqlNode2.Sql, "8");
            var varNode2 = (BindVariableNode) root.GetChild(3);
            Assert.AreEqual("deptno", varNode2.Expression, "9");
        }

        [Test]
        public void TestParseBindVariable2()
        {
            var sql = "SELECT * FROM emp WHERE job = /* job*/'CLERK'";
            var sql2 = "SELECT * FROM emp WHERE job = 'CLERK'";
            var sql3 = "SELECT * FROM emp WHERE job = ";
            var sql4 = "'CLERK'";
            ISqlParser parser = new SqlParserImpl(sql);
            ICommandContext ctx = new CommandContextImpl();
            var root = parser.Parse();
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
            Assert.AreEqual(2, root.ChildSize, "2");
            var sqlNode = (SqlNode) root.GetChild(0);
            Assert.AreEqual(sql3, sqlNode.Sql, "3");
            var sqlNode2 = (SqlNode) root.GetChild(1);
            Assert.AreEqual(sql4, sqlNode2.Sql, "4");
        }

        [Test]
        public void TestParseWhiteSpace()
        {
            var sql = "SELECT * FROM emp WHERE empno = /*empno*/1 AND 1 = 1";
            var sql2 = "SELECT * FROM emp WHERE empno = ? AND 1 = 1";
            var sql3 = " AND 1 = 1";
            ISqlParser parser = new SqlParserImpl(sql);
            ICommandContext ctx = new CommandContextImpl();
            var empno = 7788;
            ctx.AddArg("empno", empno, empno.GetExType());
            var root = parser.Parse();
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
            var sqlNode = (SqlNode) root.GetChild(2);
            Assert.AreEqual(sql3, sqlNode.Sql, "2");
        }

        [Test]
        public void TestParseIf()
        {
            var sql = "SELECT * FROM emp/*IF job != null*/ WHERE job = /*job*/'CLERK'/*END*/";
            var sql2 = "SELECT * FROM emp WHERE job = ?";
            var sql3 = "SELECT * FROM emp";
            ISqlParser parser = new SqlParserImpl(sql);
            ICommandContext ctx = new CommandContextImpl();
            var job = "CLERK";
            ctx.AddArg("job", job, job.GetExType());
            var root = parser.Parse();
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
            var vars = ctx.BindVariables;
            Assert.AreEqual(1, vars.Length, "2");
            Assert.AreEqual(job, vars[0], "3");
            Assert.AreEqual(2, root.ChildSize, "4");
            var sqlNode = (SqlNode) root.GetChild(0);
            Assert.AreEqual(sql3, sqlNode.Sql, "5");
            var ifNode = (IfNode) root.GetChild(1);
            Assert.AreEqual("self.GetArg('job') != null", ifNode.Expression, "6");
            Assert.AreEqual(2, ifNode.ChildSize, "7");
            var sqlNode2 = (SqlNode) ifNode.GetChild(0);
            Assert.AreEqual(" WHERE job = ", sqlNode2.Sql, "8");
            var varNode = (BindVariableNode) ifNode.GetChild(1);
            Assert.AreEqual("job", varNode.Expression, "9");
            ICommandContext ctx2 = new CommandContextImpl();
            root.Accept(ctx2);
            Assert.AreEqual(sql3, ctx2.Sql, "10");
        }

        [Test]
        public void TestParseIf2()
        {
            var sql = "/*IF aaa != null*/aaa/*IF bbb != null*/bbb/*END*//*END*/";
            ISqlParser parser = new SqlParserImpl(sql);
            ICommandContext ctx = new CommandContextImpl();
            var root = parser.Parse();
            root.Accept(ctx);
            Assert.AreEqual(string.Empty, ctx.Sql, "1");
            ctx.AddArg("aaa", null, typeof(string));
            ctx.AddArg("bbb", "hoge", typeof(string));
            root.Accept(ctx);
            Assert.AreEqual(string.Empty, ctx.Sql, "2");
            ctx.AddArg("aaa", "hoge", typeof(string));
            root.Accept(ctx);
            Assert.AreEqual("aaabbb", ctx.Sql, "3");
            ICommandContext ctx2 = new CommandContextImpl();
            ctx2.AddArg("aaa", "hoge", typeof(string));
            ctx2.AddArg("bbb", null, typeof(string));
            root.Accept(ctx2);
            Assert.AreEqual("aaa", ctx2.Sql, "4");
        }

        [Test]
        public void TestParseIf3()
        {
            var sql = "SELECT * FROM emp/*IF mgr != -1*/ WHERE mgr = /*mgr*/4/*END*/";
            var sql2 = "SELECT * FROM emp WHERE mgr = ?";
            var sql3 = "SELECT * FROM emp";
            ISqlParser parser = new SqlParserImpl(sql);
            ICommandContext ctx = new CommandContextImpl();
            var mgr = 2;
            ctx.AddArg("mgr", mgr, mgr.GetExType());
            var root = parser.Parse();
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
            mgr = -1;
            ICommandContext ctx2 = new CommandContextImpl();
            ctx2.AddArg("mgr", mgr, mgr.GetExType());
            root.Accept(ctx2);
            Assert.AreEqual(sql3, ctx2.Sql, "2");
        }

        [Test]
        public void TestParseIf4()
        {
            var sql = "SELECT * FROM emp/*IF mgr!=-1*/ WHERE mgr=/*mgr*/4/*END*/";
            var sql2 = "SELECT * FROM emp WHERE mgr=?";
            var sql3 = "SELECT * FROM emp";
            ISqlParser parser = new SqlParserImpl(sql);
            ICommandContext ctx = new CommandContextImpl();
            var mgr = 2;
            ctx.AddArg("mgr", mgr, mgr.GetExType());
            var root = parser.Parse();
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
            mgr = -1;
            ICommandContext ctx2 = new CommandContextImpl();
            ctx2.AddArg("mgr", mgr, mgr.GetExType());
            root.Accept(ctx2);
            Assert.AreEqual(sql3, ctx2.Sql, "2");
        }

        [Test]
        public void TestParseElse()
        {
            var sql = "SELECT * FROM emp WHERE /*IF job != null*/job = /*job*/'CLERK'-- ELSE job is null/*END*/";
            var sql2 = "SELECT * FROM emp WHERE job = ?";
            var sql3 = "SELECT * FROM emp WHERE job is null";
            ISqlParser parser = new SqlParserImpl(sql);
            ICommandContext ctx = new CommandContextImpl();
            var job = "CLERK";
            ctx.AddArg("job", job, job.GetExType());
            var root = parser.Parse();
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
            var vars = ctx.BindVariables;
            Assert.AreEqual(1, vars.Length, "2");
            Assert.AreEqual(job, vars[0], "3");
            ICommandContext ctx2 = new CommandContextImpl();
            root.Accept(ctx2);
            Assert.AreEqual(sql3, ctx2.Sql, "4");
        }

        [Test]
        public void TestParseElse2()
        {
            var sql = "/*IF false*/aaa--ELSE bbb = /*bbb*/123/*END*/";
            ISqlParser parser = new SqlParserImpl(sql);
            ICommandContext ctx = new CommandContextImpl();
            var bbb = 123;
            ctx.AddArg("bbb", bbb, bbb.GetExType());
            var root = parser.Parse();
            root.Accept(ctx);
            Assert.AreEqual("bbb = ?", ctx.Sql, "1");
            var vars = ctx.BindVariables;
            Assert.AreEqual(1, vars.Length, "2");
            Assert.AreEqual(bbb, vars[0], "3");
        }

        [Test]
        public void TestParseElse3()
        {
            var sql = "/*IF false*/aaa--ELSE bbb/*IF false*/ccc--ELSE ddd/*END*//*END*/";
            ISqlParser parser = new SqlParserImpl(sql);
            ICommandContext ctx = new CommandContextImpl();
            var root = parser.Parse();
            root.Accept(ctx);
            Assert.AreEqual("bbbddd", ctx.Sql, "1");
        }

        [Test]
        public void TestElse4()
        {
            var sql = "SELECT * FROM emp/*BEGIN*/ WHERE /*IF false*/aaa-- ELSE AND deptno = 10/*END*//*END*/";
            var sql2 = "SELECT * FROM emp WHERE deptno = 10";
            ISqlParser parser = new SqlParserImpl(sql);
            var root = parser.Parse();
            ICommandContext ctx = new CommandContextImpl();
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
        }

        [Test]
        public void TestBegin()
        {
            var sql = "SELECT * FROM emp/*BEGIN*/ WHERE /*IF job != null*/job = /*job*/'CLERK'/*END*//*IF deptno != null*/ AND deptno = /*deptno*/20/*END*//*END*/";
            var sql2 = "SELECT * FROM emp";
            var sql3 = "SELECT * FROM emp WHERE job = ?";
            var sql4 = "SELECT * FROM emp WHERE job = ? AND deptno = ?";
            var sql5 = "SELECT * FROM emp WHERE deptno = ?";
            ISqlParser parser = new SqlParserImpl(sql);
            var root = parser.Parse();
            ICommandContext ctx = new CommandContextImpl();
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");

            ICommandContext ctx2 = new CommandContextImpl();
            ctx2.AddArg("job", "CLERK", typeof(string));
            ctx2.AddArg("deptno", null, typeof(int));
            root.Accept(ctx2);
            Assert.AreEqual(sql3, ctx2.Sql, "2");

            ICommandContext ctx3 = new CommandContextImpl();
            ctx3.AddArg("job", "CLERK", typeof(string));
            ctx3.AddArg("deptno", 20, typeof(int));
            root.Accept(ctx3);
            Assert.AreEqual(sql4, ctx3.Sql, "3");

            ICommandContext ctx4 = new CommandContextImpl();
            ctx4.AddArg("deptno", 20, typeof(int));
            ctx4.AddArg("job", null, typeof(string));
            root.Accept(ctx4);
            Assert.AreEqual(sql5, ctx4.Sql, "4");
        }

        [Test]
        public void TestBeginAnd()
        {
            var sql = "/*BEGIN*/WHERE /*IF true*/aaa BETWEEN /*bbb*/111 AND /*ccc*/123/*END*//*END*/";
            var sql2 = "WHERE aaa BETWEEN ? AND ?";
            ISqlParser parser = new SqlParserImpl(sql);
            var root = parser.Parse();
            ICommandContext ctx = new CommandContextImpl();
            ctx.AddArg("bbb", "111", typeof(string));
            ctx.AddArg("ccc", "222", typeof(string));
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
        }

        [Test]
        public void TestIn()
        {
            var sql = "SELECT * FROM emp WHERE deptno IN /*deptnoList*/(10, 20) ORDER BY ename";
            var sql2 = "SELECT * FROM emp WHERE deptno IN (?, ?) ORDER BY ename";
            var sql3 = "SELECT * FROM emp WHERE deptno IN (?, ?, ?, ?, ?, ?, ?, ?, ?, ?) ORDER BY ename";
            ISqlParser parser = new SqlParserImpl(sql);
            var root = parser.Parse();
            ICommandContext ctx = new CommandContextImpl();
            IList deptnoList = new ArrayList();
            deptnoList.Add(10);
            deptnoList.Add(20);
            ctx.AddArg("deptnoList", deptnoList, typeof(IList));
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
            var vars = ctx.BindVariables;
            Assert.AreEqual(2, vars.Length, "2");
            Assert.AreEqual(10, vars[0], "3");
            Assert.AreEqual(20, vars[1], "4");
            //’Ç‰ÁƒeƒXƒg
            ctx = new CommandContextImpl();
            deptnoList = new ArrayList();
            deptnoList.Add(10);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(20);
            deptnoList.Add(100);
            ctx.AddArg("deptnoList", deptnoList, typeof(IList));
            root.Accept(ctx);
            Assert.AreEqual(sql3, ctx.Sql, "5");
            vars = ctx.BindVariables;
            Assert.AreEqual(10, vars.Length, "6");
            Assert.AreEqual(10, vars[0], "7");
            Assert.AreEqual(100, vars[9], "8");

        }

        [Test]
        public void TestIn2()
        {
            var sql = "SELECT * FROM emp WHERE deptno IN /*deptnoList*/(10, 20) ORDER BY ename";
            var sql2 = "SELECT * FROM emp WHERE deptno IN (?, ?) ORDER BY ename";
            ISqlParser parser = new SqlParserImpl(sql);
            var root = parser.Parse();
            ICommandContext ctx = new CommandContextImpl();
            int[] deptnoArray = { 10, 20 };
            ctx.AddArg("deptnoList", deptnoArray, deptnoArray.GetExType());
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
            var vars = ctx.BindVariables;
            Assert.AreEqual(2, vars.Length, "2");
            Assert.AreEqual(10, vars[0], "3");
            Assert.AreEqual(20, vars[1], "4");
        }

        [Test]
        public void TestIn3()
        {
            var sql = "SELECT * FROM emp WHERE ename IN /*enames*/('SCOTT','MARY') AND job IN /*jobs*/('ANALYST', 'FREE')";
            var sql2 = "SELECT * FROM emp WHERE ename IN (?, ?) AND job IN (?, ?)";
            ISqlParser parser = new SqlParserImpl(sql);
            var root = parser.Parse();
            ICommandContext ctx = new CommandContextImpl();
            string[] enames = { "SCOTT", "MARY" };
            string[] jobs = { "ANALYST", "FREE" };
            ctx.AddArg("enames", enames, enames.GetExType());
            ctx.AddArg("jobs", jobs, jobs.GetExType());
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
            var vars = ctx.BindVariables;
            Assert.AreEqual(4, vars.Length, "2");
            Assert.AreEqual("SCOTT", vars[0], "3");
            Assert.AreEqual("MARY", vars[1], "4");
            Assert.AreEqual("ANALYST", vars[2], "5");
            Assert.AreEqual("FREE", vars[3], "6");
        }

        [Test]
        public void TestParseBindVariable3()
        {
            var sql = "BETWEEN sal ? AND ?";
            var sql2 = "BETWEEN sal ? AND ?";

            ISqlParser parser = new SqlParserImpl(sql);
            var root = parser.Parse();
            ICommandContext ctx = new CommandContextImpl();
            ctx.AddArg("$1", 0, typeof(int));
            ctx.AddArg("$2", 1000, typeof(int));
            root.Accept(ctx);
            Assert.AreEqual(sql2, ctx.Sql, "1");
            var vars = ctx.BindVariables;
            Assert.AreEqual(2, vars.Length, "2");
            Assert.AreEqual(0, vars[0], "3");
            Assert.AreEqual(1000, vars[1], "4");
        }

        [Test, ExpectedException(typeof(EndCommentNotFoundRuntimeException))]
        public void TestEndNotFound()
        {
            var sql = "/*BEGIN*/";
            ISqlParser parser = new SqlParserImpl(sql);
            parser.Parse();
            Assert.Fail("1");
        }

        [Test]
        public void TestEndParent()
        {
            var sql = "INSERT INTO ITEM (ID, NUM) VALUES (/*id*/1, /*num*/20)";
            ISqlParser parser = new SqlParserImpl(sql);
            var root = parser.Parse();
            ICommandContext ctx = new CommandContextImpl();
            ctx.AddArg("id", 0, typeof(int));
            ctx.AddArg("num", 1, typeof(int));
            root.Accept(ctx);
            Assert.AreEqual(true, ctx.Sql.EndsWith(")"), "1");
        }

        [Test]
        public void TestEmbeddedValue()
        {
            var sql = "/*$aaa*/";
            ISqlParser parser = new SqlParserImpl(sql);
            var root = parser.Parse();
            ICommandContext ctx = new CommandContextImpl();
            ctx.AddArg("aaa", 0, typeof(int));
            root.Accept(ctx);
            Assert.AreEqual("0", ctx.Sql, "1");
        }
    }
}
