using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seasar.Quill.AOP.Adapter.Castle
{
    public class CastleDynamicProxyAdapterFactory : IAOPAdapterFactory
    {
        public object GetInterceptorAdapter()
        {
            throw new NotImplementedException();
        }

        public IMethodInvocation GetInvocationAdapter()
        {
            throw new NotImplementedException();
        }
    }
}
