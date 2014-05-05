using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seasar.Quill.AOP.Aspect
{
    /// <summary>
    /// Interceptor構築インターフェース
    /// </summary>
    public interface IInterceptorBuilder
    {
        IList<IMethodInterceptor> Build(Type t);
    }
}
