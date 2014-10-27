using Seasar.Quill.Exception;

namespace Seasar.Quill.Parts.Handler
{
    /// <summary>
    /// QuillApplicationExceptionハンドルデリゲート
    /// </summary>
    /// <param name="ex">発生例外</param>
    /// <returns>ハンドリング結果</returns>
    public delegate object HandleQuillApplicationException(QuillApplicationException ex);

    /// <summary>
    /// QuillApplicationExceptionハンドラインターフェース
    /// </summary>
    public interface IQuillApplicationExceptionHandler
    {
        /// <summary>
        /// 例外処理
        /// </summary>
        /// <param name="ex">発生例外</param>
        /// <returns>例外処理結果</returns>
        object Handle(QuillApplicationException ex);
    }
}
