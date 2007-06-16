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

namespace Seasar.Dao
{
    [Serializable]
    public class DaoNotFoundRuntimeException : SRuntimeException
    {
        private readonly Type _targetType;

        public DaoNotFoundRuntimeException(Type targetType)
            : base("EDAO0008", new object[] { targetType.Name })
        {
            _targetType = targetType;
        }

        public DaoNotFoundRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _targetType = info.GetValue("_targetType", typeof(Type)) as Type;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_targetType", _targetType, typeof(Type));
            base.GetObjectData(info, context);
        }

        public Type TargetType
        {
            get { return _targetType; }
        }
    }

    [Serializable]
    public class EndCommentNotFoundRuntimeException : SRuntimeException
    {
        public EndCommentNotFoundRuntimeException()
            : base("EDAO0007")
        {
        }
    }

    [Serializable]
    public class IfConditionNotFoundRuntimeException : SRuntimeException
    {
        public IfConditionNotFoundRuntimeException()
            : base("EDAO0004")
        {
        }
    }

    [Serializable]
    public class IllegalBoolExpressionRuntimeException : SRuntimeException
    {
        private readonly string _expression;

        public IllegalBoolExpressionRuntimeException(string expression)
            : base("EDAO0003", new object[] { expression })
        {
            _expression = expression;
        }

        public IllegalBoolExpressionRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _expression = info.GetString("_expression");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_expression", _expression, typeof(string));
            base.GetObjectData(info, context);
        }

        public string Expression
        {
            get { return _expression; }
        }
    }

    [Serializable]
    public class IllegalSignatureRuntimeException : SRuntimeException
    {
        private readonly string _signature;

        public IllegalSignatureRuntimeException(string messageCode, string signature)
            : base(messageCode, new object[] { signature })
        {
            _signature = signature;
        }

        public IllegalSignatureRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _signature = info.GetString("_signature");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_signature", _signature, typeof(string));
            base.GetObjectData(info, context);
        }

        public string Signature
        {
            get { return _signature; }
        }
    }

    [Serializable]
    public class UpdateFailureRuntimeException : SRuntimeException
    {
        private readonly object _bean;
        private readonly int _rows;

        public UpdateFailureRuntimeException(object bean, int rows)
            : base("EDAO0005", new object[] { bean.ToString(), rows.ToString() })
        {
            _bean = bean;
            _rows = rows;
        }

        public UpdateFailureRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _bean = info.GetValue("_bean", typeof(object));
            _rows = info.GetInt32("_rows");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_bean", _bean, typeof(object));
            info.AddValue("_rows", _rows, typeof(int));
            base.GetObjectData(info, context);
        }

        public object Bean
        {
            get { return _bean; }
        }

        public int Rows
        {
            get { return _rows; }
        }
    }

    [Serializable]
    public class NotSingleRowUpdatedRuntimeException : UpdateFailureRuntimeException
    {
        public NotSingleRowUpdatedRuntimeException(object bean, int rows)
            : base(bean, rows)
        {
        }
    }

    [Serializable]
    public class PrimaryKeyNotFoundRuntimeException : SRuntimeException
    {
        private readonly Type _targetType;

        public PrimaryKeyNotFoundRuntimeException(Type targetType)
            : base("EDAO0009", new object[] { targetType.Name })
        {
            _targetType = targetType;
        }

        public PrimaryKeyNotFoundRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _targetType = info.GetValue("_targetType", typeof(Type)) as Type;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_targetType", _targetType, typeof(Type));
            base.GetObjectData(info, context);
        }

        public Type TargetType
        {
            get { return _targetType; }
        }
    }

    [Serializable]
    public class TokenNotClosedRuntimeException : SRuntimeException
    {
        private readonly string _token;
        private readonly string _sql;

        public TokenNotClosedRuntimeException(string token, string sql)
            : base("EDAO0002", new object[] { token, sql })
        {
            _token = token;
            _sql = sql;
        }

        public TokenNotClosedRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _token = info.GetString("_token");
            _sql = info.GetString("_sql");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_token", _token, typeof(string));
            info.AddValue("_sql", _sql, typeof(string));
            base.GetObjectData(info, context);
        }

        public string Token
        {
            get { return _token; }
        }

        public string Sql
        {
            get { return _sql; }
        }
    }

    [Serializable]
    public class WrongPropertyTypeOfTimestampException : SRuntimeException
    {
        private readonly string _propertyName;
        private readonly string _propertyType;

        public WrongPropertyTypeOfTimestampException(string propertyName, string propertyType)
            : base("EDAO0010", new object[] { propertyName, propertyType })
        {
            _propertyName = propertyName;
            _propertyType = propertyType;
        }

        public WrongPropertyTypeOfTimestampException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _propertyName = info.GetString("_propertyName");
            _propertyType = info.GetString("_propertyType");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_propertyName", _propertyName, typeof(string));
            info.AddValue("_propertyType", _propertyType, typeof(string));
            base.GetObjectData(info, context);
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public string PropertyType
        {
            get { return _propertyType; }
        }
    }
}
