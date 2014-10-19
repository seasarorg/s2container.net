using System.Reflection;

namespace Seasar.Quill.Custom
{
    public interface IFieldInjector
    {
        void InjectField(object target, FieldInfo fieldInfo, QuillInjectionContext context);
    }
}
