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

namespace Seasar.Dao.Pager
{
    /// <summary>
    /// ページング機能を有効にするメソッドまたはインターフェースに設定する。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface)]
    public class PagerAttribute : Attribute
    {
        private readonly string _limitParameter = "limit";
        private readonly string _offsetParameter = "offset";
        private readonly string _countParameter = "count";

        public PagerAttribute()
        {
        }

        public PagerAttribute(string limitParameter, string offsetParameter, string countParameter)
        {
            _limitParameter = limitParameter;
            _offsetParameter = offsetParameter;
            _countParameter = countParameter;
        }

        public string LimitParameter
        {
            get { return _limitParameter; }
        }

        public string OffsetParameter
        {
            get { return _offsetParameter; }
        }

        public string CountParameter
        {
            get { return _countParameter; }
        }
    }
}