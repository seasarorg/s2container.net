using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Container
{
    public interface IImplTypeDetector
    {
        Type GetImplType(Type baseType);
    }
}
