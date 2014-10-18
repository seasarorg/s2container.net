using System.Collections.Generic;
using System.Reflection;

namespace Seasar.Quill.FieldSelect
{
    public interface IFieldSelector
    {
        IEnumerable<FieldInfo> Select(object target, QuillInjectionContext context);
    }
}
