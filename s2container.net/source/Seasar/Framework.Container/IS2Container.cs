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
    /// DIとAOPをサポートしたS2コンテナのインターフェースです。
    /// </summary>
    /// <remarks>
    /// <para>S2Containerの役割について</para>
    /// <para>
    /// コンポーネントの管理を行う機能を提供します。 
    /// コンポーネントとは１つかまたそれ以上のクラスで構成されるJavaオブジェクトです。
    /// S2コンテナはコンポーネントの生成、コンポーネントの初期化、コンポーネントの取得を提供します。
    /// コンポーネントを取得するキーには、コンポーネント名、コンポーネントのクラス、
    /// またはコンポーネントが実装するインターフェースを指定することができます。
    /// </para>
    /// <para>
    /// S2コンテナのインスタンス階層について
    /// </para>
    /// <para>
    /// S2コンテナ全体は複数のコンテナにより階層化されています。
    /// 一つのコンテナは複数のコンテナをインクルードすることができます。
    /// 複数のコンテナが同一のコンテナをインクルードすることができます。
    /// </para>
    /// <para>
    /// S2コンテナのインジェクションの種類について
    /// </para>
    /// <para>
    /// S2コンテナは3種類のインジェクションをサポートします。
    /// <list type="bullet">
    /// <item>
    /// <term><see cref="IConstructorAssembler">コンストラクタ・インジェクション</see></term>
    /// <description>コンストラクタ引数を利用してコンポーネントをセットします。</description>
    /// </item>
    /// <item>
    /// <term><see cref="IPropertyAssembler">プロパティ・インジェクション</see></term>
    /// <description>プロパティを利用してコンポーネントをセットします。 </description>
    /// </item>
    /// <item>
    /// <term><see cref="IMethodAssembler">メソッド・インジェクション</see></term>
    /// <description>任意のメソッドを利用してコンポーネントをセットします。</description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <seealso cref="http://s2container.seasar.org/ja/images/include_range_20040706.png">
    /// インクルードの参照範囲についてのイメージ</seealso>
    /// <seealso cref="http://s2container.seasar.org/ja/images/include_search_20040706.png">
    /// コンテナの検索順についてのイメージ</seealso>
    public interface IS2Container : IMetaDefAware
    {
        /// <summary>
        /// 指定されたキーに対するコンポーネントを返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// キーが文字列の場合、名前が一致するコンポーネントを返します。
        /// キーがクラスまたはインターフェースの場合、キーの型に代入可能なコンポーネントを返します。
        /// </para>
        /// </remarks>
        /// <param name="componentKey">コンポーネントを取得するためのキー</param>
        /// <returns>コンポーネント</returns>
        /// <exception cref="ComponentNotFoundRuntimeException">コンポーネントが見つからない場合</exception>
        /// <exception cref="TooManyRegistrationRuntimeException">
        /// 同じ名前、または同じクラスに複数のコンポーネントが登録されている場合
        /// </exception>
        /// <exception cref="CyclicReferenceRuntimeException">
        /// コンストラクタ・インジェクションでコンポーネントの参照が循環している場合
        /// </exception>
        object GetComponent(object componentKey);

        /// <summary>
        /// 指定されたキーに対応する複数のコンポーネントを検索して返します。
        /// </summary>
        /// <remarks>
        /// 検索の範囲は現在のS2コンテナおよび、インクルードしているS2コンテナの階層全体です。
        /// キーに対応するコンポーネントが最初に見つかったS2コンテナを対象とします。
        /// このS2コンテナから，キーに対応する全てのコンポーネントを配列で返します。
        /// 返される配列に含まれるコンポーネントは全て同一のS2コンテナに登録されたものです。
        /// </remarks>
        /// <param name="componentKey">コンポーネントを取得するためのキー</param>
        /// <returns>
        /// キーに対応するコンポーネントの配列を返します。 
        /// キーに対応するコンポーネントが存在しない場合は長さ0の配列を返します。
        /// </returns>
        /// <exception cref="CyclicReferenceRuntimeException">
        /// コンストラクタ・インジェクションでコンポーネントの参照が循環している場合
        /// </exception>
        /// <seealso cref="FindAllComponents"/>
        /// <seealso cref="FindLocalComponents"/>
        object[] FindComponents(object componentKey);

        /// <summary>
        /// 指定されたキーに対応する複数のコンポーネントを検索して返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 検索の範囲は現在のS2コンテナおよび、インクルードしているS2コンテナの階層全体です。
        /// キーに対応するコンポーネントが最初に見つかったS2コンテナとその子孫コンテナの全てを対象とします。
        /// 対象になるS2コンテナ全体から、キーに対応する全てのコンポーネントを配列で返します。
        /// </para>
        /// </remarks>
        /// <param name="componentKey">コンポーネントを取得するためのキー</param>
        /// <returns>
        /// キーに対応するコンポーネントの配列を返します。 
        /// キーに対応するコンポーネントが存在しない場合は長さ0の配列を返します。
        /// </returns>
        /// <exception cref="CyclicReferenceRuntimeException">
        /// コンストラクタ・インジェクションでコンポーネントの参照が循環している場合
        /// </exception>
        /// <seealso cref="FindComponents"/>
        /// <seealso cref="FindLocalComponents"/>
        object[] FindAllComponents(object componentKey);

        /// <summary>
        /// 指定されたキーに対応する複数のコンポーネントを検索して返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 検索の範囲は現在のS2コンテナのみです。
        /// 現在のS2コンテナから、キーに対応する全てのコンポーネントを配列で返します。
        /// </para>
        /// </remarks>
        /// <param name="componentKey">コンポーネントを取得するためのキー</param>
        /// <returns>
        /// キーに対応するコンポーネントの配列を返します。 
        /// キーに対応するコンポーネントが存在しない場合は長さ0の配列を返します。
        /// </returns>
        /// <exception cref="CyclicReferenceRuntimeException">
        /// コンストラクタ・インジェクションでコンポーネントの参照が循環している場合
        /// </exception>
        /// <seealso cref="FindComponents"/>
        /// <seealso cref="FindAllComponents"/>
        object[] FindLocalComponents(object componentKey);

        /// <summary>
        /// <code>outerComponent</code>のクラスをキーとして登録された
        /// <see cref="IComponentDef">コンポーネント定義</see>
        /// に従って、必要なコンポーネントのインジェクションを実行します。
        /// アスペクト、コンストラクタ・インジェクションは適用できません。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="IComponentDef">コンポーネント定義</see>の
        /// <see cref="IInstanceDef">インスタンス定義</see>はouterでなくてはなりません。
        /// </para>
        /// </remarks>
        /// <param name="outerComponent">外部コンポーネント</param>
        /// <exception cref="ClassUnmatchRuntimeException">
        /// 適合するコンポーネント定義が見つからない場合
        /// </exception>
        void InjectDependency(object outerComponent);

        /// <summary>
        /// <code>componentType</code>をキーとして登録された
        /// <see cref="IComponentDef">コンポーネント定義</see>に従って、
        /// 必要なコンポーネントのインジェクションを実行します。
        /// アスペクト、コンストラクタ・インジェクションは適用できません。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="IComponentDef">コンポーネント定義</see>の
        /// <see cref="IInstanceDef">インスタンス定義</see>はouterでなくてはなりません。
        /// </para>
        /// </remarks>
        /// <param name="outerComponent">外部コンポーネント</param>
        /// <param name="componentType">コンポーネント定義のキー (Type)</param>
        /// <exception cref="ClassUnmatchRuntimeException">
        /// 適合するコンポーネント定義が見つからない場合
        /// </exception>
        void InjectDependency(object outerComponent, Type componentType);

        /// <summary>
        /// <code>componentName</code>をキーとして登録された 
        /// <see cref="IComponentDef">コンポーネント定義</see>に従って、
        /// 必要なコンポーネントのインジェクションを実行します。
        /// アスペクト、コンストラクタ・インジェクションは適用できません。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="IComponentDef">コンポーネント定義</see>の
        /// <see cref="IInstanceDef">インスタンス定義</see>はouterでなくてはなりません。
        /// </para>
        /// </remarks>
        /// <param name="outerComponent">外部コンポーネント</param>
        /// <param name="componentName">コンポーネント定義のキー (名前)</param>
        /// <exception cref="ClassUnmatchRuntimeException">
        /// 適合するコンポーネント定義が見つからない場合
        /// </exception>
        void InjectDependency(object outerComponent, string componentName);

        /// <summary>
        /// コンポーネントを登録します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// S2コンテナに無名のコンポーネントとして登録します。
        /// 登録されたコンポーネントはインジェクションやアスペクトの適用などは出来ません。
        /// 他のコンポーネント構築時に依存オブジェクトとして利用することが可能です。
        /// </para>
        /// </remarks>
        /// <param name="component">コンポーネント</param>
        void Register(object component);

        /// <summary>
        /// 指定された名前でコンポーネントを登録します。
        /// </summary>
        /// <param name="component">コンポーネント</param>
        /// <param name="componentName">コンポーネント名</param>
        void Register(object component, string componentName);

        /// <summary>
        /// クラスのTypeをコンポーネント定義として登録します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 登録するコンポーネントは以下のものになります。
        /// <list type="table">
        /// <item>
        /// <term><see cref="IInstanceDef">インスタンス定義</see></term>
        /// <description><code>singleton</code></description>
        /// </item>
        /// <item>
        /// <term><see cref="IAutoBindingDef">自動バインディング定義</see></term>
        /// <description><code>auto</code></description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <param name="componentType">コンポーネントのクラスのType</param>
        void Register(Type componentType);

        /// <summary>
        /// 指定された名前でクラスのTypeをコンポーネント定義として登録します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 登録するコンポーネントは以下のものになります。
        /// <list type="table">
        /// <item>
        /// <term><see cref="IInstanceDef">インスタンス定義</see></term>
        /// <description><code>singleton</code></description>
        /// </item>
        /// <item>
        /// <term><see cref="IAutoBindingDef">自動バインディング定義</see></term>
        /// <description><code>auto</code></description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <param name="componentType">コンポーネントのクラスのType</param>
        /// <param name="componentName">コンポーネント名</param>
        void Register(Type componentType, string componentName);

        /// <summary>
        /// コンポーネント定義を登録します。
        /// </summary>
        /// <param name="componentDef">コンポーネント定義</param>
        void Register(IComponentDef componentDef);

        /// <summary>
        /// コンテナに登録されているコンポーネント定義の数を返します。
        /// </summary>
        /// <value>コンポーネント定義の数</value>
        int ComponentDefSize { get; }

        /// <summary>
        /// 番号で指定された位置のコンポーネント定義を返します。
        /// </summary>
        /// <param name="index">番号</param>
        /// <returns>コンポーネント定義</returns>
        IComponentDef GetComponentDef(int index);

        /// <summary>
        /// 指定されたキーに対応するコンポーネント定義を返します。
        /// </summary>
        /// <param name="componentKey">コンポーネント定義を取得するためのキー</param>
        /// <returns>コンポーネント定義</returns>
        /// <exception cref="ComponentNotFoundRuntimeException">
        /// コンポーネント定義が見つからない場合
        /// </exception>
        IComponentDef GetComponentDef(object componentKey);

        /// <summary>
        /// 指定されたキーに対応する複数のコンポーネント定義を検索して返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 検索の範囲は現在のS2コンテナおよび、インクルードしているS2コンテナの階層全体です。
        /// キーに対応するコンポーネントが最初に見つかったS2コンテナを対象とします。
        /// このS2コンテナから，キーに対応する全てのコンポーネント定義を配列で返します。
        /// 返される配列に含まれるコンポーネント定義は全て同一のS2コンテナに登録されたものです。
        /// </para>
        /// </remarks>
        /// <param name="componentKey">コンポーネント定義を取得するためのキー</param>
        /// <returns>
        /// キーに対応するコンポーネント定義の配列を返します。
        /// キーに対応するコンポーネント定義が存在しない場合は長さ0の配列を返します。
        /// </returns>
        /// <seealso cref="FindAllComponentDefs"/>
        /// <seealso cref="FindLocalComponentDefs"/>
        IComponentDef[] FindComponentDefs(object componentKey);

        /// <summary>
        /// 指定されたキーに対応する複数のコンポーネント定義を検索して返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 検索の範囲は現在のS2コンテナおよび、インクルードしているS2コンテナの階層全体です。
        /// キーに対応するコンポーネントが最初に見つかったS2コンテナとその子孫コンテナの全てを対象とします。
        /// 対象になるS2コンテナ全体から、キーに対応する全てのコンポーネント定義を配列で返します。
        /// </para>
        /// </remarks>
        /// <param name="componentKey"コンポーネント定義を取得するためのキー></param>
        /// <returns>
        /// キーに対応するコンポーネント定義の配列を返します。
        /// キーに対応するコンポーネント定義が存在しない場合は長さ0の配列を返します。
        /// </returns>
        /// <seealso cref="FindComponentDefs"/>
        /// <seealso cref="FindLocalComponentDefs"/>
        IComponentDef[] FindAllComponentDefs(object componentKey);

        /// <summary>
        /// 指定されたキーに対応する複数のコンポーネント定義を検索して返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 検索の範囲は現在のS2コンテナのみです。 
        /// 現在のS2コンテナから、キーに対応する全てのコンポーネント定義を配列で返します。
        /// </para>
        /// </remarks>
        /// <param name="componentKey">コンポーネント定義を取得するためのキー</param>
        /// <returns>
        /// キーに対応するコンポーネント定義の配列を返します。
        /// キーに対応するコンポーネント定義が存在しない場合は長さ0の配列を返します。
        /// </returns>
        /// <seealso cref="FindComponentDefs"/>
        /// <seealso cref="FindAllComponentDefs"/>
        IComponentDef[] FindLocalComponentDefs(object componentKey);

        /// <summary>
        /// 指定されたキーに対応するコンポーネント定義が存在する場合<code>true</code>を返します。
        /// </summary>
        /// <param name="componentKey">キー</param>
        /// <returns>
        /// キーに対応するコンポーネント定義が存在する場合<code>true</code>、
        /// そうでない場合は<code>false</code>
        /// </returns>
        bool HasComponentDef(object componentKey);

        /// <summary>
        /// <code>path</code>を読み込んだS2コンテナが存在する場合<code>true</code>を返します。
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>
        /// <code>path</code>を読み込んだS2コンテナが存在する場合<code>true</code>、
        /// そうでない場合は<code>false</code>
        /// </returns>
        bool HasDescendant(string path);

        /// <summary>
        /// <code>path</code>を読み込んだS2コンテナを返します。
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>S2コンテナ</returns>
        /// <exception cref="ContainerNotRegisteredRuntimeException">
        /// S2コンテナが見つからない場合
        /// </exception>
        IS2Container GetDescendant(string path);

        /// <summary>
        /// <code>descendant</code>を子孫コンテナとして登録します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 子孫コンテナとは、このコンテナに属する子のコンテナや、その子であるコンテナです。
        /// </para>
        /// </remarks>
        /// <param name="descendant">子孫コンテナ</param>
        void RegisterDescendant(IS2Container descendant);

        /// <summary>
        /// コンテナを子としてインクルードします。
        /// </summary>
        /// <param name="child">インクルードするS2コンテナ</param>
        void Include(IS2Container child);

        /// <summary>
        /// インクルードしている子コンテナの数を返します。
        /// </summary>
        /// <value>子コンテナの数</value>
        int ChildSize { get; }

        /// <summary>
        /// 番号で指定された位置の子コンテナを返します。
        /// </summary>
        /// <param name="index">子コンテナの番号</param>
        /// <returns>子コンテナ</returns>
        IS2Container GetChild(int index);

        /// <summary>
        /// このコンテナをインクルードしている親コンテナの数を返します。
        /// </summary>
        /// <value>親コンテナの数</value>
        int ParentSize { get; }

        /// <summary>
        /// 番号で指定された位置の親コンテナを返します。
        /// </summary>
        /// <param name="index">親コンテナの番号</param>
        /// <returns>親コンテナ</returns>
        IS2Container GetParent(int index);

        /// <summary>
        /// 親コンテナを追加します。
        /// </summary>
        /// <param name="parent">親として追加するS2コンテナ</param>
        void AddParent(IS2Container parent);

        /// <summary>
        /// コンテナの初期化を行います。 
        /// 子コンテナを持つ場合、子コンテナを全て初期化した後、自分の初期化を行います。
        /// </summary>
        void Init();

        /// <summary>
        /// コンテナの終了処理をおこないます。 
        /// 子コンテナを持つ場合、自分の終了処理を実行した後、子コンテナ全ての終了処理を行います。
        /// </summary>
        void Destroy();

        /// <summary>
        /// 名前空間を取得もしくは設定します。
        /// </summary>
        /// <value>名前空間</value>
        string Namespace { get; set; }

        /// <summary>
        /// コンテナ作成時に初期化を行うかを取得もしくは設定します。
        /// </summary>
        /// <value>
        /// <code>true</code>の場合はコンテナ作成時に初期化を行います。
        /// <code>false</code>の場合はコンテナ作成時に初期化を行いません。
        /// </value>
        bool InitializeOnCreate { get; set; }

        /// <summary>
        /// 設定ファイルの<code>path</code>を取得もしくは設定します。
        /// </summary>
        /// <value>設定ファイルの<code>path</code></value>
        string Path { get; set; }

        /// <summary>
        /// ルートのS2コンテナを取得もしくは設定します。
        /// </summary>
        /// <value>ルートのS2コンテナ</value>
        IS2Container Root { get; set; }

        /// <summary>
        /// 外部コンテキストを取得もしくは設定します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="IExternalContext">外部コンテキスト</see>は
        /// <code>application</code>、<code>request</code>、 <code>session</code>など
        /// 各<see cref="IInstanceDef">インスタンス定義</see>を提供するものです。
        /// これらのインスタンス定義を使用するには
        /// <code>IExternalContext</code>をS2コンテナに設定する必要があります。
        /// </para>
        /// </remarks>
        /// <value>外部コンテキスト</value>
        IExternalContext ExternalContext { get; set; }

        /// <summary>
        /// <see cref="IExternalContext">外部コンテキスト</see>
        /// が提供するコンポーネントを登録するオブジェクトを取得もしくは設定します。
        /// </summary>
        /// <value>外部コンテキストが提供するコンポーネントを登録するオブジェクト</value>
        IExternalContextComponentDefRegister 
            ExternalContextComponentDefRegister { get; set; }

        /// <summary>
        /// 子コンテナ（<code>container</code>）に登録された
        /// コンポーネント定義（<code>componentDef</code>）をこのコンテナから検索できるよう
        /// コンポーネント定義を管理するマップに登録します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="componentDef">コンポーネント定義</param>
        /// <param name="container">S2コンテナ</param>
        void RegisterMap(object key, IComponentDef componentDef, IS2Container container);
    }
}
