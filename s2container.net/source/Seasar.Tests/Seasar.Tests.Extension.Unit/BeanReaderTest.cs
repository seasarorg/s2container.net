#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
using System.Data;
using System.Data.SqlTypes;
using MbUnit.Framework;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.Unit
{
	[TestFixture]
	public class BeanReaderTest
	{
		[Test]
		public void TestReadPrimitiveType()
		{
			BasicTypeBean bean = new BasicTypeBean();
			bean.Id = 1;
			bean.BoolType = true;
			bean.SbyteType = SByte.MaxValue;
			bean.ByteType = Byte.MaxValue;
			bean.Int16Type = Int16.MaxValue;
			bean.Int32Type = Int32.MaxValue;
			bean.Int64Type = Int64.MaxValue;
			bean.DecimalType = Decimal.MaxValue;
			bean.SingleType = Single.MaxValue;
			bean.DoubleType = Double.MaxValue;
			bean.StringType = "abcde";
			bean.DateTimeType = new DateTime(1999, 12, 31);
			
			BeanReader reader = new BeanReader(bean);
			DataSet ds = reader.Read();
			DataTable table = ds.Tables[0];
			DataRow row = table.Rows[0];
			DataColumnCollection columns = table.Columns;

			Assert.AreEqual(DataRowState.Unchanged, row.RowState);
			Assert.AreEqual(13, columns.Count);

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
		public void TestReadNHibernateNullableType()
		{
			NHibernateNullableBasicTypeBean bean = new NHibernateNullableBasicTypeBean();
			bean.Id = 1;
			bean.BoolType = true;
			bean.SbyteType = SByte.MaxValue;
			bean.ByteType = Byte.MaxValue;
			bean.Int16Type = Int16.MaxValue;
			bean.Int32Type = Int32.MaxValue;
			bean.Int64Type = Int64.MaxValue;
			bean.DecimalType = Decimal.MaxValue;
			bean.SingleType = Single.MaxValue;
			bean.DoubleType = Double.MaxValue;
			bean.StringType = "abcde";
			bean.DateTimeType = new DateTime(1999, 12, 31);

			BeanReader reader = new BeanReader(bean);
			DataSet ds = reader.Read();
			DataTable table = ds.Tables[0];
			DataRow row = table.Rows[0];
			DataColumnCollection columns = table.Columns;

			Assert.AreEqual(DataRowState.Unchanged, row.RowState);
			Assert.AreEqual(13, columns.Count);

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
			NHibernateNullableBasicTypeBean bean = new NHibernateNullableBasicTypeBean();
			bean.Id = null;
			bean.BoolType = null;
			bean.SbyteType = null;
			bean.ByteType = null;
			bean.Int16Type = null;
			bean.Int32Type = null;
			bean.Int64Type = null;
			bean.DecimalType = null;
			bean.SingleType = null;
			bean.DoubleType = null;
			bean.StringType = null;
			bean.DateTimeType = null;

			BeanReader reader = new BeanReader(bean);
			DataSet ds = reader.Read();
			DataTable table = ds.Tables[0];
			DataRow row = table.Rows[0];
			DataColumnCollection columns = table.Columns;

			Assert.AreEqual(DataRowState.Unchanged, row.RowState);
			Assert.AreEqual(13, columns.Count);

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

#if !NET_1_1

		[Test]
		public void TestReadNullableType()
		{
			NullableBasicTypeBean bean = new NullableBasicTypeBean();
			bean.Id = 1;
			bean.BoolType = true;
			bean.SbyteType = SByte.MaxValue;
			bean.ByteType = Byte.MaxValue;
			bean.Int16Type = Int16.MaxValue;
			bean.Int32Type = Int32.MaxValue;
			bean.Int64Type = Int64.MaxValue;
			bean.DecimalType = Decimal.MaxValue;
			bean.SingleType = Single.MaxValue;
			bean.DoubleType = Double.MaxValue;
			bean.StringType = "abcde";
			bean.DateTimeType = new DateTime(1999, 12, 31);

			BeanReader reader = new BeanReader(bean);
			DataSet ds = reader.Read();
			DataTable table = ds.Tables[0];
			DataRow row = table.Rows[0];
			DataColumnCollection columns = table.Columns;

			Assert.AreEqual(DataRowState.Unchanged, row.RowState);
			Assert.AreEqual(13, columns.Count);

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
		public void TestReadNullableTypeNullValue()
		{
			NullableBasicTypeBean bean = new NullableBasicTypeBean();
			bean.Id = null;
			bean.BoolType = null;
			bean.SbyteType = null;
			bean.ByteType = null;
			bean.Int16Type = null;
			bean.Int32Type = null;
			bean.Int64Type = null;
			bean.DecimalType = null;
			bean.SingleType = null;
			bean.DoubleType = null;
			bean.StringType = null;
			bean.DateTimeType = null;

			BeanReader reader = new BeanReader(bean);
			DataSet ds = reader.Read();
			DataTable table = ds.Tables[0];
			DataRow row = table.Rows[0];
			DataColumnCollection columns = table.Columns;

			Assert.AreEqual(DataRowState.Unchanged, row.RowState);
			Assert.AreEqual(13, columns.Count);

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
			SqlTypeBasicTypeBean bean = new SqlTypeBasicTypeBean();
			bean.Id = 1;
			bean.BoolType = true;
			bean.ByteType = Byte.MaxValue;
			bean.Int16Type = Int16.MaxValue;
			bean.Int32Type = Int32.MaxValue;
			bean.Int64Type = Int64.MaxValue;
			bean.DecimalType = Decimal.MaxValue;
			bean.SingleType = Single.MaxValue;
			bean.DoubleType = Double.MaxValue;
			bean.StringType = "abcde";
			bean.DateTimeType = new DateTime(1999, 12, 31);

			BeanReader reader = new BeanReader(bean);
			DataSet ds = reader.Read();
			DataTable table = ds.Tables[0];
			DataRow row = table.Rows[0];
			DataColumnCollection columns = table.Columns;

			Assert.AreEqual(DataRowState.Unchanged, row.RowState);
			Assert.AreEqual(13, columns.Count);

			Assert.AreEqual(1, row["id"]);
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
			SqlTypeBasicTypeBean bean = new SqlTypeBasicTypeBean();
			bean.Id = SqlInt64.Null;
			bean.BoolType = SqlBoolean.Null;
			bean.ByteType = SqlByte.Null;
			bean.Int16Type = SqlInt16.Null;
			bean.Int32Type = SqlInt32.Null;
			bean.Int64Type = SqlInt64.Null;
			bean.DecimalType = SqlDecimal.Null;
			bean.SingleType = SqlSingle.Null;
			bean.DoubleType = SqlDouble.Null;
			bean.StringType = SqlString.Null;
			bean.DateTimeType = SqlDateTime.Null;

			BeanReader reader = new BeanReader(bean);
			DataSet ds = reader.Read();
			DataTable table = ds.Tables[0];
			DataRow row = table.Rows[0];
			DataColumnCollection columns = table.Columns;

			Assert.AreEqual(DataRowState.Unchanged, row.RowState);
			Assert.AreEqual(13, columns.Count);

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
