
using Seasar.Quill.Parts.Injector;

namespace Seasar.Quill.Parts.Injector.FieldInjector.Impl
{
    /// <summary>
    /// フィールドへのインジェクション実装クラス
    /// </summary>
    public class FieldInjectorImpl : IFieldInjector
    {
        /// <summary>
        /// フィールドへのインジェクション実行
        /// </summary>
        /// <param name="target">インジェクション対象のオブジェクト</param>
        /// <param name="fieldInfo">インジェクションするフィールド情報</param>
        /// <param name="context">インジェクション状態管理</param>
        public virtual void InjectField(object target, System.Reflection.FieldInfo fieldInfo, QuillInjectionContext context)
        {
            fieldInfo.SetValue(target, context.Container.GetComponent(fieldInfo.FieldType));
        }
    }
}
