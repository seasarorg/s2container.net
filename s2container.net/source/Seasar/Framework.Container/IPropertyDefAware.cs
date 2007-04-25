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
    /// このインターフェースはプロパティ定義を登録および取得する方法を
    /// 定義するオブジェクトを表します。
    /// </summary>
    /// <remarks>
    /// <para>
    /// プロパティ定義は複数登録することが出来ます。
    /// プロパティ定義の取得はインデックス番号を指定して行います。
    /// </para>
    /// </remarks>
    /// <seealso cref="IPropertyDef"/>
    public interface IPropertyDefAware
    {
        /// <summary>
        /// <see cref="IPropertyDef">プロパティ定義</see>を追加します。
        /// </summary>
        /// <param name="propertyDef">プロパティ定義</param>
        void AddPropertyDef(IPropertyDef propertyDef);

        /// <summary>
        /// <see cref="IPropertyDef">プロパティ定義</see>の数を返します。
        /// </summary>
        /// <value>登録されているプロパティ定義の数</value>
        int PropertyDefSize { get; }

        /// <summary>
        /// 指定されたインデックス番号<code>index</code>の
        /// <see cref="IPropertyDef">プロパティ定義</see>を返します。
        /// </summary>
        /// <param name="index">プロパティ定義を指定するインデックス番号</param>
        /// <returns>プロパティ定義</returns>
        IPropertyDef GetPropertyDef(int index);

        /// <summary>
        /// 指定したプロパティ名で登録されている
        /// <see cref="IPropertyDef">プロパティ定義</see>を返します。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>プロパティ定義</returns>
        IPropertyDef GetPropertyDef(string propertyName);

        /// <summary>
        /// 指定したプロパティ名のプロパティ定義があれば<code>true</code>を返します。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>
        /// プロパティ定義が存在していれば<code>true</code>、存在していなければ<code>false</code>
        /// </returns>
        bool HasPropertyDef(string propertyName);
    }
}
