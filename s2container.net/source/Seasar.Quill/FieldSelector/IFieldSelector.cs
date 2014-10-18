﻿using System.Collections.Generic;
using System.Reflection;

namespace Seasar.Quill.FieldSelector
{
    public interface IFieldSelector
    {
        IEnumerable<FieldInfo> Select(object target, QuillInjectionContext context);
    }
}
