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
using System.Collections;
using System.Data;
using System.Data.SqlTypes;
using Nullables;
using Seasar.Extension.ADO.Types;

namespace Seasar.Extension.DataSets.Types
{
    public sealed class ColumnTypes
    {
        public static readonly IColumnType STRING = new StringType();
        public static readonly IColumnType DECIMAL = new DecimalType();
        public static readonly IColumnType DATETIME = new DateTimeType();
        public static readonly IColumnType BINARY = new BinaryType();
        public static readonly IColumnType BOOLEAN = new BooleanType();
        public static readonly IColumnType OBJECT = new ObjectType();
        private static readonly Hashtable _types = new Hashtable();

        static ColumnTypes()
        {
            RegisterColumnType(typeof(String), STRING);
            RegisterColumnType(typeof(Byte), DECIMAL);
            RegisterColumnType(typeof(SByte), DECIMAL);
            RegisterColumnType(typeof(Int16), DECIMAL);
            RegisterColumnType(typeof(Int32), DECIMAL);
            RegisterColumnType(typeof(Int64), DECIMAL);
            RegisterColumnType(typeof(Single), DECIMAL);
            RegisterColumnType(typeof(Double), DECIMAL);
            RegisterColumnType(typeof(Decimal), DECIMAL);
            RegisterColumnType(typeof(DateTime), DATETIME);
            RegisterColumnType(ValueTypes.BYTE_ARRAY_TYPE, BINARY);
            RegisterColumnType(typeof(Boolean), BOOLEAN);
            RegisterColumnType(typeof(Guid), OBJECT);

            RegisterColumnType(typeof(SqlString), STRING);
            RegisterColumnType(typeof(SqlByte), DECIMAL);
            RegisterColumnType(typeof(SqlInt16), DECIMAL);
            RegisterColumnType(typeof(SqlInt32), DECIMAL);
            RegisterColumnType(typeof(SqlInt64), DECIMAL);
            RegisterColumnType(typeof(SqlSingle), DECIMAL);
            RegisterColumnType(typeof(SqlDouble), DECIMAL);
            RegisterColumnType(typeof(SqlDecimal), DECIMAL);
            RegisterColumnType(typeof(SqlDateTime), DATETIME);
            RegisterColumnType(typeof(SqlBinary), BINARY);
            RegisterColumnType(typeof(SqlBoolean), BOOLEAN);
            RegisterColumnType(typeof(SqlMoney), DECIMAL);
            RegisterColumnType(typeof(SqlGuid), OBJECT);

            RegisterColumnType(typeof(NullableChar), STRING);
            RegisterColumnType(typeof(NullableByte), DECIMAL);
            RegisterColumnType(typeof(NullableSByte), DECIMAL);
            RegisterColumnType(typeof(NullableInt16), DECIMAL);
            RegisterColumnType(typeof(NullableInt32), DECIMAL);
            RegisterColumnType(typeof(NullableInt64), DECIMAL);
            RegisterColumnType(typeof(NullableSingle), DECIMAL);
            RegisterColumnType(typeof(NullableDouble), DECIMAL);
            RegisterColumnType(typeof(NullableDecimal), DECIMAL);
            RegisterColumnType(typeof(NullableDateTime), DATETIME);
            RegisterColumnType(ValueTypes.NHIBERNATE_NULLABLE_BYTE_ARRAY_TYPE, BINARY);
            RegisterColumnType(typeof(NullableBoolean), BOOLEAN);
            RegisterColumnType(typeof(NullableGuid), OBJECT);

#if !NET_1_1
            RegisterColumnType(typeof(Nullable<Byte>), DECIMAL);
            RegisterColumnType(typeof(Nullable<SByte>), DECIMAL);
            RegisterColumnType(typeof(Nullable<Int16>), DECIMAL);
            RegisterColumnType(typeof(Nullable<Int32>), DECIMAL);
            RegisterColumnType(typeof(Nullable<Int64>), DECIMAL);
            RegisterColumnType(typeof(Nullable<Single>), DECIMAL);
            RegisterColumnType(typeof(Nullable<Double>), DECIMAL);
            RegisterColumnType(typeof(Nullable<Decimal>), DECIMAL);
            RegisterColumnType(typeof(Nullable<DateTime>), DATETIME);
            RegisterColumnType(ValueTypes.NULLABLE_BYTE_ARRAY_TYPE, BINARY);
            RegisterColumnType(typeof(Nullable<Boolean>), BOOLEAN);
#endif
        }

        private ColumnTypes()
        {
        }

        public static void RegisterColumnType(Type type, IColumnType columnType)
        {
            lock (_types)
            {
                _types[type] = columnType;
            }
        }

        public static IColumnType GetColumnType(DbType type)
        {
            switch (type)
            {
                case DbType.Byte:
                case DbType.SByte:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.Single:
                case DbType.Double:
                case DbType.Decimal:
                case DbType.VarNumeric:
                    return DECIMAL;
                case DbType.Boolean:
                    return BOOLEAN;
                case DbType.Date:
                case DbType.Time:
                case DbType.DateTime:
                    return DATETIME;
                case DbType.Binary:
                    return BINARY;
                case DbType.String:
                case DbType.StringFixedLength:
                    return STRING;
                case DbType.Guid:
                default:
                    return OBJECT;
            }
        }

        public static IColumnType GetColumnType(object value)
        {
            if (value == null)
            {
                return OBJECT;
            }
            return GetColumnType(value.GetType());
        }

        public static IColumnType GetColumnType(Type type)
        {
            IColumnType columnType = GetColumnType0(type);
            if (columnType != null)
            {
                return columnType;
            }
            return OBJECT;
        }

        private static IColumnType GetColumnType0(Type type)
        {
            lock (_types)
            {
                return (IColumnType) _types[type];
            }
        }
    }
}
