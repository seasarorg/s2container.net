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

namespace Seasar.Framework.Container.Factory
{
    [Serializable]
    public class TagAttributeNotDefinedRuntimeException : SRuntimeException
    {
        private readonly string _tagName;
        private readonly string _attributeName;

        public TagAttributeNotDefinedRuntimeException(string tagName, string attributeName)
            : base("ESSR0056", new object[] { tagName, attributeName })
        {
            _tagName = tagName;
            _attributeName = attributeName;
        }

        public TagAttributeNotDefinedRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _tagName = info.GetString("_tagName");
            _attributeName = info.GetString("_attributeName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_tagName", _tagName, typeof(string));
            info.AddValue("_attributeName", _attributeName, typeof(string));
            base.GetObjectData(info, context);
        }

        public string TagName
        {
            get { return _tagName; }
        }

        public string AttributeName
        {
            get { return _attributeName; }
        }
    }
}
