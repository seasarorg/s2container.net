using System;
using System.Runtime.Serialization;
using Seasar.Framework.Exceptions;

namespace Seasar.Dao.Pager
{
    [Serializable]
    public class PagingParameterDefinitionException : SRuntimeException
    {
        private string _parameterName;

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