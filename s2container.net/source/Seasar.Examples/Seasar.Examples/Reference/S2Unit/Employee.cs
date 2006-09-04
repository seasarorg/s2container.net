#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
using System.Text;

namespace Seasar.Examples.Reference.S2Unit
{
	[Serializable]
	public class Employee
	{
		private long empno;
		private string ename;
		private short deptno;
		private string dname;

		public Employee() { }

		public Employee(long empno)
		{
			this.empno = empno;
		}

		public Employee(
			long empno,
			string ename,
			short deptno,
			string dname
		)
		{
			this.empno = empno;
			this.ename = ename;
			this.deptno = deptno;
			this.dname = dname;
		}

		public long Empno
		{
			get { return empno; }
			set { empno = value; }
		}

		public string Ename
		{
			get { return ename; }
			set { ename = value; }
		}

		public short Deptno
		{
			get { return deptno; }
			set { deptno = value; }
		}

		public string Dname
		{
			get { return dname; }
			set { dname = value; }
		}

		public override bool Equals(object other)
		{
			if (!(other is Employee)) return false;
			Employee castOther = (Employee) other;
			return this.Empno == castOther.Empno;
		}

		public override int GetHashCode()
		{
			return (int) this.Empno;
		}

		public override string ToString()
		{
			StringBuilder buf = new StringBuilder();
			buf.Append(empno).Append(", ");
			buf.Append(ename).Append(", ");
			buf.Append(deptno).Append(", ");
			buf.Append(dname);
			return buf.ToString();
		}
	}
}
