using Seasar.Quill.Container;
using Seasar.Quill.Injection;
using Seasar.Quill.Log;

namespace Seasar.Quill.Config.Impl
{
    /// <summary>
    /// Quill設定構築抽象クラス
    /// </summary>
    public abstract class AbstractQuillConfigBuilder : IQuillConfigBuilder
    {
        public virtual ILoggerFactory LoggerFactory { get; protected set;}

        public virtual IQuillContainer Container { get; protected set; }

        public virtual IQuillInjector Injector { get; protected set; }
    }
}
