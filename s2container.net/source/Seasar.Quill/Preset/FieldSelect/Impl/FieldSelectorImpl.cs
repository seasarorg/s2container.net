using Seasar.Quill.Attr;
using System.Collections.Generic;
using System.Linq;

namespace Seasar.Quill.Preset.FieldSelect.Impl
{
    public class FieldSelectorImpl : IFieldSelector
    {
        public IEnumerable<System.Reflection.FieldInfo> Select(object target, QuillInjectionContext context)
        {
            var fields = target.GetType().GetFields(context.Condition);
            return fields.Where(fi => fi.FieldType.IsImplementationAttrAttached());
        }
    }
}
