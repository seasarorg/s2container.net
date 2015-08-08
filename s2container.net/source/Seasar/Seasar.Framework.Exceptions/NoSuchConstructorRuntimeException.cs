﻿#region Copyright
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
using Seasar.Framework.Util;

namespace Seasar.Framework.Exceptions
{
    /// <summary>
    /// �R���X�g���N�^��������Ȃ��ꍇ�̎��s����O�ł��B
    /// </summary>
    [Serializable]
    public class NoSuchConstructorRuntimeException : SRuntimeException
    {
        public NoSuchConstructorRuntimeException(Type targetType, Type[] argTypes)
            : base("ESSR0064", new object[] { targetType.FullName, MethodUtil.GetSignature(targetType.Name, argTypes) })
        {
            TargetType = targetType;
            ArgTypes = argTypes;
        }

        public NoSuchConstructorRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            TargetType = info.GetValue("_targetType", typeof(Type)) as Type;
            ArgTypes = info.GetValue("_argTypes", typeof(Type[])) as Type[];
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_targetType", TargetType, typeof(Type));
            info.AddValue("_argTypes", ArgTypes, typeof(Type[]));
            base.GetObjectData(info, context);
        }

        public Type TargetType { get; }

        public Type[] ArgTypes { get; }
    }
}
