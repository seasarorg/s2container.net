#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
            var component = _container.GetComponent(componentName);
            Type type = null;
            try
            {
                if (RemotingServices.IsTransparentProxy(component))
                {
                    var aopProxy = RemotingServices.GetRealProxy(component) as AopProxy;
                    type = aopProxy?.TargetType;
                }
                else
                {
                    type = component.GetExType();
                }

                var methodInfo = type?.GetMethod(methodName);

                if (methodInfo == null)
                {
                    throw new MissingMethodException();
                }

                var parametersSize = methodInfo.GetParameters().Length;

                if ((parametersSize > 0 && args == null) ||
                     (args != null && parametersSize != args.Length))
                {
                    throw new IllegalMethodRuntimeException(type, methodName, null);
                }

                if (type.IsMarshalByRef)
                {
                    return MethodUtil.Invoke(methodInfo, component, args);
                }
                else
                {
                    var componentDef = _container.GetComponentDef(componentName);
                    return InvokeNonMarshalByRefObject(component,
                        _GetAspects(componentDef), methodInfo, args);
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

            var interceptorList = new ArrayList();

            if (aspects != null)
            {
                // ��`���ꂽAspect����Interceptor�̃��X�g�̍쐬
                foreach (var aspect in aspects)
                {
                    var pointcut = aspect.Pointcut;
                    // IPointcut���Advice(Interceptor)��}�����邩�m�F
                    if (pointcut == null || pointcut.IsApplied(method))
                    {
                        // Aspect��K�p����ꍇ
                        interceptorList.Add(aspect.MethodInterceptor);
                    }
                }
            }

            object ret;
            if (interceptorList.Count == 0)
            {
                // Interceptor��}�����Ȃ��ꍇ
                ret = MethodUtil.Invoke((MethodInfo)method, target, args);
//                ret = method.Invoke(target, args);
            }
            else
            {
                // Interceptor��}������ꍇ
                var interceptors = (IMethodInterceptor[])
                    interceptorList.ToArray(typeof(IMethodInterceptor));

                IMethodInvocation invocation = new MethodInvocationImpl(target,
                    method, args, interceptors, new Hashtable());

                ret = interceptors[0].Invoke(invocation);

            }
            return ret;
        }

        private static IAspect[] _GetAspects(IComponentDef componentDef)
        {
            var size = componentDef.AspectDefSize;
            var aspects = new IAspect[size];
            for (var i = 0; i < size; ++i)
            {
                aspects[i] = componentDef.GetAspectDef(i).Aspect;
            }
            return aspects;
        }
    }
}
