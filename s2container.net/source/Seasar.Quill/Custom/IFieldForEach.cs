using System.Collections.Generic;
using System.Reflection;

namespace Seasar.Quill.Custom
{
    public interface IFieldForEach
    {
        void ForEach(object target, QuillInjectionContext context, IEnumerable<FieldInfo> fields,
            Seasar.Quill.QuillInjector.CallbackInjectField injectField);
    }
}
