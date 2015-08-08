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

using System;
using System.IO;
using System.EnterpriseServices;
using System.Reflection;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;

namespace Seasar.Tests.Extension.Tx.Impl
{
    [Ignore("DTC系は調査が難しいと思われるため保留")]
    [TestFixture]
    //[Transaction(TransactionOption.RequiresNew)]
    public class DTCRequiresNewInterceptorTest : ServicedComponent
    {
        private const string PATH = "Seasar/Tests/Extension/Tx/Impl/DTCRequiresNewInterceptorTest.dicon";
        private IS2Container _container = null;
        private Tx2Test _tester = null;

        static DTCRequiresNewInterceptorTest()
        {
            var info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        [SetUp]
        public void SetUp()
        {
            _container = S2ContainerFactory.Create(PATH);
            _container.Init();
            _tester = _container.GetComponent(typeof(Tx2Test)) as Tx2Test;
        }

        [Ignore("DTC系は調査が難しいと思われるため保留")]
        [Test]
        [AutoComplete]
        public void TestProceed()
        {
            var txid = ContextUtil.TransactionId;
            Assert.IsFalse(txid.Equals(_tester.GetTransactionId()));
            Assert.IsTrue(_tester.IsInTransaction());
        }
    }
}
