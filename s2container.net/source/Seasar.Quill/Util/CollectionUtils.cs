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
        /// <typeparam name="ITEM">コレクション要素の型</typeparam>
        /// <param name="targetSet">追加先のセット</param>
        /// <param name="newItems">追加するセット</param>
        public static void AddAll<ITEM>(this ISet<ITEM> targetSet, IEnumerable<ITEM> newItems)
        {
            foreach(ITEM newItem in newItems)
            {
                targetSet.Add(newItem);
            }
        }

        /// <summary>
        /// コレクション内の要素を一つずつ実行
        /// </summary>
        /// <typeparam name="ITEM">コレクション要素の型</typeparam>
        /// <param name="collection">コレクション</param>
        /// <param name="invoke">要素一つ一つに対して行う処理コールバック</param>
        public static void ForEach<ITEM>(this IEnumerable<ITEM> collection, Action<ITEM> invoke)
        {
            foreach(ITEM item in collection)
            {
                invoke(item);
            }
        }
    }
}
