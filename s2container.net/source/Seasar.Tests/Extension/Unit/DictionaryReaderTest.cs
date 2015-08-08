#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
using MbUnit.Framework;
using Seasar.Extension.Unit;

#if NHIBERNATE_NULLABLES
using Nullables;
#endif

namespace Seasar.Tests.Extension.Unit
{
    [TestFixture]
    public class DictionaryReaderTest
    {
        [Test]
        public void TestReadPrimitiveType()
        {
            IDictionary dictionary = new Hashtable();
            dictionary.Add("Id", 1L);
            dictionary.Add("BoolType", true);
            dictionary.Add("SbyteType", SByte.MaxValue);
            dictionary.Add("ByteType", Byte.MaxValue);
            dictionary.Add("Int16Type", Int16.MaxValue);
            dictionary.Add("Int32Type", Int32.MaxValue);
            dictionary.Add("Int64Type", Int64.MaxValue);
            dictionary.Add("DecimalType", Decimal.MaxValue);
            dictionary.Add("SingleType", Single.MaxValue);
            dictionary.Add("DoubleType", Double.MaxValue);
            dictionary.Add("StringType", "abcde");
            dictionary.Add("DateTimeType", new DateTime(1999, 12, 31));

            var reader = new DictionaryReader(dictionary);
            var ds = reader.Read();
            var table = ds.Tables[0];
            var row = table.Rows[0];
            var columns = table.Columns;

            Assert.AreEqual(DataRowState.Unchanged, row.RowState);
            Assert.AreEqual(12, columns.Count);

            // 数値で比較すると32bit/64bit環境で実行結果が変わるので文字列で比較
            Assert.AreEqual("1", row["id"].ToString());
            Assert.AreEqual(true, row["booltype"]);
            Assert.AreEqual(SByte.MaxValue, row["sbytetype"]);
            Assert.AreEqual(Byte.MaxValue, row["bytetype"]);
            Assert.AreEqual(Int16.MaxValue, row["int16type"]);
            Assert.AreEqual(Int32.MaxValue, row["int32type"]);
            Assert.AreEqual(Int64.MaxValue, row["int64type"]);
            Assert.AreEqual(Decimal.MaxValue, row["decimaltype"]);
            Assert.AreEqual(Single.MaxValue, row["singletype"]);
            Assert.AreEqual(Double.MaxValue, row["doubletype"]);
            Assert.AreEqual("abcde", row["stringtype"]);
            Assert.AreEqual(new DateTime(1999, 12, 31), row["datetimetype"]);

            Assert.AreEqual(typeof(long), columns["id"].DataType);
            Assert.AreEqual(typeof(bool), columns["booltype"].DataType);
            Assert.AreEqual(typeof(sbyte), columns["sbytetype"].DataType);
            Assert.AreEqual(typeof(byte), columns["bytetype"].DataType);
            Assert.AreEqual(typeof(short), columns["int16type"].DataType);
            Assert.AreEqual(typeof(int), columns["int32type"].DataType);
            Assert.AreEqual(typeof(long), columns["int64type"].DataType);
            Assert.AreEqual(typeof(decimal), columns["decimaltype"].DataType);
            Assert.AreEqual(typeof(float), columns["singletype"].DataType);
            Assert.AreEqual(typeof(double), columns["doubletype"].DataType);
            Assert.AreEqual(typeof(string), columns["stringtype"].DataType);
            Assert.AreEqual(typeof(DateTime), columns["datetimetype"].DataType);
        }

#if NHIBERNATE_NULLABLES
        [Test]
        public void TestReadNHibernateNullableType()
        {
            IDictionary dictionary = new Hashtable();
            dictionary.Add("Id", new NullableInt64(1L));
            dictionary.Add("BoolType", new NullableBoolean(true));
            dictionary.Add("SbyteType", new NullableSByte(SByte.MaxValue));
            dictionary.Add("ByteType", new NullableByte(Byte.MaxValue));
            dictionary.Add("Int16Type", new NullableInt16(Int16.MaxValue));
            dictionary.Add("Int32Type", new NullableInt32(Int32.MaxValue));
            dictionary.Add("Int64Type", new NullableInt64(Int64.MaxValue));
            dictionary.Add("DecimalType", new NullableDecimal(Decimal.MaxValue));
            dictionary.Add("SingleType", new NullableSingle(Single.MaxValue));
            dictionary.Add("DoubleType", new NullableDouble(Double.MaxValue));
            dictionary.Add("StringType", "abcde");
            dictionary.Add("DateTimeType", new NullableDateTime(new DateTime(1999, 12, 31)));

            DictionaryReader reader = new DictionaryReader(dictionary);
            DataSet ds = reader.Read();
            DataTable table = ds.Tables[0];
            DataRow row = table.Rows[0];
            DataColumnCollection columns = table.Columns;

            Assert.AreEqual(DataRowState.Unchanged, row.RowState);
            Assert.AreEqual(12, columns.Count);

            Assert.AreEqual(1, row["id"]);
            Assert.AreEqual(true, row["booltype"]);
            Assert.AreEqual(SByte.MaxValue, row["sbytetype"]);
            Assert.AreEqual(Byte.MaxValue, row["bytetype"]);
            Assert.AreEqual(Int16.MaxValue, row["int16type"]);
            Assert.AreEqual(Int32.MaxValue, row["int32type"]);
            Assert.AreEqual(Int64.MaxValue, row["int64type"]);
            Assert.AreEqual(Decimal.MaxValue, row["decimaltype"]);
            Assert.AreEqual(Single.MaxValue, row["singletype"]);
            Assert.AreEqual(Double.MaxValue, row["doubletype"]);
            Assert.AreEqual("abcde", row["stringtype"]);
            Assert.AreEqual(new DateTime(1999, 12, 31), row["datetimetype"]);

            Assert.AreEqual(typeof(long), columns["id"].DataType);
            Assert.AreEqual(typeof(bool), columns["booltype"].DataType);
            Assert.AreEqual(typeof(sbyte), columns["sbytetype"].DataType);
            Assert.AreEqual(typeof(byte), columns["bytetype"].DataType);
            Assert.AreEqual(typeof(short), columns["int16type"].DataType);
            Assert.AreEqual(typeof(int), columns["int32type"].DataType);
            Assert.AreEqual(typeof(long), columns["int64type"].DataType);
            Assert.AreEqual(typeof(decimal), columns["decimaltype"].DataType);
            Assert.AreEqual(typeof(float), columns["singletype"].DataType);
            Assert.AreEqual(typeof(double), columns["doubletype"].DataType);
            Assert.AreEqual(typeof(string), columns["stringtype"].DataType);
            Assert.AreEqual(typeof(DateTime), columns["datetimetype"].DataType);
        }

