#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
using System.Collections;

namespace Seasar.Framework.Util
{
    public class CaseInsentiveSet : CollectionBase, IList
    {
        public CaseInsentiveSet()
        {
        }

        bool IList.Contains(object value)
        {
            return Contains(value);
        }

        int IList.Add(object value)
        {
            if(Contains(value))
                return -1;
            else
                return InnerList.Add(value);
        }

        private bool Contains(object value)
        {
            InnerList.Sort(CaseInsensitiveComparer.Default);
            return InnerList.BinarySearch(value, CaseInsensitiveComparer.Default) >= 0;
        }
    }
}
