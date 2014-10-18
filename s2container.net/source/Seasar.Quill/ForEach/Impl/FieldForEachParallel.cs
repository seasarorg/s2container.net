using System.Collections.Generic;
using System.Linq;

namespace Seasar.Quill.ForEach.Impl
{
    public class FieldForEachParallel : IFieldForEach
    {
        public void ForEach(object target, QuillInjectionContext context, IEnumerable<System.Reflection.FieldInfo> fields, QuillInjector.CallbackInjectField injectField)
        {
            fields.AsParallel().ForAll(field => injectField(target, field, context));
        }
    }
}
