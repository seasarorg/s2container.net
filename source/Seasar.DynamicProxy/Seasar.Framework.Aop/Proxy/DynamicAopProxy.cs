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
    /// Castle.DynamicProxyを使用した、Aopにおけるメソッドの委譲をAspectとして実行するためのプロキシクラス
    /// </summary>
    /// <author>Kazz
    /// </author>
    /// <version>1.0 2006/04/18</version>
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
        /// <param name="type">Aspectされるターゲットの型のType</param>
        public DynamicAopProxy(Type type)
            :this(type, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectされるターゲットの型のType</param>
        /// <param name="aspects">適応するAspect</param>
        public DynamicAopProxy(Type type, IAspect[] aspects)
            : this(type, aspects, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectされるターゲットの型のType</param>
        /// <param name="aspects">適応するAspect</param>
        /// <param name="parameters">パラメータ</param>
        public DynamicAopProxy(Type type, IAspect[] aspects, Hashtable parameters)
            : this(type, aspects, parameters, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectされるターゲットの型のType</param>
        /// <param name="aspects">適応するAspect</param>
        /// <param name="parameters">パラメータ</param>
        /// <param name="target">Aspectされるターゲット</param>
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

        public object Create()
        {
            object result = null;
            if (this.type.IsInterface)
            {
                result = this.generator.CreateProxy(this.type, this, this.target);
            }
            else
            {
                result = this.generator.CreateClassProxy(this.type, this, new object[] { null });
            }
            return result;
        }

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
            Debug.WriteLine(target.ToString() + " invocation method = " + invocation.Method.Name);
            ArrayList interceptorList = new ArrayList();
            object ret = null;

            if (aspects != null)
            {
                // 定義されたAspectからInterceptorのリストの作成
                foreach (IAspect aspect in aspects)
                {
                    IPointcut pointcut = aspect.Pointcut;
                    // IPointcutよりAdvice(Interceptor)を挿入するか確認
                    if (pointcut == null || pointcut.IsApplied(invocation.Method))
                    {
                        // Aspectを適用する場合
                        interceptorList.Add(aspect.MethodInterceptor);
                    }
                }
            }

            if (interceptorList.Count == 0)
            {
                // Interceptorを挿入しない場合
                //ret = invocation.Method.Invoke(target, args);
                ret = invocation.Proceed(args);
            }
            else
            {
                // Interceptorを挿入する場合
                IMethodInterceptor[] interceptors = (IMethodInterceptor[])
                    interceptorList.ToArray(typeof(IMethodInterceptor));
                IMethodInvocation mehotdInvocation =
                    new DynamicProxyMethodInvocation(target, invocation, args, interceptors, parameters);
                ret = interceptors[0].Invoke(mehotdInvocation);
            }
            return ret;
        }

        #endregion
    }
}
