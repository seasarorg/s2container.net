using System;

namespace Seasar.Quill.Custom
{
    public interface IImplTypeFactory
    {
        Type GetImplType(Type receiptType);
    }
}
