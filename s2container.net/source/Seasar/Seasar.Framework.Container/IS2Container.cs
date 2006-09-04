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
using System.Web;
using System.Web.SessionState;

namespace Seasar.Framework.Container
{
	/// <summary>
	/// コンポーネントを管理するDIコンテナのインターフェース
	/// </summary>
	public interface IS2Container : IMetaDefAware
	{

		/// <summary>
		/// キーを指定してコンポーネントを取得します。
		/// キーと一致するコンポーネント名を持つコンポーネントを取得します。
		/// </summary>
		/// <param name="componentKey">コンポーネントを取得するためのキー</param>
		/// <returns>コンポーネント</returns>
		Object GetComponent(object componentKey);

		/// <summary>
		/// 外部コンポーネントにプロパティ・インジェクション、
		/// メソッド・インジェクションを実行します。
		/// 外部コンポーネントと互換性のあるコンポーネント定義を利用します。
		/// instanceモードが"outer"と定義されたコンポーネントのみ有効です。
		/// </summary>
		/// <param name="outerComponent">外部コンポーネント</param>
		void InjectDependency(Object outerComponent);

		/// <summary>
		/// 外部コンポーネントにプロパティ・インジェクション、
		/// メソッド・インジェクションを実行します。
		/// 外部コンポーネント定義のキーと互換性のある
		/// コンポーネント定義を利用します。
		/// instanceモードが"outer"と定義されたコンポーネントのみ有効です。
		/// </summary>
		/// <param name="outerComponent">外部コンポーネント</param>
		/// <param name="componentType">外部コンポーネント定義のキー(Type)</param>
		void InjectDependency(Object outerComponent,Type componentType);

		/// <summary>
		/// 外部コンポーネントにプロパティ・インジェクション、
		/// メソッド・インジェクションを実行します。
		/// 外部コンポーネント定義のキーと一致する名前のコンポーネント定義を
		/// 取得します。
		/// instanceモードが"outer"と定義されたコンポーネントのみ有効です。
		/// </summary>
		/// <param name="outerComponent">外部コンポーネント</param>
		/// <param name="componentName">外部コンポーネント定義のキー（名前）</param>
		void InjectDependency(Object outerComponent,string componentName);

		/// <summary>
		/// オブジェクトをコンポーネントとして登録します。
		/// キーはオブジェクトのクラスになります。
		/// </summary>
		/// <param name="component">コンポーネントとして登録するオブジェクト</param>
		void Register(Object component);

		/// <summary>
		/// オブジェクトを名前付きコンポーネントとして登録します。
		/// </summary>
		/// <param name="component">コンポーネントとして登録するオブジェクト</param>
		/// <param name="componentName">コンポーネント名</param>
		void Register(Object component,string componentName);

		/// <summary>
		/// Typeをコンポーネント定義として登録します
		/// </summary>
		/// <param name="componentType">コンポーネントのType</param>
		void Register(Type componentType);

		/// <summary>
		/// Typeを名前付きコンポーネント定義として登録します。
		/// </summary>
		/// <param name="componentType">コンポーネントのType</param>
		/// <param name="componentName">コンポーネント名</param>
		void Register(Type componentType,string componentName);

		/// <summary>
		/// コンポーネント定義を登録します。
		/// </summary>
		/// <param name="componentDef">登録するコンポーネント定義</param>
		void Register(IComponentDef componentDef);

		/// <summary>
		/// コンポーネント定義の数を取得します。
		/// </summary>
		int ComponentDefSize{get;}

		/// <summary>
		/// 番号を指定してコンポーネント定義を取得します。
		/// </summary>
		/// <param name="index">番号</param>
		/// <returns>コンポーネント定義</returns>
		IComponentDef GetComponentDef(int index);

		/// <summary>
		/// 指定したキーからコンポーネント定義を取得します。
		/// </summary>
		/// <param name="componentName">コンポーネントのキー</param>
		/// <returns>コンポーネント定義</returns>
		IComponentDef GetComponentDef(object key);

		/// <summary>
		/// 指定したキーのコンポーネント定義を持っているか判定します。
		/// </summary>
		/// <param name="componentKey">コンポーネントのキー</param>
		/// <returns>存在するならtrue</returns>
		bool HasComponentDef(object componentKey);

		/// <summary>
		/// rootのコンテナで、pathに対応するコンテナが既にロードされて
		/// いるかを返します。
		/// </summary>
		/// <param name="path">パス</param>
		/// <returns>ロードされていたらtrue</returns>
		bool HasDescendant(string path);

		/// <summary>
		/// rootのコンテナで、指定したパスに対応するロード済みのコンテナを
		/// 取得します。
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		IS2Container GetDescendant(string path);

		/// <summary>
		/// rootのコンテナに、ロード済みのコンテナを登録します。
		/// </summary>
		/// <param name="descendant"></param>
		void RegisterDescendant(IS2Container descendant);

		/// <summary>
		/// 子コンテナをincludeします。
		/// </summary>
		/// <param name="child">includeする子コンテナ</param>
		void Include(IS2Container child);

		/// <summary>
		/// 子コンテナの数を取得します。
		/// </summary>
		int ChildSize{get;}

		/// <summary>
		/// 番号を指定して子コンテナを取得します
		/// </summary>
		/// <param name="index">子コンテナの番号</param>
		/// <returns>子コンテナ</returns>
		IS2Container GetChild(int index);

		/// <summary>
		/// コンテナを初期化します。
		/// 子コンテナを持つ場合、子コンテナを全て初期化した後、自分を初期化します。
		/// </summary>
		void Init();

		/// <summary>
		/// 名前空間
		/// </summary>
		string Namespace{set;get;}

		/// <summary>
		/// 設定ファイルのパス
		/// </summary>
		string Path{set;get;}

		/// <summary>
		/// ルートのコンテナ
		/// </summary>
		IS2Container Root{set;get;}

		/// <summary>
		/// コンテナの終了処理を行います。
		/// 子コンテナを持つ場合、自分の終了処理を実行した後、
		/// 子コンテナ全ての終了処理を行います。
		/// </summary>
		void Destroy();

		/// <summary>
		/// HTTPのレスポンス
		/// </summary>
		HttpResponse Response { get; }

		/// <summary>
		/// HTTPのリクエスト
		/// </summary>
		HttpRequest Request { get; }

		/// <summary>
		/// HTTPのセッション
		/// </summary>
		HttpSessionState Session { get; }

		/// <summary>
		/// アプリケーションの基本クラス
		/// </summary>
		HttpApplication HttpApplication { get; }

		/// <summary>
		/// HTTP 要求に関する HTTP 固有のすべての情報
		/// </summary>
		HttpContext HttpContext { set;get; }
	}
}
