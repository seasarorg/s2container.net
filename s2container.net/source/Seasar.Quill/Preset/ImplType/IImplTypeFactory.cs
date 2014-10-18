using System;

namespace Seasar.Quill.Preset.Factory
{
    public interface IImplTypeFactory
    {
        Type GetImplType(Type receiptType);
    }
}
