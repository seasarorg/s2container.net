using System;

namespace Seasar.Quill.Log
{
    /// <summary>
    /// Loggerを取得するファクトリインターフェース
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Loggerの取得
        /// </summary>
        /// <param name="t">Loggerを利用するクラス</param>
        /// <returns>Loggerオブジェクト</returns>
        ILogger GetLogger(Type t);
    }
}
