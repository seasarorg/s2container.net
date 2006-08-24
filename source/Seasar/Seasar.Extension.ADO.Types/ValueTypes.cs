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

		public readonly static IValueType NHIBERNATE_NULLABLE_CHAR = new NHibernateNullableCharType();
		public readonly static IValueType NHIBERNATE_NULLABLE_BYTE = new NHibernateNullableByteType();
		public readonly static IValueType NHIBERNATE_NULLABLE_SBYTE = new NHibernateNullableSByteType();
		public readonly static IValueType NHIBERNATE_NULLABLE_INT16 = new NHibernateNullableInt16Type();
		public readonly static IValueType NHIBERNATE_NULLABLE_INT32 = new NHibernateNullableInt32Type();
		public readonly static IValueType NHIBERNATE_NULLABLE_INT64 = new NHibernateNullableInt64Type();
		public readonly static IValueType NHIBERNATE_NULLABLE_SINGLE = new NHibernateNullableSingleType();
		public readonly static IValueType NHIBERNATE_NULLABLE_DOUBLE = new NHibernateNullableDoubleType();
		public readonly static IValueType NHIBERNATE_NULLABLE_DECIMAL = new NHibernateNullableDecimalType();
		public readonly static IValueType NHIBERNATE_NULLABLE_DATETIME = new NHibernateNullableDateTimeType();
		public readonly static IValueType NHIBERNATE_NULLABLE_BINARY = new NHibernateNullableBinaryType();
		public readonly static IValueType NHIBERNATE_NULLABLE_BOOLEAN = new NHibernateNullableBooleanType();
		public readonly static IValueType NHIBERNATE_NULLABLE_GUID = new NHibernateNullableGuidType();

		public readonly static IValueType SQL_STRING = new SqlStringType();
		public readonly static IValueType SQL_BYTE = new SqlByteType();
		public readonly static IValueType SQL_INT16 = new SqlInt16Type();
		public readonly static IValueType SQL_INT32 = new SqlInt32Type();
		public readonly static IValueType SQL_INT64 = new SqlInt64Type();
		public readonly static IValueType SQL_SINGLE = new SqlSingleType();
		public readonly static IValueType SQL_DOUBLE = new SqlDoubleType();
		public readonly static IValueType SQL_DECIMAL = new SqlDecimalType();
		public readonly static IValueType SQL_DATETIME = new SqlDateTimeType();
		public readonly static IValueType SQL_BINARY = new SqlBinaryType();
		public readonly static IValueType SQL_BOOLEAN = new SqlBooleanType();
		public readonly static IValueType SQL_GUID = new SqlGuidType();
		
		private readonly static byte[] BYTE_ARRAY = new byte[0];
		private readonly static NullableByte[] NHIBERNATE_NULLABLE_BYTE_ARRAY = new NullableByte[0];

#if !NET_1_1
        public readonly static IValueType NULLABLE_BYTE = new NullableByteType();
		public readonly static IValueType NULLABLE_SBYTE = new NullableSByteType();
		public readonly static IValueType NULLABLE_INT16 = new NullableInt16Type();
		public readonly static IValueType NULLABLE_INT32 = new NullableInt32Type();
		public readonly static IValueType NULLABLE_INT64 = new NullableInt64Type();
		public readonly static IValueType NULLABLE_SINGLE = new NullableSingleType();
		public readonly static IValueType NULLABLE_DOUBLE = new NullableDoubleType();
		public readonly static IValueType NULLABLE_DECIMAL = new NullableDecimalType();
		public readonly static IValueType NULLABLE_DATETIME = new NullableDateTimeType();
		public readonly static IValueType NULLABLE_BINARY = new NullableBinaryType();
		public readonly static IValueType NULLABLE_BOOLEAN = new NullableBooleanType();
		public readonly static IValueType NULLABLE_GUID = new NullableGuidType();

        private readonly static Nullable<Byte>[] NULLABLE_BYTE_ARRAY = new Nullable<Byte>[0];

        public readonly static Type NULLABLE_BYTE_ARRAY_TYPE = NULLABLE_BYTE_ARRAY.GetType();
