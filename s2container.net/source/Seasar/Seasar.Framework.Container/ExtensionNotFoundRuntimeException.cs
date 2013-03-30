#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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

namespace Seasar.Framework.Container
{
    /// <summary>
    /// ägí£éqÇ™å©Ç¬Ç©ÇÁÇ»Ç©Ç¡ÇΩÇ∆Ç´ÇÃé¿çséûó·äO
    /// </summary>
    [Serializable]
    public class ExtensionNotFoundRuntimeException : SRuntimeException
    {
        private readonly string path;

        public ExtensionNotFoundRuntimeException(string path)
            : base("ESSR0074", new object[] { path })
        {
            this.path = path;
        }

        public ExtensionNotFoundRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            path = info.GetString("path");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("path", path, typeof(string));
            base.GetObjectData(info, context);
        }

        public string Path
        {
            get { return path; }
        }
    }
}
