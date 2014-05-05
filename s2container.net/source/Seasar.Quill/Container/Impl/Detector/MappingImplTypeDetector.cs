using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Container.Impl.Detector
{
    public class MappingImplTypeDetector : IImplTypeDetector
    {
        private readonly IDictionary<Type, Type> _typeMap;

        public MappingImplTypeDetector(IDictionary<Type, Type> typeMap)
        {
            _typeMap = typeMap;
        }

        public virtual Type GetImplType(Type baseType)
        {
            if (_typeMap.ContainsKey(baseType)) 
            {
                return _typeMap[baseType];
            }
            return null;
        }
    }
}
