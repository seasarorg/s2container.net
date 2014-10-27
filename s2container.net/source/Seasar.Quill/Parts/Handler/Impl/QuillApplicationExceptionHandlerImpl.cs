using Seasar.Quill.Parts;

namespace Seasar.Quill.Parts.Handler.Impl
{
    /// <summary>
    /// QuillApplicationExceptionハンドラ実装クラス
    /// </summary>
    public class QuillApplicationExceptionHandlerImpl : IQuillApplicationExceptionHandler
    {
        /// <summary>
        /// 例外処理
        /// </summary>
        /// <param name="ex">発生例外</param>
        /// <returns>例外処理結果</returns>
        public virtual object Handle(Exception.QuillApplicationException ex)
        {
            throw ex;
        }
    }
}
