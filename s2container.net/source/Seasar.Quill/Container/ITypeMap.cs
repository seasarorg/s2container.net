using System;

namespace Quill.Container {
    /// <summary>
    /// コンポーネントの受け取り側の型とコンテナ上で管理している型の紐づけインターフェース
    /// </summary>
    public interface ITypeMap : IDisposable {
        /// <summary>
        /// 紐づける型の追加
        /// </summary>
        /// <param name="receiptType">受け取り型</param>
        /// <param name="implType">実装型</param>
        void Add(Type receiptType, Type implType);

        /// <summary>
        /// コンポーネントの型を取得
        /// </summary>
        /// <param name="receiptType"></param>
        /// <returns></returns>
        Type GetComponentType(Type receiptType);

        /// <summary>
        /// コンポーネントの受け取り側の型とコンテナ上で管理している型が紐づけられているか判定
        /// </summary>
        /// <param name="receipedType"></param>
        /// <returns></returns>
        bool IsMapped(Type receipedType);
    }
}
