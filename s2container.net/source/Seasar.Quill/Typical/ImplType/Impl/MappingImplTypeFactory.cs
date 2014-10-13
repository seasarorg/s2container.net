using System;
using System.Collections.Generic;

namespace Seasar.Quill.Typical.ImplType.Impl
{
    public class MappingImplTypeFactory : IImplTypeFactory
    {
        private readonly IDictionary<Type, Type> _implTypeMap;

        public MappingImplTypeFactory(IDictionary<Type, Type> implTypeMap)
        {
            if (implTypeMap == null) { throw new ArgumentNullException("implTypeMap"); }
            _implTypeMap = implTypeMap;
        }

        public Type GetImplType(Type targetType, IQuillContainerContext context)
        {
            if (_implTypeMap.ContainsKey(targetType))
            {
                return _implTypeMap[targetType];
            }
            return null;
        }
    }
}
