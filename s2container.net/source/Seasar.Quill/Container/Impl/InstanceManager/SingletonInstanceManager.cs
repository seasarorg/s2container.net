using System;
using System.Collections.Generic;

namespace Seasar.Quill.Container.Impl.InstanceManager
{
    public class SingletonInstanceManager : AbstractInstanceManager
    {
        private readonly IDictionary<Type, object> _instanceHolder = new Dictionary<Type, object>();

        protected override object GetInstance(Type t, Func<IComponentCreater, object> createInvoker)
        {
            if (!_instanceHolder.ContainsKey(t))
            {
                _instanceHolder.Add(t, base.GetInstance(t, createInvoker));
            }
            return _instanceHolder[t];
        }
    }
}
