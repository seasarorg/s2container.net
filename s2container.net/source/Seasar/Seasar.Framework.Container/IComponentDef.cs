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

namespace Seasar.Framework.Container
{
	/// <summary>
	/// コンポーネントの定義をします
	/// </summary>
	public interface IComponentDef : IArgDefAware, IPropertyDefAware, IInitMethodDefAware, 
		IDestroyMethodDefAware, IAspectDefAware,IMetaDefAware
	{
		/// <summary>
		/// コンポーネントを取得します
		/// </summary>
		///	<returns>コンポーネント</returns>
		object GetComponent();

		/// <summary>
		/// 受け取る型を指定してコンポーネントを取得します
		/// </summary>
		/// <param name="receiveType">受け取る型</param>
		/// <returns>コンポーネント</returns>
		object GetComponent(Type receiveType);

		/// <summary>
		/// 外部コンポーネントにプロパティ・インジェクション、
		/// メソッド・インジェクションを実行します。
		/// 外部コンポーネントと互換性のあるコンポーネント定義を利用します。
		/// instanceモードが"outer"と定義されたコンポーネントのみ有効です。
		/// </summary>
		/// <param name="outerComponent">外部コンポーネント</param>
		void InjectDependency(Object outerComponent);

		/// <summary>
		/// S2Container
		/// </summary>
		IS2Container Container{set;get;}

		/// <summary>
		/// コンポーネントのType
		/// </summary>
		Type ComponentType{get;}

		/// <summary>
		/// コンポーネントの名前
		/// </summary>
		string ComponentName{get;}

		/// <summary>
		/// 自動バインディングモード
		/// </summary>
		string AutoBindingMode{set;get;}

		/// <summary>
		/// インスタンスモード
		/// </summary>
		string InstanceMode{set;get;}

		/// <summary>
		/// Expression
		/// </summary>
		string Expression{set;get;}

		/// <summary>
		/// コンポーネントの定義を初期化します。
		/// </summary>
		void Init();

		/// <summary>
		/// プロキシを取得します。
		/// </summary>
		object GetProxy(Type proxyType);

		void AddProxy(Type proxyType, object proxy);

		void Destroy();

	}
}
