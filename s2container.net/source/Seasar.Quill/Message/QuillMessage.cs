using System.Collections.Generic;

namespace Quill.Message {
    /// <summary>
    /// Quillメッセージコード列挙体
    /// </summary>
    public enum QMsg {
        /// <summary></summary>
        NotAssignable = 0,
        /// <summary></summary>
        NotInjectionTargetType,
        /// <summary></summary>
        AlreadyInjected,
        /// <summary></summary>
        ArgumentNull,
        /// <summary></summary>
        FileNotFound,
        /// <summary></summary>
        ErrorLoadingConfig,
        /// <summary></summary>
        NotFoundRequireSection,
        /// <summary></summary>
        NotFoundNodePath,
        /// <summary></summary>
        IllegalConstructor,

        /// <summary></summary>
        NotFoundDBConnectionDecorator,
        /// <summary></summary>
        NoDbConnectionInstance,
        /// <summary></summary>
        ConnectionOpened,
        /// <summary></summary>
        ConnectionClosed,
        /// <summary></summary>
        ConnectionDisposed,
        /// <summary></summary>
        BeginTx,
        /// <summary></summary>
        Committed,
        /// <summary></summary>
        Rollbacked,
        /// <summary></summary>
        NotRegisteredDataSource,
        /// <summary></summary>
        TypeNotFound,
    };

    /// <summary>
    /// Quillメッセージクラス
    /// </summary>
    public class QuillMessage {
        #region メッセージ保持、基本処理
        private readonly IDictionary<QMsg, string> _messageMap;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="messageMap"></param>
        public QuillMessage(IDictionary<QMsg, string> messageMap) {
            _messageMap = messageMap;
        }

        /// <summary>
        /// メッセージの取得
        /// </summary>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        public virtual string GetMessage(QMsg messageCode) {
            if(_messageMap.ContainsKey(messageCode)) {
                return _messageMap[messageCode];
            }
            return "[Unknown Message]";
        }
        #endregion

        /// <summary>
        /// Quillメッセージ（日本語）の取得
        /// </summary>
        /// <returns></returns>
        public static QuillMessage CreateForJPN() {
            var msgMap = new Dictionary<QMsg, string>();
            msgMap[QMsg.NotAssignable] = "引数の型を戻り値の型に設定することはできません。";
            msgMap[QMsg.NotInjectionTargetType] = "インジェクション対象の型ではありません。";
            msgMap[QMsg.AlreadyInjected] = "既にインジェクション済です。";
            msgMap[QMsg.ArgumentNull] = "Null入力は禁止です。";
            msgMap[QMsg.FileNotFound] = "ファイルが見つかりません。";
            msgMap[QMsg.ErrorLoadingConfig] = "設定ファイルの読み込みに失敗しました。";
            msgMap[QMsg.NotFoundRequireSection] = "必須セクションが定義されていません。";
            msgMap[QMsg.NotFoundNodePath] = "指定されたノードパスが見つかりません。";
            msgMap[QMsg.IllegalConstructor] = "Quillで使用可能なコンストラクタがありません。";
            msgMap[QMsg.NotFoundDBConnectionDecorator] = "DB接続修飾オブジェクトが設定されていません。";
            msgMap[QMsg.NoDbConnectionInstance] = "DB接続オブジェクトが設定されていません";
            msgMap[QMsg.ConnectionOpened] = "DB接続を開始しました。";
            msgMap[QMsg.ConnectionClosed] = "DB接続を終了しました。";
            msgMap[QMsg.ConnectionDisposed] = "DB接続のリソースを解放しました。";
            msgMap[QMsg.BeginTx] = "トランザクションを開始しました。";
            msgMap[QMsg.Committed] = "コミットしました。";
            msgMap[QMsg.Rollbacked] = "ロールバックしました。";
            msgMap[QMsg.NotRegisteredDataSource] = "データソースが登録されていません。";
            msgMap[QMsg.TypeNotFound] = "型情報が見つかりません。";

            return new QuillMessage(msgMap);
        }
    }
}
