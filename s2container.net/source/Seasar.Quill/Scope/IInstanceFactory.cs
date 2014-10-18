using System;

namespace Seasar.Quill.Scope
{
    public interface IInstanceFactory
    {
        object CreateInstance(Type targetType);
    }
}
