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
using Seasar.Framework.Aop;

namespace Seasar.Framework.Container
{
    /// <summary>
    /// コンポーネントに適用するアスペクトを定義するインターフェースです。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 1つのコンポーネントに複数のアスペクトを定義することが可能です。 
    /// 定義した順にアスペクトのインターセプタが実行されます。
    /// </para>
    /// <para>
    /// S2AOP.NETにおけるインターセプタは、<see cref="Seasar.Framework.Aop.IMethodInterceptor"/>
    /// インターフェースを実装したクラスのコンポーネントとして定義します。
    /// インターセプターのセットを、複数のコンポーネントに適用する場合には、 
    /// 複数のインターセプタを1つのインターセプタ・コンポーネントとして定義できる、
    /// <see cref="Seasar.Framework.Aop.Interceptors.InterceptorChain"/>
    /// を使用すると設定を簡略化できます。
    /// </para>
    /// </remarks>
    /// <seealso cref="http://s2container.net.seasar.org/ja/aop.html">S2AOP.NETの詳細</seealso>
    public interface IAspectDef : IArgDef
    {
        /// <summary>
        /// ポイントカット
        /// </summary>
        IPointcut Pointcut { get; set; }

        /// <summary>
        /// アスペクト
        /// </summary>
        IAspect Aspect { get; }
    }
}
