#region using directives

using System;
using System.Text;
using System.Reflection;
using System.Collections;

using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Interceptors;

using Castle.DynamicProxy;

#endregion

namespace Seasar.Framework.Aop.Impl
{
    /// <summary>
    /// 複数のAdvice(Interceptor)によるチェーンを抽象化したインタフェースの実装クラスです
    /// </summary>
    /// <author>Kazz</author>
    /// <version>1.0 2006/04/18</version>
    ///
    public class DynamicProxyMethodInvocation : IMethodInvocation
    {
        #region fields

        private Object target;
        private IInvocation invocation;
        private IMethodInterceptor[] interceptors;
        private int interceptorsIndex = 1;
        private Object[] arguments;
        private Hashtable parameters;

        #endregion
        #region constructors

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">Interceptされるオブジェクト</param>
        /// <param name="invocation">InterceptされるIInvocationインタフェース</param>
        /// <param name="interceptors">メソッドをInterceptするInterceptor</param>
        public DynamicProxyMethodInvocation(object target, IInvocation invocation
                                            , object[] arguments
                                            , IMethodInterceptor[] interceptors
                                            , Hashtable parameters)
        {
            if(target==null) throw new NullReferenceException("target");
            if (invocation == null) throw new NullReferenceException("invocation");
            if(interceptors==null) throw new NullReferenceException("interceptors");
            this.target = target;
            this.invocation = invocation;
            this.arguments = arguments;
            this.interceptors = interceptors;
            this.parameters = parameters;
        }

        #endregion
        #region IMethodInvocation member

        public MethodBase Method
        {
            get    { return this.invocation.Method ;}
        }

        public Object Target
        {
            get    { return this.target; }
        }
        public Type TargetType
        {
            get { return this.target.GetType(); }
        }
        public Object[] Arguments
        {
            get    { return this.arguments;}
        }

        public Object Proceed()
        {
            while(interceptorsIndex < interceptors.Length)
            {
                // 他にInterceptorがあれば、Interceptorを呼び出す
                return interceptors[interceptorsIndex++].Invoke(this);
            }
            // Interceptされたメソッドを実行する
            return this.invocation.Proceed(arguments);
        }

        public object GetParameter(string name)
        {
            return this.parameters[name];
        }

        #endregion
    }
}
