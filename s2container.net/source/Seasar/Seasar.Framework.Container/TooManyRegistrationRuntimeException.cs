#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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

namespace Seasar.Framework.Container
{
    [Serializable]
    public sealed class TooManyRegistrationRuntimeException : SRuntimeException
    {
        private readonly object _key;
        private readonly Type[] _componentTypes;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="componentTypes"></param>
        public TooManyRegistrationRuntimeException(object key, Type[] componentTypes)
            : base("ESSR0045", new object[] { key, GetTypeNames(componentTypes) })
        {
            _key = key;
            _componentTypes = componentTypes;
        }

        public TooManyRegistrationRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _key = info.GetValue("_key", typeof(object));
            _componentTypes = info.GetValue("_componentTypes", typeof(Type[])) as Type[];
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_key", _key, typeof(object));
            info.AddValue("_componentTypes", _componentTypes, typeof(Type[]));
            base.GetObjectData(info, context);
        }

        public object Key
        {
            get { return _key; }
        }

        public Type[] ComponentTypes
        {
            get { return _componentTypes; }
        }

        public static string GetTypeNames(Type[] componentTypes)
        {
            StringBuilder buf = new StringBuilder(255);
            foreach (Type componentType in componentTypes)
            {
                buf.Append(componentType.FullName);
                buf.Append(", ");
            }
            buf.Length -= 2;
            return buf.ToString();
        }
    }
}
