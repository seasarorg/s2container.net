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
using MbUnit.Framework;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.ADO.Impl
{
	[TestFixture]
	public class BasicUpdateHandlerTest : S2TestCase
	{
		private const string PATH = "Ado.dicon";

		public void SetUpExecute() 
		{
			Include(PATH);
		}

		[Test, S2(Tx.Rollback)]
		public void Execute()
		{
			string sql = "update emp set ename = @ename, comm = @comm where empno = @empno";
			BasicUpdateHandler handler = new BasicUpdateHandler(DataSource, sql);
			object[] args = new object[] { "SCOTT", null, 7788 };
			Type[] argTypes = new Type[] { typeof(string), typeof(Nullables.NullableInt32), typeof(int) };
			string[] argNames = new string[] { "ename", "comm", "empno" };
			int ret = handler.Execute(args, argTypes, argNames);
			Assert.AreEqual(1, ret, "1");
		}

		public void SetUpExecuteNullArgs() 
		{
			Include(PATH);
		}

		[Test, S2(Tx.Rollback)]
		public void ExecuteNullArgs()
		{
			string sql = "delete from emp";
			BasicUpdateHandler handler = new BasicUpdateHandler(DataSource, sql);
			int ret = handler.Execute(null);
			Assert.AreEqual(14, ret, "1");
		}
	}
}
