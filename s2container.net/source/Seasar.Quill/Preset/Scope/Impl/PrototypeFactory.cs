using System;

namespace Seasar.Quill.Preset.Scope.Impl
{
    public class PrototypeFactory : IInstanceFactory
    {
        public object CreateInstance(Type targetType)
        {
            return Activator.CreateInstance(targetType);
        }
    }
}
