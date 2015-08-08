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
    /// �v���p�e�B��������Ȃ��Ƃ��ɓ��������O�ł�
    /// </summary>
    [Serializable]
    public class PropertyNotFoundRuntimeException : SRuntimeException
    {
        public PropertyNotFoundRuntimeException(Type componentType, string propertyName)
            : base("ESSR0065", new object[] { componentType.FullName, propertyName })
        {
            ComponentType = componentType;
            PropertyName = propertyName;
        }

        public PropertyNotFoundRuntimeException(SerializationInfo info, StreamingContext context)
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