        [Test]
        public void TestReadNHibernateNullableTypeNullValue()
        {
            IDictionary dictionary = new Hashtable();
            dictionary.Add("Id", new NullableInt64());
            dictionary.Add("BoolType", new NullableBoolean());
            dictionary.Add("SbyteType", new NullableSByte());
            dictionary.Add("ByteType", new NullableByte());
            dictionary.Add("Int16Type", new NullableInt16());
            dictionary.Add("Int32Type", new NullableInt32());
            dictionary.Add("Int64Type", new NullableInt64());
            dictionary.Add("DecimalType", new NullableDecimal());
            dictionary.Add("SingleType", new NullableSingle());
            dictionary.Add("DoubleType", new NullableDouble());
            dictionary.Add("StringType", null);
            dictionary.Add("DateTimeType", new NullableDateTime());

            DictionaryReader reader = new DictionaryReader(dictionary);
            DataSet ds = reader.Read();
            DataTable table = ds.Tables[0];
            DataRow row = table.Rows[0];
            DataColumnCollection columns = table.Columns;

            Assert.AreEqual(DataRowState.Unchanged, row.RowState);
            Assert.AreEqual(12, columns.Count);

            Assert.AreEqual(DBNull.Value, row["id"]);
            Assert.AreEqual(DBNull.Value, row["booltype"]);
            Assert.AreEqual(DBNull.Value, row["sbytetype"]);
            Assert.AreEqual(DBNull.Value, row["bytetype"]);
            Assert.AreEqual(DBNull.Value, row["int16type"]);
            Assert.AreEqual(DBNull.Value, row["int32type"]);
            Assert.AreEqual(DBNull.Value, row["int64type"]);
            Assert.AreEqual(DBNull.Value, row["decimaltype"]);
            Assert.AreEqual(DBNull.Value, row["singletype"]);
            Assert.AreEqual(DBNull.Value, row["doubletype"]);
            Assert.AreEqual(DBNull.Value, row["stringtype"]);
            Assert.AreEqual(DBNull.Value, row["datetimetype"]);

            Assert.AreEqual(typeof(long), columns["id"].DataType);
            Assert.AreEqual(typeof(bool), columns["booltype"].DataType);
            Assert.AreEqual(typeof(sbyte), columns["sbytetype"].DataType);
            Assert.AreEqual(typeof(byte), columns["bytetype"].DataType);
            Assert.AreEqual(typeof(short), columns["int16type"].DataType);
            Assert.AreEqual(typeof(int), columns["int32type"].DataType);
            Assert.AreEqual(typeof(long), columns["int64type"].DataType);
            Assert.AreEqual(typeof(decimal), columns["decimaltype"].DataType);
            Assert.AreEqual(typeof(float), columns["singletype"].DataType);
            Assert.AreEqual(typeof(double), columns["doubletype"].DataType);
            Assert.AreEqual(typeof(string), columns["stringtype"].DataType);
            Assert.AreEqual(typeof(DateTime), columns["datetimetype"].DataType);
        }
#endif

#if !NET_1_1

