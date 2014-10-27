using Seasar.Quill.Exception;

namespace Seasar.Quill.Custom
{
    /// <summary>
    /// QuillApplicationExceptionハンドルデリゲート
    /// </summary>
    /// <param name="ex">発生例外</param>
    /// <returns>ハンドリング結果</returns>
    public delegate object HandleQuillApplicationException(QuillApplicationException ex);

    public interface IQuillApplicationExceptionHandler
    {
        object Handle(QuillApplicationException ex);
    }
}
