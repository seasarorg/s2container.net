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
using System.Reflection;

namespace Seasar.Framework.Aop
{
    /// <summary>
    /// Interceptorからインターセプトされているメソッドの情報にアクセスするためのインターフェイス
    /// </summary>
    /// <remarks>
    /// このインターフェイスはAOPアライアンス準拠。
    /// </remarks>
    /// <seealso href="http://aopalliance.sourceforge.net/doc/index.html">AOP Alliance</seealso>
    public interface IMethodInvocation
    {
        /// <summary>
        /// InterceptされるメソッドのMethodBase
        /// </summary>
        MethodBase Method { get; }

        /// <summary>
        /// Interceptされるオブジェクト
        /// </summary>
        object Target { get; }

        /// <summary>
        /// Interceptされるメソッドの引数
        /// </summary>
        object[] Arguments { get; }

        /// <summary>
        /// 他にチェーンされているInterceptorがあれば、Interceptorを呼び出します（再帰的に呼び出される）
        /// 他にチェーンされているInterceptorが無ければ、Interceptされているメソッドを実行します
        /// </summary>
        /// <returns>Interceptされたメソッドの戻り値</returns>
        object Proceed();
    }
}
