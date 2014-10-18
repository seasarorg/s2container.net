using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Scope
{
    public interface IInstanceFactory
    {
        object CreateInstance(Type targetType);
    }
}
