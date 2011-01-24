#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
        private readonly Type _componentType;
        private readonly string _methodName;
        private readonly Type[] _methodArgTypes;

        public MethodNotFoundRuntimeException(
            Type componentType, string methodName, object[] methodArgs)
            : base("ESSR0049", new object[] { componentType.FullName, MethodUtil.GetSignature(methodName,methodArgs) })
        {
            _componentType = componentType;
            _methodName = methodName;
            if (methodArgs != null)
            {
                _methodArgTypes = Type.GetTypeArray(methodArgs);
            }
        }

        public MethodNotFoundRuntimeException(
            Type componentType, string methodName, Type[] methodArgTypes)
            : base("ESSR0049", new object[] { componentType.FullName, MethodUtil.GetSignature(methodName,methodArgTypes)})
        {
            _componentType = componentType;
            _methodName = methodName;
            _methodArgTypes = methodArgTypes;
        }

        public MethodNotFoundRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _componentType = info.GetValue("_componentType", typeof(Type)) as Type;
            _methodName = info.GetString("_methodName");
            _methodArgTypes = info.GetValue("_methodArgTypes", typeof(Type[])) as Type[];
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_componentType", _componentType, typeof(Type));
            info.AddValue("_methodName", _methodName, typeof(string));
            info.AddValue("_methodArgTypes", _methodArgTypes, typeof(Type[]));
            base.GetObjectData(info, context);
        }

        public Type ComponentType
        {
            get { return _componentType; }
        }

        public string MethodName
        {
            get { return _methodName; }
        }

        public Type[] MethodArgTypes
        {
            get { return _methodArgTypes; }
        }
    }
}
