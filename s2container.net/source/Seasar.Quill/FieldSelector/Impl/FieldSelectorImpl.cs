using Seasar.Quill.Attr;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Seasar.Quill.FieldSelector.Impl
{
    public class FieldSelectorImpl : IFieldSelector
    {
        public IEnumerable<System.Reflection.FieldInfo> Select(object target, QuillInjectionContext context)
        {
            var fields = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return fields.Where(fi => fi.FieldType.IsImplementationAttrAttached());
        }
    }
}
