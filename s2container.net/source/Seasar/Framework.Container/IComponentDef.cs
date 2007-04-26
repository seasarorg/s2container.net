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
    /// S2コンテナが管理するコンポーネントの定義を表すインターフェースです。
    /// </summary>
    /// <remarks>
    /// <para>コンポーネント定義は、 コンポーネントの管理に必要な以下の情報を保持します。</para>
    /// <list type="bullet">
    /// <item>
    /// <term>ライフサイクル</term>
    /// <description>
    /// コンポーネントのスコープや、生成と消滅については、 このコンポーネントの
    /// <see cref="IInstanceDef">インスタンス定義</see>で設定します。生成については、
    /// <see cref="IExpression">コンポーネント生成式</see>により指定することも可能です。
    /// </description>
    /// </item>
    /// <item>
    /// <term>依存性注入(Dependency Injection)</term>
    /// <description>
    /// このコンポーネントが依存する他のコンポーネントやパラメータは、
    /// <see cref="IArgDef">引数定義</see>などにより設定します。
    /// </description>
    /// </item>
    /// <item>
    /// <term>アスペクト</term>
    /// <description>
    /// このコンポーネントの<see cref="IAspectDef">アスペクト定義</see>などにより設定します。
    /// </description>
    /// </item>
    /// <item>
    /// <term>メタデータ</term>
    /// <description>
    /// <see cref="IMetaDef">メタデータ定義</see>により、コンポーネントに付加情報を設定できます。
    /// メタデータは、特殊なコンポーネントであることを識別する場合などに利用します。
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <seealso cref="IArgDef"/>
    /// <seealso cref="IInterTypeDef"/>
    /// <seealso cref="IPropertyDef"/>
    /// <seealso cref="IInitMethodDef"/>
    /// <seealso cref="IDestroyMethodDef"/>
    /// <seealso cref="IAspectDef"/>
    /// <seealso cref="IMetaDef"/>
    public interface IComponentDef : IArgDefAware, IInterTypeDefAware,
        IPropertyDefAware, IInitMethodDefAware, IDestroyMethodDefAware,
        IAspectDefAware, IMetaDefAware
    {
        /// <summary>
        /// 定義に基づいてコンポーネントを返します。
        /// </summary>
        /// <value>コンポーネント</value>
        /// <exception cref="TooManyRegistrationRuntimeException">コンポーネント定義が重複している場合</exception>
        /// <exception cref="CyclicReferenceRuntimeException">コンポーネント間に循環参照がある場合</exception>
        /// <seealso cref="ITooManyRegistrationComponentDef"/>
        object Component { get; }

        /// <summary>
        /// 外部コンポーネント<code>outerComponent</code>に対し、
        /// <see cref="IComponentDef">コンポーネント定義</see>に基づいて、
        /// S2コンテナ上のコンポーネントをインジェクションします。
        /// </summary>
        /// <param name="outerComponent">外部コンポーネント</param>
        void InjectDependency(object outerComponent);

        /// <summary>
        /// このコンポーネント定義を含むS2コンテナを取得・設定します。
        /// </summary>
        /// <value>S2コンテナ</value>
        IS2Container Container { get; set; }

        /// <summary>
        /// 定義上のクラスのTypeを返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// diconファイルの<code>&lt;component&gt;</code>タグにおける、
        /// <code>class</code>属性で指定されたクラスのTypeを表します。
        /// 自動バインディングされる際には、このクラス(インターフェース)が使用されます。
        /// </para>
        /// </remarks>
        /// <value>定義上のクラス</value>
        Type ComponentType { get; }

        /// <summary>
        /// コンポーネント名を取得・設定します。
        /// </summary>
        /// <value>コンポーネント名</value>
        string ComponentName { get; set; }

        /// <summary>
        /// アスペクト適用後の、 実際にインスタンス化されるコンポーネントのTypeを返します。
        /// </summary>
        /// <value>実際のクラスのType</value>
        Type ConcreteType { get; }

        /// <summary>
        /// 自動バインディング定義を取得・設定します。
        /// </summary>
        /// <value>自動バインディング定義</value>
        IAutoBindingDef AutoBindingDef { get; set; }

        /// <summary>
        /// インスタンス定義を取得・設定します。
        /// </summary>
        /// <value>インスタンス定義</value>
        IInstanceDef InstanceDef { get; set; }

        /// <summary>
        /// コンポーネントを生成する式を取得・設定します。
        /// </summary>
        /// <value>コンポーネントを生成式</value>
        IExpression Expression { get; set; }

        /// <summary>
        /// 外部バインディングが有効であるかどうかを取得・設定します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 外部バインディングとは、 外部コンテキストにあるオブジェクトを、
        /// 指定したコンポーネントの対応するプロパティにバインディングする機能です。
        /// </para>
        /// <para>
        /// Webアプリケーションにおいて、リクエストコンテキストに入力された値を、
        /// リクエストインスタンスを通して取得し、リクエスト間(ページ間)で
        /// 透過的に引き継ぐ場合などに利用されます。
        /// </para>
        /// </remarks>
        /// <value>外部バインディングが有効な場合<code>true</code></value>
        bool ExternalBinding { get; set; }

        /// <summary>
        /// コンポーネント定義を初期化します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="IInstanceDef">コンポーネントインスタンス定義</see>が<code>singleton</code>の場合には、
        /// <see cref="IAspectDef">アスペクト</see>を適用したインスタンスの生成、 配備、 プロパティ設定の後に、
        /// <see cref="IInitMethodDef">InitMethod</see>が呼ばれます。
        /// </para>
        /// </remarks>
        /// <seealso cref="Seasar.Framework.Container.Deployer.SingletonComponentDeployer.Init"/>
        void Init();

        /// <summary>
        /// コンポーネント定義を破棄します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="IInstanceDef">コンポーネントインスタンス定義</see>が
        /// <code>singleton</code>の場合には、<see cref="IDestroyMethodDef">DestroyMethod</see>が呼ばれます。
        /// </para>
        /// </remarks>
        /// <seealso cref="Seasar.Framework.Container.Deployer.SingletonComponentDeployer.Destroy"/>
        void Destroy();
    }
}
