using System.Reflection;

namespace Seasar.Quill.Injection
{
    public interface IFieldInjector
    {
        void InjectField(object target, FieldInfo fieldInfo, QuillInjectionContext context);
    }
}
