using Quill.Message;
using QM = Quill.QuillManager;

namespace Quill.Exception {
    /// <summary>
    /// Quill内部で発生する例外クラス
    /// </summary>
    public class QuillException : System.Exception {
        /// <summary>
        /// メッセージ出力元
        /// </summary>
        private const string MSG_SOURCE = "QuillException#ctor";

        /// <summary>
        /// 例外メッセージを指定してインスタンス生成（ログ出力付き）
        /// </summary>
        /// <param name="message"></param>
        public QuillException(string message) : base(message) {
            QM.OutputLog(MSG_SOURCE, EnumMsgCategory.ERROR, message);
        }

        /// <summary>
        /// 例外メッセージ、内部例外を指定してインスタンス生成（ログ出力付き）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public QuillException(string message, System.Exception innerException)
            : base(message, innerException) {
            QM.OutputLog(MSG_SOURCE, EnumMsgCategory.ERROR, message);
        }
    }
}
