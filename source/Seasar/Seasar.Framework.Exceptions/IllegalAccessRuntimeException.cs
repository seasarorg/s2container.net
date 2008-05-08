#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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

namespace Seasar.Framework.Exceptions
{
    /// <summary>
    /// TargetException, ArgumentException, TargetParameterCountException,
    /// MethodAccessExceptionをラップする実行時例外です。
    /// メソッド・コンストラクタ・プロパティの呼び出しに関する例外です。
    /// 呼び出される前に例外は発生します。
    /// </summary>
    [Serializable]
    public class IllegalAccessRuntimeException : SRuntimeException
    {
        private readonly Type _targetType;

        public IllegalAccessRuntimeException(Type targetType, Exception cause)
            : base("ESSR0042", new object[] { targetType.FullName, cause }, cause)
        {
            _targetType = targetType;
        }

        public IllegalAccessRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _targetType = info.GetValue("_targetType", typeof(Type)) as Type;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_targetType", _targetType, typeof(Type));
            base.GetObjectData(info, context);
        }

        public Type TargetType
        {
            get { return _targetType; }
        }
    }
}
