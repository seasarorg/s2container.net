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
    /// コンポーネントデプロイヤは、 コンポーネントを利用可能な状態にして提供するためのインターフェースです。
    /// </summary>
    public interface IComponentDeployer
    {
        /// <summary>
        /// インスタンス定義に応じてインスタンス生成や外部コンテキストへの配備などを行った後に、
        /// そのコンポーネントのインスタンスを返します。
        /// </summary>
        /// <returns>コンポーネントのインスタンス</returns>
        /// <seealso cref="Seasar.Framework.Container.Deployer.SingletonComponentDeployer.Deploy"/>
        /// <seealso cref="Seasar.Framework.Container.Deployer.PrototypeComponentDeployer.Deploy"/>
        /// <seealso cref="Seasar.Framework.Container.Deployer.ApplicationComponentDeployer.Deploy"/>
        /// <seealso cref="Seasar.Framework.Container.Deployer.RequestComponentDeployer.Deploy"/>
        /// <seealso cref="Seasar.Framework.Container.Deployer.SessionComponentDeployer.Deploy"/>
        object Deploy();

        /// <summary>
        /// 外部コンポーネント<code>outerComponent</code>に対し、
        /// この<see cref="IComponentDeployer">コンポーネントデプロイヤ</see>の
        /// <see cref="IComponentDef">コンポーネント定義</see>に基づいて、
        /// S2コンテナ上のコンポーネントをインジェクションします。
        /// </summary>
        /// <param name="outerComponent">外部コンポーネント</param>
        /// <seealso cref="Seasar.Framework.Container.Deployer.OuterComponentDeployer.InjectDependency"/>
        void InjectDependency(object outerComponent);

        /// <summary>
        /// コンポーネントデプロイヤを初期化します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// デプロイするコンポーネントの<see cref="IInstanceDef">インスタンス定義</see>が
        /// <code>singleton</code>の場合には、<see cref="IAspectDef">アスペクト</see>を適用した
        /// インスタンスの生成、 配備、 プロパティ設定の後に、
        /// <see cref="IInitMethodDef.InitMethod"/>が呼ばれます。
        /// </para>
        /// </remarks>
        /// <seealso cref="Seasar.Framework.Container.Deployer.SingletonComponentDeployer.Init"/>
        /// <seealso cref="Seasar.Framework.Container.Assembler.DefaultInitMethodAssembler.Assemble"/>
        void Init();

        /// <summary>
        /// コンポーネントデプロイヤを破棄します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// デプロイするコンポーネントの<see cref="IInstanceDef">インスタンス定義</see>が
        /// <code>singleton</code>の場合には、<see cref="IDestroyMethodDef.DestroyMethod"/>が呼ばれます。
        /// </para>
        /// </remarks>
        /// <seealso cref="Seasar.Framework.Container.Deployer.SingletonComponentDeployer.Destroy"/>
        /// <seealso cref="Seasar.Framework.Container.Assembler.DefaultDestroyMethodAssembler.Assemble"/>
        void Destroy();
    }
}
