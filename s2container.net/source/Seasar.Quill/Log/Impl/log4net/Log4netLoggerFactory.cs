using System;

namespace Seasar.Quill.Log.Impl.log4net
{
    /// <summary>
    /// log4net用Loggerファクトリクラス
    /// </summary>
    public class Log4netLoggerFactory : ILoggerFactory
    {
        public ILogger GetLogger(Type t)
        {
            return Logger.GetLogger(t);
        }
    }
}
