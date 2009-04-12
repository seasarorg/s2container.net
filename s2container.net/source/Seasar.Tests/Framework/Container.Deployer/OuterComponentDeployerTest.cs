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

using System;
using System.Collections;
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Deployer;
using Seasar.Framework.Container.Impl;

namespace Seasar.Tests.Framework.Container.Deployer
{
    [TestFixture]
    public class OuterComponentDeployerTest
    {
        [Test]
        public void TestInjectDependency()
        {
            ComponentDefImpl cd = new ComponentDefImpl(typeof(Hashtable));
            IInitMethodDef md = new InitMethodDefImpl("Add");
            md.AddArgDef(new ArgDefImpl("aaa"));
            md.AddArgDef(new ArgDefImpl("hoge"));
            cd.AddInitMethodDef(md);
            IComponentDeployer deployer = new OuterComponentDeployer(cd);
            Hashtable myTable = new Hashtable();
            deployer.InjectDependency(myTable);
            Assert.AreEqual("hoge", myTable["aaa"]);
        }

        [Test]
        public void TestDeploy()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(Hashtable));
            container.Register(cd);
            IComponentDeployer deployer = new OuterComponentDeployer(cd);
            try
            {
                deployer.Deploy(typeof(Hashtable));
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                Trace.WriteLine(ex);
            }
        }
    }
}
