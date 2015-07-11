#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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

namespace Seasar.Windows.Attr
{
    /// <summary>
    /// コントロール修飾子属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class ControlModifierAttribute : Attribute
    {
        /// <summary>
        /// コントロール接頭辞
        /// </summary>
        private string _prefix = string.Empty;

        /// <summary>
        /// コントロール接尾辞
        /// </summary>
        private string _suffix = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="prefix">コントロール接頭辞</param>
        /// <param name="suffix">コントロール接尾辞</param>
        public ControlModifierAttribute(string prefix, string suffix)
        {
            this._prefix = prefix;
            this._suffix = suffix;
        }

        /// <summary>
        /// コントロール接頭辞
        /// </summary>
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        /// <summary>
        /// コントロール接尾辞
        /// </summary>
        public string Suffix
        {
            get { return _suffix; }
            set { _suffix = value; }
        }
    }
}
