#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.ADO.Impl
{
    [TestFixture]
    public class BasicSelectHandlerTest : S2TestCase
    {
        private const string PATH = "Ado.dicon";

        static BasicSelectHandlerTest()
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
            string sql = "SELECT * FROM emp WHERE empno = @empno";
            BasicSelectHandler handler = new BasicSelectHandler(
                DataSource,
                sql,
                new DictionaryDataReaderHandler()
                );
            IDictionary ret = (IDictionary) handler.Execute(new object[] { 7788 });
            Console.Out.WriteLine(ret);
            Assert.IsNotNull(ret, "1");
            Assert.AreEqual(9, ret.Count, "2");
        }

        public void SetUpExecuteDuplicationParam()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void ExecuteDuplicationParam()
        {
            string sql = "SELECT * FROM emp WHERE empno = @empno OR empno = @empno2 OR empno = @empno OR ename = @ename";
            BasicSelectHandler handler = new BasicSelectHandler(
                DataSource,
                sql,
                new DictionaryDataReaderHandler()
                );
            IDictionary ret = (IDictionary) handler.Execute(new object[] { 7788, 7789, 7788, "SCOTT" });
            Console.Out.WriteLine(ret);
            Assert.IsNotNull(ret, "1");
            Assert.AreEqual(9, ret.Count, "2");
        }

        public void SetUpExecuteNullArgs()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void ExecuteNullArgs()
        {
            string sql = "SELECT * FROM emp WHERE empno = 7788";
            BasicSelectHandler handler = new BasicSelectHandler(
                DataSource,
                sql,
                new DictionaryDataReaderHandler()
                );
            IDictionary ret = (IDictionary) handler.Execute(null);
            Console.Out.WriteLine(ret);
            Assert.IsNotNull(ret, "1");
            Assert.AreEqual(9, ret.Count, "2");
        }

        public void SetUpExecuteParam()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void ExecuteParam()
        {
            string sql = "SELECT * FROM emp WHERE empno = @empno OR empno = :empno OR empno = ?";
            BasicSelectHandler handler = new BasicSelectHandler(
                DataSource,
                sql,
                new DictionaryDataReaderHandler()
                );
            IDictionary ret = (IDictionary) handler.Execute(new object[] { 7788, 7788, 7788 });
            Console.Out.WriteLine(ret);
            Assert.IsNotNull(ret, "1");
            Assert.AreEqual(9, ret.Count, "2");
        }

        public void SetUpIdentity()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void Identity()
        {
            if (DataSource.GetCommand().GetType().Name.Equals("SqlCommand")) 
            {
                string sql = "select @@identity";
                BasicSelectHandler handler = new BasicSelectHandler(
                    DataSource,
                    sql,
                    new DictionaryDataReaderHandler()
                    );
                IDictionary ret = (IDictionary) handler.Execute(null);
                Assert.IsNotNull(ret, "1");
            }
        }
    }
}
