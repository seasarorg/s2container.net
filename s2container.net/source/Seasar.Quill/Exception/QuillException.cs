using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quill.Exception {
    /// <summary>
    /// Quill内部で発生する例外クラス
    /// </summary>
    public class QuillException : System.Exception {
        /// <summary>
        /// 例外メッセージを指定してインスタンス生成（ログ出力付き）
        /// </summary>
        /// <param name="message"></param>
        public QuillException(string message) : base(message) {

            QuillManager.OutputLog(typeof(QuillException).Name, message);
        }

        /// <summary>
        /// 例外メッセージ、内部例外を指定してインスタンス生成（ログ出力付き）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public QuillException(string message, System.Exception innerException)
            : base(message, innerException) {

            QuillManager.OutputLog(typeof(QuillException).Name, message);
        }
    }
}
