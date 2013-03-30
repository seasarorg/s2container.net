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
using System.Reflection;
using System.Text;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Util
{
    public sealed class MethodUtil
    {
        private MethodUtil()
        {
        }

        public static object Invoke(MethodInfo method, object target, object[] args)
        {
            try
            {
                return method.Invoke(target, args);
            }
            catch (TargetInvocationException ex)
            {
                throw new InvocationTargetRuntimeException(method.DeclaringType, ex);
            }
            catch (TargetException ex)
            {
                throw new IllegalAccessRuntimeException(method.DeclaringType, ex);
            }
            catch (ArgumentException ex)
            {
                throw new IllegalAccessRuntimeException(method.DeclaringType, ex);
            }
            catch (TargetParameterCountException ex)
            {
                throw new IllegalAccessRuntimeException(method.DeclaringType, ex);
            }
            catch (MethodAccessException ex)
            {
                throw new IllegalAccessRuntimeException(method.DeclaringType, ex);
            }
        }

        public static string GetSignature(string methodName, Type[] argTypes)
        {
            StringBuilder buf = new StringBuilder(100);
            buf.Append(methodName);
            buf.Append("(");
            if (argTypes != null)
            {
                for (int i = 0; i < argTypes.Length; ++i)
                {
                    if (i > 0) buf.Append(", ");
                    buf.Append(argTypes[i].FullName);
                }
            }
            buf.Append(")");
            return buf.ToString();
        }


        public static string GetSignature(string methodName, object[] methodArgs)
        {
            StringBuilder buf = new StringBuilder(100);
            buf.Append(methodName);
            buf.Append("(");
            if (methodArgs != null)
            {
                for (int i = 0; i < methodArgs.Length; ++i)
                {
                    if (i > 0) buf.Append(", ");
                    if (methodArgs[i] != null)
                    {
                        buf.Append(methodArgs[i].GetType().FullName);
                    }
                    else
                    {
                        buf.Append("null");
                    }
                }
            }
            buf.Append(")");
            return buf.ToString();
        }

        public static string[] GetParameterNames(MethodInfo mi)
        {
            ParameterInfo[] parameters = mi.GetParameters();
            string[] argNames = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; ++i)
            {
                argNames[i] = parameters[i].Name;
            }
            return argNames;
        }

        public static Type[] GetParameterTypes(MethodInfo mi)
        {
            ParameterInfo[] parameters = mi.GetParameters();
            Type[] argTypes = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; ++i)
            {
                argTypes[i] = parameters[i].ParameterType;
            }
            return argTypes;
        }

        /// <summary>
        /// Override‰Â”\‚©‚Ç‚¤‚©”»’è‚·‚é
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool CanOverride(MethodBase info)
        {
            if (info.DeclaringType.IsInterface ||
                info.IsAbstract ||
                info.IsVirtual)
            {
                return true;
            }
            return false;
        }
    }
}
