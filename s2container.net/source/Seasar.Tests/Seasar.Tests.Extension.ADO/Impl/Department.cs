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
using Nullables;

namespace Seasar.Tests.Extension.ADO.Impl
{
	[Serializable]
	public class Department
	{
		private int deptno;

		private string dname;

		private string loc;
    
		private NullableInt32 versionNo;
    
		private NullableBoolean active;
    
		public Department()
		{
		}

		public Department(
			int deptno,
			string dname,
			string loc,
			NullableInt32 versionNo,
			NullableBoolean active
			)
		{
			this.deptno = deptno;
			this.dname = dname;
			this.loc = loc;
			this.versionNo = versionNo;
			this.active = active;
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

		public string Loc
		{
			set { loc = value; }
			get { return loc; }
		}

		public NullableInt32 VersionNo
		{
			set { versionNo = value; }
			get { return versionNo; }
		}

		public NullableBoolean Active
		{
			set { active = value; }
			get { return active; }
		}
	}
}
