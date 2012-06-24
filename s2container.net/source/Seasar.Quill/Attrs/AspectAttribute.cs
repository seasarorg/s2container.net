#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
    /// Aspectを指定する属性クラス
    /// </summary>
    /// <remarks>
    /// クラス・インターフェース・メソッドに設定することができる。
    /// (複数設定することができる)
    /// </remarks>
    [AttributeUsage(AttributeTargets.Interface |
       AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AspectAttribute : Attribute
    {
        /// <summary>
        /// InterceptorのType
        /// </summary>
        protected Type interceptorType;

        ///// <summary>
        ///// S2Containerにおけるコンポーネント名
        ///// </summary>
        //protected string componentName;

        /// <summary>
        /// Aspectを適用する順番(数が小さいほうが先に適用される)
        /// </summary>
        protected int ordinal = 0;

        /// <summary>
        /// InterceptorのTypeを指定してAspectAttributeを初期化するコンストラクタ
        /// </summary>
        /// <param name="interceptorType">InterceptorのType</param>
        public AspectAttribute(Type interceptorType)
        {
            // InterceptorのTypeを設定する
            this.interceptorType = interceptorType;
        }

        /// <summary>
        /// InterceptorのTypeを指定してAspectAttributeを初期化するコンストラクタ
        /// </summary>
        /// <param name="interceptorType">InterceptorのType</param>
        /// <param name="ordinal">Aspectを適用する順番</param>
        public AspectAttribute(Type interceptorType, int ordinal)
        {
            // InterceptorのTypeを設定する
            this.interceptorType = interceptorType;

            // Aspectを適用する順番を設定する
            this.ordinal = ordinal;
        }

        ///// <summary>
        ///// InterceptorのS2Containerにおけるコンポーネント名を指定して
        ///// AspectAttributeを初期化するコンストラクタ
        ///// </summary>
        ///// <param name="componentName">S2Containerにおけるコンポーネント名</param>
        //public AspectAttribute(string componentName)
        //{
        //    // S2Containerにおけるコンポーネント名を設定する
        //    this.componentName = componentName;
        //}

        ///// <summary>
        ///// InterceptorのS2Containerにおけるコンポーネント名を指定して
        ///// AspectAttributeを初期化するコンストラクタ
        ///// </summary>
        ///// <param name="componentName">S2Containerにおけるコンポーネント名</param>
        ///// <param name="ordinal">Aspectを適用する順番</param>
        //public AspectAttribute(string componentName, int ordinal)
        //{
        //    // S2Containerにおけるコンポーネント名を設定する
        //    this.componentName = componentName;

        //    // Aspectを適用する順番を設定する
        //    this.ordinal = ordinal;
        //}

        /// <summary>
        /// InterceptorのTypeを取得する
        /// </summary>
        /// <value>InterceptorのType</value>
        public Type InterceptorType
        {
            get { return interceptorType; }
        }

        ///// <summary>
        ///// S2Containerにおけるコンポーネント名を取得する
        ///// </summary>
        ///// <value>S2Containerにおけるコンポーネント名</value>
        //public string ComponentName
        //{
        //    get { return componentName; }
        //}

        /// <summary>
        /// Aspectを適用する順番を取得する
        /// (数が小さいほうが先に適用される)
        /// </summary>
        /// <value>Aspectを適用する順番(数が小さいほうが先に適用される)</value>
        public int Ordinal
        {
            get { return ordinal; }
        }
    }
}
