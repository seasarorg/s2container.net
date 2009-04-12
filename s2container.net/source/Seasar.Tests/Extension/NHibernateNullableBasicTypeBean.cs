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

#if NHIBERNATE_NULLABLES

using System;
using System.Text;
using Nullables;

namespace Seasar.Tests.Extension
{
    [Serializable]
    public class NHibernateNullableBasicTypeBean
    {
        private NullableInt64 _id;

        public NullableInt64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private NullableBoolean _boolType;

        public NullableBoolean BoolType
        {
            get { return _boolType; }
            set { _boolType = value; }
        }

        private NullableSByte _sbyteType;

        public NullableSByte SbyteType
        {
            get { return _sbyteType; }
            set { _sbyteType = value; }
        }

        private NullableByte _byteType;

        public NullableByte ByteType
        {
            get { return _byteType; }
            set { _byteType = value; }
        }

        private NullableInt16 _int16Type;

        public NullableInt16 Int16Type
        {
            get { return _int16Type; }
            set { _int16Type = value; }
        }

        private NullableInt32 _int32Type;

        public NullableInt32 Int32Type
        {
            get { return _int32Type; }
            set { _int32Type = value; }
        }

        private NullableInt64 _int64Type;

        public NullableInt64 Int64Type
        {
            get { return _int64Type; }
            set { _int64Type = value; }
        }

        private NullableDecimal _decimalType;

        public NullableDecimal DecimalType
        {
            get { return _decimalType; }
            set { _decimalType = value; }
        }

        private NullableSingle _singleType;

        public NullableSingle SingleType
        {
            get { return _singleType; }
            set { _singleType = value; }
        }

        private NullableDouble _doubleType;

        public NullableDouble DoubleType
        {
            get { return _doubleType; }
            set { _doubleType = value; }
        }

        private string _stringType;

        public string StringType
        {
            get { return _stringType; }
            set { _stringType = value; }
        }

        private NullableDateTime _dateTimeType;

        public NullableDateTime DateTimeType
        {
            get { return _dateTimeType; }
            set { _dateTimeType = value; }
        }

        public NHibernateNullableBasicTypeBean()
        {
        }

        public NHibernateNullableBasicTypeBean(
            NullableInt64 id,
            NullableBoolean boolType,
            NullableSByte sbyteType,
            NullableByte byteType,
            NullableInt16 int16Type,
            NullableInt32 int32Type,
            NullableInt64 int64Type,
            NullableDecimal decimalType,
            NullableSingle singleType,
            NullableDouble doubleType,
            string stringType,
            NullableDateTime dateTimeType
            )
        {
            _id = id;
            _boolType = boolType;
            _sbyteType = sbyteType;
            _byteType = byteType;
            _int16Type = int16Type;
            _int32Type = int32Type;
            _int64Type = int64Type;
            _decimalType = decimalType;
            _singleType = singleType;
            _doubleType = doubleType;
            _stringType = stringType;
            _dateTimeType = dateTimeType;
        }

        public override int GetHashCode()
        {
            return (int) Id;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(_id).Append(", ");
            buf.Append(_boolType).Append(", ");
            buf.Append(_sbyteType).Append(", ");
            buf.Append(_byteType).Append(", ");
            buf.Append(_int16Type).Append(", ");
            buf.Append(_int32Type).Append(", ");
            buf.Append(_int64Type).Append(", ");
            buf.Append(_decimalType).Append(", ");
            buf.Append(_singleType).Append(", ");
            buf.Append(_doubleType).Append(", ");
            buf.Append(_stringType).Append(", ");
            buf.Append(_dateTimeType);
            return buf.ToString();
        }
    }
}

#endif
