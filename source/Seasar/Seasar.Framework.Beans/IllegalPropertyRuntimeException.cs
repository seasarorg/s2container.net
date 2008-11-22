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
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Beans
{
    /// <summary>
    /// プロパティの値の設定に失敗したときにスローされる例外です。
    /// </summary>
    [Serializable]
    public class IllegalPropertyRuntimeException : SRuntimeException
    {
        private readonly Type _componentType;
        private readonly string _propertyName;

        public IllegalPropertyRuntimeException(
            Type componentType, string propertyName, Exception cause)
            : base("ESSR0059", new object[] { componentType.FullName, propertyName, cause }, cause)
        {
            _componentType = componentType;
            _propertyName = propertyName;
        }

        public IllegalPropertyRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _componentType = info.GetValue("_componentType", typeof(Type)) as Type;
            _propertyName = info.GetString("_propertyName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_componentType", _componentType, typeof(Type));
            info.AddValue("_propertyName", _propertyName, typeof(string));
            base.GetObjectData(info, context);
        }

        public Type ComponentType
        {
            get { return _componentType; }
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }
    }
}
