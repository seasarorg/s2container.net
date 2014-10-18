using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Injection.Impl
{
    public class FieldInjectorImpl : IFieldInjector
    {
        public void InjectField(object target, System.Reflection.FieldInfo fieldInfo, QuillInjectionContext context)
        {
            fieldInfo.SetValue(target, context.Container.GetComponent(fieldInfo.FieldType));
        }
    }
}
