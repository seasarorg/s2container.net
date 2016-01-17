using System;

namespace Quill.Message {
    /// <summary>
    /// Quillメッセージインターフェース
    /// </summary>
    public interface IQuillMessage : IDisposable {
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

        /// <summary>
        /// 適切なコンストラクタが見つからなかった場合のメッセージ
        /// </summary>
        /// <param name="type">適切なコンストラクタが見つからなかった型</param>
        /// <returns></returns>
        string GetIllegalConstructor(Type type);

        #region DB

        /// <summary>
        /// DB接続修飾オブジェクトが設定されていない場合のメッセージ
        /// </summary>
        /// <returns></returns>
        string GetNotFoundDBConnectionDecorator();

        /// <summary>
        /// DB接続が未設定だった場合のメッセージ
        /// </summary>
        /// <returns></returns>
        string GetNoDbConnectionInstance();

        /// <summary>
        /// コネクション開始メッセージ
        /// </summary>
        /// <returns></returns>
        string GetConnectionOpened();

        /// <summary>
        /// コネクション終了メッセージ
        /// </summary>
        /// <returns></returns>
        string GetConnectionClosed();

        /// <summary>
        /// コネクションリソース解放メッセージ
        /// </summary>
        /// <returns></returns>
        string GetConnectionDisposed();

        /// <summary>
        /// トランザクション開始メッセージ
        /// </summary>
        /// <returns></returns>
        string GetBeginTx();

        /// <summary>
        /// コミット完了メッセージ
        /// </summary>
        /// <returns></returns>
        string GetCommitted();

        /// <summary>
        /// ロールバック完了メッセージ
        /// </summary>
        /// <returns></returns>
        string GetRollbacked();

        /// <summary>
        /// データソース未登録メッセージ
        /// </summary>
        /// <param name="dataSourceName">データソース名</param>
        /// <returns></returns>
        string GetNotRegisteredDataSource(string dataSourceName);
        #endregion
    }
}
