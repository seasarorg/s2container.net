using System.Collections.Generic;
using System.Reflection;

namespace Seasar.Quill.Parts.Injector.FieldSelector
{
    /// <summary>
    /// インジェクション先フィールド抽出インターフェース
    /// </summary>
    public interface IFieldSelector
    {
        /// <summary>
        /// フィールド抽出
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <returns>抽出したフィールドのコレクション</returns>
        IEnumerable<FieldInfo> Select(object target, QuillInjectionContext context);
    }
}
