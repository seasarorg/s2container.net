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
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.Unit;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Tests.Extension.Unit
{
	[TestFixture]
	public class S2TestCaseTest : S2TestCase
	{
		private static Logger logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private const String PATH = "Ado.dicon";

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

		[Test, S2(Seasar.Extension.Unit.Tx.NotSupported)]
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

		[Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
		public void Rollback()
		{
			logger.Debug( "Rollback test" );
		}

		public void SetUpCommit()
		{
			Include(PATH);
		}

		[Test, S2(Seasar.Extension.Unit.Tx.Commit)]
		public void Commit()
		{
			logger.Debug( "Commit test" );
		}

		public void SetUpReadXlsTx() 
		{
			Include(PATH);
		}

		[Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
		public void ReadXlsTx() 
		{
			DataSet dataSet = ReadXls("testdata.xls");
			Trace.WriteLine(ToStringUtil.ToString(dataSet));
			Assert.AreEqual(2, dataSet.Tables.Count, "1");
			DataTable table = dataSet.Tables["emp"];
			Assert.AreEqual(2, table.Rows.Count, "2");
			Assert.AreEqual(3, table.Columns.Count, "3");
			DataRow row = table.Rows[0];
			Assert.AreEqual(9900m, row["empno"], "4");
			Assert.AreEqual("hoge", row["ename"], "5");
			Assert.AreEqual("aaa", row["dname"], "6");
		}

		public void SetUpReadDbByTableTx() 
		{
			Include(PATH);
		}

		[Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
		public void ReadDbByTableTx() 
		{
			DataTable table = ReadDbByTable("emp", "empno = 7788");
            Trace.WriteLine(ToStringUtil.ToString(table));
			Assert.AreEqual(1, table.Rows.Count, "1");
		}

		public void SetUpWriteXlsTx() 
		{
			Include(PATH);
			string exportPath = Path.GetFullPath(ConvertPath("aaa.xls"));
			if (File.Exists(exportPath))
			{
				File.Delete(Path.GetFullPath(ConvertPath("aaa.xls")));		
			}
		}

		[Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
		public void WriteXlsTx() 
		{
			DataSet dataSet = ReadXls("testdata.xls");
			WriteXls("aaa.xls", dataSet);
            Trace.WriteLine(ToStringUtil.ToString(dataSet));
            DataSet dataSet2 = ReadXls("aaa.xls");
			S2Assert.AreEqual(dataSet, dataSet2);
		}
	}
}
