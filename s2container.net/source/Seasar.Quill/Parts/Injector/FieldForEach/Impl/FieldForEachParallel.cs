using System.Collections.Generic;
using System.Linq;

namespace Seasar.Quill.Parts.Injector.FieldForEach.Impl
{
    /// <summary>
    /// 抽出したフィールドを並列で処理するクラス
    /// </summary>
    public class FieldForEachParallel : IFieldForEach
    {
        /// <summary>
        /// ループ処理実行
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <param name="fields">インジェクション先として抽出したフィールドのコレクション</param>
        /// <param name="callbackInjectField">フィールドへのインジェクション実行コールバック Action&lt;object, FieldInfo, QuillInjectionContext&gt;</param>
        public virtual void ForEach(object target, QuillInjectionContext context, IEnumerable<System.Reflection.FieldInfo> fields, 
            QuillInjector.CallbackInjectField callbackInjectField)
        {
            fields.AsParallel().ForAll(field => callbackInjectField(target, field, context));
        }
    }
}
