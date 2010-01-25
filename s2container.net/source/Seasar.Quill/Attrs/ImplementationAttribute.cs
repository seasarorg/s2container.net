#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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

namespace Seasar.Quill.Attrs
{
    /// <summary>
    /// 実装クラスを指定する属性クラス
    /// </summary>
    /// <remarks>
    /// クラス・インターフェースに設定することができる。
    /// (複数設定することはできない)
    /// </remarks>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class,
       AllowMultiple = false)]
    public class ImplementationAttribute : Attribute
    {
        // 実装クラスのType
        protected Type implementationType;

        /// <summary>
        /// 属性が指定されているクラス自身を実装クラスとして
        /// ImplementationAttributeを初期化するコンストラクタ
        /// </summary>
        public ImplementationAttribute()
        {
        }

        /// <summary>
        /// 実装クラスのTypeを指定してImplementationAttributeを
        /// 初期化するコンストラクタ
        /// </summary>
        /// <param name="implementationType">実装クラスのType</param>
        public ImplementationAttribute(Type implementationType)
        {
            this.implementationType = implementationType;
        }

        /// <summary>
        /// 実装クラスのTypeを返す
        /// </summary>
        /// <value>実装クラスのType</value>
        public Type ImplementationType
        {
            get { return implementationType; }
        }
    }
}
