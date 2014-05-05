using Seasar.Quill.Container;
using Seasar.Quill.Container.Impl;
using Seasar.Quill.Container.Impl.ComponentCreator;
using Seasar.Quill.Container.Impl.Detector;
using Seasar.Quill.Container.Impl.InstanceManager;
using Seasar.Quill.Injection.Impl;
using Seasar.Quill.Log.Impl.log4net;

namespace Seasar.Quill.Config.Impl
{
    /// <summary>
    /// 既定の設定を適用するQuill設定構築クラス
    /// </summary>
    public class DefaultQuillConfigBuilder : AbstractQuillConfigBuilder
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DefaultQuillConfigBuilder()
        {
            var container = new QuillContainer();
            var injector = new QuillInjector(container);

            // そのままインスタンスnew
            var newInstanceCreator = new NewInstanceCreator();
            // Interceptorを適用したProxyオブジェクトを生成（Castle.DynamicProxyを使用）
            var proxyCreator = new CastleDynamicProxyCreator(injector);
            // コンテナでのインスタンス生成はSingleton
            var instanceManager = new SingletonInstanceManager();
            // ProxyCreater, NewInstanceの優先順でインスタンスを生成
            instanceManager.SetComponentCreator(proxyCreator, newInstanceCreator);

            container.SetInstanceManager(instanceManager);
            // Implementation属性から実装クラスを取得する
            container.SetImplTypeDetector(new IImplTypeDetector[] { new AttributeImplTypeDetector() });

            // 作成した設定を適用
            Container = container;
            Injector = injector;
            LoggerFactory = new Log4netLoggerFactory();
        }
    }
}
