using System;
using System.Collections.Generic;

namespace Quill.Container.Impl {
    /// <summary>
    /// 戻り値型ーコンポーネント型紐づけクラス
    /// </summary>
    public class TypeMapImpl : ITypeMap {
        private readonly IDictionary<Type, Type> _typeMap;

        /// <summary>
        /// コンストラクタ（内部で保持しているDictionaryを初期化
        /// </summary>
        public TypeMapImpl() {
            _typeMap = CreateDictionary();
        }

        /// <summary>
        /// 戻り値型ーコンポーネント型組み合わせの追加
        /// </summary>
        /// <param name="receiptType">戻り値型</param>
        /// <param name="componentType">コンポーネント型</param>
        public virtual void Add(Type receiptType, Type componentType) {
            _typeMap[receiptType] = componentType;
        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        public virtual void Dispose() {
            _typeMap.Clear();
        }

        /// <summary>
        /// コンポーネント型の取得（引数の型をそのまま返す）
        /// </summary>
        /// <param name="receiptType">戻り値として返す型</param>
        /// <returns>コンポーネント型（紐づく型がなければreceiptTypeをそのまま返す</returns>
        public virtual Type GetComponentType(Type receiptType) {
            if(_typeMap.ContainsKey(receiptType)) {
                return _typeMap[receiptType];
            }
            return receiptType;
        }

        /// <summary>
        /// コンポーネントの受け取り側の型とコンテナ上で管理している型が紐づけられているか判定
        /// </summary>
        /// <param name="receipedType">コンポーネントを受け取る型</param>
        /// <returns>true:紐づけあり, false:紐づけなし</returns>
        public virtual bool IsMapped(Type receipedType) {
            return _typeMap.ContainsKey(receipedType);
        }

        /// <summary>
        /// 戻り値型ーコンポーネント型紐づけDictionaryの生成
        /// </summary>
        /// <returns>戻り値型ーコンポーネント型紐づけDictionary</returns>
        protected virtual IDictionary<Type, Type> CreateDictionary() {
            return new Dictionary<Type, Type>();
        }
    }
}
