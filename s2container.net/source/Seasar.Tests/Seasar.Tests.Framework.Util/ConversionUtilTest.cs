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
using System.Data.SqlTypes;
using NUnit.Framework;
using Seasar.Framework.Util;
using Nullables;

namespace TestSeasar.Framework.Util
{
    [TestFixture]
	public class ConversionUtilTest
	{
        [Test]
        public void TestConvertNullable_SqlBinary()
        {
            SqlBinary ret = (SqlBinary) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlBinary));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlBinary) ConversionUtil.ConvertNullable(null, typeof(SqlBinary));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlBinary) ConversionUtil.ConvertNullable(new byte[] { 1, 2, 3}, typeof(SqlBinary));
            Assert.AreEqual(new SqlBinary(new byte[] { 1, 2, 3}), ret, "3");
        }

        [Test]
        public void TestConvertNullable_SqlInt64()
        {
            SqlInt64 ret = (SqlInt64) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlInt64));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlInt64) ConversionUtil.ConvertNullable(null, typeof(SqlInt64));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlInt64) ConversionUtil.ConvertNullable(12345, typeof(SqlInt64));
            Assert.AreEqual(new SqlInt64(12345), ret, "3");
        }

        [Test]
        public void TestConvertNullable_SqlString()
        {
            SqlString ret = (SqlString) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlString));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlString) ConversionUtil.ConvertNullable(null, typeof(SqlString));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlString) ConversionUtil.ConvertNullable("aiueo", typeof(SqlString));
            Assert.AreEqual(new SqlString("aiueo"), ret, "3");
        }

        [Test]
        public void TestConvertNullable_SqlDateTime()
        {
            SqlDateTime ret = (SqlDateTime) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlDateTime));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlDateTime) ConversionUtil.ConvertNullable(null, typeof(SqlDateTime));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlDateTime) ConversionUtil.ConvertNullable(new DateTime(2006, 3, 14), typeof(SqlDateTime));
            Assert.AreEqual(new SqlDateTime(new DateTime(2006, 3, 14)), ret, "3");
        }

        [Test]
        public void TestConvertNullable_SqlDecimal()
        {
            SqlDecimal ret = (SqlDecimal) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlDecimal));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlDecimal) ConversionUtil.ConvertNullable(null, typeof(SqlDecimal));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlDecimal) ConversionUtil.ConvertNullable(3, typeof(SqlDecimal));
            Assert.AreEqual(new SqlDecimal(3), ret, "3");
        }

        [Test]
        public void TestConvertNullable_SqlDouble()
        {
            SqlDouble ret = (SqlDouble) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlDouble));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlDouble) ConversionUtil.ConvertNullable(null, typeof(SqlDouble));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlDouble) ConversionUtil.ConvertNullable(3.1415, typeof(SqlDouble));
            Assert.AreEqual(new SqlDouble(3.1415), ret, "3");
        }

        [Test]
        public void TestConvertNullable_SqlInt32()
        {
            SqlInt32 ret = (SqlInt32) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlInt32));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlInt32) ConversionUtil.ConvertNullable(null, typeof(SqlInt32));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlInt32) ConversionUtil.ConvertNullable(10310712, typeof(SqlInt32));
            Assert.AreEqual(new SqlInt32(10310712), ret, "3");
        }

        [Test]
        public void TestConvertNullable_SqlMoney()
        {
            SqlMoney ret = (SqlMoney) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlMoney));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlMoney) ConversionUtil.ConvertNullable(null, typeof(SqlMoney));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlMoney) ConversionUtil.ConvertNullable(10310712, typeof(SqlMoney));
            Assert.AreEqual(new SqlMoney(10310712), ret, "3");
        }

        [Test]
        public void TestConvertNullable_SqlSingle()
        {
            SqlSingle ret = (SqlSingle) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlSingle));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlSingle) ConversionUtil.ConvertNullable(null, typeof(SqlSingle));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlSingle) ConversionUtil.ConvertNullable(250.1, typeof(SqlSingle));
            Assert.AreEqual(new SqlSingle(250.1), ret, "3");
        }

        [Test]
        public void TestConvertNullable_SqlInt16()
        {
            SqlInt16 ret = (SqlInt16) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlInt16));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlInt16) ConversionUtil.ConvertNullable(null, typeof(SqlInt16));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlInt16) ConversionUtil.ConvertNullable(30140, typeof(SqlInt16));
            Assert.AreEqual(new SqlInt16(30140), ret, "3");
        }
        
        [Test]
        public void TestConvertNullable_SqlByte()
        {
            SqlByte ret = (SqlByte) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlByte));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlByte) ConversionUtil.ConvertNullable(null, typeof(SqlByte));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlByte) ConversionUtil.ConvertNullable(254, typeof(SqlByte));
            Assert.AreEqual(new SqlByte(254), ret, "3");
        }

        [Test]
        public void TestConvertNullable_SqlGuid()
        {
            SqlGuid ret = (SqlGuid) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlGuid));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlGuid) ConversionUtil.ConvertNullable(null, typeof(SqlGuid));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlGuid) ConversionUtil.ConvertNullable(
                new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4"), typeof(SqlGuid));
            Assert.AreEqual(new SqlGuid(new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4")), ret, "3");
        }

        [Test]
        public void TestConvertNullable_SqlBoolean()
        {
            SqlBoolean ret = (SqlBoolean) ConversionUtil.ConvertNullable(DBNull.Value, typeof(SqlBoolean));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlBoolean) ConversionUtil.ConvertNullable(null, typeof(SqlBoolean));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlBoolean) ConversionUtil.ConvertNullable(true, typeof(SqlBoolean));
            Assert.AreEqual(new SqlBoolean(true), ret, "3");
        }

        [Test]
        public void TestConvertNullableType_NullableBoolean()
        {
            NullableBoolean ret = (NullableBoolean) ConversionUtil.ConvertNullableType(
                DBNull.Value, typeof(NullableBoolean));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableBoolean) ConversionUtil.ConvertNullableType(null, typeof(NullableBoolean));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableBoolean) ConversionUtil.ConvertNullableType(true, typeof(NullableBoolean));
            Assert.AreEqual(new NullableBoolean(true), ret, "3");
        }

        [Test]
        public void TestConvertNullableType_NullableByte()
        {
            NullableByte ret = (NullableByte) ConversionUtil.ConvertNullableType(
                DBNull.Value, typeof(NullableByte));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableByte) ConversionUtil.ConvertNullableType(null, typeof(NullableByte));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableByte) ConversionUtil.ConvertNullableType(240, typeof(NullableByte));
            Assert.AreEqual(new NullableByte(240), ret, "3");
        }

        [Test]
        public void TestConvertNullableType_NullableChar()
        {
            NullableChar ret = (NullableChar) ConversionUtil.ConvertNullableType(
                DBNull.Value, typeof(NullableChar));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableChar) ConversionUtil.ConvertNullableType(null, typeof(NullableChar));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableChar) ConversionUtil.ConvertNullableType('s', typeof(NullableChar));
            Assert.AreEqual(new NullableChar('s'), ret, "3");
        }

        [Test]
        public void TestConvertNullableType_NullableDateTime()
        {
            NullableDateTime ret = (NullableDateTime) ConversionUtil.ConvertNullableType(
                DBNull.Value, typeof(NullableDateTime));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableDateTime) ConversionUtil.ConvertNullableType(null, typeof(NullableDateTime));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableDateTime) ConversionUtil.ConvertNullableType(
                new DateTime(2006, 4, 1), typeof(NullableDateTime));
            Assert.AreEqual(new NullableDateTime(new DateTime(2006, 4, 1)), ret, "3");
        }

        [Test]
        public void TestConvertNullableType_NullableDecimal()
        {
            NullableDecimal ret = (NullableDecimal) ConversionUtil.ConvertNullableType(
                DBNull.Value, typeof(NullableDecimal));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableDecimal) ConversionUtil.ConvertNullableType(null, typeof(NullableDecimal));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableDecimal) ConversionUtil.ConvertNullableType(122345, typeof(NullableDecimal));
            Assert.AreEqual(new NullableDecimal(122345), ret, "3");
        }

        [Test]
        public void TestConvertNullableType_NullableDouble()
        {
            NullableDouble ret = (NullableDouble) ConversionUtil.ConvertNullableType(
                DBNull.Value, typeof(NullableDouble));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableDouble) ConversionUtil.ConvertNullableType(null, typeof(NullableDouble));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableDouble) ConversionUtil.ConvertNullableType(3.1415, typeof(NullableDouble));
            Assert.AreEqual(new NullableDouble(3.1415), ret, "3");
        }

        [Test]
        public void TestConvertNullableType_NullableGuid()
        {
            NullableGuid ret = (NullableGuid) ConversionUtil.ConvertNullableType(
                DBNull.Value, typeof(NullableGuid));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableGuid) ConversionUtil.ConvertNullableType(null, typeof(NullableGuid));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableGuid) ConversionUtil.ConvertNullableType(
                new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4"), typeof(NullableGuid));
            Assert.AreEqual(new NullableGuid(new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4")), ret, "3");
        }

        [Test]
        public void TestConvertNullableType_NullableInt16()
        {
            NullableInt16 ret = (NullableInt16) ConversionUtil.ConvertNullableType(
                DBNull.Value, typeof(NullableInt16));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableInt16) ConversionUtil.ConvertNullableType(null, typeof(NullableInt16));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableInt16) ConversionUtil.ConvertNullableType(1031, typeof(NullableInt16));
            Assert.AreEqual(new NullableInt16(1031), ret, "3");
        }

        [Test]
        public void TestConvertNullableType_NullableInt32()
        {
            NullableInt32 ret = (NullableInt32) ConversionUtil.ConvertNullableType(
                DBNull.Value, typeof(NullableInt32));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableInt32) ConversionUtil.ConvertNullableType(null, typeof(NullableInt32));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableInt32) ConversionUtil.ConvertNullableType(1031, typeof(NullableInt32));
            Assert.AreEqual(new NullableInt32(1031), ret, "3");
        }

        [Test]
        public void TestConvertNullableType_NullableInt64()
        {
            NullableInt64 ret = (NullableInt64) ConversionUtil.ConvertNullableType(
                DBNull.Value, typeof(NullableInt64));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableInt64) ConversionUtil.ConvertNullableType(null, typeof(NullableInt64));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableInt64) ConversionUtil.ConvertNullableType(1031, typeof(NullableInt64));
            Assert.AreEqual(new NullableInt64(1031), ret, "3");
        }

        [Test]
        public void TestConvertNullableType_NullableSByte()
        {
            NullableSByte ret = (NullableSByte) ConversionUtil.ConvertNullableType(
                DBNull.Value, typeof(NullableSByte));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableSByte) ConversionUtil.ConvertNullableType(null, typeof(NullableSByte));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableSByte) ConversionUtil.ConvertNullableType(112, typeof(NullableSByte));
            Assert.AreEqual(new NullableSByte(112), ret, "3");
        }

        [Test]
        public void TestConvertNullableType_NullableSingle()
        {
            NullableSingle ret = (NullableSingle) ConversionUtil.ConvertNullableType(
                DBNull.Value, typeof(NullableSingle));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableSingle) ConversionUtil.ConvertNullableType(null, typeof(NullableSingle));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableSingle) ConversionUtil.ConvertNullableType(1.2f, typeof(NullableSingle));
            Assert.AreEqual(new NullableSingle(1.2f), ret, "3");
        }
	}
}
