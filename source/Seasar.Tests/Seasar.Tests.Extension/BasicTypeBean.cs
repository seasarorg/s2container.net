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
    public class BasicTypeBean
    {
        private long id;

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        private bool boolType;

        public bool BoolType
        {
            get { return boolType; }
            set { boolType = value; }
        }

        private sbyte sbyteType;

        public sbyte SbyteType
        {
            get { return sbyteType; }
            set { sbyteType = value; }
        }

        private byte byteType;

        public byte ByteType
        {
            get { return byteType; }
            set { byteType = value; }
        }

        private short int16Type;

        public short Int16Type
        {
            get { return int16Type; }
            set { int16Type = value; }
        }

        private int int32Type;

        public int Int32Type
        {
            get { return int32Type; }
            set { int32Type = value; }
        }

        private long int64Type;

        public long Int64Type
        {
            get { return int64Type; }
            set { int64Type = value; }
        }

        private decimal decimalType;

        public decimal DecimalType
        {
            get { return decimalType; }
            set { decimalType = value; }
        }

        private float singleType;

        public float SingleType
        {
            get { return singleType; }
            set { singleType = value; }
        }

        private double doubleType;

        public double DoubleType
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

        private DateTime dateTimeType;

        public DateTime DateTimeType
        {
            get { return dateTimeType; }
            set { dateTimeType = value; }
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
