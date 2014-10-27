using Seasar.Quill.Parts.Container;
using System;
using System.Collections.Generic;

namespace Seasar.Quill.Parts.Container.ImplTypeFactory.Impl
{
    /// <summary>
    /// 実装型取得処理ファクトリ集合クラス
    /// </summary>
    public class ImplTypeFactories : IImplTypeFactory
    {
        /// <summary>
        /// 実装型取得処理オブジェクトのコレクション
        /// </summary>
        private readonly IList<IImplTypeFactory> _factories = new List<IImplTypeFactory>();

        /// <summary>
        /// 実装型の取得（追加順に実装型取得処理が実行され、最初に該当した実装型を返す）
        /// </summary>
        /// <param name="receiptType">受け取り側の型</param>
        /// <returns>実装型</returns>
        public virtual Type GetImplType(Type targetType)
        {
            foreach (var factory in _factories)
            {
                var implType = factory.GetImplType(targetType);
                if (implType != null)
                {
                    return implType;
                }
            }
            return null;
        }

        /// <summary>
        ///  実装型取得処理ファクトリの追加
        /// </summary>
        /// <param name="factory"> 実装型取得処理ファクトリ</param>
        public virtual void AddFactory(IImplTypeFactory factory)
        {
            _factories.Add(factory);
        }

        public virtual void Dispose()
        {
            foreach(var factory in _factories)
            {
                factory.Dispose();
            }
        }
    }
}
