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
	public class NestedMethodInvocation : IS2MethodInvocation{
        private readonly IS2MethodInvocation parent;

        private readonly IMethodInterceptor[] interceptors;
        private int interceptorIndex = 0;

        public NestedMethodInvocation(IS2MethodInvocation parent,
            IMethodInterceptor[] interceptors) {
            this.parent = parent;
            this.interceptors = interceptors;
        }

        #region IS2MethodInvocationÉÅÉìÉo

        public Object[] Arguments {
            get {
                return parent.Arguments;
            }
        }

        public MethodBase Method {
            get {
                return parent.Method;
            }
        }

        public Object Target {
            get {
                return parent.Target;
            }
        }

        public Type TargetType {
            get {
                return parent.TargetType;
            }
        }

        public Object Proceed() {
            if (interceptorIndex < interceptors.Length ) {
                return interceptors[interceptorIndex++].Invoke(this);
            }
            return parent.Proceed();
        }

        public Object GetParameter(String name) {
            return parent.GetParameter(name);
        }

        #endregion
    }
}