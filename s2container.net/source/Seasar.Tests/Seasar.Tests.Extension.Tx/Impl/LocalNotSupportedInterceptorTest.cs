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
using System.Data;
using System.IO;
using System.Reflection;

using Seasar.Extension.Tx;

using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;

using log4net;
using log4net.Config;
using log4net.Util;

using NUnit.Framework;

namespace Seasar.Tests.Extension.Tx.Impl
{
	/// <summary>
	/// LocalNotSupportedInterceptorTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class LocalNotSupportedInterceptorTest
	{
		private const string path = "Seasar/Tests/Extension/Tx/Impl/LocalNotSupportedInterceptorTest.dicon";

		public LocalNotSupportedInterceptorTest()
		{
			FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
				Assembly.GetExecutingAssembly()) + ".config");
			XmlConfigurator.Configure(LogManager.GetRepository(), info);
		}

		private IS2Container container = null;

		private ILocalTxTest tester = null;
		private ITransactionContext context = null;

		[SetUp]
		public void SetUp()
		{
			container = S2ContainerFactory.Create(path);
			container.Init();
			tester = container.GetComponent(typeof(ILocalTxTest)) as ILocalTxTest;
			context = container.GetComponent(typeof(ITransactionContext)) as ITransactionContext;

		}

		[TearDown]
		public void TearDown()
		{
			container.Destroy();
		}

		[Test]
		public void TestProceed()
		{
			Assert.IsFalse(tester.IsInTransaction());
			Assert.IsFalse(context.Current.IsInTransaction);
		}

		[Test]
		public void TestProceedInTx()
		{
			using(ITransactionContext ctx = this.context.Create())
			{
				ctx.Begin();
				this.context.Current = ctx;

				Assert.IsFalse(tester.IsInTransaction());
				
				IDbConnection con = tester.GetConnection();
				Assert.IsFalse(Object.ReferenceEquals(ctx.Connection, con));
				Assert.IsFalse(canStartTx(con));
				ctx.Rollback();
			}
		}

		protected bool canStartTx(IDbConnection con)
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
			using(ITransactionContext ctx = this.context.Create())
			{
				ctx.Begin();
				this.context.Current = ctx;

				try
				{
					tester.throwException();
					Assert.Fail();
				}
				catch(Exception e)
				{
					Assert.IsTrue(e is NotSupportedException);
					Assert.IsTrue(context.Current.IsInTransaction);
				}
				IDbConnection con = tester.GetConnection();
				Assert.IsFalse(Object.ReferenceEquals(ctx.Connection, con));
				Assert.IsFalse(canStartTx(con));
				ctx.Rollback();
			}

		}
	}
}
