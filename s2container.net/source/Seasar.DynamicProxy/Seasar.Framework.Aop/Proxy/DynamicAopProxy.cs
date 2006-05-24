#region using directives

using System;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.Collections;

using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Util;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Framework.Container.Util;

using Castle.DynamicProxy;

#endregion

namespace Seasar.Framework.Aop.Proxy
{
    /// <summary>
    /// Castle.DynamicProxyを使用した、Aspect実行のためのプロキシクラス
    /// </summary>
    /// <author>Kazz
    /// </author>
    /// <version>1.3 2006/05/23</version>
    ///
    [Serializable]
    public class DynamicAopProxy : IInterceptor
    {
        #region fields

        private ProxyGenerator generator;

        private object target;
        private IAspect[] aspects;
        private Type type;
        private Hashtable parameters;

        #endregion
        #region constructors

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectが摘要される型</param>
        public DynamicAopProxy(Type type)
            : this(type, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectが摘要される型</param>
        /// <param name="aspects">摘要するAspectの配列</param>
        public DynamicAopProxy(Type type, IAspect[] aspects)
            : this(type, aspects, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectが摘要される型</param>
        /// <param name="aspects">摘要するAspectの配列</param>
        /// <param name="parameters">パラメータ</param>
        public DynamicAopProxy(Type type, IAspect[] aspects, Hashtable parameters)
            : this(type, aspects, parameters, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectが摘要される型</param>
        /// <param name="aspects">摘要するAspectの配列</param>
        /// <param name="parameters">パラメータ</param>
        /// <param name="target">Aspectが摘要されるターゲット</param>
        public DynamicAopProxy(Type type, IAspect[] aspects, Hashtable parameters, object target)
        {
            this.type = type;
            this.target = target;
            if (this.target == null) this.target = new object();
            this.aspects = aspects;
            this.parameters = parameters;
            this.generator = new ProxyGenerator();
        }

        #endregion
        #region public method

        /// <summary>
        /// プロキシオブジェクトを生成します
        /// </summary>
        public object Create()
        {
            object result = null;
            if (this.type.IsInterface)
            {
                result = this.generator.CreateProxy(this.type, this, this.target);
            }
            else
            {
                result = this.generator.CreateClassProxy(this.type, this, new object[] { });
                
            }
            return result;
        }

        /// <summary>
        /// プロキシオブジェクトを生成します    
        /// </summary>
        /// <param name="argTypes">パラメタ型の配列</param>
        /// <param name="args">生成時のパラメタの配列</param>
        public object Create(Type[] argTypes, object[] args)
        {
            object result = null;
            if (this.type.IsInterface)
            {
                result = this.generator.CreateProxy(this.type, this, args);
            }
            else
            {
                result = this.generator.CreateClassProxy(this.type, this, args);
            }
            return result;
        }
        /// <summary>
        /// プロキシオブジェクトを生成します
        /// </summary>
        /// <param name="argTypes">パラメタ型の配列</param>
        /// <param name="args">生成時のパラメタの配列</param>
        /// <param name="targetType">ターゲットの型</param>
        public object Create(Type[] argTypes, object[] args, Type targetType)
        {
            object result = null;
            if (this.type.IsInterface)
            {
                result = this.generator.CreateProxy(targetType, this, args);
            }
            else
            {
                result = this.generator.CreateClassProxy(targetType, this, args);
            }
            return result;
        }

        #endregion
        #region IInterceptor member

        public object Intercept(IInvocation invocation, params object[] args)
        {
            ArrayList interceptorList = new ArrayList();
            object ret = null;

            if (aspects != null)
            {
                foreach (IAspect aspect in aspects)
                {
                    IPointcut pointcut = aspect.Pointcut;
                    if (pointcut == null || pointcut.IsApplied(invocation.Method))
                    {
                        interceptorList.Add(aspect.MethodInterceptor);
                    }
                }
            }
            if (interceptorList.Count == 0)
            {
                ret = invocation.Proceed(args);
            }
            else
            {
                IMethodInterceptor[] interceptors = (IMethodInterceptor[])
                    interceptorList.ToArray(typeof(IMethodInterceptor));
                IMethodInvocation mehotdInvocation =
                    new DynamicProxyMethodInvocation(
                        invocation.InvocationTarget, this.type, invocation, args, interceptors, parameters);
                ret = interceptors[0].Invoke(mehotdInvocation);
            }
            return ret;
        }

        #endregion
    }
}
