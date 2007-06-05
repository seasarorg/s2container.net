using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Framework.Aop;

namespace Seasar.Quill.Examples
{
    public class HogeS2Interceptor : IMethodInterceptor
    {
        #region IMethodInterceptor ÉÅÉìÉo

        public object Invoke(IMethodInvocation invocation)
        {
            Console.WriteLine("HogeS2Interceptor");
            return null;
        }

        #endregion
    }
}
