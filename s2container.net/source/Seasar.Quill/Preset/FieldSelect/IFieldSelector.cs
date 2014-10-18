using System.Collections.Generic;
using System.Reflection;

namespace Seasar.Quill.Preset.FieldSelect
{
    public interface IFieldSelector
    {
        IEnumerable<FieldInfo> Select(object target, QuillInjectionContext context);
    }
}
