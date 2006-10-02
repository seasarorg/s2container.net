using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Util;

namespace Seasar.Framework.Aop.Interceptors
{
    /// <summary>
    /// Interceptor collection
    /// </summary>
	public class InterceptorChain : AbstractInterceptor{
        private IMethodInterceptor[] interceptors = new IMethodInterceptor[0];

        /// <summary>
        /// Add an interceptor
        /// </summary>
        /// <param name="interceptor"></param>
        public void Add(IMethodInterceptor interceptor) {
            interceptors = (IMethodInterceptor[])ArrayUtil.Add(
                interceptors, interceptor);
        }

        /// <summary>
        /// Execute registed interceptors
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public override Object Invoke(IMethodInvocation invocation) {
            IMethodInvocation nestInvocation = new NestedMethodInvocation(
                (IS2MethodInvocation)invocation, interceptors);
            return nestInvocation.Proceed();
        }
	}
}