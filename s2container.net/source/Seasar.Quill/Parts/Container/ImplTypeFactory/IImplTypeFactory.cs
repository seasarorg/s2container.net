using System;

namespace Seasar.Quill.Parts.Container.ImplTypeFactory
{
    /// <summary>
    /// 実装型取得ファクトリインターフェース
    /// </summary>
    public interface IImplTypeFactory : IDisposable
    {
        /// <summary>
        /// 実装型の取得
        /// </summary>
        /// <param name="receiptType">受け取り側の型</param>
        /// <returns>実装型</returns>
        Type GetImplType(Type receiptType);
    }
}
