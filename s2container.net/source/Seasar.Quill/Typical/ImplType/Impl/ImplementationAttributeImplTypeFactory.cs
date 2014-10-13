using Seasar.Quill.Attr;
using System;

namespace Seasar.Quill.Typical.ImplType.Impl
{
    public class ImplementationAttributeImplTypeFactory : IImplTypeFactory
    {
        public Type GetImplType(Type targetType, IQuillContainerContext context)
        {
            if (targetType.IsImplementationAttrAttached())
            {
                return targetType.GetImplType();
            }
            return null;
        }
    }
}
