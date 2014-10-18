using System.Collections.Generic;

namespace Seasar.Quill.Preset.FieldSelect.Impl
{
    public class FieldSelectorImpl : IFieldSelector
    {
        public IEnumerable<System.Reflection.FieldInfo> Select(object target, QuillInjectionContext context)
        {
            return target.GetType().GetFields(context.Condition);
        }
    }
}
