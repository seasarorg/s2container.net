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

namespace Seasar.Framework.Beans
{
    /// <summary>
    /// フィールドが見つからない場合にスローされる例外です。
    /// </summary>
    [Serializable]
    public class FieldNotFoundRuntimeException : SRuntimeException
    {
        private readonly Type _componentType;

        private readonly string _fieldName;

        /// <summary>
        /// ターゲットの型
        /// </summary>
        public Type ComponentType
        {
            get { return _componentType; }
        }

        /// <summary>
        /// フィールド名
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
        }

        /// <summary>
        /// FieldNotFoundRuntimeExceptionを作成します。
        /// </summary>
        /// <param name="componentType"></param>
        /// <param name="fieldName"></param>
        public FieldNotFoundRuntimeException(Type componentType, string fieldName)
            : base("ESSR0070", new Object[] { componentType.Name, fieldName })
        {
            _componentType = componentType;
            _fieldName = fieldName;
        }

        public FieldNotFoundRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _componentType = info.GetValue("_componentType", typeof(Type)) as Type;
            _fieldName = info.GetString("_fieldName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_componentType", _componentType, typeof(Type));
            info.AddValue("_fieldName", _fieldName, typeof(string));
            base.GetObjectData(info, context);
        }
    }
}
