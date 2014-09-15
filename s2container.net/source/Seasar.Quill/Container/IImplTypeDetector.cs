using System;

namespace Seasar.Quill.Container
{
    /// <summary>
    /// 実装型走査インターフェース
    /// </summary>
    public interface IImplTypeDetector
    {
        /// <summary>
        /// 実装型の取得
        /// </summary>
        /// <param name="baseType">走査元の型</param>
        /// <returns>実装型</returns>
        Type GetImplType(Type baseType);
    }
}
