#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Util;

namespace Seasar.Framework.Aop.Interceptors
{
    /// <summary>
    /// Interceptor collection
    /// </summary>
    public class InterceptorChain : AbstractInterceptor
    {
        private IMethodInterceptor[] _interceptors = new IMethodInterceptor[0];

        /// <summary>
        /// Add an interceptor
        /// </summary>
        /// <param name="interceptor"></param>
        public void Add(IMethodInterceptor interceptor)
        {
            _interceptors = (IMethodInterceptor[]) ArrayUtil.Add(
                _interceptors, interceptor);
        }

        /// <summary>
        /// Execute registed interceptors
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public override object Invoke(IMethodInvocation invocation)
        {
            IMethodInvocation nestInvocation = new NestedMethodInvocation(
                (IS2MethodInvocation) invocation, _interceptors);
            return nestInvocation.Proceed();
        }
    }
}
