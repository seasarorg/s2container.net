using System;

namespace Seasar.Quill.Custom
{
    public interface IInstanceFactory : IDisposable
    {
        object GetInstance(Type targetType);
    }
}
