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
        private long? id;

        public long? Id
        {
            get { return id; }
            set { id = value; }
        }

        private Numbers? sbyteType;

        public Numbers? SbyteType
        {
            get { return sbyteType; }
            set { sbyteType = value; }
        }

        private Numbers? byteType;

        public Numbers? ByteType
        {
            get { return byteType; }
            set { byteType = value; }
        }

        private Numbers? int16Type;

        public Numbers? Int16Type
        {
            get { return int16Type; }
            set { int16Type = value; }
        }

        private Numbers? int32Type;

        public Numbers? Int32Type
        {
            get { return int32Type; }
            set { int32Type = value; }
        }

        private Numbers? int64Type;

        public Numbers? Int64Type
        {
            get { return int64Type; }
            set { int64Type = value; }
        }

        private Numbers? decimalType;

        public Numbers? DecimalType
        {
            get { return decimalType; }
            set { decimalType = value; }
        }

        private Numbers? singleType;

        public Numbers? SingleType
        {
            get { return singleType; }
            set { singleType = value; }
        }

        private Numbers? doubleType;

        public Numbers? DoubleType
        {
            get { return doubleType; }
            set { doubleType = value; }
        }

        private Numbers? stringType;

        public Numbers? StringType
        {
            get { return stringType; }
            set { stringType = value; }
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
            this.id = id;
            this.sbyteType = sbyteType;
            this.byteType = byteType;
            this.int16Type = int16Type;
            this.int32Type = int32Type;
            this.int64Type = int64Type;
            this.decimalType = decimalType;
            this.singleType = singleType;
            this.doubleType = doubleType;
            this.stringType = stringType;
        }

        public override int GetHashCode()
        {
            return (int) this.Id;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(id).Append(", ");
            buf.Append(sbyteType).Append(", ");
            buf.Append(byteType).Append(", ");
            buf.Append(int16Type).Append(", ");
            buf.Append(int32Type).Append(", ");
            buf.Append(int64Type).Append(", ");
            buf.Append(decimalType).Append(", ");
            buf.Append(singleType).Append(", ");
            buf.Append(doubleType).Append(", ");
            buf.Append(stringType);
            return buf.ToString();
        }
    }
}
