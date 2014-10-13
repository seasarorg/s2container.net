using System;
using System.Collections.Generic;

namespace Seasar.Quill.Typical.ImplType.Impl
{
    public class ImplTypeFactories : IImplTypeFactory
    {
        private readonly IList<IImplTypeFactory> _factories = new List<IImplTypeFactory>();

        public Type GetImplType(Type targetType, IQuillContainerContext context)
        {
            foreach (var factory in _factories)
            {
                var implType = factory.GetImplType(targetType, context);
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
