
using System;

namespace Quill.Container {
    /// <summary>
    /// コンポーネントのインスタンス生成インターフェース
    /// </summary>
    public interface IComponentCreator {
        /// <summary>
        /// インスタンス生成
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        object Create(Type componentType);
    }
}
