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
using System.IO;
using System.Reflection;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Assembler;
using Seasar.Framework.Container.Impl;
using MbUnit.Framework;
using log4net;
using log4net.Config;
using log4net.Util;

namespace Seasar.Tests.Framework.Container.Assembler
{
    [TestFixture]
    public class DefaultConstructorAssemblerTest
    {
        [SetUp]
        public void SetUp()
        {
            FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        [Test]
        public void TestAssemble()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(ArrayList));
            container.Register(cd);
            IConstructorAssembler assembler = new DefaultConstructorAssembler(cd);
            Assert.IsNotNull(assembler.Assemble());
        }

        [Test]
        public void TestAssembleAspect()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
            cd.AddAspeceDef(new AspectDefImpl(new TraceInterceptor()));
            container.Register(cd);
            IConstructorAssembler assembler = new DefaultConstructorAssembler(cd);
            A a = (A) assembler.Assemble();
            Trace.WriteLine(a.Name);
        }

        public class A : MarshalByRefObject
        {
            public string Name { get { return "A"; } }
        }
    }
}
