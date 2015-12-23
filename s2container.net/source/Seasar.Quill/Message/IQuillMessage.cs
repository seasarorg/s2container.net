using System;

namespace Quill.Message {
    /// <summary>
    /// Quillメッセージインターフェース
    /// </summary>
    public interface IQuillMessage {
        /// <summary>
        /// アサイン不可メッセージの取得
        /// </summary>
        /// <param name="receiptType"></param>
        /// <param name="componentType"></param>
        string GetNotAssignable(Type receiptType, Type componentType);

        /// <summary>
        /// Injection対象ではないメッセージの取得
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetNotInjectionTargetType(Type type);

        /// <summary>
        /// Injection済メッセージの取得
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        string GetAlreadyInjected(Type targetType);
    }
}
