using System.Collections.Generic;
using System.Linq;

namespace Seasar.Quill.Preset.ForEach.Impl
{
    public class FieldForEachParallel : IFieldForEach
    {
        /// <summary>
        /// ループ処理を並列で実行
        /// </summary>
        /// <param name="target"></param>
        /// <param name="context"></param>
        /// <param name="fields"></param>
        /// <param name="callbackInjectField">フィールドインジェクション処理デリゲート（引数：object/fieldInfo/context, 戻り値なし）</param>
        public void ForEach(object target, QuillInjectionContext context, IEnumerable<System.Reflection.FieldInfo> fields, 
            QuillInjector.CallbackInjectField callbackInjectField)
        {
            fields.AsParallel().ForAll(field => callbackInjectField(target, field, context));
        }
    }
}
