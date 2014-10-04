using System;
using System.Collections.Generic;

namespace Seasar.Quill.Util
{
    /// <summary>
    /// コレクション操作ユーティリティクラス
    /// </summary>
    public static class CollectionUtils
    {
        /// <summary>
        /// コレクション追加
        /// </summary>
        /// <typeparam name="ITEM"></typeparam>
        /// <param name="targetSet"></param>
        /// <param name="newItems"></param>
        /// <param name="isCheckDupricate"></param>
        public static void AddAll<ITEM>(this ISet<ITEM> targetSet, IEnumerable<ITEM> newItems, bool isCheckDupricate = false)
        {
            foreach(ITEM newItem in newItems)
            {
                if (isCheckDupricate && targetSet.Contains(newItem))
                {
                    continue;
                }
                targetSet.Add(newItem);
            }
        }

        /// <summary>
        /// コレクション内の要素を一つずつ実行
        /// </summary>
        /// <typeparam name="ITEM"></typeparam>
        /// <param name="collection"></param>
        /// <param name="invoke"></param>
        public static void ForEach<ITEM>(this IEnumerable<ITEM> collection, Action<ITEM> invoke)
        {
            foreach(ITEM item in collection)
            {
                invoke(item);
            }
        }
    }
}
