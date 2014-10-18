using Seasar.Quill.Factory;
using System;
using System.Collections.Generic;

namespace Seasar.Quill.Factory.Impl
{
    public class ImplTypeFactories : IImplTypeFactory
    {
        private readonly IList<IImplTypeFactory> _factories = new List<IImplTypeFactory>();

        public Type GetImplType(Type targetType)
        {
            foreach (var factory in _factories)
            {
                var implType = factory.GetImplType(targetType);
                if (implType != null)
                {
                    return implType;
                }
            }
            return null;
        }

        public virtual void AddFactory(IImplTypeFactory factory)
        {
            _factories.Add(factory);
        }
    }
}
