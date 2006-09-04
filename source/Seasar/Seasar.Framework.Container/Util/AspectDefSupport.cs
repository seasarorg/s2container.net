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
	/// AspectDefSupport の概要の説明です。
	/// </summary>
	public sealed class AspectDefSupport
	{
		private IList aspectDefs_ = ArrayList.Synchronized(new ArrayList());
		private IS2Container container_;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public AspectDefSupport()
		{
		}

		/// <summary>
		/// IAspectDefを追加します。
		/// </summary>
		/// <param name="aspectDef"></param>
		public void AddAspectDef(IAspectDef aspectDef)
		{
			if(container_ != null)
			{
				aspectDef.Container = container_;
			}
			aspectDefs_.Add(aspectDef);
		}

		/// <summary>
		/// IAspectDefの数
		/// </summary>
		public int AspectDefSize
		{
			get { return aspectDefs_.Count; }
		}

		/// <summary>
		/// 番号を指定してIAspectDefを返します。
		/// </summary>
		/// <param name="index">番号</param>
		/// <returns>IAspectDef</returns>
		public IAspectDef GetAspectDef(int index)
		{
			return (IAspectDef) aspectDefs_[index];
		}

		/// <summary>
		/// S2Container
		/// </summary>
		public IS2Container Container
		{
			set
			{
				container_ = value;
				IEnumerator enu = aspectDefs_.GetEnumerator();
				while(enu.MoveNext())
				{
					((IAspectDef) enu.Current).Container = value;
				}
			}
		}
	}
}
