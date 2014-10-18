using System;

namespace Seasar.Quill.Factory
{
    public interface IImplTypeFactory
    {
        Type GetImplType(Type receiptType);
    }
}
