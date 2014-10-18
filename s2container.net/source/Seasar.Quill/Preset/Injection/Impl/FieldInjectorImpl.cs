
namespace Seasar.Quill.Preset.Injection.Impl
{
    public class FieldInjectorImpl : IFieldInjector
    {
        public void InjectField(object target, System.Reflection.FieldInfo fieldInfo, QuillInjectionContext context)
        {
            fieldInfo.SetValue(target, context.Container.GetComponent(fieldInfo.FieldType));
        }
    }
}
