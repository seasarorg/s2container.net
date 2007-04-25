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
    /// メソッド・インジェクションを実行してコンポーネントを組み立てます。
    /// </summary>
    /// <remarks>
    /// <para>
    /// インジェクションの実行は<see cref="IMethodDef">メソッド定義</see>に基づいて行います。
    /// </para>
    /// </remarks>
    public interface IMethodAssembler
    {
        /// <summary>
        /// 指定された<code>component</code>に対して、 メソッド・インジェクションを実行します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// メソッドの引数として指定されたコンポーネントが見つからない場合には、
        /// IllegalMethodRuntimeExceptionがスローされます。
        /// </para>
        /// </remarks>
        /// <param name="component">S2コンテナ上のコンポーネントがセットされる対象</param>
        /// <exception cref="IllegalMethodRuntimeException">
        /// メソッドの引数として指定されたコンポーネントが見つからない場合
        /// </exception>
        /// <exception cref="InvocationTargetRuntimeException">
        /// メソッド実行時に検査例外が発生した場合
        /// (実行時例外とエラーが発生した場合にはそのままスローされます)
        /// </exception>
        /// <exception cref="IllegalAccessRuntimeException"></exception>
        void Assemble(object component);
    }
}
