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
using System.Collections;

namespace Seasar.Framework.Container.Util
{
	/// <summary>
	/// DestroyMethodDefSupport の概要の説明です。
	/// </summary>
	public sealed class DestroyMethodDefSupport
	{
		private IList methodDefs_ = ArrayList.Synchronized(new ArrayList());
		private IS2Container container_;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public DestroyMethodDefSupport()
		{
		}

		/// <summary>
		/// DestroyMethodDefを追加します。
		/// </summary>
		/// <param name="methodDef">MethodDef</param>
		public void AddDestroyMethodDef(IDestroyMethodDef methodDef)
		{
			if(container_ != null)
			{
				methodDef.Container = container_;
			}
			methodDefs_.Add(methodDef);
		}

		/// <summary>
		/// DestroyMethodDefの数
		/// </summary>
		public int DestroyMethodDefSize
		{
			get { return methodDefs_.Count; }
		}

		/// <summary>
		/// 番号を指定してIDestroyMethodDefを取得します。
		/// </summary>
		/// <param name="index">IDestroyMethodDefの番号</param>
		/// <returns>IDestroyMethodDef</returns>
		public IDestroyMethodDef GetDestroyMethodDef(int index)
		{
			return (IDestroyMethodDef) methodDefs_[index];
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
					IDestroyMethodDef methodDef = (IDestroyMethodDef) enu.Current;
					methodDef.Container = value;
				}
			}
		}
	}
}
