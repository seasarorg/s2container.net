using System;
using System.Collections;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.Unit;
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
		private string ccc_ = null;
		private Hashtable bbb_ = null;
		private DateTime ddd_ = new DateTime();
		private IList list1_ = null;

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
			Assert.IsNotNull(date_, "1");
			Assert.IsNotNull(bbb_, "2");
			Assert.IsTrue(bbb_.Count == 1, "3");
		}
	
		public void SetUpBindField2() 
		{
			Include("Seasar/Tests/Framework/Unit/bbb.dicon");
		}
		
		[Test, S2]
		public void testBindField2() 
		{
			Assert.IsNotNull(bbb_, "1");
			Assert.IsNotNull(ddd_, "2");
			Assert.IsNotNull(ccc_, "3");
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
			Assert.AreEqual(ccc_, "hoge","1");
		}
		
		[Test, S2]
		public void TestEmptyComponent() 
		{
			Include("empty.dicon");
		}
	}
	public class Hoge 
	{
		private string aaa;
		/**
				* @return Returns the aaa.
				*/
		public String GetAaa() 
		{
			return aaa;
		}
		/**
				* @param aaa The aaa to set.
				*/
		public void SetAaa(String aaa) 
		{
			this.aaa = aaa;
		}
	}
}