        [Test]
        public void TestReadNullableType()
        {
            IDictionary dictionary = new Hashtable();
            dictionary.Add("Id", new long?(1L));
            dictionary.Add("BoolType", new bool?(true));
            dictionary.Add("SbyteType", new sbyte?(SByte.MaxValue));
            dictionary.Add("ByteType", new byte?(Byte.MaxValue));
            dictionary.Add("Int16Type", new short?(Int16.MaxValue));
            dictionary.Add("Int32Type", new int?(Int32.MaxValue));
            dictionary.Add("Int64Type", new long?(Int64.MaxValue));
            dictionary.Add("DecimalType", new decimal?(Decimal.MaxValue));
            dictionary.Add("SingleType", new float?(Single.MaxValue));
            dictionary.Add("DoubleType", new double?(Double.MaxValue));
            dictionary.Add("StringType", "abcde");
            dictionary.Add("DateTimeType", new DateTime?(new DateTime(1999, 12, 31)));

            var reader = new DictionaryReader(dictionary);
            var ds = reader.Read();
            var table = ds.Tables[0];
            var row = table.Rows[0];
            var columns = table.Columns;

            Assert.AreEqual(DataRowState.Unchanged, row.RowState);
            Assert.AreEqual(12, columns.Count);

            // 数値で比較すると32bit/64bit環境で実行結果が変わるので文字列で比較
            Assert.AreEqual("1", row["id"].ToString());
            Assert.AreEqual(true, row["booltype"]);
            Assert.AreEqual(SByte.MaxValue, row["sbytetype"]);
            Assert.AreEqual(Byte.MaxValue, row["bytetype"]);
            Assert.AreEqual(Int16.MaxValue, row["int16type"]);
            Assert.AreEqual(Int32.MaxValue, row["int32type"]);
            Assert.AreEqual(Int64.MaxValue, row["int64type"]);
            Assert.AreEqual(Decimal.MaxValue, row["decimaltype"]);
            Assert.AreEqual(Single.MaxValue, row["singletype"]);
            Assert.AreEqual(Double.MaxValue, row["doubletype"]);
            Assert.AreEqual("abcde", row["stringtype"]);
            Assert.AreEqual(new DateTime(1999, 12, 31), row["datetimetype"]);

            Assert.AreEqual(typeof(long), columns["id"].DataType);
            Assert.AreEqual(typeof(bool), columns["booltype"].DataType);
            Assert.AreEqual(typeof(sbyte), columns["sbytetype"].DataType);
            Assert.AreEqual(typeof(byte), columns["bytetype"].DataType);
            Assert.AreEqual(typeof(short), columns["int16type"].DataType);
            Assert.AreEqual(typeof(int), columns["int32type"].DataType);
            Assert.AreEqual(typeof(long), columns["int64type"].DataType);
            Assert.AreEqual(typeof(decimal), columns["decimaltype"].DataType);
            Assert.AreEqual(typeof(float), columns["singletype"].DataType);
            Assert.AreEqual(typeof(double), columns["doubletype"].DataType);
            Assert.AreEqual(typeof(string), columns["stringtype"].DataType);
            Assert.AreEqual(typeof(DateTime), columns["datetimetype"].DataType);
        }

