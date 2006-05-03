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

using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Examples.Reference.S2Unit
{
	public class EmployeeDao : IEmployeeDao
	{
		private ISelectHandler getEmployeeHandler_;

		public EmployeeDao() { }

		public ISelectHandler GetEmployeeHandler
		{
			get { return getEmployeeHandler_; }
			set { getEmployeeHandler_ = value; }
		}

		public Employee GetEmployee(long empno)
		{
			// TODO 暫定措置。DICONファイルでクラスが指定できるようになったら削除する。
			if (getEmployeeHandler_ is BasicSelectHandler)
			{
				((BasicSelectHandler) getEmployeeHandler_).DataReaderHandler = new BeanDataReaderHandler(typeof(Employee));
			}
			return (Employee) getEmployeeHandler_.Execute(new object[] { empno });
		}
	}
}
