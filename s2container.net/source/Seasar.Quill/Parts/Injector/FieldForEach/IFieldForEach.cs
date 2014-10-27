using System.Collections.Generic;
using System.Reflection;

namespace Seasar.Quill.Parts.Injector.FieldForEach
{
    /// <summary>
    /// インジェクション抽出フィールドループ処理インターフェース
    /// </summary>
    public interface IFieldForEach
    {
        /// <summary>
        /// ループ処理実行
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <param name="fields">インジェクション先として抽出したフィールドのコレクション</param>
        /// <param name="injectField">フィールドへのインジェクション実行コールバック Action&lt;object, FieldInfo, QuillInjectionContext&gt;</param>
        void ForEach(object target, QuillInjectionContext context, IEnumerable<FieldInfo> fields,
            Seasar.Quill.QuillInjector.CallbackInjectField injectField);
    }
}
