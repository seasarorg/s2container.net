using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Attr
{
    public class ImplementationAttribute : Attribute
    {
        private readonly Type _implType;

        public Type ImplType
        {
            get
            {
                return _implType;
            }
        }

        public ImplementationAttribute(Type implType)
        {
            _implType = implType;
        }
    }
}