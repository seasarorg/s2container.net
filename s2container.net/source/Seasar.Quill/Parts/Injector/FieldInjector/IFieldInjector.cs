using System.Reflection;

namespace Seasar.Quill.Parts.Injector.FieldInjector
{
    /// <summary>
    /// フィールドへのインジェクションインターフェース
    /// </summary>
    public interface IFieldInjector
    {
        /// <summary>
        /// フィールドへのインジェクション実行
        /// </summary>
        /// <param name="target">インジェクション対象のオブジェクト</param>
        /// <param name="fieldInfo">インジェクションするフィールド情報</param>
        /// <param name="context">インジェクション状態管理</param>
        void InjectField(object target, FieldInfo fieldInfo, QuillInjectionContext context);
    }
}
