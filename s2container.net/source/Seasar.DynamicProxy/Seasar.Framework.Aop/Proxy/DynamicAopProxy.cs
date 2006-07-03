#region using directives

using System;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

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
    /// <remarks>edited Kazuya Sugimoto</remarks>
    /// <version>1.7.1 2006/07/03</version>
    ///
    [Serializable]
    public class DynamicAopProxy : IInterceptor
    {
        #region fields

        private ProxyGenerator generator;
        private object target;
        private IAspect[] aspects;
        private Hashtable interceptors = new Hashtable();
        private Type type;
        private Type enhancedType;
        private Hashtable parameters;

        #endregion
        #region constructors

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectが適用される型</param>
        public DynamicAopProxy(Type type)
            : this(type, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectが適用される型</param>
        /// <param name="aspects">適用するAspectの配列</param>
        public DynamicAopProxy(Type type, IAspect[] aspects)
            : this(type, aspects, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectが適用される型</param>
        /// <param name="aspects">適用するAspectの配列</param>
        /// <param name="parameters">パラメータ</param>
        public DynamicAopProxy(Type type, IAspect[] aspects, Hashtable parameters)
            : this(type, aspects, parameters, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectが適用される型</param>
        /// <param name="aspects">適用するAspectの配列</param>
        /// <param name="parameters">パラメータ</param>
        /// <param name="target">Aspectが適用されるターゲット</param>
        public DynamicAopProxy(Type type, IAspect[] aspects, Hashtable parameters, object target)
        {
            this.type = type;
            this.target = target;
            if (this.target == null) this.target = new object();
            this.aspects = aspects;
            this.parameters = parameters;
            this.generator = new ProxyGenerator();

            if (this.type.IsInterface)
            {
                this.enhancedType = this.generator.ProxyBuilder.CreateInterfaceProxy(new Type[] { this.type }, this.target.GetType());
            }
            else
            {
                this.enhancedType = this.generator.ProxyBuilder.CreateClassProxy(this.type);
            }
            this.SetUpAspects();
        }

        #endregion

        #region properties

        /// <summary>
        /// Proxyにより拡張された型を取得するプロパティ
        /// </summary>
        /// <value>Type 拡張された型</value>
        public Type EnhancedType
        {
            get { return this.enhancedType; }
        }

        #endregion
        #region public method

        /// <summary>
        /// プロキシオブジェクトを生成します
        /// </summary>
        public object Create()
        {
            return Create(type, target);
        }

        public object Create(Type type, object target)
        {
            ArrayList args = new ArrayList();
            args.Add(this);
            if (this.type.IsInterface)
            {
                args.AddRange(new object[] { null });
            }
            if (type.IsInterface && this.target.GetType() != typeof(object))
            {
                return this.generator.CreateProxy(type, this, target);
            }
            else
            {
                return Activator.CreateInstance(this.enhancedType, args.ToArray());
            }
        }

        /// <summary>
        /// プロキシオブジェクトを生成します    
        /// </summary>
        /// <param name="argTypes">パラメタ型の配列</param>
        /// <param name="args">生成時のパラメタの配列</param>
        public object Create(Type[] argTypes, object[] args)
        {
            ArrayList newArgs = new ArrayList();
            newArgs.Add(this);
            newArgs.AddRange(args);
            return Activator.CreateInstance(this.enhancedType, newArgs.ToArray());
        }
        /// <summary>
        /// プロキシオブジェクトを生成します
        /// </summary>
        /// <param name="argTypes">パラメタ型の配列</param>
        /// <param name="args">生成時のパラメタの配列</param>
        /// <param name="targetType">ターゲットの型</param>
        public object Create(Type[] argTypes, object[] args, Type targetType)
        {
            if (this.type.IsInterface)
            {
                return this.generator.CreateProxy(targetType, this, args);
            }
            else
            {
                return this.generator.CreateClassProxy(targetType, this, args);
            }
        }

        #endregion

        #region IInterceptor member

        public object Intercept(IInvocation invocation, params object[] args)
        {
            object ret = null;
            if ((invocation.Proxy == invocation.InvocationTarget ||
                !(invocation.Method.IsVirtual && !invocation.Method.IsFinal)) &&
                this.interceptors.ContainsKey(invocation.Method.Name))
            {
                IMethodInterceptor[] interceptors = this.interceptors[invocation.Method.Name] as IMethodInterceptor[];
                IMethodInvocation mehotdInvocation =
                   new DynamicProxyMethodInvocation(invocation.InvocationTarget, this.type, invocation, args, interceptors, parameters);
                ret = interceptors[0].Invoke(mehotdInvocation);

            }
            else
            {
                ret = invocation.Proceed(args);
            }
            return ret;
        }

        #endregion

        #region private methods

        /// <summary>
        /// アスペクトをセットアップします
        /// </summary>
        private void SetUpAspects()
        {
            if (this.aspects != null)
            {
                MethodInfo[] methodInfos = this.type.GetMethods();
                foreach (MethodInfo method in methodInfos)
                {
                    if (method.IsVirtual || this.type.IsInterface)
                    {
                        ArrayList interceptorList = new ArrayList();
                        foreach (IAspect aspect in this.aspects)
                        {
                            IPointcut pointcut = aspect.Pointcut;
                            if (pointcut == null || pointcut.IsApplied(method))
                            {
                                interceptorList.Add(aspect.MethodInterceptor);
                            }
                        }
                        if (interceptorList.Count > 0)
                        {
                            IMethodInterceptor[] interceptors = (IMethodInterceptor[])
                            interceptorList.ToArray(typeof(IMethodInterceptor));
                            this.interceptors.Add(method.Name, interceptors);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
