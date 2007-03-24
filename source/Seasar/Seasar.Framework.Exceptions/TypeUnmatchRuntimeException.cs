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
using System.Runtime.Serialization;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Container
{
    [Serializable]
    public class TypeUnmatchRuntimeException : SRuntimeException
    {
        private readonly Type _componentType;
        private readonly Type _realComponentType;

        public TypeUnmatchRuntimeException(Type componentType, Type realComponentType)
            : base("ESSR0069", new object[] { componentType.FullName, realComponentType.FullName != null ? realComponentType.FullName : "null" })
        {
            _componentType = componentType;
            _realComponentType = realComponentType;
        }

        public TypeUnmatchRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _componentType = info.GetValue("_componentType", typeof(Type)) as Type;
            _realComponentType = info.GetValue("_realComponentType", typeof(Type)) as Type;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_componentType", _componentType, typeof(Type));
            info.AddValue("_realComponentType", _realComponentType, typeof(Type));
            base.GetObjectData(info, context);
        }

        public Type ComponentType
        {
            get { return _componentType; }
        }

        public Type RealComponentType
        {
            get { return _realComponentType; }
        }
    }
}
