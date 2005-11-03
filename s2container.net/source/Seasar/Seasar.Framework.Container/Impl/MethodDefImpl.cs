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
using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Impl
{
	/// <summary>
	/// MethodDefImpl の概要の説明です。
	/// </summary>
	public class MethodDefImpl : IMethodDef
	{
		private string methodName_;
		private ArgDefSupport argDefSupport_ = new ArgDefSupport();
		private IS2Container container_;
		private string expression_;

		public MethodDefImpl()
		{
		}

		public MethodDefImpl(string methodName)
		{
			methodName_ = methodName;
		}

		#region MethodDef メンバ

		public string MethodName
		{
			get
			{
				
				return methodName_;
			}
		}

		public object[] Args
		{
			get
			{
				
				object[] args = new object[this.ArgDefSize];
				for(int i = 0; i < this.ArgDefSize; ++i)
				{
					args[i] = this.GetArgDef(i).Value;
				}
				return args;
			}
		}

		public IS2Container Container
		{
			get
			{
				
				return container_;
			}
			set
			{
				
				container_ = value;
				argDefSupport_.Container = value;
			}
		}

		public string Expression
		{
			get
			{
				
				return expression_;
			}
			set
			{
				
				expression_ = value;
			}
		}

		#endregion

		#region IArgDefAware メンバ

		public void AddArgDef(IArgDef argDef)
		{
			
			argDefSupport_.AddArgDef(argDef);
		}

		public int ArgDefSize
		{
			get
			{
				
				return argDefSupport_.ArgDefSize;
			}
		}

		public IArgDef GetArgDef(int index)
		{
			
			return argDefSupport_.GetArgDef(index);
		}

		#endregion
	}
}
