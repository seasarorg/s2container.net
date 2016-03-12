namespace Quill.Message {
    /// <summary>
    /// Quillメッセージユーティリティクラス
    /// </summary>
    public static class QuillMessageUtils {
        /// <summary>
        /// メッセージの取得
        /// </summary>
        /// <param name="messageCode">メッセージコード</param>
        /// <returns>メッセージ</returns>
        public static string Get(this QMsg messageCode) {
            return QuillManager.Message.GetMessage(messageCode);
        }
    }
}
