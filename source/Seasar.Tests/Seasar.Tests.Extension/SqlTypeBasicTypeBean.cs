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
using System.Data.SqlTypes;
using System.Text;

namespace Seasar.Tests.Extension
{
    [Serializable]
    public class SqlTypeBasicTypeBean
    {
        private SqlInt64 id;

        public SqlInt64 Id
        {
            get { return id; }
            set { id = value; }
        }

        private SqlBoolean boolType;

        public SqlBoolean BoolType
        {
            get { return boolType; }
            set { boolType = value; }
        }

        private SqlByte byteType;

        public SqlByte ByteType
        {
            get { return byteType; }
            set { byteType = value; }
        }

        private SByte sbyteType;

        public SByte SbyteType
        {
            get { return sbyteType; }
            set { sbyteType = value; }
        }

        private SqlInt16 int16Type;

        public SqlInt16 Int16Type
        {
            get { return int16Type; }
            set { int16Type = value; }
        }

        private SqlInt32 int32Type;

        public SqlInt32 Int32Type
        {
            get { return int32Type; }
            set { int32Type = value; }
        }

        private SqlInt64 int64Type;

        public SqlInt64 Int64Type
        {
            get { return int64Type; }
            set { int64Type = value; }
        }

        private SqlDecimal decimalType;

        public SqlDecimal DecimalType
        {
            get { return decimalType; }
            set { decimalType = value; }
        }

        private SqlSingle singleType;

        public SqlSingle SingleType
        {
            get { return singleType; }
            set { singleType = value; }
        }

        private SqlDouble doubleType;

        public SqlDouble DoubleType
        {
            get { return doubleType; }
            set { doubleType = value; }
        }

        private SqlString stringType;

        public SqlString StringType
        {
            get { return stringType; }
            set { stringType = value; }
        }

        private SqlDateTime dateTimeType;

        public SqlDateTime DateTimeType
        {
            get { return dateTimeType; }
            set { dateTimeType = value; }
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
