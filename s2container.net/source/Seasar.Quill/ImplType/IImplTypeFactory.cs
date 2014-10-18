using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Factory
{
    public interface IImplTypeFactory
    {
        Type GetImplType(Type receiptType);
    }
}
