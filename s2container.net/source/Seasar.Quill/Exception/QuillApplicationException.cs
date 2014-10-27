
namespace Seasar.Quill.Exception
{
    /// <summary>
    /// Quillで発生する一般例外クラス
    /// </summary>
    public class QuillApplicationException : System.Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public QuillApplicationException() : base()
        { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public QuillApplicationException(string message) : base(message)
        { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="innerException">内部例外</param>
        public QuillApplicationException(string message, System.Exception innerException) : base(message, innerException)
        { }
    }
}
