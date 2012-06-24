#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
using System.Collections.Generic;

namespace Seasar.Framework.Util
{
    /// <summary>
    /// 配列のように順番を保証するHashMap
    /// (Java版のように厳密に配列＋Mapのインターフェースをもつわけではなく
    /// 順番を保証する配列も返せるHashMapとして実装しています）
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ArrayMap<TKey, TValue> : IEnumerable<TValue>
    {
        private readonly List<TValue> _list;
        private readonly IDictionary<TKey, int> _map;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ArrayMap()
        {
            _list = new List<TValue>();
            _map = new Dictionary<TKey, int>();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="capacity">想定されるコレクションのサイズ</param>
        public ArrayMap(int capacity)
        {
            _list = new List<TValue>(capacity);
            _map = new Dictionary<TKey, int>(capacity);
        }

        /// <summary>
        /// キー一覧
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return _map.Keys; }
        }

        /// <summary>
        /// キーを使って値を取得
        /// </summary>
        /// <param name="key">要素を一意に特定するキー</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public TValue this[TKey key]
        {
            get { return _list[_map[key]]; }
        }

        /// <summary>
        /// 要素番号を使ってキーを取得
        /// </summary>
        /// <param name="index">要素番号</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public TValue this[int index]
        {
            get { return _list[index]; }
        }

        /// <summary>
        /// 値の配列を返す
        /// </summary>
        /// <returns></returns>
        public TValue[] Items
        {
            get { return _list.ToArray(); }
        }

        /// <summary>
        /// 要素の追加
        /// </summary>
        /// <param name="key">要素を一意に特定するキー</param>
        /// <param name="val">要素</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(TKey key, TValue val)
        {
            if (_map.ContainsKey(key)) { return; }
            _list.Add(val);
            _map.Add(key, _list.Count - 1);
        }

        /// <summary>
        /// 指定したキーがコレクションに含まれているか判定
        /// </summary>
        /// <param name="key">要素を一意に特定するキー</param>
        /// <returns>true=キーが含まれている、false=キーは含まれていない</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool ContainsKey(TKey key)
        {
            return _map.ContainsKey(key);
        }

        /// <summary>
        /// 指定したキーに該当する要素をコレクションから削除する
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void Remove(TKey key)
        {
            int index = _map[key];
            _list.RemoveAt(index);
            _map.Remove(key);
        }

        #region IEnumerable<TValue> メンバ

        public IEnumerator<TValue> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable メンバ

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }
}
