using System;

namespace Quill.Scope {
    /// <summary>
    /// Quill修飾インターフェース
    /// </summary>
    public interface IQuillDecorator {
        /// <summary>
        /// 前処理、後処理で挟んで実行
        /// </summary>
        /// <param name="action">本処理</param>
        void Decorate(Action action);

        /// <summary>
        /// 前処理、後処理で挟んで実行
        /// </summary>
        /// <typeparam name="RETURN_TYPE">本処理の戻り値型</typeparam>
        /// <param name="func">本処理</param>
        /// <returns>本処理の戻り値</returns>
        RETURN_TYPE Decorate<RETURN_TYPE>(Func<RETURN_TYPE> func);
    }

    /// <summary>
    /// Quill修飾インターフェース
    /// </summary>
    public interface IQuillDecorator<PARAMETER_TYPE> {
        /// <summary>
        /// 前処理、後処理で挟んで実行
        /// </summary>
        /// <param name="action">本処理</param>
        void Decorate(Action<PARAMETER_TYPE> action);

        /// <summary>
        /// 前処理、後処理で挟んで実行
        /// </summary>
        /// <typeparam name="RETURN_TYPE">本処理の戻り値型</typeparam>
        /// <param name="func">本処理</param>
        /// <returns>本処理の戻り値</returns>
        RETURN_TYPE Decorate<RETURN_TYPE>(Func<PARAMETER_TYPE, RETURN_TYPE> func);
    }
}
