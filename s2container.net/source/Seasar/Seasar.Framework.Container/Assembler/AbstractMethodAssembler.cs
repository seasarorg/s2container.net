#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using Seasar.Framework.Util;
using Seasar.Framework.Beans;
using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Assembler
{
    public abstract class AbstractMethodAssembler : AbstractAssembler, IMethodAssembler
    {
        public AbstractMethodAssembler(IComponentDef componentDef)
            : base(componentDef)
        {
        }

        protected void Invoke(Type type, object component, IMethodDef methodDef)
        {
            string expression = methodDef.Expression;
            string methodName = methodDef.MethodName;
            if (methodName != null)
            {
                object[] args = new object[0];
                MethodInfo method = null;
                try
                {
                    if (methodDef.ArgDefSize > 0)
                    {
                        args = methodDef.Args;
                    }
                    else
                    {
                        MethodInfo[] methods = type.GetMethods();
                        method = GetSuitableMethod(methods, methodName);
                        if (method != null)
                        {
                            ParameterInfo[] parameters = method.GetParameters();
                            Type[] argTypes = new Type[parameters.Length];
                            for (int i = 0; i < parameters.Length; ++i)
                            {
                                argTypes[i] = parameters[i].ParameterType;
                            }
                            args = GetArgs(argTypes);
                        }
                    }
                }
                catch (ComponentNotFoundRuntimeException cause)
                {
                    throw new IllegalMethodRuntimeException(
                        GetComponentType(component), methodName, cause);
                }
                if (method != null)
                {
                    MethodUtil.Invoke(method, component, args);
                }
                else
                {
                    Invoke(type, component, methodName, args);
                }
            }
            else
            {
                InvokeExpression(component, expression);
            }
        }

        private void InvokeExpression(object component, string expression)
        {
            Hashtable ctx = new Hashtable();
            ctx["self"] = component;
            ctx["out"] = Console.Out;
            ctx["err"] = Console.Error;
            JScriptUtil.Evaluate(expression, ctx, null);
        }

        private MethodInfo GetSuitableMethod(MethodInfo[] methods, string methodName)
        {
            int argSize = -1;
            MethodInfo method = null;
            for (int i = 0; i < methods.Length; ++i)
            {
                int tempArgSize = methods[i].GetParameters().Length;
                if (methods[i].Name.Equals(methodName)
                    && tempArgSize > argSize
                    && AutoBindingUtil.IsSuitable(methods[i].GetParameters()))
                {
                    method = methods[i];
                    argSize = tempArgSize;
                }
            }
            return method;
        }

        private void Invoke(Type type, object component, string methodName, object[] args)
        {
            try
            {
                type.InvokeMember(methodName, BindingFlags.InvokeMethod,
                    null, component, args);
            }
            catch (MissingMethodException ex)
            {
                ex.ToString();
                throw new MethodNotFoundRuntimeException(type, methodName, args);
            }
            catch (Exception ex)
            {
                throw new IllegalMethodRuntimeException(ComponentDef.ComponentType,
                    methodName, ex);
            }
        }

        #region MethodAssembler ÉÅÉìÉo

        public virtual void Assemble(object component)
        {
        }

        #endregion
    }
}
