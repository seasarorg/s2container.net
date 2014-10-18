using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Scope.Impl
{
    public class PrototypeFactory : IInstanceFactory
    {
        public object CreateInstance(Type targetType)
        {
            return Activator.CreateInstance(targetType);
        }
    }
}
