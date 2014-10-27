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
        public virtual Type GetImplType(Type targetType)
        {
            if (targetType.IsImplementationAttrAttached())
            {
                return targetType.GetImplType();
            }
            return null;
        }

        public virtual void Dispose()
        {
            // キャッシュはしていないので処理なし
        }
    }
}
