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
        /// <summary>
        /// ログファクトリ
        /// </summary>
        public virtual ILoggerFactory LoggerFactory { get; protected set;}

        /// <summary>
        /// DIコンテナ
        /// </summary>
        public virtual IQuillContainer Container { get; protected set; }

        /// <summary>
        /// インジェクションオブジェクト
        /// </summary>
        public virtual IQuillInjector Injector { get; protected set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AbstractQuillConfigBuilder()
        {
            var instanceManager = CreateInstanceManager();
            var implTypeDetectors = CreateImplTypeDetectors();
            var container = CreateContainer(instanceManager, implTypeDetectors);
            var context = CreateContext();
            var injector = CreateInjector(container, context);
            var loggerFactory = CreateLoggerFactory();

            Container = container;
            Injector = injector;
            LoggerFactory = loggerFactory;
        }

        protected abstract IInstanceManager CreateInstanceManager();
        protected abstract IImplTypeDetector[] CreateImplTypeDetectors();
        protected abstract IQuillContainer CreateContainer(IInstanceManager instanceManager, IImplTypeDetector[] implTypeDetectors);
        protected abstract IQuillInjectionContext CreateContext();
        protected abstract IQuillInjector CreateInjector(IQuillContainer container, IQuillInjectionContext context);
        protected abstract ILoggerFactory CreateLoggerFactory();
    }
}
