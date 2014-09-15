
using System;
using System.Reflection;

namespace Seasar.Quill.Injection
{
    /// <summary>
    /// インジェクションインターフェース
    /// </summary>
    public interface IQuillInjector
    {
        /// <summary>
        /// インジェクション実行
        /// </summary>
        /// <param name="root">インジェクション対象オブジェクト</param>
        void Inject(object root);
    }
}
