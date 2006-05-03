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

using System.Data;
using MbUnit.Core.Cons;
using MbUnit.Framework;
using Seasar.Extension.DataSets.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Examples.Reference.S2Unit
{
	[TestFixture]
	public class EmployeeDaoTest : S2TestCase
	{
		private IEmployeeDao dao_ = null;

		public void SetUpGetEmployee()
		{
			Include("Seasar.Examples/Reference/S2Unit/EmployeeDao.dicon");
		}

		[Test, S2(Tx.Rollback)]
		public void GetEmployee()
		{
			ReadXlsWriteDb("Seasar.Examples/Reference/S2Unit/GetEmployeePrepare.xls");
			Employee emp = dao_.GetEmployee(9900);
			DataSet expected = ReadXls("Seasar.Examples/Reference/S2Unit/GetEmployeeExpected.xls");
			S2Assert.AreEqual(expected, emp, "1");
		}

		public void Main()
		{
			using (MainClass mc = new MainClass())
			{
				mc.Main(new string[] { "Seasar.Examples.exe" });
			}
		}
	}
}
