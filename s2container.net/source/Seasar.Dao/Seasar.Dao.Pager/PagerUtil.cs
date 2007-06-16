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
using System.Collections;

namespace Seasar.Dao.Pager
{
    public sealed class PagerUtil
    {
        public static bool IsPrev(IPagerCondition condition)
        {
            bool prev = condition.Offset > 0;
            return prev;
        }

        public static bool IsNext(IPagerCondition condition)
        {
            bool next = condition.Count > 0 && condition.Offset + condition.Limit < condition.Count;
            return next;
        }

        public static int GetCurrentLastOffset(IPagerCondition condition)
        {
            int nextOffset = GetNextOffset(condition);
            if (nextOffset <= 0 || condition.Count <= 0)
            {
                return 0;
            }
            else
            {
                return nextOffset < condition.Count ? nextOffset - 1 : condition.Count - 1;
            }
        }

        public static int GetNextOffset(IPagerCondition condition)
        {
            return condition.Offset + condition.Limit;
        }

        public static int GetPrevOffset(IPagerCondition condition)
        {
            int prevOffset = condition.Offset - condition.Limit;
            return prevOffset < 0 ? 0 : prevOffset;
        }

        public static int GetPageIndex(IPagerCondition condition)
        {
            if (condition.Limit == 0)
            {
                return 1;
            }
            else
            {
                return condition.Offset / condition.Limit;
            }
        }

        public static int GetPageCount(IPagerCondition condition)
        {
            return GetPageIndex(condition) + 1;
        }

        public static int GetLastPageIndex(IPagerCondition condition)
        {
            if (condition.Limit == 0)
            {
                return 0;
            }
            else
            {
                return (condition.Count - 1) / condition.Limit;
            }
        }

        public static int GetDisplayPageIndexBegin(IPagerCondition condition, int displayPageMax)
        {
            int lastPageIndex = GetLastPageIndex(condition);
            if (lastPageIndex < displayPageMax)
            {
                return 0;
            }
            else
            {
                int currentPageIndex = GetPageIndex(condition);
                int displayPageIndexBegin = currentPageIndex
                                            - ((int) Math.Floor((double) displayPageMax / 2));
                return displayPageIndexBegin < 0 ? 0 : displayPageIndexBegin;
            }
        }

        public static int GetDisplayPageIndexEnd(IPagerCondition condition,
                                                 int displayPageMax)
        {
            int lastPageIndex = GetLastPageIndex(condition);
            int displayPageIndexBegin = GetDisplayPageIndexBegin(condition,
                                                                 displayPageMax);
            int displayPageRange = lastPageIndex - displayPageIndexBegin;
            if (displayPageRange < displayPageMax)
            {
                return lastPageIndex;
            }
            else
            {
                return displayPageIndexBegin + displayPageMax - 1;
            }
        }

        /// <summary>
        /// IListの内容をIPagerConditionの条件でフィルタリングします。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="condition">条件</param>
        /// <returns>フィルタリング後のIList</returns>
        public static IList Filter(IList list, IPagerCondition condition)
        {
            condition.Count = list.Count;
            if (condition.Limit == PagerConditionConstants.NONE_LIMIT)
            {
                return list;
            }
            else
            {
                IList result = new ArrayList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (i >= condition.Offset && i < condition.Offset + condition.Limit)
                    {
                        result.Add(list[i]);
                    }
                }
                return result;
            }
        }
    }
}