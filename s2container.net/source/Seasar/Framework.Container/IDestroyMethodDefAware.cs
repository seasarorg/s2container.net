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
    /// このインターフェースは、 destroyメソッド定義を追加および取得することができるオブジェクトを表します。
    /// </summary>
    /// <remarks>
    /// <para>
    /// destroyメソッド定義は複数追加することが出来ます。 
    /// destroyメソッド定義の取得はインデックス番号を指定して行います。
    /// </para>
    /// </remarks>
    /// <seealso cref="IDestroyMethodDef"/>
    public interface IDestroyMethodDefAware
    {
        /// <summary>
        /// destroyメソッド定義を追加します。
        /// </summary>
        /// <param name="methodDef">destroyメソッド定義</param>
        void AddDestroyMethodDef(IDestroyMethodDef methodDef);

        /// <summary>
        /// <see cref="IDestroyMethodDef">destroyメソッド定義</see>の数を返します。
        /// </summary>
        /// <value>destroyメソッド定義の数</value>
        int DestroyMethodDefSize { get; }

        /// <summary>
        /// 指定されたインデックス番号<code>index</code>のdestroyメソッド定義を返します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// インデックス番号は、 追加した順番に 0,1,2,… となります。
        /// </para>
        /// </remarks>
        /// <param name="index">destroyメソッド定義を指定するインデックス番号</param>
        /// <returns>destroyメソッド定義</returns>
        IDestroyMethodDef GetDestroyMethodDef(int index);
    }
}
