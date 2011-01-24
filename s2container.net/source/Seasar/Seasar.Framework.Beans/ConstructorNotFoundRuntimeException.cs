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
using System.Text;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Beans
{
    /// <summary>
    /// 対象のクラスに適用可能なコンストラクタが見つからなかった場合の実行時例外です。
    /// </summary>
    [Serializable]
    public class ConstructorNotFoundRuntimeException : SRuntimeException
    {
        private readonly Type _componentType;
        private readonly object[] _methodArgs;

        public ConstructorNotFoundRuntimeException(Type componentType,
            object[] methodArgs)
            : base("ESSR0048",
            new object[] { componentType.FullName, GetSignature(methodArgs) })
        {
            _componentType = componentType;
            _methodArgs = methodArgs;
        }

        public ConstructorNotFoundRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _componentType = info.GetValue("_componentType", typeof(Type)) as Type;
            _methodArgs = info.GetValue("_methodArgs", typeof(object[])) as object[];
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_componentType", _componentType, typeof(Type));
            info.AddValue("_methodArgs", _methodArgs, typeof(object[]));
            base.GetObjectData(info, context);
        }

        public Type ComponentType
        {
            get { return _componentType; }
        }

        public object[] MethodArgs
        {
            get { return _methodArgs; }
        }

        private static string GetSignature(object[] methodArgs)
        {
            StringBuilder buf = new StringBuilder(100);
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
            return buf.ToString();
        }
    }
}
