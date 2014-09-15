using Seasar.Quill.Config.Impl;
using Seasar.Quill.Container;
using Seasar.Quill.Injection;
using Seasar.Quill.Log;

namespace Seasar.Quill.Config
{
    /// <summary>
    /// Quill設定クラス
    /// </summary>
    public class QuillConfig
    {
        #region static

        /// <summary>
        /// 設定
        /// </summary>
        private static QuillConfig _config;

        private static object _lock = new object();

        /// <summary>
        /// 設定オブジェクトの取得
        /// </summary>
        /// <returns></returns>
        public static QuillConfig GetInstance()
        {
            return _config;
        }
        #endregion

        /// <summary>
        /// ログファクトリ
        /// </summary>
        public virtual ILoggerFactory LoggerFactory { get; protected set; }

        /// <summary>
        /// DIコンテナ
        /// </summary>
        public virtual IQuillContainer Container { get; protected set; }

        /// <summary>
        /// インジェクションオブジェクト
        /// </summary>
        public virtual IQuillInjector Injector { get; protected set; }

        protected QuillConfig()
        { }

        /// <summary>
        /// Quill設定のセットアップ
        /// </summary>
        /// <param name="builder">Quill設定構築オブジェクト</param>
        public static void Setup(IQuillConfigBuilder builder)
        {
            lock (_lock)
            {
                var config = new QuillConfig();
                config.Container = builder.Container;
                config.Injector = builder.Injector;
                config.LoggerFactory = builder.LoggerFactory;
                _config = config;
            }
        }

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static QuillConfig()
        {
            Setup(new DefaultQuillConfigBuilder());
        } 
    }
}
