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
using MbUnit.Framework;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;
using Seasar.Extension.DataSets.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.ADO.Impl
{
	[TestFixture]
	public class BasicSelectHandlerTest : S2TestCase
	{
		private const string PATH = "Ado.dicon";

		public void SetUpExecute() 
		{
			Include(PATH);
		}

		[Test, S2(Tx.Rollback)]
		public void Execute()
		{
			ValueTypes.Init(DataSource);
			string sql = "select ename, empno from emp where empno = @empno";
			BasicSelectHandler handler = new BasicSelectHandler(DataSource, sql, new DataTableResultSetHandler("emp"));
			object[] args = new object[] { 7788 };
			Type[] argTypes = new Type[] { typeof(string) };
			string[] argNames = new string[] { "empno" };
			DataTable ret = handler.Execute(args, argTypes, argNames) as DataTable;
			Assert.AreEqual(1, ret.Rows.Count);
		}

		public void SetUpExecuteNullArgs() 
		{
			Include(PATH);
		}

		[Test, S2(Tx.Rollback)]
		public void ExecuteNullArgs()
		{
			ValueTypes.Init(DataSource);
			string sql = "select * from emp";
			BasicSelectHandler handler = new BasicSelectHandler(DataSource, sql, new DataTableResultSetHandler("emp"));
			DataTable ret = handler.Execute(null) as DataTable;
			Assert.AreEqual(14, ret.Rows.Count);
		}
	}
}
