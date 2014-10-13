using System;

namespace Seasar.Quill.Typical.ImplType
{
    public interface IImplTypeFactory
    {
        Type GetImplType(Type targetType);
    }
}
