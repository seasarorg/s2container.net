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
using System.Text;

namespace Seasar.Tests.Extension
{
    [Serializable]
    public class NullableEnumTypeBean
    {
        private long? _id;

        public long? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private Numbers? _sbyteType;

        public Numbers? SbyteType
        {
            get { return _sbyteType; }
            set { _sbyteType = value; }
        }

        private Numbers? _byteType;

        public Numbers? ByteType
        {
            get { return _byteType; }
            set { _byteType = value; }
        }

        private Numbers? _int16Type;

        public Numbers? Int16Type
        {
            get { return _int16Type; }
            set { _int16Type = value; }
        }

        private Numbers? _int32Type;

        public Numbers? Int32Type
        {
            get { return _int32Type; }
            set { _int32Type = value; }
        }

        private Numbers? _int64Type;

        public Numbers? Int64Type
        {
            get { return _int64Type; }
            set { _int64Type = value; }
        }

        private Numbers? _decimalType;

        public Numbers? DecimalType
        {
            get { return _decimalType; }
            set { _decimalType = value; }
        }

        private Numbers? _singleType;

        public Numbers? SingleType
        {
            get { return _singleType; }
            set { _singleType = value; }
        }

        private Numbers? _doubleType;

        public Numbers? DoubleType
        {
            get { return _doubleType; }
            set { _doubleType = value; }
        }

        private Numbers? _stringType;

        public Numbers? StringType
        {
            get { return _stringType; }
            set { _stringType = value; }
        }

        public NullableEnumTypeBean()
        {
        }

        public NullableEnumTypeBean(
            long? id,
            Numbers? sbyteType,
            Numbers? byteType,
            Numbers? int16Type,
            Numbers? int32Type,
            Numbers? int64Type,
            Numbers? decimalType,
            Numbers? singleType,
            Numbers? doubleType,
            Numbers? stringType
            )
        {
            _id = id;
            _sbyteType = sbyteType;
            _byteType = byteType;
            _int16Type = int16Type;
            _int32Type = int32Type;
            _int64Type = int64Type;
            _decimalType = decimalType;
            _singleType = singleType;
            _doubleType = doubleType;
            _stringType = stringType;
        }

        public override int GetHashCode()
        {
            return (int) this.Id;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(_id).Append(", ");
            buf.Append(_sbyteType).Append(", ");
            buf.Append(_byteType).Append(", ");
            buf.Append(_int16Type).Append(", ");
            buf.Append(_int32Type).Append(", ");
            buf.Append(_int64Type).Append(", ");
            buf.Append(_decimalType).Append(", ");
            buf.Append(_singleType).Append(", ");
            buf.Append(_doubleType).Append(", ");
            buf.Append(_stringType);
            return buf.ToString();
        }
    }
}
