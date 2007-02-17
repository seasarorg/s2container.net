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
using Nullables;

namespace Seasar.Tests.Extension
{
    [Serializable]
    public class NHibernateNullableBasicTypeBean
    {
        private NullableInt64 id;

        public NullableInt64 Id
        {
            get { return id; }
            set { id = value; }
        }

        private NullableBoolean boolType;

        public NullableBoolean BoolType
        {
            get { return boolType; }
            set { boolType = value; }
        }

        private NullableSByte sbyteType;

        public NullableSByte SbyteType
        {
            get { return sbyteType; }
            set { sbyteType = value; }
        }

        private NullableByte byteType;

        public NullableByte ByteType
        {
            get { return byteType; }
            set { byteType = value; }
        }

        private NullableInt16 int16Type;

        public NullableInt16 Int16Type
        {
            get { return int16Type; }
            set { int16Type = value; }
        }

        private NullableInt32 int32Type;

        public NullableInt32 Int32Type
        {
            get { return int32Type; }
            set { int32Type = value; }
        }

        private NullableInt64 int64Type;

        public NullableInt64 Int64Type
        {
            get { return int64Type; }
            set { int64Type = value; }
        }

        private NullableDecimal decimalType;

        public NullableDecimal DecimalType
        {
            get { return decimalType; }
            set { decimalType = value; }
        }

        private NullableSingle singleType;

        public NullableSingle SingleType
        {
            get { return singleType; }
            set { singleType = value; }
        }

        private NullableDouble doubleType;

        public NullableDouble DoubleType
        {
            get { return doubleType; }
            set { doubleType = value; }
        }

        private string stringType;

        public string StringType
        {
            get { return stringType; }
            set { stringType = value; }
        }

        private NullableDateTime dateTimeType;

        public NullableDateTime DateTimeType
        {
            get { return dateTimeType; }
            set { dateTimeType = value; }
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
            this.id = id;
            this.boolType = boolType;
            this.sbyteType = sbyteType;
            this.byteType = byteType;
            this.int16Type = int16Type;
            this.int32Type = int32Type;
            this.int64Type = int64Type;
            this.decimalType = decimalType;
            this.singleType = singleType;
            this.doubleType = doubleType;
            this.stringType = stringType;
            this.dateTimeType = dateTimeType;
        }

        public override int GetHashCode()
        {
            return (int) this.Id;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(id).Append(", ");
            buf.Append(boolType).Append(", ");
            buf.Append(sbyteType).Append(", ");
            buf.Append(byteType).Append(", ");
            buf.Append(int16Type).Append(", ");
            buf.Append(int32Type).Append(", ");
            buf.Append(int64Type).Append(", ");
            buf.Append(decimalType).Append(", ");
            buf.Append(singleType).Append(", ");
            buf.Append(doubleType).Append(", ");
            buf.Append(stringType).Append(", ");
            buf.Append(dateTimeType);
            return buf.ToString();
        }
    }
}
