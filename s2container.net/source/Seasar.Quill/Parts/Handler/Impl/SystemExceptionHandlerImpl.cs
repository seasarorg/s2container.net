using Seasar.Quill.Parts;

namespace Seasar.Quill.Parts.Handler.Impl
{
    /// <summary>
    /// System.Exceptionハンドラ実装クラス
    /// </summary>
    public class SystemExceptionHandlerImpl : ISystemExceptionHandler
    {
        /// <summary>
        /// 例外処理
        /// </summary>
        /// <param name="ex">発生例外</param>
        /// <returns>例外処理結果</returns>
        public virtual object Handle(System.Exception ex)
        {
            throw ex;
        }
    }
}
