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
    /// コンポーネントに対して<var>destroy</var>メソッド・インジェクションを定義するためのインターフェースです。
    /// </summary>
    /// <remarks>
    /// <para>
    /// <var>destroy</var>メソッド・インジェクションとは、 S2コンテナによって管理されているコンポーネントが
    /// 破棄される際に、1個以上の任意のメソッド(終了処理メソッド)を実行するという機能です。
    /// </para>
    /// <para>
    /// コンポーネントの<see cref="IInstanceDef">インスタンス定義</see>が<code>singleton</code>の場合には、
    /// S2コンテナが終了する際に<var>destroy</var>メソッド・インジェクションが実行されます。
    /// </para>
    /// </remarks>
    /// <seealso cref="IComponentDeployer.Destroy"/>
    /// <seealso cref="IComponentDef.Destroy"/>
    /// <seealso cref="IS2Container.Destroy"/>
    /// <seealso cref="Seasar.Framework.Container.Factory.S2ContainerFactory.Destroy"/>
    /// <seealso cref="Seasar.Framework.Container.Factory.SingletonS2ContainerFactory.Destroy"/>
    public interface IDestroyMethodDef : IMethodDef
    {
    }
}
