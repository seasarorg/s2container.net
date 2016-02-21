using System;
using Quill.Config;
using Quill.Container;
using Quill.Container.Impl;
using Quill.Inject;
using Quill.Inject.Impl;
using Quill.Message;

namespace Quill {
    /// <summary>
    /// ログ出力デリゲート
    /// </summary>
    /// <param name="source">ログ出力元</param>
    /// <param name="category">メッセージカテゴリ</param>
    /// <param name="log">ログ出力内容</param>
    public delegate void OutputLogDelegate(Type source, EnumMsgCategory category, string log);

    /// <summary>
    /// Quill挙動管理クラス
    /// </summary>
    public static class QuillManager {
        /// <summary>
        /// 設定情報
        /// </summary>
        public static IQuillConfig Config { get; set; }

        /// <summary>
        /// Quillコンテナ
        /// </summary>
        public static QuillContainer Container { get; set; }

        /// <summary>
        /// 受け取り型、実装型紐づけMap
        /// </summary>
        public static ITypeMap TypeMap { get; set; }

        /// <summary>
        /// インジェクション実行オブジェクト
        /// </summary>
        public static IQuillInjector Injector { get; set; }

        /// <summary>
        /// インジェクションフィルター
        /// </summary>
        public static IInjectionFilter InjectionFilter { get; set; }

        /// <summary>
        /// コンポーネント生成オブジェクト
        /// </summary>
        public static IComponentCreator ComponentCreator { get; set; }

        ///// <summary>
        ///// 出力メッセージ
        ///// </summary>
        //public static IQuillMessage Message { get; set; }

        /// <summary>
        /// 出力メッセージ
        /// </summary>
        public static QuillMessage Message { get; set; }

        /// <summary>
        /// ログ出力
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
            Message = QuillMessage.CreateForJPN();
            OutputLog = OutputLogToConsole;

            // SQL Server用パラメータマーク
            ReplaceToParamMark = (paramName) => "@" + paramName;
        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        public static void Dispose() {
            var targets = new IDisposable[] {
                Config, Container, TypeMap, Injector, InjectionFilter, ComponentCreator
            };

            foreach(var target in targets) {
                if(target != null) {
                    target.Dispose();
                }
            }

            OutputLog = null;
        }

        /// <summary>
        /// ログ出力（コンソール）
        /// </summary>
        /// <param name="source"></param>
        /// <param name="category"></param>
        /// <param name="log"></param>
        private static void OutputLogToConsole(Type source, EnumMsgCategory category, string log) {            
            Console.WriteLine(string.Format("{0} {1}:[{2}] {3}", 
                DateTime.Now, source.Name, category.GetCategoryName(), log));
        }
    }

}
