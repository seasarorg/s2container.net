using System;

namespace Seasar.Quill.Preset.Scope
{
    public interface IInstanceFactory
    {
        object CreateInstance(Type targetType);
    }
}
