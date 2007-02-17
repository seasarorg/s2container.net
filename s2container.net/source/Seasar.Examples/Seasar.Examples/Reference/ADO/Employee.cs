#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using System.Data.SqlTypes;
using System.Text;
using Nullables;

namespace Seasar.Examples.Reference.ADO
{
	[Serializable]
	public class Employee 
	{
		private long empno;

		private string ename;

		private string job;

		private NullableInt16 mgr;

		private NullableDateTime hiredate;

		private NullableSingle sal;

		private NullableSingle comm;

		private NullableInt32 deptno;

		private NullableDateTime tstamp;

		public Employee()
		{
		}

		public Employee(
			long empno,
			string ename,
			string job,
			NullableInt16 mgr,
			NullableDateTime hiredate,
			NullableSingle sal,
			NullableSingle comm,
			NullableInt32 deptno,
			NullableDateTime tstamp
			)
		{
			this.empno = empno;
			this.ename = ename;
			this.job = job;
			this.mgr = mgr;
			this.sal = sal;
			this.comm = comm;
			this.deptno = deptno;
			this.tstamp = tstamp;
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

		public string Job
		{
			set { job = value; }
			get { return job; }
		}

		public NullableInt16 Mgr
		{
			set { mgr = value; }
			get { return mgr; }
		}

        public NullableDateTime Hiredate
        {
            set { hiredate = value; }
            get { return hiredate; }
        }

		public NullableSingle Sal
		{
			set { sal = value; }
			get { return sal; }
		}

		public NullableSingle Comm
		{
		    set { comm = value; }
		    get { return comm; }
		}

		public NullableInt32 Deptno
		{
			set { deptno = value; }
			get { return deptno; }
		}

		public NullableDateTime Tstamp
		{
			set { tstamp = value; }
			get { return tstamp; }
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
			buf.Append(job).Append(", ");
			buf.Append(mgr).Append(", ");
			buf.Append(hiredate).Append(", ");
			buf.Append(sal).Append(", ");
			buf.Append(comm).Append(", ");
			buf.Append(deptno).Append(", ");
			buf.Append(tstamp);
			return buf.ToString();
		}
	}
}
