
namespace Seasar.Quill.Exception
{
    /// <summary>
    /// Quillで発生する一般例外クラス
    /// </summary>
    public class QuillApplicationException : System.Exception
    {
        public QuillApplicationException() : base()
        { }

        public QuillApplicationException(string message) : base(message)
        { }

        public QuillApplicationException(string message, System.Exception innerException) : base(message, innerException)
        { }
    }
}
