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

namespace Seasar.Framework.Exceptions
{
    /// <summary>
    /// ëŒè€Ç™ê›íËÇ≥ÇÍÇƒÇ¢Ç»Ç¢èÍçáÇÃé¿çséûó·äOÇ≈Ç∑ÅB
    /// </summary>
    [Serializable]
    public sealed class EmptyRuntimeException : SRuntimeException
    {
        private readonly string _targetName;

        public EmptyRuntimeException(string targetName)
            : base("ESSR0007", new object[] { targetName })
        {
            _targetName = targetName;
        }

        public EmptyRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _targetName = info.GetString("_targetName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_targetName", _targetName, typeof(string));
            base.GetObjectData(info, context);
        }

        public string TargetName
        {
            get { return _targetName; }
        }
    }
}
