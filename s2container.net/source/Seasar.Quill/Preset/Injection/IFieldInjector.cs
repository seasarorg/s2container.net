using System.Reflection;

namespace Seasar.Quill.Preset.Injection
{
    public interface IFieldInjector
    {
        void InjectField(object target, FieldInfo fieldInfo, QuillInjectionContext context);
    }
}
