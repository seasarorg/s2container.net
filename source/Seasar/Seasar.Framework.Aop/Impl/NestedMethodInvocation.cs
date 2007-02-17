#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Seasar.Framework.Aop;

namespace Seasar.Framework.Aop.Impl
{
    /// <summary>
    /// NestedMethodInvocation
    /// </summary>
    public class NestedMethodInvocation : IS2MethodInvocation
    {
        private readonly IS2MethodInvocation parent;

        private readonly IMethodInterceptor[] interceptors;
        private int interceptorIndex = 0;

        public NestedMethodInvocation(IS2MethodInvocation parent,
            IMethodInterceptor[] interceptors)
        {
            this.parent = parent;
            this.interceptors = interceptors;
        }

        #region IS2MethodInvocationÉÅÉìÉo

        public Object[] Arguments
        {
            get
            {
                return parent.Arguments;
            }
        }

        public MethodBase Method
        {
            get
            {
                return parent.Method;
            }
        }

        public Object Target
        {
            get
            {
                return parent.Target;
            }
        }

        public Type TargetType
        {
            get
            {
                return parent.TargetType;
            }
        }

        public Object Proceed()
        {
            if (interceptorIndex < interceptors.Length)
            {
                return interceptors[interceptorIndex++].Invoke(this);
            }
            return parent.Proceed();
        }

        public Object GetParameter(String name)
        {
            return parent.GetParameter(name);
        }

        #endregion
    }
}