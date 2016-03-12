
using System;

namespace Quill.Container {
    /// <summary>
    /// コンポーネントのインスタンス生成インターフェース
    /// </summary>
    public interface IComponentCreator : IDisposable {
        /// <summary>
        /// インスタンス生成
        /// </summary>
        /// <param name="componentType">コンポーネント型</param>
        /// <returns>生成したインスタンス</returns>
        object Create(Type componentType);
    }
}
