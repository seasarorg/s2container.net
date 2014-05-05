using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Container
{
    public interface IInstanceManager
    {
        void SetComponentCreator(params IComponentCreater[] creator);
        object GetInstance(Type t);
        object GetInstance(Type i, Type impl);
    }
}
