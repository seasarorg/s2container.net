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
using System.Data.SqlTypes;
using System.Text;

namespace Seasar.Tests.Extension
{
    [Serializable]
    public class SqlTypeBasicTypeBean
    {
        private SqlInt64 _id;

        public SqlInt64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private SqlBoolean _boolType;

        public SqlBoolean BoolType
        {
            get { return _boolType; }
            set { _boolType = value; }
        }

        private SqlByte _byteType;

        public SqlByte ByteType
        {
            get { return _byteType; }
            set { _byteType = value; }
        }

        private SByte _sbyteType;

        public SByte SbyteType
        {
            get { return _sbyteType; }
            set { _sbyteType = value; }
        }

        private SqlInt16 _int16Type;

        public SqlInt16 Int16Type
        {
            get { return _int16Type; }
            set { _int16Type = value; }
        }

        private SqlInt32 _int32Type;

        public SqlInt32 Int32Type
        {
            get { return _int32Type; }
            set { _int32Type = value; }
        }

        private SqlInt64 _int64Type;

        public SqlInt64 Int64Type
        {
            get { return _int64Type; }
            set { _int64Type = value; }
        }

        private SqlDecimal _decimalType;

        public SqlDecimal DecimalType
        {
            get { return _decimalType; }
            set { _decimalType = value; }
        }

        private SqlSingle _singleType;

        public SqlSingle SingleType
        {
            get { return _singleType; }
            set { _singleType = value; }
        }

        private SqlDouble _doubleType;

        public SqlDouble DoubleType
        {
            get { return _doubleType; }
            set { _doubleType = value; }
        }

        private SqlString _stringType;

        public SqlString StringType
        {
            get { return _stringType; }
            set { _stringType = value; }
        }

        private SqlDateTime _dateTimeType;

        public SqlDateTime DateTimeType
        {
            get { return _dateTimeType; }
            set { _dateTimeType = value; }
        }

        public SqlTypeBasicTypeBean()
        {
        }

        public SqlTypeBasicTypeBean(
            SqlInt64 id,
            SqlBoolean boolType,
            SByte sbyteType,
            SqlByte byteType,
            SqlInt16 int16Type,
            SqlInt32 int32Type,
            SqlInt64 int64Type,
            SqlDecimal decimalType,
            SqlSingle singleType,
            SqlDouble doubleType,
            SqlString stringType,
            SqlDateTime dateTimeType
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
