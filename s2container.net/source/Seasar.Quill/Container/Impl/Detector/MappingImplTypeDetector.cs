using System;
using System.Collections.Generic;

namespace Seasar.Quill.Container.Impl.Detector
{
    /// <summary>
    /// 型と型のマッピングによる実装型走査クラス
    /// </summary>
    public class MappingImplTypeDetector : IImplTypeDetector
    {
        private readonly IDictionary<Type, Type> _typeMap;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="typeMap">指定する型と取得する実装型のマップ</param>
        public MappingImplTypeDetector(IDictionary<Type, Type> typeMap)
        {
            _typeMap = typeMap;
        }

        /// <summary>
        /// 実装型の取得
        /// </summary>
        /// <param name="baseType">走査元の型</param>
        /// <returns>実装型</returns>
        public virtual Type GetImplType(Type baseType)
        {
            if (_typeMap.ContainsKey(baseType)) 
            {
                return _typeMap[baseType];
            }
            return null;
        }
    }
}
