using System;

namespace Quill.Scope {
    /// <summary>
    /// Quill修飾インターフェース
    /// </summary>
    public interface IQuillDecorator {
        /// <summary>
        /// 修飾して実行
        /// </summary>
        /// <param name="action"></param>
        void Decorate(Action action);

        /// <summary>
        /// 修飾して実行
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        RETURN_TYPE Decorate<RETURN_TYPE>(Func<RETURN_TYPE> func);
    }

    /// <summary>
    /// Quill修飾インターフェース
    /// </summary>
    public interface IQuillDecorator<PARAMETER_TYPE> {
        /// <summary>
        /// 修飾して実行
        /// </summary>
        /// <param name="action"></param>
        void Decorate(Action<PARAMETER_TYPE> action);

        /// <summary>
        /// 修飾して実行
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        RETURN_TYPE Decorate<RETURN_TYPE>(Func<PARAMETER_TYPE, RETURN_TYPE> func);
    }
}
