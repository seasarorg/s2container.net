
using System;

namespace Seasar.Quill.Log
{
    /// <summary>
    /// ログ出力インターフェース
    /// </summary>
    public interface ILogger
    {
        bool IsDebugEnabled { get; }

        void Debug(object message, System.Exception exception);

        void Debug(object message);

        bool IsInfoEnabled { get; }

        void Info(object message, System.Exception exception);

        void Info(object message);

        void Warn(object message, System.Exception exception);

        void Warn(object message);

        void Error(object message, System.Exception exception);

        void Error(object message);

        void Fatal(object message, System.Exception exception);

        void Fatal(object message);

        void Log(System.Exception exception);

        void Log(string messageCode, object[] args);

        void Log(string messageCode, object[] args, System.Exception exception);

        void Log(string messageCode, object[] args, System.Exception exception, string nameSpace);
    }
}
