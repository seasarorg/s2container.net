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

using System;
using System.Text;

namespace Seasar.Tests.Extension
{
    [Serializable]
    public class BasicTypeBean
    {
        private long _id;

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private bool _boolType;

        public bool BoolType
        {
            get { return _boolType; }
            set { _boolType = value; }
        }

        private sbyte _sbyteType;

        public sbyte SbyteType
        {
            get { return _sbyteType; }
            set { _sbyteType = value; }
        }

        private byte _byteType;

        public byte ByteType
        {
            get { return _byteType; }
            set { _byteType = value; }
        }

        private short _int16Type;

        public short Int16Type
        {
            get { return _int16Type; }
            set { _int16Type = value; }
        }

        private int _int32Type;

        public int Int32Type
        {
            get { return _int32Type; }
            set { _int32Type = value; }
        }

        private long _int64Type;

        public long Int64Type
        {
            get { return _int64Type; }
            set { _int64Type = value; }
        }

        private decimal _decimalType;

        public decimal DecimalType
        {
            get { return _decimalType; }
            set { _decimalType = value; }
        }

        private float _singleType;

        public float SingleType
        {
            get { return _singleType; }
            set { _singleType = value; }
        }

        private double _doubleType;

        public double DoubleType
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

        private DateTime _dateTimeType;

        public DateTime DateTimeType
        {
            get { return _dateTimeType; }
            set { _dateTimeType = value; }
        }

        public BasicTypeBean()
        {
        }

        public BasicTypeBean(
            long id,
            bool boolType,
            sbyte sbyteType,
            byte byteType,
            short int16Type,
            int int32Type,
            long int64Type,
            decimal decimalType,
            float singleType,
            double doubleType,
            string stringType,
            DateTime dateTimeType
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
