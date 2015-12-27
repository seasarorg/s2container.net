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

        /// <summary>
        /// nulll禁止な引数にnullを渡してしまった場合のメッセージ
        /// </summary>
        /// <param name="argName"></param>
        /// <returns></returns>
        string GetArgumentNull(string argName);

        /// <summary>
        /// 指定した場所にファイルが見つからなかった場合のメッセージ
        /// </summary>
        /// <returns></returns>
        string GetFileNotFound();

        /// <summary>
        /// 設定ファイル読み込みエラーメッセージ
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string GetErrorLoadingConfig(string path);

        /// <summary>
        /// 設定ファイルに必須セクションが定義されていないメッセージ
        /// </summary>
        /// <param name="requireSectionName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        string GetNotFoundRequireSection(string requireSectionName, string path);

        /// <summary>
        /// ノードパスが見つからなかった場合のメッセージ
        /// </summary>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        string GetNotFoundNodePath(string nodePath);
    }
}
