#region Copyright

/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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

namespace Seasar.Dxo.Annotation
{
    /// <summary>
    /// 日付書式パターン属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class DatePatternAttribute : Attribute
    {
        private string _format;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="format">日付書式</param>
        public DatePatternAttribute(string format)
        {
            this._format = format;
        }

        /// <summary>
        /// 日付書式
        /// </summary>
        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }
    }
}