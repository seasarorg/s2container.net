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

namespace Seasar.Framework.Beans
{
    /// <summary>
    /// �v���p�e�B�̒l�̐ݒ�Ɏ��s�����Ƃ��ɃX���[������O�ł��B
    /// </summary>
    [Serializable]
    public class IllegalPropertyRuntimeException : SRuntimeException
    {
        public IllegalPropertyRuntimeException(
            Type componentType, string propertyName, Exception cause)
            : base("ESSR0059", new object[] { componentType.FullName, propertyName, cause }, cause)
        {
            ComponentType = componentType;
            PropertyName = propertyName;
        }

        public IllegalPropertyRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ComponentType = info.GetValue("_componentType", typeof(Type)) as Type;
            PropertyName = info.GetString("_propertyName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_componentType", ComponentType, typeof(Type));
            info.AddValue("_propertyName", PropertyName, typeof(string));
            base.GetObjectData(info, context);
        }

        public Type ComponentType { get; }

        public string PropertyName { get; }
    }
}
