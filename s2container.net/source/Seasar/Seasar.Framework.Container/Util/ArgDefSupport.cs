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
using System.Collections;

namespace Seasar.Framework.Container.Util
{
	/// <summary>
	/// IArgDefの設定をサポートします。
	/// </summary>
	public class ArgDefSupport
	{
		private IList argDefs_ = ArrayList.Synchronized(new ArrayList());
		private IS2Container container_;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ArgDefSupport()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="argDef">IArgDef</param>
		public void AddArgDef(IArgDef argDef)
		{
			if(container_ != null)
			{
				argDef.Container = container_;
			}
			argDefs_.Add(argDef);
		}

		/// <summary>
		/// IArgDefの数
		/// </summary>
		public int ArgDefSize
		{
			get
			{
				return argDefs_.Count;
			}
		}

		/// <summary>
		/// 番号を指定してIArgDefを取得します。
		/// </summary>
		/// <param name="index">番号</param>
		/// <returns>IArgDef</returns>
		public IArgDef GetArgDef(int index)
		{
			return (IArgDef) argDefs_[index];
		}

		/// <summary>
		/// S2Container
		/// </summary>
		public IS2Container Container
		{
			set
			{
				container_ = value;
				IEnumerator enu = argDefs_.GetEnumerator();
				while(enu.MoveNext())
				{
					IArgDef argDef = (IArgDef)enu.Current;
					argDef.Container = value;
				}
			}
		}
	}
}
