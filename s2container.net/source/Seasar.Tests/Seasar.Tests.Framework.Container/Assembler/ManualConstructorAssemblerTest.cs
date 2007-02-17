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
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting;
using System.Reflection;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Assembler;
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Exceptions;
using MbUnit.Framework;
using log4net;
using log4net.Config;
using log4net.Util;

namespace Seasar.Tests.Framework.Container.Assembler
{
	/// <summary>
	/// ManualConstructorAssemblerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class ManualConstructorAssemblerTest
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
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			IArgDef argDef = new ArgDefImpl(new B());
			cd.AddArgDef(argDef);
			container.Register(cd);
			IConstructorAssembler assembler = new ManualConstructorAssembler(cd);
			A a = (A) assembler.Assemble();
			Assert.AreEqual("B", a.HogeName);
		}

		[Test]
		public void TestAssembleAspect()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			cd.AddAspeceDef(new AspectDefImpl(new TraceInterceptor()));
			IArgDef argDef = new ArgDefImpl(new B());
			cd.AddArgDef(argDef);
			container.Register(cd);
			IConstructorAssembler assembler = new ManualConstructorAssembler(cd);
			A a = (A) assembler.Assemble();
			Assert.AreEqual("B",a.HogeName);
		}

		[Test]
		public void TestAssembleIllegalConstructorArgument()
		{
			IS2Container container = new S2ContainerImpl();
			ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
			IArgDef argDef = new ArgDefImpl();
			argDef.Expression = "hoge";
			cd.AddArgDef(argDef);
			container.Register(cd);
			IConstructorAssembler assembler = new ManualConstructorAssembler(cd);
			try
			{
				assembler.Assemble();
				Assert.Fail();
			}
			catch(JScriptEvaluateRuntimeException ex)
			{
				Trace.WriteLine(ex);
			}
		}

        [Test]
        public void TestAssembleWithAspect()
        {
            IS2Container container = new S2ContainerImpl();
            ComponentDefImpl cd = new ComponentDefImpl(typeof(A));
            ComponentDefImpl cdB = new ComponentDefImpl(typeof(B), "B");
            IArgDef argDef = new ArgDefImpl();
            argDef.Expression = "B";
            cd.AddArgDef(argDef);
            container.Register(cd);
            AspectDefImpl ad = new AspectDefImpl(new TraceInterceptor());
            cdB.AddAspeceDef(ad);
            container.Register(cdB);
            IConstructorAssembler assembler = new ManualConstructorAssembler(cd);
            A a = (A)assembler.Assemble();
            Assert.AreEqual("B", a.HogeName, "1");
            Assert.IsTrue(RemotingServices.IsTransparentProxy(a.Hoge), "2");
        }

		public interface IFoo
		{
			string HogeName { get; }
		}

		public class A : MarshalByRefObject, IFoo
		{
			private IHoge hoge_;

			public A(IHoge hoge)
			{
				hoge_ = hoge;
			}

			public IHoge Hoge
			{
				get { return hoge_; }
			}

			public string HogeName
			{
				get
				{
					return hoge_.Name;
				}
			}
		}

		public interface IHoge
		{
			string Name { get; }
		}

		public class B : IHoge
		{
			public string Name
			{
				get
				{
					return "B";
				}
			}
		}


	}
}
