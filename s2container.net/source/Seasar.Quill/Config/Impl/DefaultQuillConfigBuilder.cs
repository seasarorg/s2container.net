using Seasar.Quill.Container;
using Seasar.Quill.Container.Impl;
using Seasar.Quill.Container.Impl.Detector;
using Seasar.Quill.Container.Impl.InstanceManager;
using Seasar.Quill.Injection;
using Seasar.Quill.Injection.Impl;
using Seasar.Quill.Log.Impl.log4net;
using System;
using System.Collections.Concurrent;

namespace Seasar.Quill.Config.Impl
{
    /// <summary>
    /// 既定の設定を適用するQuill設定構築クラス
    /// </summary>
    public class DefaultQuillConfigBuilder : AbstractQuillConfigBuilder
    {
        protected override IInstanceManager CreateInstanceManager()
        {
            return new SingletonInstanceManager();
        }

        protected override IImplTypeDetector[] CreateImplTypeDetectors()
        {
            return new IImplTypeDetector[] { new MappingImplTypeDetector(new ConcurrentDictionary<Type, Type>()), new AttributeImplTypeDetector() };
        }

        protected override IQuillContainer CreateContainer(IInstanceManager instanceManager, IImplTypeDetector[] implTypeDetectors)
        {
            var container = new QuillContainer();
            container.SetInstanceManager(instanceManager);
            container.SetImplTypeDetector(implTypeDetectors);
            return container;
        }

        protected override IQuillInjectionContext CreateContext()
        {
            return new QuillInjectionContext();
        }

        protected override IQuillInjector CreateInjector(IQuillContainer container, Injection.IQuillInjectionContext context)
        {
            var injector = new QuillInjector();
            injector.Container = container;
            injector.Context = context;
            return injector;
        }

        protected override Log.ILoggerFactory CreateLoggerFactory()
        {
            return new Log4netLoggerFactory();
        }
    }
}
