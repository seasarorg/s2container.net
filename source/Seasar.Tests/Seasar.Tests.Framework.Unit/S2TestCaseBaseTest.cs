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
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.Unit;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Container;
using Seasar.Framework.Log;


namespace Seasar.Tests.Framework.Unit
{
	/// <summary>
	/// FrameworkTestCaseTest の概要の説明です。
	/// </summary>
	[TestFixture]
	public class S2TestCaseBaseTest : S2TestCase
	{
		private static Logger logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private const String PATH = "S2FrameworkTestCaseTest_ado.dicon";
		private bool testAaaSetUpInvoked = false;
		private DateTime date_ = new DateTime();
		private string _ccc = null;
		private Hashtable bbb_ = null;
		private DateTime ddd_ = new DateTime();
		private IList list1_ = null;
		private Hoge hoge_ = null;

		static S2TestCaseBaseTest()
		{
			FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
				Assembly.GetExecutingAssembly()) + ".config");
			XmlConfigurator.Configure(LogManager.GetRepository(), info);
		}

		[Test, S2]
		public void TestContainer()
		{
			Assert.IsNotNull(this.Container, "コンテナが取得できるはず");
		}

		[SetUp]
		public void SetUp() 
		{
			logger.Debug("SetUp");
		}
		[Test]
		public void TestStandard() 
		{
			logger.Debug("TestStandard");
		}

		[TearDown]
		public void TearDown() 
		{
			logger.Debug("TearDown");
		}		

		public void SetUpAaa() 
		{
			logger.Debug("SetUpAaa");
			testAaaSetUpInvoked = true;
		}
		[Test, S2]
		public void TestAaa() 
		{
			Assert.IsTrue(testAaaSetUpInvoked, "1");
		}

		public void TearDownAaa() 
		{
			logger.Debug("tearDownAaa");
		}
	
		public void SetUpBbbTx() 
		{
			logger.Debug("setUpBbbTx");
			Include(PATH);
		}

		[Test, S2]
		public void TestBbbTx() 
		{
			logger.Debug("testBbbTx");
		}
	
		public void SetUpBindField() 
		{
			Include(PATH);
			Register(typeof(Hashtable));
			Hashtable s = this.Container.GetComponent(typeof(Hashtable)) as Hashtable;
			s.Add("1", "hoge");
		}

		[Test, S2]
		public void TestBindField() 
		{
			Assert.IsNotNull(bbb_, "2");
			Assert.IsTrue(bbb_.Count == 1, "3");
		}
	
		public void SetUpBindField2() 
		{
			Include("Seasar/Tests/Framework/Unit/bbb.dicon");
		}
		
		[Test, S2]
		public void TestBindField2() 
		{
			Assert.IsNotNull(bbb_, "1");
            Assert.AreEqual(new DateTime(2006, 4, 1), ddd_, "2");
            Assert.AreEqual("hoge", _ccc, "3");
		}
   
		public void SetUpBindField3() 
		{
			Include("ccc.dicon");
		}
    
		[Test, S2]
		public void TestBindField3() 
		{
			Assert.IsNotNull(list1_, "1");
		}
		
		[Test, S2]
		[ExpectedException(typeof(TooManyRegistrationRuntimeException))]
		public void TestInclude() 
		{
			Include("aaa.dicon");
			GetComponent(typeof(DateTime));
		}
	
		public void SetUpIsAssignableFrom() 
		{
			Include("bbb.dicon");
		}

		[Test, S2]
		public void TestIsAssignableFrom() 
		{
			Assert.AreEqual(_ccc, "hoge","1");
		}
		
		public void SetUpPointcut() 
		{
			Include("ddd.dicon");
		}

		[Test, S2]
		public void TestPointcut() 
		{
			AopProxy aopProxy = RemotingServices.GetRealProxy(hoge_) as AopProxy;
			
			FieldInfo fieldInfo = aopProxy.GetType()
				.GetField("aspects_", BindingFlags.NonPublic | BindingFlags.Instance);
			
			IAspect[] aspects = fieldInfo.GetValue(aopProxy) as IAspect[];
			
			PointcutImpl pointcut = aspects[0].Pointcut as PointcutImpl;
			
			Assert.AreEqual(pointcut.IsApplied("GetAaa"), false, "1");
			Assert.AreEqual(pointcut.IsApplied("GetGreeting"), false, "2");
			Assert.AreEqual(pointcut.IsApplied("Greeting"), true, "3");
			Assert.AreEqual(pointcut.IsApplied("Greeting2"), true, "4");
			Assert.AreEqual(pointcut.IsApplied("GetGreetingEx"), false, "5");

			hoge_.GetAaa();
			hoge_.GetGreeting();
			hoge_.Greeting();
			hoge_.Greeting2();
			hoge_.GetGreetingEx();
		}

		[Test, S2]
		public void TestEmptyComponent() 
		{
			Include("empty.dicon");
		}
	}

}
