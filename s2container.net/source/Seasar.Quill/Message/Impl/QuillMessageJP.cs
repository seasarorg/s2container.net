using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quill.Message.Impl {
    /// <summary>
    /// Quill出力メッセージ（日本語）クラス
    /// </summary>
    public class QuillMessageJP : IQuillMessage {
        /// <summary>
        /// リソースの解放
        /// </summary>
        public virtual void Dispose() {
            // 保持リソースがないため、実装なし
        }

        public string GetAlreadyInjected(Type targetType) {
            return "GetAlreadyInjected";
        }

        public string GetArgumentNull(string argName) {
            return "GetArgumentNull";
        }

        public string GetErrorLoadingConfig(string path) {
            return "GetErrorLoadingConfig";
        }

        public string GetFileNotFound() {
            return "GetFileNotFound";
        }

        public string GetIllegalConstructor(Type type) {
            return string.Format(
                "Quillコンテナで管理するには、publicで宣言された引数なしのコンストラクタが必要です。[{0}]", 
                type.FullName);
        }

        public string GetNotAssignable(Type receiptType, Type componentType) {
            return "GetNotAssignable";
        }

        public string GetNotFoundNodePath(string nodePath) {
            return string.Format("[{0}]に該当するノードが見つかりませんでした。", nodePath);
        }

        public string GetNotFoundRequireSection(string requireSectionName, string path) {
            return string.Format("設定ファイルに必須なセクション({0})が定義されていません。[{1}]", 
                requireSectionName, path);
        }

        public string GetNotInjectionTargetType(Type type) {
            return "GetNotInjectionTargetType";
        }

        #region DB
        /// <summary>
        /// DB接続修飾オブジェクトが設定されていない場合のメッセージ
        /// </summary>
        /// <returns>メッセージ</returns>
        public string GetNotFoundDBConnectionDecorator() {
            return "DB接続修飾オブジェクトが設定されていません。";
        }

        /// <summary>
        /// DB接続が未設定だった場合のメッセージ
        /// </summary>
        /// <returns></returns>
        public string GetNoDbConnectionInstance() {
            return "DB接続が設定されていません。IDbConnectionの設定見直しが必要です。";
        }

        /// <summary>
        /// コネクション開始メッセージ
        /// </summary>
        /// <returns></returns>
        public string GetConnectionOpened() {
            return "コネクションを開始しました。";
        }

        /// <summary>
        /// コネクション終了メッセージ
        /// </summary>
        /// <returns></returns>
        public string GetConnectionClosed() {
            return "コネクションを閉じました。";
        }

        /// <summary>
        /// コネクションリソース解放メッセージ
        /// </summary>
        /// <returns></returns>
        public string GetConnectionDisposed() {
            return "コネクションのリソースを解放しました。";
        }

        /// <summary>
        /// トランザクション開始メッセージ
        /// </summary>
        /// <returns></returns>
        public string GetBeginTx() {
            return "トランザクションを開始しました。";
        }

        /// <summary>
        /// コミット完了メッセージ
        /// </summary>
        /// <returns></returns>
        public string GetCommitted() {
            return "コミット完了しました。";
        }

        /// <summary>
        /// ロールバック完了メッセージ
        /// </summary>
        /// <returns></returns>
        public string GetRollbacked() {
            return "ロールバック完了しました。";
        }

        /// <summary>
        /// データソース未登録メッセージ
        /// </summary>
        /// <param name="dataSourceName">データソース名</param>
        /// <returns></returns>
        public string GetNotRegisteredDataSource(string dataSourceName) {
            return string.Format("データソース名（{0}）に対応するデータソースが登録されていません。", dataSourceName);
        }
        #endregion
    }
}
