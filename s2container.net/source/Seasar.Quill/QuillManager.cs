using System;
using Quill.Config;
using Quill.Config.Impl;
using Quill.Container;
using Quill.Container.Impl;
using Quill.Inject;
using Quill.Inject.Impl;
using Quill.Message;
using Quill.Message.Impl;

namespace Quill {
    /// <summary>
    /// ログ出力デリゲート
    /// </summary>
    /// <param name="source">ログ出力場所</param>
    /// <param name="category">メッセージカテゴリ</param>
    /// <param name="log">ログ出力内容</param>
    public delegate void OutputLogDelegate(string source, EnumMsgCategory category, string log);

    /// <summary>
    /// Quill挙動管理クラス
    /// </summary>
    public static class QuillManager {
        /// <summary>
        /// 設定情報
        /// </summary>
        public static IQuillConfig Config { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static QuillContainer Container { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static ITypeMap TypeMap { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static IQuillInjector Injector { get; set; }

        /// <summary>
        /// Injectionフィルター
        /// </summary>
        public static IInjectionFilter InjectionFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static IComponentCreator ComponentCreator { get; set; }

        /// <summary>
        /// 出力メッセージ
        /// </summary>
        public static IQuillMessage Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static OutputLogDelegate OutputLog { get; set; }

        /// <summary>
        /// SQLパラメータ置換処理
        /// </summary>
        public static Func<string, string> ReplaceToParamMark { get; set; }

        /// <summary>
        /// 既定の設定で初期化
        /// </summary>
        public static void InitializeDefault() {
            Config = null;
            Container = new QuillContainer();
            TypeMap = new TypeMapImpl();
            ComponentCreator = new ComponentCreators();
            Injector = new QuillInjector();
            InjectionFilter = new InjectionFilterBase();
            Message = new QuillMessageJP();
            OutputLog = OutputLogToConsole;

            // SQL Server用パラメータマーク
            ReplaceToParamMark = (paramName) => "@" + paramName;
        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        public static void Dispose() {
            var targets = new IDisposable[] {
                Config, Container, TypeMap, Injector, InjectionFilter, ComponentCreator, Message
            };

            foreach(var target in targets) {
                if(target != null) {
                    target.Dispose();
                }
            }

            OutputLog = null;
        }

        private static void OutputLogToConsole(string source, EnumMsgCategory category, string log) {            
            Console.WriteLine(string.Format("{0} {1}:[{2}] {3}", 
                DateTime.Now, source, category.GetCategoryName(), log));
        }
    }

}
