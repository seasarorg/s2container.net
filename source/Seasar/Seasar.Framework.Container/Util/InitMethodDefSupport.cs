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
	/// InitMethodDefSupport の概要の説明です。
	/// </summary>
	public sealed class InitMethodDefSupport
	{
		private IList methodDefs_ = ArrayList.Synchronized(new ArrayList());
		private IS2Container container_;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public InitMethodDefSupport()
		{
		}

		/// <summary>
		/// MethodDefを追加します。
		/// </summary>
		/// <param name="methodDef">MethodDef</param>
		public void AddInitMethodDef(IInitMethodDef methodDef)
		{
			if(container_ != null)
			{
				methodDef.Container = container_;
			}
			methodDefs_.Add(methodDef);
		}

		/// <summary>
		/// IInitMethodDefの数
		/// </summary>
		public int InitMethodDefSize
		{
			get { return methodDefs_.Count; }
		}

		/// <summary>
		/// 番号を指定してIInitMethodDefを取得します。
		/// </summary>
		/// <param name="index">IInitMethodDefの番号</param>
		/// <returns>IInitMethodDef</returns>
		public IInitMethodDef GetInitMethodDef(int index)
		{
			return (IInitMethodDef) methodDefs_[index];
		}

		/// <summary>
		/// S2Container
		/// </summary>
		public IS2Container Container
		{
			set
			{
				container_ = value;
				IEnumerator enu = methodDefs_.GetEnumerator();
				while(enu.MoveNext())
				{
					IInitMethodDef methodDef = (IInitMethodDef) enu.Current;
					methodDef.Container = value;
				}
			}
		}
	}
}
