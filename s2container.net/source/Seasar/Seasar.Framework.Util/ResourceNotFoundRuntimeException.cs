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

namespace Seasar.Framework.Util
{
    [Serializable]
    public class ResourceNotFoundRuntimeException : SRuntimeException
    {
        private readonly string _path;

        public ResourceNotFoundRuntimeException(string path)
            : base("ESSR0055", new object[] { path })
        {
            _path = path;
        }

        public ResourceNotFoundRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _path = info.GetString("_path");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_path", _path, typeof(string));
            base.GetObjectData(info, context);
        }

        public string Path
        {
            get { return _path; }
        }
    }
}
