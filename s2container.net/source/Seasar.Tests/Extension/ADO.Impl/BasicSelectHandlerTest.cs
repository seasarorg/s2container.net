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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Extension.ADO.Impl
{
    [TestFixture]
    public class BasicSelectHandlerTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Ado.dicon";

        static BasicSelectHandlerTest()
        {
            var info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        public void SetUpExecute()
        {
            Include(PATH);
        }

        [Test, S2]
        public void Execute()
        {
            var sql = "SELECT * FROM emp WHERE empno = @empno";
            var handler = new BasicSelectHandler(
                DataSource,
                sql,
                new DictionaryDataReaderHandler()
                );
            var ret = (IDictionary) handler.Execute(new object[] { 7788 });
            Trace.WriteLine(ToStringUtil.ToString(ret));
            Assert.IsNotNull(ret, "1");
            Assert.AreEqual(9, ret.Count, "2");
        }

        public void SetUpExecuteDuplicationParam()
        {
            Include(PATH);
        }

        [Test, S2]
        public void ExecuteDuplicationParam()
        {
            var sql = "SELECT * FROM emp WHERE empno = @empno OR empno = @empno2 OR empno = @empno OR ename = @ename";
            var handler = new BasicSelectHandler(
                DataSource,
                sql,
                new DictionaryDataReaderHandler()
                );
            var ret = (IDictionary) handler.Execute(new object[] { 7788, 7789, 7788, "SCOTT" });
            Trace.WriteLine(ToStringUtil.ToString(ret));
            Assert.IsNotNull(ret, "1");
            Assert.AreEqual(9, ret.Count, "2");
        }

        public void SetUpExecuteNullArgs()
        {
            Include(PATH);
        }

        [Test, S2]
        public void ExecuteNullArgs()
        {
            var sql = "SELECT * FROM emp WHERE empno = 7788";
            var handler = new BasicSelectHandler(
                DataSource,
                sql,
                new DictionaryDataReaderHandler()
                );
            var ret = (IDictionary) handler.Execute(null);
            Trace.WriteLine(ToStringUtil.ToString(ret));
            Assert.IsNotNull(ret, "1");
            Assert.AreEqual(9, ret.Count, "2");
        }

        public void SetUpExecuteParam()
        {
            Include(PATH);
        }

        [Test, S2]
        public void ExecuteParam()
        {
            var sql = "SELECT * FROM emp WHERE empno = @empno OR empno = :empno OR empno = ?";
            var handler = new BasicSelectHandler(
                DataSource,
                sql,
                new DictionaryDataReaderHandler()
                );
            var ret = (IDictionary) handler.Execute(new object[] { 7788, 7788, 7788 });
            Trace.WriteLine(ToStringUtil.ToString(ret));
            Assert.IsNotNull(ret, "1");
            Assert.AreEqual(9, ret.Count, "2");
        }

        public void SetUpExecuteDoubleAtParam()
        {
            Include(PATH);
        }

        [Test, S2]
        public void ExecuteDoubleAtParam()
        {
            if (DataSource.GetCommand().GetExType().Name.Equals("SqlCommand"))
            {
                var sql = "SELECT *, @@version FROM emp WHERE empno = @empno";
                var handler = new BasicSelectHandler(
                    DataSource,
                    sql,
                    new DictionaryDataReaderHandler()
                    );
                var ret = (IDictionary) handler.Execute(new object[] { 7788 });
                Assert.IsNotNull(ret, "1");
            }
        }
    }
}