#endif
		
		public readonly static Type BYTE_ARRAY_TYPE = BYTE_ARRAY.GetType();
		public readonly static Type NHIBERNATE_NULLABLE_BYTE_ARRAY_TYPE = NHIBERNATE_NULLABLE_BYTE_ARRAY.GetType();

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

			RegisterValueType(typeof(SqlString), SQL_STRING);
			RegisterValueType(typeof(SqlByte), SQL_BYTE);
			RegisterValueType(typeof(SqlInt16), SQL_INT16);
			RegisterValueType(typeof(SqlInt32), SQL_INT32);
			RegisterValueType(typeof(SqlInt64), SQL_INT64);
			RegisterValueType(typeof(SqlSingle), SQL_SINGLE);
			RegisterValueType(typeof(SqlDouble), SQL_DOUBLE);
			RegisterValueType(typeof(SqlDecimal), SQL_DECIMAL);
			RegisterValueType(typeof(SqlDateTime), SQL_DATETIME);
			RegisterValueType(typeof(SqlBinary), SQL_BINARY);
			RegisterValueType(typeof(SqlBoolean), SQL_BOOLEAN);
			RegisterValueType(typeof(SqlMoney), SQL_DECIMAL);
			RegisterValueType(typeof(SqlGuid), SQL_GUID);

			RegisterValueType(typeof(NullableChar), NHIBERNATE_NULLABLE_CHAR);
			RegisterValueType(typeof(NullableByte), NHIBERNATE_NULLABLE_BYTE);
			RegisterValueType(typeof(NullableSByte), NHIBERNATE_NULLABLE_SBYTE);
			RegisterValueType(typeof(NullableInt16), NHIBERNATE_NULLABLE_INT16);
			RegisterValueType(typeof(NullableInt32), NHIBERNATE_NULLABLE_INT32);
			RegisterValueType(typeof(NullableInt64), NHIBERNATE_NULLABLE_INT64);
			RegisterValueType(typeof(NullableSingle), NHIBERNATE_NULLABLE_SINGLE);
			RegisterValueType(typeof(NullableDouble), NHIBERNATE_NULLABLE_DOUBLE);
			RegisterValueType(typeof(NullableDecimal), NHIBERNATE_NULLABLE_DECIMAL);
			RegisterValueType(typeof(NullableDateTime), NHIBERNATE_NULLABLE_DATETIME);
			RegisterValueType(NHIBERNATE_NULLABLE_BYTE_ARRAY_TYPE, NHIBERNATE_NULLABLE_BINARY);
			RegisterValueType(typeof(NullableBoolean), NHIBERNATE_NULLABLE_BOOLEAN);
			RegisterValueType(typeof(NullableGuid), NHIBERNATE_NULLABLE_GUID);

#if !NET_1_1
            RegisterValueType(NULLABLE_BYTE_ARRAY_TYPE, NULLABLE_BINARY);
			RegisterValueType(typeof(Nullable<Byte>), NULLABLE_BYTE);
			RegisterValueType(typeof(Nullable<SByte>), NULLABLE_SBYTE);
			RegisterValueType(typeof(Nullable<Int16>), NULLABLE_INT16);
			RegisterValueType(typeof(Nullable<Int32>), NULLABLE_INT32);
			RegisterValueType(typeof(Nullable<Int64>), NULLABLE_INT64);
			RegisterValueType(typeof(Nullable<Single>), NULLABLE_SINGLE);
			RegisterValueType(typeof(Nullable<Double>), NULLABLE_DOUBLE);
			RegisterValueType(typeof(Nullable<Decimal>), NULLABLE_DECIMAL);
			RegisterValueType(typeof(Nullable<DateTime>), NULLABLE_DATETIME);
			RegisterValueType(typeof(Nullable<Boolean>), NULLABLE_BOOLEAN);
			RegisterValueType(typeof(Nullable<Guid>), NULLABLE_GUID);
#endif

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
