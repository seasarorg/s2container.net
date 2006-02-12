using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.Unit;
using Seasar.Framework.Log;

namespace Seasar.Tests.Extension.Unit
{
	[TestFixture]
	public class S2TestCaseTest : S2TestCase
	{
		private static Logger logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private const String PATH = "ado.dicon";

		static S2TestCaseTest()
		{
			FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
				Assembly.GetExecutingAssembly()) + ".config");
			XmlConfigurator.Configure(LogManager.GetRepository(), info);
		}

		public void SetUpNotSupported()
		{
			Include(PATH);
		}

		[Test, S2(Tx.NotSupported)]
		public void NotSupported()
		{
			logger.Debug( "NotSupported test" );
		}

		public void SetUpNotSupported2()
		{
			Include(PATH);
		}

		[Test, S2]
		public void NotSupported2()
		{
			logger.Debug( "NotSupported2 test" );
		}

		public void SetUpRollback()
		{
			Include(PATH);
		}

		[Test, S2(Tx.Rollback)]
		public void Rollback()
		{
			logger.Debug( "Rollback test" );
		}

		public void SetUpCommit()
		{
			Include(PATH);
		}

		[Test, S2(Tx.Commit)]
		public void Commit()
		{
			logger.Debug( "Commit test" );
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
