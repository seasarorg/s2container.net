using Seasar.Quill.Attr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Container.Impl.Detector
{
    public class AttributeImplTypeDetector : IImplTypeDetector
    {
        public virtual Type GetImplType(Type currentType)
        {
            ImplementationAttribute implAttr = GetImplementationAttribute(currentType);
            if (implAttr != null)
            {
                return implAttr.ImplType;
            }

            // 実装クラスが見つからない場合はnullを返す
            return null;
        }

        protected virtual ImplementationAttribute GetImplementationAttribute(Type baseType)
        {
            object[] attrs = baseType.GetCustomAttributes(typeof(ImplementationAttribute), false);
            if (attrs == null || attrs.Length == 0)
            {
                return null;
            }
            return (ImplementationAttribute)attrs[0];
        }
    }
}
