#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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

namespace Seasar.Extension.ADO.Types
{
    public sealed class ValueTypes
    {
		public readonly static IValueType STRING = new StringType();
		public readonly static IValueType BYTE = new ByteType();
		public readonly static IValueType SBYTE = new SByteType();
		public readonly static IValueType INT16 = new Int16Type();
		public readonly static IValueType INT32 = new Int32Type();
		public readonly static IValueType INT64 = new Int64Type();
		public readonly static IValueType SINGLE = new SingleType();
		public readonly static IValueType DOUBLE = new DoubleType();
		public readonly static IValueType DECIMAL = new DecimalType();
		public readonly static IValueType DATETIME = new DateTimeType();
		public readonly static IValueType BINARY = new BinaryType();
		public readonly static IValueType BOOLEAN = new BooleanType();
		public readonly static IValueType GUID = new GuidType();
		public readonly static IValueType OBJECT = new ObjectType();

        private readonly static byte[] BYTE_ARRAY = new byte[0];
        private readonly static NullableByte[] NULLABLE_BYTE_ARRAY = new NullableByte[0];
        public readonly static Type BYTE_ARRAY_TYPE = BYTE_ARRAY.GetType();
        public readonly static Type NULLABLE_BYTE_ARRAY_TYPE = NULLABLE_BYTE_ARRAY.GetType();
        private static Hashtable types = new Hashtable();

		static ValueTypes()
        {
            RegisterValueType(typeof(String), STRING);
            RegisterValueType(typeof(Byte), BYTE);
            RegisterValueType(typeof(SByte), SBYTE);
            RegisterValueType(typeof(Int16), INT16);
            RegisterValueType(typeof(Int32), INT32);
            RegisterValueType(typeof(Int64), INT64);
            RegisterValueType(typeof(Single), SINGLE);
            RegisterValueType(typeof(Double), DOUBLE);
            RegisterValueType(typeof(Decimal),DECIMAL);
            RegisterValueType(typeof(DateTime), DATETIME);
            RegisterValueType(BYTE_ARRAY_TYPE, BINARY);
            RegisterValueType(typeof(Boolean), BOOLEAN);
            RegisterValueType(typeof(Guid), GUID);

            RegisterValueType(typeof(SqlString), STRING);
            RegisterValueType(typeof(SqlByte), BYTE);
            RegisterValueType(typeof(SqlInt16), INT16);
            RegisterValueType(typeof(SqlInt32), INT32);
            RegisterValueType(typeof(SqlInt64), INT64);
            RegisterValueType(typeof(SqlSingle), SINGLE);
            RegisterValueType(typeof(SqlDouble), DOUBLE);
            RegisterValueType(typeof(SqlDecimal), DECIMAL);
            RegisterValueType(typeof(SqlDateTime), DATETIME);
            RegisterValueType(typeof(SqlBinary), BINARY);
            RegisterValueType(typeof(SqlBoolean), BOOLEAN);
            RegisterValueType(typeof(SqlMoney), DECIMAL);
            RegisterValueType(typeof(SqlGuid), GUID);

            RegisterValueType(typeof(NullableChar), STRING);
            RegisterValueType(typeof(NullableByte), BYTE);
            RegisterValueType(typeof(NullableSByte), SBYTE);
            RegisterValueType(typeof(NullableInt16), INT16);
            RegisterValueType(typeof(NullableInt32), INT32);
            RegisterValueType(typeof(NullableInt64), INT64);
            RegisterValueType(typeof(NullableSingle), SINGLE);
            RegisterValueType(typeof(NullableDouble), DOUBLE);
            RegisterValueType(typeof(NullableDecimal), DECIMAL);
            RegisterValueType(typeof(NullableDateTime), DATETIME);
            RegisterValueType(NULLABLE_BYTE_ARRAY_TYPE, BINARY);
            RegisterValueType(typeof(NullableBoolean), BOOLEAN);
            RegisterValueType(typeof(NullableGuid), GUID);

        }

		private ValueTypes()
		{
		}

		[Obsolete("メソッドを呼び出す必要がなくなりました。")]
		public static void Init(IDataSource dataSource)
		{
		}

        public static void RegisterValueType(Type type, IValueType valueType)
        {
            lock(types)
            {
                types[type] = valueType;
            }
        }

        public static IValueType GetValueType(object obj)
        {
            if(obj == null) return OBJECT;
            return GetValueType(obj.GetType());
        }

        public static IValueType GetValueType(Type type)
        {
            if(type == null) return OBJECT;
            IValueType valueType = GetValueType0(type);
            if(valueType != null) return valueType;
            return OBJECT;
        }

        private static IValueType GetValueType0(Type type)
        {
            lock(types)
            {
                return (IValueType) types[type];
            }
        }

        public static IValueType GetValueType(DbType type)
        {
            switch(type)
            {
                case DbType.Byte :
                    return GetValueType(typeof(Byte));
                case DbType.SByte :
                    return GetValueType(typeof(SByte));
                case DbType.Int16 :
                    return GetValueType(typeof(Int16));
                case DbType.Int32 :
                    return GetValueType(typeof(Int32));
                case DbType.Int64 : 
                    return GetValueType(typeof(Int64));
                case DbType.Single :
                    return GetValueType(typeof(Single));
                case DbType.Double :
                    return GetValueType(typeof(Double));
                case DbType.Decimal :
                case DbType.VarNumeric :
                    return GetValueType(typeof(Decimal));
                case DbType.Date :
                case DbType.Time :
                case DbType.DateTime :
                    return GetValueType(typeof(DateTime));
                case DbType.Binary :
                    return GetValueType(BYTE_ARRAY_TYPE);
                case DbType.String :
                case DbType.StringFixedLength :
                    return GetValueType(typeof(String));
                case DbType.Boolean :
                    return GetValueType(typeof(Boolean));
                case DbType.Guid :
                    return GetValueType(typeof(Guid));
                default :
                    return OBJECT;
            }
        }
    }
}
