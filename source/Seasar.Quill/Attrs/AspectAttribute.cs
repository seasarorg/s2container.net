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
        AttributeTargets.Class | AttributeTargets.Method)]
    public class AspectAttribute : Attribute
    {
        /// <summary>
        /// InterceptorのType
        /// </summary>
        private Type interceptorType;

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
        /// InterceptorのTypeを取得する
        /// </summary>
        /// <value>InterceptorのType</value>
        public Type InterceptorType
        {
            get { return interceptorType; }
        }
    }
}
