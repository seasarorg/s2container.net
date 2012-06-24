#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

namespace Seasar.Dao.Pager
{
    [Serializable]
    public class PagingParameterDefinitionException : SRuntimeException
    {
        private readonly string _parameterName;

        public PagingParameterDefinitionException(string parameterName)
            : this(parameterName, null)
        {
        }

        public PagingParameterDefinitionException(string parameterName, Exception inner)
            : base("EDAO0011", new object[] { parameterName }, inner)
        {
            _parameterName = parameterName;
        }

        protected PagingParameterDefinitionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _parameterName = info.GetValue("_parameterName", typeof(string)) as string;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_parameterName", _parameterName, typeof(string));
            base.GetObjectData(info, context);
        }

        public string ParameterName
        {
            get { return _parameterName; }
        }
    }
}