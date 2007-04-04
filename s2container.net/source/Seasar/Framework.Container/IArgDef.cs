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
    /// コンポーネントのコンストラクタおよびメソッドに与えられる
    /// 引数定義のためのインターフェースです。
    /// </summary>
    public interface IArgDef : IMetaDefAware
    {
        /// <summary>
        /// 引数定義の値
        /// </summary>
        /// <remarks>
        /// <para>
        /// 引数定義の値とは、diconファイルに記述した<code>&lt;arg&gt;</code>要素の内容です。
        /// インジェクションする際に、コンストラクタや初期化メソッド等の引数値になります。
        /// </para>
        /// </remarks>
        object Value { get; set; }

        /// <summary>
        /// 引数を評価するコンテキストとなるS2コンテナ
        /// </summary>
        IS2Container Container { get; set; }

        /// <summary>
        /// 引数定義の値となる式
        /// </summary>
        IExpression Expression { get; set; }

        /// <summary>
        /// 引数定義の値となる式、引数定義の値、引数定義の値となるコンポーネント定義のいずれかが存在し、
        /// 値の取得が可能かどうか
        /// </summary>
        /// <remarks>
        /// <para>
        /// 値の取得が可能な場合、<code>true</code>、
        /// そうでない場合は<code>false</code>を返します。
        /// </para>
        /// </remarks>
        bool ValueGettable { get; }

        /// <summary>
        /// 引数定義の値となるコンポーネント定義
        /// </summary>
        IComponentDef ChildComponentDef { set; }
    }
}
