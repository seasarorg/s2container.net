#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
    [Serializable]
    public class DefaultPagerCondition : IPagerCondition
    {
        private int _limit = PagerConditionConstants.NONE_LIMIT;
        private int _offset;
        private int _count;

        public DefaultPagerCondition()
        {
        }

        public DefaultPagerCondition(int limit, int offset)
        {
            _limit = limit;
            _offset = offset;
        }

        #region IPagerCondition �����o

        public int Limit
        {
            get { return _limit; }
            set { _limit = value; }
        }

        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        #endregion
    }
}
