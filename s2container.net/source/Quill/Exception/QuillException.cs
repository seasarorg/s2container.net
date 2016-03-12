using Quill.Message;
using QM = Quill.QuillManager;

namespace Quill.Exception {
    /// <summary>
    /// Quill内部で発生する例外クラス
    /// </summary>
    public class QuillException : System.Exception {
        /// <summary>
        /// 例外メッセージを指定してインスタンス生成（ログ出力付き）
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public QuillException(string message) : base(message) {
            QM.OutputLog(GetType(), EnumMsgCategory.ERROR, message);
        }

        /// <summary>
        /// 例外メッセージ、内部例外を指定してインスタンス生成（ログ出力付き）
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="innerException">内部例外</param>
        public QuillException(string message, System.Exception innerException)
            : base(message, innerException) {
            QM.OutputLog(GetType(), EnumMsgCategory.ERROR, message);
        }
    }
}