        [Test]
        [Ignore("Nullable<T>.Value")]
        public void TestReadNullableTypeNullValue()
        {
            IDictionary dictionary = new Hashtable();
            dictionary.Add("Id", new long?());
            dictionary.Add("BoolType", new bool?());
            dictionary.Add("SbyteType", new sbyte?());
            dictionary.Add("ByteType", new byte?());
            dictionary.Add("Int16Type", new short?());
            dictionary.Add("Int32Type", new int?());
            dictionary.Add("Int64Type", new long?());
            dictionary.Add("DecimalType", new decimal?());
            dictionary.Add("SingleType", new float?());
            dictionary.Add("DoubleType", new double?());
            dictionary.Add("StringType", null);
            dictionary.Add("DateTimeType", new DateTime?());

            var reader = new DictionaryReader(dictionary);
            var ds = reader.Read();
            var table = ds.Tables[0];
            var row = table.Rows[0];
            var columns = table.Columns;

            Assert.AreEqual(DataRowState.Unchanged, row.RowState);
            Assert.AreEqual(12, columns.Count);

            Assert.AreEqual(DBNull.Value, row["id"]);
            Assert.AreEqual(DBNull.Value, row["booltype"]);
            Assert.AreEqual(DBNull.Value, row["sbytetype"]);
            Assert.AreEqual(DBNull.Value, row["bytetype"]);
            Assert.AreEqual(DBNull.Value, row["int16type"]);
            Assert.AreEqual(DBNull.Value, row["int32type"]);
            Assert.AreEqual(DBNull.Value, row["int64type"]);
            Assert.AreEqual(DBNull.Value, row["decimaltype"]);
            Assert.AreEqual(DBNull.Value, row["singletype"]);
            Assert.AreEqual(DBNull.Value, row["doubletype"]);
            Assert.AreEqual(DBNull.Value, row["stringtype"]);
            Assert.AreEqual(DBNull.Value, row["datetimetype"]);

            Assert.AreEqual(typeof(long), columns["id"].DataType);
            Assert.AreEqual(typeof(bool), columns["booltype"].DataType);
            Assert.AreEqual(typeof(sbyte), columns["sbytetype"].DataType);
            Assert.AreEqual(typeof(byte), columns["bytetype"].DataType);
            Assert.AreEqual(typeof(short), columns["int16type"].DataType);
            Assert.AreEqual(typeof(int), columns["int32type"].DataType);
            Assert.AreEqual(typeof(long), columns["int64type"].DataType);
            Assert.AreEqual(typeof(decimal), columns["decimaltype"].DataType);
            Assert.AreEqual(typeof(float), columns["singletype"].DataType);
            Assert.AreEqual(typeof(double), columns["doubletype"].DataType);
            Assert.AreEqual(typeof(string), columns["stringtype"].DataType);
            Assert.AreEqual(typeof(DateTime), columns["datetimetype"].DataType);
        }

#endif

