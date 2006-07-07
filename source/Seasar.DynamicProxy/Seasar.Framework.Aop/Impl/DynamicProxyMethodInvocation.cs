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
    /// <version>1.3 2006/05/23</version>
    ///
    public class DynamicProxyMethodInvocation : IS2MethodInvocation
    {
        #region fields

        private Object target;
        private Type targetType;
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
        /// <param name="target">対象のオブジェクトをセット</param>
        /// <param name="targetType">対象の型をセット</param>
        /// <param name="invocation">IInvocationインタフェースをセット</param>
        /// <param name="interceptors">インターセプタの配列をセット</param>
        public DynamicProxyMethodInvocation(object target
                                            , Type targetType
                                            , IInvocation invocation
                                            , object[] arguments
                                            , IMethodInterceptor[] interceptors
                                            , Hashtable parameters)
        {
            if (target == null) throw new NullReferenceException("target");
            if (targetType == null) throw new NullReferenceException("target");
            if (invocation == null) throw new NullReferenceException("invocation");
            if (interceptors == null) throw new NullReferenceException("interceptors");
            this.target = target;
            this.targetType = targetType;
            this.invocation = invocation;
            this.arguments = arguments;
            this.interceptors = interceptors;
            this.parameters = parameters;
        }

        #endregion

        #region IMethodInvocation member

        public MethodBase Method
        {
            get { return this.invocation.Method; }
        }

        public Object Target
        {
            get { return this.target; }
        }

        public Object[] Arguments
        {
            get { return this.arguments; }
        }

        public Object Proceed()
        {
            while (interceptorsIndex < interceptors.Length)
            {
                return interceptors[interceptorsIndex++].Invoke(this);
            }
            return this.invocation.Proceed(arguments);
        }

        #endregion

        #region IS2MethodInvocation メンバ

        public Type TargetType
        {
            get { return this.targetType; }
        }

        public object GetParameter(string name)
        {
            return this.parameters[name];
        }

        #endregion
    }
}
