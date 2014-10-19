using System;

namespace Seasar.Quill.Custom
{
    public interface IInstanceFactory
    {
        object CreateInstance(Type targetType);
    }
}
