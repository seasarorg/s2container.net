using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seasar.Quill.FieldSelector
{
    public interface IFieldSelector
    {
        IEnumerable<FieldInfo> Select(object target, QuillInjectionContext context);
    }
}
