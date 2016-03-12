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
        /// コンポーネント型の取得（引数の型をそのまま返す）
        /// </summary>
        /// <param name="receiptType">戻り値として返す型</param>
        /// <returns>コンポーネント型（紐づく型がなければreceiptTypeをそのまま返す</returns>
        Type GetComponentType(Type receiptType);

        /// <summary>
        /// コンポーネントの受け取り側の型とコンテナ上で管理している型が紐づけられているか判定
        /// </summary>
        /// <param name="receipedType">コンポーネントを受け取る型</param>
        /// <returns>true:紐づけあり, false:紐づけなし</returns>
        bool IsMapped(Type receipedType);
    }
}
