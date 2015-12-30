using System;

namespace Quill.Inject {
    /// <summary>
    /// Injection実行インターフェース
    /// </summary>
    public interface IQuillInjector : IDisposable {
        /// <summary>
        /// Inject実行
        /// </summary>
        /// <param name="target">Inject対象のオブジェクト</param>
        void Inject(object target);
    }
}