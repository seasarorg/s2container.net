#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using System.Reflection;

namespace Seasar.Framework.Aop.Impl
{
    public class NestedMethodInvocation : IS2MethodInvocation
    {
        private readonly IS2MethodInvocation _parent;
        private readonly IMethodInterceptor[] _interceptors;
        private int _interceptorIndex = 0;

        public NestedMethodInvocation(IS2MethodInvocation parent,
            IMethodInterceptor[] interceptors)
        {
            _parent = parent;
            _interceptors = interceptors;
        }

        #region IS2MethodInvocationÉÅÉìÉo

        public object[] Arguments
        {
            get { return _parent.Arguments; }
        }

        public MethodBase Method
        {
            get { return _parent.Method; }
        }

        public object Target
        {
            get { return _parent.Target; }
        }

        public Type TargetType
        {
            get { return _parent.TargetType; }
        }

        public object Proceed()
        {
            if (_interceptorIndex < _interceptors.Length)
            {
                return _interceptors[_interceptorIndex++].Invoke(this);
            }
            return _parent.Proceed();
        }

        public Object GetParameter(string name)
        {
            return _parent.GetParameter(name);
        }

        #endregion
    }
}