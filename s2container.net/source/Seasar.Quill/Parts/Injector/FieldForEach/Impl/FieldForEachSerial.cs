using System.Collections.Generic;

namespace Seasar.Quill.Parts.Injector.FieldForEach.Impl
{
    /// <summary>
    /// 抽出したフィールドを直列でループして処理するクラス
    /// </summary>
    public class FieldForEachSerial : IFieldForEach
    {
        /// <summary>
        /// ループ処理実行
        /// </summary>
        /// <param name="target">インジェクション対象オブジェクト</param>
        /// <param name="context">インジェクション状態管理</param>
        /// <param name="fields">インジェクション先として抽出したフィールドのコレクション</param>
        /// <param name="injectField">フィールドへのインジェクション実行コールバック Action&lt;object, FieldInfo, QuillInjectionContext&gt;</param>
        public virtual void ForEach(object target, QuillInjectionContext context, 
            IEnumerable<System.Reflection.FieldInfo> fields, QuillInjector.CallbackInjectField injectField)
        {
            foreach(var field in fields)
            {
                injectField(target, field, context);
            }
        }
    }
}
