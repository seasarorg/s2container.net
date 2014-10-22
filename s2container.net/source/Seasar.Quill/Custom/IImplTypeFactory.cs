using System;

namespace Seasar.Quill.Custom
{
    public interface IImplTypeFactory : IDisposable
    {
        Type GetImplType(Type receiptType);
    }
}
