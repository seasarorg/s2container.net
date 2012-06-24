#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Beans;
using Seasar.Framework.Container;
using Seasar.Framework.Util;

namespace Seasar.Extension.Component.Impl
{
    public class ComponentInvoker : IComponentInvoker
    {
        private IS2Container _container;

        public IS2Container Container
        {
            set { _container = value; }
        }

        public object Invoke(string componentName, string methodName, object[] args)
        {
            object component = _container.GetComponent(componentName);
            Type type = null;
            try
            {
                if (RemotingServices.IsTransparentProxy(component))
                {
                    AopProxy aopProxy = RemotingServices.GetRealProxy(component) as AopProxy;
                    type = aopProxy.TargetType;
                }
                else
                {
                    type = component.GetType();
                }

                MethodInfo methodInfo = type.GetMethod(methodName);

                if (methodInfo == null)
                {
                    throw new MissingMethodException();
                }

                int ParametersSize = methodInfo.GetParameters().Length;

                if ((ParametersSize > 0 && args == null) ||
                     (args != null && ParametersSize != args.Length))
                {
                    throw new IllegalMethodRuntimeException(type, methodName, null);
                }

                if (type.IsMarshalByRef)
                {
                    return MethodUtil.Invoke(methodInfo, component, args);
                }
                else
                {
                    IComponentDef componentDef = _container.GetComponentDef(componentName);
                    return InvokeNonMarshalByRefObject(component,
                        GetAspects(componentDef), methodInfo, args);
                }
            }

            catch (MissingMethodException)
            {
                throw new MethodNotFoundRuntimeException(type, methodName, args);
            }
            catch (Exception ex)
            {
                throw new IllegalMethodRuntimeException(type, methodName, ex);
            }
        }

        private object InvokeNonMarshalByRefObject(
            object target, IAspect[] aspects, MethodBase method, object[] args)
        {

            ArrayList interceptorList = new ArrayList();

            if (aspects != null)
            {
                // 定義されたAspectからInterceptorのリストの作成
                foreach (IAspect aspect in aspects)
                {
                    IPointcut pointcut = aspect.Pointcut;
                    // IPointcutよりAdvice(Interceptor)を挿入するか確認
                    if (pointcut == null || pointcut.IsApplied(method))
                    {
                        // Aspectを適用する場合
                        interceptorList.Add(aspect.MethodInterceptor);
                    }
                }
            }

            object ret;
            if (interceptorList.Count == 0)
            {
                // Interceptorを挿入しない場合
                ret = method.Invoke(target, args);
            }
            else
            {
                // Interceptorを挿入する場合
                IMethodInterceptor[] interceptors = (IMethodInterceptor[])
                    interceptorList.ToArray(typeof(IMethodInterceptor));

                IMethodInvocation invocation = new MethodInvocationImpl(target,
                    method, args, interceptors, new Hashtable());

                ret = interceptors[0].Invoke(invocation);

            }
            return ret;
        }

        private static IAspect[] GetAspects(IComponentDef componentDef)
        {
            int size = componentDef.AspectDefSize;
            IAspect[] aspects = new IAspect[size];
            for (int i = 0; i < size; ++i)
            {
                aspects[i] = componentDef.GetAspectDef(i).Aspect;
            }
            return aspects;
        }
    }
}
