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
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;

namespace Seasar.Framework.Beans
{
    [Serializable]
    public class MethodNotFoundRuntimeException : SRuntimeException
    {
        private Type targetType;
        private string methodName;
        private Type[] methodArgTypes;

        public MethodNotFoundRuntimeException(
            Type targetType, string methodName, Type[] methodArgs)
            : base("ESSR0049", new object[] { targetType.FullName, 
                MethodUtil.GetSignature(methodName, methodArgs) })
        {
            this.targetType = targetType;
            this.methodName = methodName;

            if (methodArgs != null)
            {
                methodArgTypes = new Type[methodArgs.Length];

                for (int i = 0; i < methodArgs.Length; ++i)
                {
                    if (methodArgs[i] != null)
                    {
                        methodArgTypes[i] = methodArgs[i].GetType();
                    }
                }
            }
        }
    }
}
