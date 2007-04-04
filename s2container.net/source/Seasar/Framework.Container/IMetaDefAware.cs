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
    /// このインターフェースは、メタデータ定義を追加および取得することのできるオブジェクトを表します。 
    /// </summary>
    /// <remarks>
    /// <para>メタデータ定義は複数追加することができます。 メタデータ定義の取得は、
    /// メタデータ定義名またはインデックス番号を指定して行います。</para>
    /// </remarks>
    public interface IMetaDefAware
    {
        /// <summary>
        /// メタデータ定義を追加します。
        /// </summary>
        /// <param name="metaDef">メタデータ定義</param>
        void AddMetaDef(IMetaDef metaDef);

        /// <summary>
        /// メタデータ定義の数を返します。
        /// </summary>
        /// <returns>メタデータ定義の数</returns>
        int MetaDefSize{ get; }

        /// <summary>
        /// インデックス番号<code>index</code>で指定されたメタデータ定義を返します。
        /// </summary>
        /// <remarks>
        /// <para>インデックス番号は、追加した順に0, 1, 2…となります。</para>
        /// </remarks>
        /// <param name="index">メタデータ定義を指定するインデックス番号</param>
        /// <returns>メタデータ定義</returns>
        IMetaDef GetMetaDef(int index);

        /// <summary>
        /// 指定したメタデータ定義名で登録されているメタデータ定義を取得します。
        /// </summary>
        /// <remarks>
        /// <para>メタデータ定義が登録されていない場合、<code>null</code>を返します。</para>
        /// </remarks>
        /// <param name="name">メタデータ定義名</param>
        /// <returns>メタデータ定義</returns>
        IMetaDef GetMetaDef(string name);


        /// <summary>
        /// 指定したメタデータ定義名で登録されているメタデータ定義を取得します。
        /// </summary>
        /// <remarks>
        /// <para>メタデータ定義が登録されていない場合、要素数0の配列を返します。</para>
        /// </remarks>
        /// <param name="name">メタデータ定義名</param>
        /// <returns>メタデータ定義を格納した配列</returns>
        IMetaDef[] GetMetaDefs(string name);
    }
}
