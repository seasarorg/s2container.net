using Castle.DynamicProxy;
using System.Collections.Generic;

namespace Seasar.Quill.AOP.Impl.Castle
{
    /// <summary>
    /// Castle.DynamicProxy用MethodInterceptorアダプタクラス
    /// </summary>
    public class InterceptorAdapter : IInterceptor
    {
        private readonly IList<IMethodInterceptor> _interceptors;
        private readonly MethodInvocationAdapter _adapter = new MethodInvocationAdapter();

        public InterceptorAdapter(IList<IMethodInterceptor> interceptors)
        {
            _interceptors = interceptors;
        }

        public void Intercept(IInvocation invocation)
        {
            if (_interceptors != null && _interceptors.Count > 0)
            {
                _adapter.Invocation = invocation;
                foreach(var interceptor in _interceptors)
                {
                    invocation.ReturnValue = interceptor.Invoke(_adapter);
                }
            }
        }
    }
}
