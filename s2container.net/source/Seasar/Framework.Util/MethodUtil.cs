#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using System.Text;
using System.Reflection;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Util
{
    public static class MethodUtil
    {
        //private const MethodInfo IS_BRIDGE_METHOD;
        //private const MethodInfo IS_SYNTHETIC_METHOD;
        //private const string REFLECTION_UTIL_CLASS_NAME = "";
        //private const MethodInfo GET_ELEMENT_TYPE_FROM_PARAMETER_METHOD;
        //private const MethodInfo GET_ELEMENT_TYPE_FROM_RETURN_METHOD;

        public static object Invoke(MethodInfo method, object target, object[] args)
        {
            try
            {
                return method.Invoke(target, args);
            }
            catch (TargetInvocationException ex)
            {
                Exception cause = ex.InnerException;

                if(cause is ApplicationException)
                {
                    throw (ApplicationException) cause;
                }

                throw new InvocationTargetRuntimeException(method.DeclaringType, ex);
            }
            catch (MethodAccessException ex)
            {
                throw new IllegalAccessRuntimeException(method.DeclaringType, ex);
            }
        }

        public static bool IsAbstract(MethodInfo method)
        {
            return method.IsAbstract;
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
                    if (i > 0)
                    {
                        buf.Append(", ");
                    }

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
                    if (i > 0)
                    {
                        buf.Append(", ");
                    }

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
    }
}
