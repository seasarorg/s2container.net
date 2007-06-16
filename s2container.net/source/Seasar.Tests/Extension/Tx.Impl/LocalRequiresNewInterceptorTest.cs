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

using System;
using System.Data;
using System.IO;
using System.Reflection;
using Seasar.Extension.Tx;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;

namespace Seasar.Tests.Extension.Tx.Impl
{
    [TestFixture]
    public class LocalRequiresNewInterceptorTest
    {
        private const string PATH = "Seasar/Tests/Extension/Tx/Impl/LocalRequiresNewInterceptorTest.dicon";
        private IS2Container _container = null;
        private ILocalTxTest _tester = null;
        private ITransactionContext _context = null;

        public LocalRequiresNewInterceptorTest()
        {
            FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        [SetUp]
        public void SetUp()
        {
            _container = S2ContainerFactory.Create(PATH);
            _container.Init();
            _tester = _container.GetComponent(typeof(ILocalTxTest)) as ILocalTxTest;
            _context = _container.GetComponent(typeof(ITransactionContext)) as ITransactionContext;

        }

        [TearDown]
        public void TearDown()
        {
            _container.Destroy();
        }

        [Test]
        public void TestProceed()
        {
            Assert.IsTrue(_tester.IsInTransaction());
            Assert.IsFalse(_context.Current.IsInTransaction);
        }

        [Test]
        public void TestProceedInTx()
        {
            using (ITransactionContext ctx = _context.Create())
            {
                ctx.Begin();
                _context.Current = ctx;

                Assert.IsTrue(_tester.IsInTransaction());

                IDbConnection con = _tester.GetConnection();
                Assert.IsFalse(ReferenceEquals(ctx.Connection, con));
                Assert.IsFalse(CanStartTx(con));
                ctx.Rollback();
            }
        }

        protected bool CanStartTx(IDbConnection con)
        {
            Assert.IsNotNull(con);
            try
            {
                con.BeginTransaction();
                return true;
            }
            catch
            {
                return false;
            }

        }

        [Test]
        public void TestProceedExceptionInTx()
        {
            using (ITransactionContext ctx = _context.Create())
            {
                ctx.Begin();
                _context.Current = ctx;

                try
                {
                    _tester.throwException();
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e is NotSupportedException);
                    Assert.IsTrue(_context.Current.IsInTransaction);
                }
                IDbConnection con = _tester.GetConnection();
                Assert.IsFalse(object.ReferenceEquals(ctx.Connection, con));
                Assert.IsFalse(CanStartTx(con));
                ctx.Rollback();
            }
        }
    }
}