        [Test]
        public void TestReadSqlType()
        {
            IDictionary dictionary = new Hashtable();
            dictionary.Add("Id", new SqlInt64(1L));
            dictionary.Add("BoolType", new SqlBoolean(true));
            dictionary.Add("ByteType", new SqlByte(Byte.MaxValue));
            dictionary.Add("Int16Type", new SqlInt16(Int16.MaxValue));
            dictionary.Add("Int32Type", new SqlInt32(Int32.MaxValue));
            dictionary.Add("Int64Type", new SqlInt64(Int64.MaxValue));
            dictionary.Add("DecimalType", new SqlDecimal(Decimal.MaxValue));
            dictionary.Add("SingleType", new SqlSingle(Single.MaxValue));
            dictionary.Add("DoubleType", new SqlDouble(Double.MaxValue));
            dictionary.Add("StringType", new SqlString("abcde"));
            dictionary.Add("DateTimeType", new SqlDateTime(new DateTime(1999, 12, 31)));

            var reader = new DictionaryReader(dictionary);
            var ds = reader.Read();
            var table = ds.Tables[0];
            var row = table.Rows[0];
            var columns = table.Columns;

            Assert.AreEqual(DataRowState.Unchanged, row.RowState);
            Assert.AreEqual(11, columns.Count);

            // 数値で比較すると32bit/64bit環境で実行結果が変わるので文字列で比較
            Assert.AreEqual("1", row["id"].ToString());
            Assert.AreEqual(true, row["booltype"]);
            Assert.AreEqual(Byte.MaxValue, row["bytetype"]);
            Assert.AreEqual(Int16.MaxValue, row["int16type"]);
            Assert.AreEqual(Int32.MaxValue, row["int32type"]);
            Assert.AreEqual(Int64.MaxValue, row["int64type"]);
            Assert.AreEqual(Decimal.MaxValue, row["decimaltype"]);
            Assert.AreEqual(Single.MaxValue, row["singletype"]);
            Assert.AreEqual(Double.MaxValue, row["doubletype"]);
            Assert.AreEqual("abcde", row["stringtype"]);
            Assert.AreEqual(new DateTime(1999, 12, 31), row["datetimetype"]);

            Assert.AreEqual(typeof(long), columns["id"].DataType);
            Assert.AreEqual(typeof(bool), columns["booltype"].DataType);
            Assert.AreEqual(typeof(byte), columns["bytetype"].DataType);
            Assert.AreEqual(typeof(short), columns["int16type"].DataType);
            Assert.AreEqual(typeof(int), columns["int32type"].DataType);
            Assert.AreEqual(typeof(long), columns["int64type"].DataType);
            Assert.AreEqual(typeof(decimal), columns["decimaltype"].DataType);
            Assert.AreEqual(typeof(float), columns["singletype"].DataType);
            Assert.AreEqual(typeof(double), columns["doubletype"].DataType);
            Assert.AreEqual(typeof(string), columns["stringtype"].DataType);
            Assert.AreEqual(typeof(DateTime), columns["datetimetype"].DataType);
        }

        [Test]
        public void TestReadSqlTypeNullValue()
        {
            IDictionary dictionary = new Hashtable();
            dictionary.Add("Id", SqlInt64.Null);
            dictionary.Add("BoolType", SqlBoolean.Null);
            dictionary.Add("ByteType", SqlByte.Null);
            dictionary.Add("Int16Type", SqlInt16.Null);
            dictionary.Add("Int32Type", SqlInt32.Null);
            dictionary.Add("Int64Type", SqlInt64.Null);
            dictionary.Add("DecimalType", SqlDecimal.Null);
            dictionary.Add("SingleType", SqlSingle.Null);
            dictionary.Add("DoubleType", SqlDouble.Null);
            dictionary.Add("StringType", SqlString.Null);
            dictionary.Add("DateTimeType", SqlDateTime.Null);

            var reader = new DictionaryReader(dictionary);
            var ds = reader.Read();
            var table = ds.Tables[0];
            var row = table.Rows[0];
            var columns = table.Columns;

            Assert.AreEqual(DataRowState.Unchanged, row.RowState);
            Assert.AreEqual(11, columns.Count);

            Assert.AreEqual(DBNull.Value, row["id"]);
            Assert.AreEqual(DBNull.Value, row["booltype"]);
            Assert.AreEqual(DBNull.Value, row["bytetype"]);
            Assert.AreEqual(DBNull.Value, row["int16type"]);
            Assert.AreEqual(DBNull.Value, row["int32type"]);
            Assert.AreEqual(DBNull.Value, row["int64type"]);
            Assert.AreEqual(DBNull.Value, row["decimaltype"]);
            Assert.AreEqual(DBNull.Value, row["singletype"]);
            Assert.AreEqual(DBNull.Value, row["doubletype"]);
            Assert.AreEqual(DBNull.Value, row["stringtype"]);
            Assert.AreEqual(DBNull.Value, row["datetimetype"]);

            Assert.AreEqual(typeof(long), columns["id"].DataType);
            Assert.AreEqual(typeof(bool), columns["booltype"].DataType);
            Assert.AreEqual(typeof(byte), columns["bytetype"].DataType);
            Assert.AreEqual(typeof(short), columns["int16type"].DataType);
            Assert.AreEqual(typeof(int), columns["int32type"].DataType);
            Assert.AreEqual(typeof(long), columns["int64type"].DataType);
            Assert.AreEqual(typeof(decimal), columns["decimaltype"].DataType);
            Assert.AreEqual(typeof(float), columns["singletype"].DataType);
            Assert.AreEqual(typeof(double), columns["doubletype"].DataType);
            Assert.AreEqual(typeof(string), columns["stringtype"].DataType);
            Assert.AreEqual(typeof(DateTime), columns["datetimetype"].DataType);
        }
    }
}
