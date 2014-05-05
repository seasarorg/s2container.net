using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Container
{
    public interface IQuillContainer
    {
        void SetInstanceManager(IInstanceManager manager);
        void SetImplTypeDetector(params IImplTypeDetector[] detectors);
        T GetComponent<T>();
        IF GetComponent<IF, IMPL>() where IMPL : IF;
        object GetComponent(Type t);
        object GetComponent(Type i, Type t);
    }
}
