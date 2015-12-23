using System;
using System.Collections.Generic;

namespace Quill.Container.Impl {
    /// <summary>
    /// 受け取り／コンポーネント型マッピングクラス
    /// </summary>
    public class TypeMapImpl : ITypeMap {
        private readonly IDictionary<Type, Type> _typeMap = new Dictionary<Type, Type>();

        public virtual void Add(Type receiptType, Type implType) {
            _typeMap[receiptType] = implType;
        }

        /// <summary>
        /// コンポーネント型の取得（引数の型をそのまま返す）
        /// </summary>
        /// <param name="receiptType"></param>
        /// <returns></returns>
        public Type GetComponentType(Type receiptType) {
            return _typeMap[receiptType];
        }

        /// <summary>
        /// マッピングされているか判定（常にfalse）
        /// </summary>
        /// <param name="receipedType"></param>
        /// <returns></returns>
        public bool IsMapped(Type receipedType) {
            return _typeMap.ContainsKey(receipedType);
        }
    }
}
