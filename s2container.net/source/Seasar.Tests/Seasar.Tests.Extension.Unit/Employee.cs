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
using System.Text;

namespace Seasar.Tests.Extension.Unit
{
	[Serializable]
	public class Employee 
	{
		private long empno;

		private string ename;

		private int deptno;
    
		private string dname;
    
		public Employee()
		{
		}

		public Employee(
			long empno,
			string ename,
			int deptno,
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
			set { empno = value; }
			get { return empno; }
		}

		public string Ename
		{
			set { ename = value; }
			get { return ename; }
		}

		public int Deptno
		{
			set { deptno = value; }
			get { return deptno; }
		}

		public string Dname
		{
			set { dname = value; }
			get { return dname; }
		}

		public override bool Equals(object other)
		{
			if (!(other is Employee)) { return false; }
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
