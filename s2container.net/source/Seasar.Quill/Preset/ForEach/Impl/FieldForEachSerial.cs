using System.Collections.Generic;

namespace Seasar.Quill.Preset.ForEach.Impl
{
    public class FieldForEachSerial : IFieldForEach
    {
        public void ForEach(object target, QuillInjectionContext context, IEnumerable<System.Reflection.FieldInfo> fields, QuillInjector.CallbackInjectField injectField)
        {
            foreach(var field in fields)
            {
                injectField(target, field, context);
            }
        }
    }
}
