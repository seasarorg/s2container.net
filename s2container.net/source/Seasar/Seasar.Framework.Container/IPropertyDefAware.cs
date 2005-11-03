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

namespace Seasar.Framework.Container
{
	/// <summary>
	/// IPropertyDefの設定が可能になります。
	/// </summary>
	public interface IPropertyDefAware
	{

		/// <summary>
		/// IPropertyDefを追加します。
		/// </summary>
		/// <param name="propertyDef">IPropertyDef</param>
		void AddPropertyDef(IPropertyDef propertyDef);

		/// <summary>
		/// IPropertyDefの数
		/// </summary>
		int PropertyDefSize{get;}

		/// <summary>
		/// 番号を指定してIPropertyDefを取得します。
		/// </summary>
		/// <param name="index">IPropertyDefの番号</param>
		/// <returns>IPropertyDef</returns>
		IPropertyDef GetPropertyDef(int index);

		/// <summary>
		/// 名前を指定してIPropertyDefを取得します。
		/// </summary>
		/// <param name="propertyName">IPropertyDefの名前</param>
		/// <returns>IPropertyDef</returns>
		IPropertyDef GetPropertyDef(string propertyName);

		/// <summary>
		/// 指定した名前のIPropertyDefを持っているか判定します。
		/// </summary>
		/// <param name="propertyName">IPropertyDefの名前</param>
		/// <returns>存在するならtrue</returns>
		bool HasPropertyDef(string propertyName);
	}
}
