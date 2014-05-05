using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seasar.Quill.AOP
{
    /// <summary>
    /// AOPアダプタファクトリインターフェース
    /// </summary>
    public interface IAOPAdapterFactory
    {
        object GetInterceptorAdapter();
        IMethodInvocation GetInvocationAdapter();
    }
}
