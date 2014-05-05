using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Container
{
    public interface IComponentCreater
    {
        bool IsTarget(Type t);
        object Create(Type t);
        object Create(Type i, Type impl);
    }
}
