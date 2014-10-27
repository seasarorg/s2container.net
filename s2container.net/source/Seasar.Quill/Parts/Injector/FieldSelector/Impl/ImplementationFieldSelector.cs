using Seasar.Quill.Attr;
using System.Collections.Generic;
using System.Linq;

namespace Seasar.Quill.Parts.Injector.FieldSelector.Impl
{
    /// <summary>
    /// Implementationが付加された型のフィールドを抽出するクラス
    /// </summary>
    public class ImplementationFieldSelector : IFieldSelector
    {
        /// <summary>
        /// フィールド抽出
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <returns>抽出したフィールドのコレクション</returns>
        public virtual IEnumerable<System.Reflection.FieldInfo> Select(object target, QuillInjectionContext context)
        {
            var fields = target.GetType().GetFields(context.Condition);
            return fields.Where(fi => fi.FieldType.IsImplementationAttrAttached());
        }
    }
}
