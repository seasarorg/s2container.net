using Seasar.Quill.Parts.Container;
using System;
using System.Collections.Generic;

namespace Seasar.Quill.Parts.Container.ImplTypeFactory.Impl
{
    /// <summary>
    /// マッピングによる実装型取得ファクトリ
    /// </summary>
    public class MappingImplTypeFactory : IImplTypeFactory
    {
        /// <summary>
        /// 受取型と実装型の対応マップ
        /// </summary>
        private readonly IDictionary<Type, Type> _implTypeMap;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="implTypeMap">受取型と実装型の対応マップ</param>
        public MappingImplTypeFactory(IDictionary<Type, Type> implTypeMap)
        {
            if (implTypeMap == null) { throw new ArgumentNullException("implTypeMap"); }
            _implTypeMap = implTypeMap;
        }

        /// <summary>
        /// 実装型の取得
        /// </summary>
        /// <param name="receiptType">受け取り側の型</param>
        /// <returns>実装型</returns>
        public virtual Type GetImplType(Type targetType)
        {
            if (_implTypeMap.ContainsKey(targetType))
            {
                return _implTypeMap[targetType];
            }
            return null;
        }

        public virtual void Dispose()
        {
            _implTypeMap.Clear();
        }
    }
}
