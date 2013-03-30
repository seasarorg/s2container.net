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

using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.ADO.Impl
{
    [TestFixture]
    public class BasicUpdateHandlerTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Ado.dicon";

        static BasicUpdateHandlerTest()
        {
            FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        public void SetUpExecute()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void Execute()
        {
            string sql = "update emp set ename = @ename, comm = @comm where empno = @empno";
            BasicUpdateHandler handler = new BasicUpdateHandler(DataSource, sql);
            object[] args = new object[] { "SCOTT", null, 7788 };
            Type[] argTypes = new Type[] { typeof(string), typeof(Nullable<int>), typeof(int) };
            string[] argNames = new string[] { "ename", "comm", "empno" };
            int ret = handler.Execute(args, argTypes, argNames);
            Assert.AreEqual(1, ret, "1");
        }

        public void SetUpExecuteNullArgs()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void ExecuteNullArgs()
        {
            string sql = "delete from emp";
            BasicUpdateHandler handler = new BasicUpdateHandler(DataSource, sql);
            int ret = handler.Execute(null);
            Assert.AreEqual(14, ret, "1");
        }

        public void SetUpExecuteAtmarkWithParam()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void ExecuteAtmarkWithParam()
        {
            string sql = "update emp set ename = @ename, comm = @comm where empno = @empno";
            BasicUpdateHandler handler = new BasicUpdateHandler(DataSource, sql);
            object[] args = new object[] { "SCOTT", null, 7788 };
            int ret = handler.Execute(args);
            Assert.AreEqual(1, ret, "1");
        }

        public void SetUpExecuteColonWithParam()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void ExecuteColonWithParam()
        {
            string sql = "update emp set ename = :ename, comm = :comm where empno = :empno";
            BasicUpdateHandler handler = new BasicUpdateHandler(DataSource, sql);
            object[] args = new object[] { "SCOTT", null, 7788 };
            int ret = handler.Execute(args);
            Assert.AreEqual(1, ret, "1");
        }

        public void SetUpExecuteQuestionWithParam()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void ExecuteQuestionWithParam()
        {
            string sql = "update emp set ename = ?, comm = ? where empno = ?";
            BasicUpdateHandler handler = new BasicUpdateHandler(DataSource, sql);
            object[] args = new object[] { "SCOTT", null, 7788 };
            int ret = handler.Execute(args);
            Assert.AreEqual(1, ret, "1");
        }

        public void SetUpExecuteEnumType()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void ExecuteEnumType()
        {
            string sql = "update basictype set bytetype = ?, sbytetype = ?, int16type = ?, int32type = ?, int64type = ?, singletype = ?, doubletype = ?, decimaltype = ?, stringtype = ? where id = ?";
            BasicUpdateHandler handler = new BasicUpdateHandler(DataSource, sql);
            object[] args = new object[] { Numbers.ONE, Numbers.TWO, Numbers.THREE, Numbers.FOUR, Numbers.FIVE, Numbers.SIX, Numbers.SEVEN, Numbers.EIGHT, Numbers.NINE, 1 };
            int ret = handler.Execute(args);
            Assert.AreEqual(1, ret, "1");
        }
    }
}
