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
    /// S2Containerのコンポーネントをバインディングするための属性クラス
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class BindingAttribute : Attribute
    {
        /// <summary>
        /// S2Containerにおけるコンポーネント名
        /// </summary>
        protected string componentName;

        /// <summary>
        /// S2Containerにおけるコンポーネント名を指定して
        /// BindingAttributeを初期化するコンストラクタ
        /// </summary>
        /// <param name="componentName">S2Containerにおけるコンポーネント名</param>
        public BindingAttribute(string componentName)
        {
            // コンポーネント名をセットする
            this.componentName = componentName;
        }

        /// <summary>
        /// S2Containerにおけるコンポーネント名を取得する
        /// </summary>
        /// <value>S2Containerにおけるコンポーネント名</value>
        public string ComponentName
        {
            get { return componentName; }
        }
    }
}
