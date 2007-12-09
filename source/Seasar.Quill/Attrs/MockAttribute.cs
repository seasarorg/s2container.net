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
    /// Mockを指定する属性クラス
    /// </summary>
    /// <remarks>
    /// インターフェースに設定することができる。（複数設定することはできない）
    /// </remarks>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class MockAttribute : Attribute
    {
        // MockクラスのType
        protected Type mockType;

        /// <summary>
        /// MockクラスのTypeを指定してMockAttributeを
        /// 初期化するコンストラクタ
        /// </summary>
        /// <param name="mockType">MockクラスのType</param>
        public MockAttribute(Type mockType)
        {
            this.mockType = mockType;
        }

        /// <summary>
        /// MockクラスのTypeを返す
        /// </summary>
        /// <value>MockクラスのType</value>
        public Type MockType
        {
            get { return mockType; }
        }
    }
}
