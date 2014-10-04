using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Seasar.Quill.SupportType
{
    /// <summary>
    /// 重複有無チェック用の簡易Setクラス
    /// </summary>
    /// <typeparam name="T">管理するキーの型</typeparam>
    public class QuillSet<T>
    {
        private readonly IDictionary<T, T> _dic;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dic">一意であることを管理するためのマップ</param>
        public QuillSet(bool isConcurrent = true)
        {
            if (isConcurrent)
            {
                _dic = new ConcurrentDictionary<T, T>();
            }
            else
            {
                _dic = new Dictionary<T, T>();
            }
        }

        // ==================================================================
        
        /// <summary>
        /// 指定要素が既にコレクションに含まれているか判定
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool Contains(T target)
        {
            return _dic.ContainsKey(target);
        }

        /// <summary>
        /// コレクションに追加
        /// </summary>
        /// <param name="target"></param>
        public virtual void Add(T target)
        {
            _dic[target] = target;
        }

        /// <summary>
        /// コレクションをクリア
        /// </summary>
        public void Clear()
        {
            _dic.Clear();
        }
    }
}
