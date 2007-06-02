using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Framework.Aop;
using System.Reflection;

namespace Seasar.Quill.Examples
{
    public class ConsoleWriteInterceptor : IMethodInterceptor
    {
        #region IMethodInterceptor ÉÅÉìÉo

        public object Invoke(IMethodInvocation invocation)
        {
            MethodBase method = invocation.Method;
            Console.WriteLine("Start:" + method.DeclaringType.FullName
                + "#" + method.Name);

            object ret = null;

            if (!method.DeclaringType.IsInterface)
            {
                ret = invocation.Proceed();
            }

            Console.WriteLine("End  :" + method.DeclaringType.FullName
                + "#" + method.Name);

            return ret;
        }

        #endregion
    }
}
