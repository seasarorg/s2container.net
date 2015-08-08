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
using System.Runtime.Serialization;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;

namespace Seasar.Framework.Beans
{
    [Serializable]
    public class MethodNotFoundRuntimeException : SRuntimeException
    {
        public MethodNotFoundRuntimeException(
            Type componentType, string methodName, object[] methodArgs)
            : base("ESSR0049", new object[] { componentType.FullName, MethodUtil.GetSignature(methodName,methodArgs) })
        {
            ComponentType = componentType;
            MethodName = methodName;
            if (methodArgs != null)
            {
                MethodArgTypes = Type.GetTypeArray(methodArgs);
            }
        }

        public MethodNotFoundRuntimeException(
            Type componentType, string methodName, Type[] methodArgTypes)
            : base("ESSR0049", new object[] { componentType.FullName, MethodUtil.GetSignature(methodName,methodArgTypes)})
        {
            ComponentType = componentType;
            MethodName = methodName;
            MethodArgTypes = methodArgTypes;
        }

        public MethodNotFoundRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ComponentType = info.GetValue("_componentType", typeof(Type)) as Type;
            MethodName = info.GetString("_methodName");
            MethodArgTypes = info.GetValue("_methodArgTypes", typeof(Type[])) as Type[];
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_componentType", ComponentType, typeof(Type));
            info.AddValue("_methodName", MethodName, typeof(string));
            info.AddValue("_methodArgTypes", MethodArgTypes, typeof(Type[]));
            base.GetObjectData(info, context);
        }

        public Type ComponentType { get; }

        public string MethodName { get; }

        public Type[] MethodArgTypes { get; }
    }
}
