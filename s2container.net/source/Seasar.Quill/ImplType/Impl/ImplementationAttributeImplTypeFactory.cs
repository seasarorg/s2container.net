using Seasar.Quill.Attr;
using System;

namespace Seasar.Quill.Factory.Impl
{
    public class ImplementationAttributeImplTypeFactory : IImplTypeFactory
    {
        public Type GetImplType(Type targetType)
        {
            if (targetType.IsImplementationAttrAttached())
            {
                return targetType.GetImplType();
            }
            return null;
        }
    }
}
