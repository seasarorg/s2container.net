
namespace Seasar.Quill.Parts.Handler
{
    /// <summary>
    /// その他Exceptionハンドルデリゲート
    /// </summary>
    /// <param name="ex">発生例外</param>
    /// <returns>ハンドリング結果</returns>
    public delegate object HandleSystemException(System.Exception ex);

    /// <summary>
    /// その他Exceptionハンドラインターフェース
    /// </summary>
    public interface ISystemExceptionHandler
    {
        /// <summary>
        /// 例外処理
        /// </summary>
        /// <param name="ex">発生例外</param>
        /// <returns>例外処理結果</returns>
        object Handle(System.Exception ex);
    }
}
