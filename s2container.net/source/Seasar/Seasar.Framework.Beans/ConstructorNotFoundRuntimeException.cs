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
using System.Text;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;

namespace Seasar.Framework.Beans
{
    /// <summary>
    /// �Ώۂ̃N���X�ɓK�p�\�ȃR���X�g���N�^��������Ȃ������ꍇ�̎��s����O�ł��B
    /// </summary>
    [Serializable]
    public class ConstructorNotFoundRuntimeException : SRuntimeException
    {
        public ConstructorNotFoundRuntimeException(Type componentType,
            object[] methodArgs)
            : base("ESSR0048",
            new object[] { componentType.FullName, _GetSignature(methodArgs) })
        {
            ComponentType = componentType;
            MethodArgs = methodArgs;
        }

        public ConstructorNotFoundRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ComponentType = info.GetValue("_componentType", typeof(Type)) as Type;
            MethodArgs = info.GetValue("_methodArgs", typeof(object[])) as object[];
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_componentType", ComponentType, typeof(Type));
            info.AddValue("_methodArgs", MethodArgs, typeof(object[]));
            base.GetObjectData(info, context);
        }

        public Type ComponentType { get; }

        public object[] MethodArgs { get; }

        private static string _GetSignature(object[] methodArgs)
        {
            var buf = new StringBuilder(100);
            if (methodArgs != null)
            {
                for (var i = 0; i < methodArgs.Length; ++i)
                {
                    if (i > 0) buf.Append(", ");
                    if (methodArgs[i] != null)
                    {
                        buf.Append(methodArgs[i].GetExType().FullName);
                    }
                    else
                    {
                        buf.Append("null");
                    }
                }
            }
            return buf.ToString();
        }
    }
}
