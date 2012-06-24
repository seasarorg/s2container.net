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

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MbUnit.Framework;
using Seasar.Framework.Log;
using log4net;
using log4net.Config;
using log4net.Util;

namespace Seasar.Tests.Framework.Log
{
    [TestFixture]
    public class LoggerTest
    {
        private readonly Logger _logger = Logger.GetLogger(typeof(LoggerTest));

        static LoggerTest()
        {
            FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void TestGetLogger()
        {
            Assert.AreEqual(_logger, Logger.GetLogger(GetType()));
        }

        [Test]
        public void TestDebug()
        {
            _logger.Debug("debug");
        }

        [Test]
        public void TestInfo()
        {
            _logger.Info("info");
        }

        [Test]
        public void TestWarn()
        {
            _logger.Warn("warn");
        }

        [Test]
        public void TestError()
        {
            _logger.Error("error");
        }

        [Test]
        public void TestFatal()
        {
            _logger.Fatal("fatal");
        }

        [Test]
        public void TestLog()
        {
            _logger.Log("ESSR0001", new object[] { "test" });
        }

        [Test]
        public void TestLog_NameSpace()
        {
            //  ó·äOÇÕî≠ê∂ÇµÇ»Ç¢Ç±Ç∆Ç™ämîFÇ≈Ç´ÇÍÇŒOK
            _logger.Log("ESSR0001", new object[] { "test" }, null, "Hoge");
        }

        [Test]
        public void TestPerformance()
        {
            int num = 100;
            long start = DateTime.Now.Ticks;
            for (int i = 0; i < num; ++i)
            {
                Trace.WriteLine("test" + i);
            }
            long csout = DateTime.Now.Ticks - start;
            start = DateTime.Now.Ticks;
            for (int i = 0; i < num; ++i)
            {
                _logger.Fatal("test" + i);
            }
            long loggerTicks = DateTime.Now.Ticks - start;
            Trace.WriteLine("Console:" + csout);
            Trace.WriteLine("Logger:" + loggerTicks);
        }
    }
}
