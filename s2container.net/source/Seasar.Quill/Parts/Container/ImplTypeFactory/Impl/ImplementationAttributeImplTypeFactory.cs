using Seasar.Quill.Attr;
using Seasar.Quill.Parts.Container;
using System;

namespace Seasar.Quill.Parts.Container.ImplTypeFactory.Impl
{
    /// <summary>
    /// Implementation属性からの実装クラス取得処理ファクトリ
    /// </summary>
    public class ImplementationAttributeImplTypeFactory : IImplTypeFactory
    {
        /// <summary>
        /// 実装型の取得
        /// </summary>
        /// <param name="receiptType">受け取り側の型</param>
        /// <returns>実装型</returns>
        public virtual Type GetImplType(Type receiptType)
        {
            if (receiptType.IsImplementationAttrAttached())
            {
                return receiptType.GetImplType();
            }
            return null;
        }

        /// <summary>
        /// リソースの破棄
        /// </summary>
        public virtual void Dispose()
        {
            // キャッシュなどはしていないので処理なし
        }
    }
}
