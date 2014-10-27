using Seasar.Quill.Parts.Injector;
using System.Collections.Generic;

namespace Seasar.Quill.Parts.Injector.FieldSelector.Impl
{
    /// <summary>
    /// インジェクション先フィールド抽出実装クラス
    /// </summary>
    public class FieldSelectorImpl : IFieldSelector
    {
        /// <summary>
        /// フィールド抽出
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <returns>抽出したフィールドのコレクション</returns>
        public virtual IEnumerable<System.Reflection.FieldInfo> Select(object target, QuillInjectionContext context)
        {
            return target.GetType().GetFields(context.Condition);
        }
    }
}
