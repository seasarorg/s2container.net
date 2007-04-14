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
    /// SMART deployにおいて、自動登録されるコンポーネントの<see cref="IComponentDef">コンポーネント定義</see>
    /// を作成するためのインターフェースです。
    /// </summary>
    /// <remarks>
    /// <para>
    /// コンポーネント定義は<see cref="Seasar.Framework.Convention.NamingConvention">命名規約</see>
    /// に基づいて作成され、<see cref="IComponentCustomizer">コンポーネント定義カスタマイザ</see>
    /// によってアスペクト定義の追加などのカスタマイズを施してから返却されます。 
    /// </para>
    /// </remarks>
    public interface IComponentCreator
    {
        /// <summary>
        /// 指定されたクラスから、<see cref="Seasar.Framework.Convention.NamingConvention">命名規約</see>
        /// に従ってコンポーネント定義を作成します。
        /// </summary>
        /// <param name="componentType">コンポーネント定義を作成する対象のクラス</param>
        /// <returns>
        /// 作成されたコンポーネント定義。 指定されたクラスがこのCreatorの対象でなかった場合は、
        /// <code>null</code>を返す
        /// </returns>
        IComponentDef CreateComponentDef(Type componentType);

        /// <summary>
        /// 指定されたコンポーネント名から<see cref="Seasar.Framework.Convention.NamingConvention">命名規約</see>
        /// に従ってコンポーネント定義を作成します。
        /// </summary>
        /// <param name="componentName">コンポーネント定義を作成する対象のコンポーネント名</param>
        /// <returns>
        /// 作成されたコンポーネント定義。 指定されたクラスがこのCreatorの対象でなかった場合、
        /// またはコンポーネント名に対応するクラスが存在しなかった場合は、 <code>null</code>を返す
        /// </returns>
        /// <exception cref="Seasar.Framework.Exceptions.EmptyRuntimeException">
        /// コンポーネント名に<code>null</code>または空文字列を指定した場合
        /// </exception>
        /// <seealso cref="Seasar.Framework.Convention.INamingConvention.FromComponentNameToType"/>
        IComponentDef CreateComponentDef(string componentName);
    }
}
