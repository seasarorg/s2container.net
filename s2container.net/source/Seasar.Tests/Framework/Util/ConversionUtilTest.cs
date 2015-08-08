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
using System.Data.SqlTypes;
using MbUnit.Framework;
using Seasar.Framework.Util;

#if NHIBERNATE_NULLABLES
using Nullables;
#endif

namespace Seasar.Tests.Framework.Util
{
    [TestFixture]
    public class ConversionUtilTest
    {
        [Test]
        public void TestConvertSqlTypesNullable_SqlBinary()
        {
            var ret = (SqlBinary) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlBinary));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlBinary) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlBinary));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlBinary) ConversionUtil.ConvertSqlTypesNullable(new byte[] { 1, 2, 3 }, typeof(SqlBinary));
            Assert.AreEqual(new SqlBinary(new byte[] { 1, 2, 3 }), ret, "3");
        }

        [Test]
        public void TestConvertSqlTypesNullable_SqlInt64()
        {
            var ret = (SqlInt64) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlInt64));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlInt64) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlInt64));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlInt64) ConversionUtil.ConvertSqlTypesNullable(12345, typeof(SqlInt64));
            Assert.AreEqual(new SqlInt64(12345), ret, "3");
        }

        [Test]
        public void TestConvertSqlTypesNullable_SqlString()
        {
            var ret = (SqlString) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlString));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlString) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlString));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlString) ConversionUtil.ConvertSqlTypesNullable("aiueo", typeof(SqlString));
            Assert.AreEqual(new SqlString("aiueo"), ret, "3");
        }

        [Test]
        public void TestConvertSqlTypesNullable_SqlDateTime()
        {
            var ret = (SqlDateTime) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlDateTime));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlDateTime) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlDateTime));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlDateTime) ConversionUtil.ConvertSqlTypesNullable(new DateTime(2006, 3, 14), typeof(SqlDateTime));
            Assert.AreEqual(new SqlDateTime(new DateTime(2006, 3, 14)), ret, "3");
        }

        [Test]
        public void TestConvertSqlTypesNullable_SqlDecimal()
        {
            var ret = (SqlDecimal) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlDecimal));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlDecimal) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlDecimal));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlDecimal) ConversionUtil.ConvertSqlTypesNullable(3, typeof(SqlDecimal));
            Assert.AreEqual(new SqlDecimal(3), ret, "3");
        }

        [Test]
        public void TestConvertSqlTypesNullable_SqlDouble()
        {
            var ret = (SqlDouble) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlDouble));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlDouble) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlDouble));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlDouble) ConversionUtil.ConvertSqlTypesNullable(3.1415, typeof(SqlDouble));
            Assert.AreEqual(new SqlDouble(3.1415), ret, "3");
        }

        [Test]
        public void TestConvertSqlTypesNullable_SqlInt32()
        {
            var ret = (SqlInt32) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlInt32));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlInt32) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlInt32));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlInt32) ConversionUtil.ConvertSqlTypesNullable(10310712, typeof(SqlInt32));
            Assert.AreEqual(new SqlInt32(10310712), ret, "3");
        }

        [Test]
        public void TestConvertSqlTypesNullable_SqlMoney()
        {
            var ret = (SqlMoney) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlMoney));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlMoney) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlMoney));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlMoney) ConversionUtil.ConvertSqlTypesNullable(10310712, typeof(SqlMoney));
            Assert.AreEqual(new SqlMoney(10310712), ret, "3");
        }

        [Test]
        public void TestConvertSqlTypesNullable_SqlSingle()
        {
            var ret = (SqlSingle) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlSingle));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlSingle) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlSingle));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlSingle) ConversionUtil.ConvertSqlTypesNullable(250.1, typeof(SqlSingle));
            Assert.AreEqual(new SqlSingle(250.1), ret, "3");
        }

        [Test]
        public void TestConvertSqlTypesNullable_SqlInt16()
        {
            var ret = (SqlInt16) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlInt16));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlInt16) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlInt16));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlInt16) ConversionUtil.ConvertSqlTypesNullable(30140, typeof(SqlInt16));
            Assert.AreEqual(new SqlInt16(30140), ret, "3");
        }

        [Test]
        public void TestConvertSqlTypesNullable_SqlByte()
        {
            var ret = (SqlByte) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlByte));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlByte) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlByte));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlByte) ConversionUtil.ConvertSqlTypesNullable(254, typeof(SqlByte));
            Assert.AreEqual(new SqlByte(254), ret, "3");
        }

        [Test]
        public void TestConvertSqlTypesNullable_SqlGuid()
        {
            var ret = (SqlGuid) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlGuid));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlGuid) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlGuid));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlGuid) ConversionUtil.ConvertSqlTypesNullable(
                new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4"), typeof(SqlGuid));
            Assert.AreEqual(new SqlGuid(new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4")), ret, "3");
        }

        [Test]
        public void TestConvertSqlTypesNullable_SqlBoolean()
        {
            var ret = (SqlBoolean) ConversionUtil.ConvertSqlTypesNullable(DBNull.Value, typeof(SqlBoolean));
            Assert.IsTrue(ret.IsNull, "1");
            ret = (SqlBoolean) ConversionUtil.ConvertSqlTypesNullable(null, typeof(SqlBoolean));
            Assert.IsTrue(ret.IsNull, "2");
            ret = (SqlBoolean) ConversionUtil.ConvertSqlTypesNullable(true, typeof(SqlBoolean));
            Assert.AreEqual(new SqlBoolean(true), ret, "3");
        }

#if NHIBERNATE_NULLABLES
        [Test]
        public void TestConvertNHibernateNullable_NullableBoolean()
        {
            NullableBoolean ret = (NullableBoolean) ConversionUtil.ConvertNHibernateNullable(
                DBNull.Value, typeof(NullableBoolean));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableBoolean) ConversionUtil.ConvertNHibernateNullable(null, typeof(NullableBoolean));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableBoolean) ConversionUtil.ConvertNHibernateNullable(true, typeof(NullableBoolean));
            Assert.AreEqual(new NullableBoolean(true), ret, "3");
        }

        [Test]
        public void TestConvertNHibernateNullable_NullableByte()
        {
            NullableByte ret = (NullableByte) ConversionUtil.ConvertNHibernateNullable(
                DBNull.Value, typeof(NullableByte));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableByte) ConversionUtil.ConvertNHibernateNullable(null, typeof(NullableByte));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableByte) ConversionUtil.ConvertNHibernateNullable(240, typeof(NullableByte));
            Assert.AreEqual(new NullableByte(240), ret, "3");
        }

        [Test]
        public void TestConvertNHibernateNullable_NullableChar()
        {
            NullableChar ret = (NullableChar) ConversionUtil.ConvertNHibernateNullable(
                DBNull.Value, typeof(NullableChar));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableChar) ConversionUtil.ConvertNHibernateNullable(null, typeof(NullableChar));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableChar) ConversionUtil.ConvertNHibernateNullable('s', typeof(NullableChar));
            Assert.AreEqual(new NullableChar('s'), ret, "3");
        }

        [Test]
        public void TestConvertNHibernateNullable_NullableDateTime()
        {
            NullableDateTime ret = (NullableDateTime) ConversionUtil.ConvertNHibernateNullable(
                DBNull.Value, typeof(NullableDateTime));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableDateTime) ConversionUtil.ConvertNHibernateNullable(null, typeof(NullableDateTime));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableDateTime) ConversionUtil.ConvertNHibernateNullable(
                new DateTime(2006, 4, 1), typeof(NullableDateTime));
            Assert.AreEqual(new NullableDateTime(new DateTime(2006, 4, 1)), ret, "3");
        }

        [Test]
        public void TestConvertNHibernateNullable_NullableDecimal()
        {
            NullableDecimal ret = (NullableDecimal) ConversionUtil.ConvertNHibernateNullable(
                DBNull.Value, typeof(NullableDecimal));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableDecimal) ConversionUtil.ConvertNHibernateNullable(null, typeof(NullableDecimal));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableDecimal) ConversionUtil.ConvertNHibernateNullable(122345, typeof(NullableDecimal));
            Assert.AreEqual(new NullableDecimal(122345), ret, "3");
        }

        [Test]
        public void TestConvertNHibernateNullable_NullableDouble()
        {
            NullableDouble ret = (NullableDouble) ConversionUtil.ConvertNHibernateNullable(
                DBNull.Value, typeof(NullableDouble));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableDouble) ConversionUtil.ConvertNHibernateNullable(null, typeof(NullableDouble));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableDouble) ConversionUtil.ConvertNHibernateNullable(3.1415, typeof(NullableDouble));
            Assert.AreEqual(new NullableDouble(3.1415), ret, "3");
        }

        [Test]
        public void TestConvertNHibernateNullable_NullableGuid()
        {
            NullableGuid ret = (NullableGuid) ConversionUtil.ConvertNHibernateNullable(
                DBNull.Value, typeof(NullableGuid));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableGuid) ConversionUtil.ConvertNHibernateNullable(null, typeof(NullableGuid));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableGuid) ConversionUtil.ConvertNHibernateNullable(
                new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4"), typeof(NullableGuid));
            Assert.AreEqual(new NullableGuid(new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4")), ret, "3");
        }

        [Test]
        public void TestConvertNHibernateNullable_NullableInt16()
        {
            NullableInt16 ret = (NullableInt16) ConversionUtil.ConvertNHibernateNullable(
                DBNull.Value, typeof(NullableInt16));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableInt16) ConversionUtil.ConvertNHibernateNullable(null, typeof(NullableInt16));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableInt16) ConversionUtil.ConvertNHibernateNullable(1031, typeof(NullableInt16));
            Assert.AreEqual(new NullableInt16(1031), ret, "3");
        }

        [Test]
        public void TestConvertNHibernateNullable_NullableInt32()
        {
            NullableInt32 ret = (NullableInt32) ConversionUtil.ConvertNHibernateNullable(
                DBNull.Value, typeof(NullableInt32));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableInt32) ConversionUtil.ConvertNHibernateNullable(null, typeof(NullableInt32));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableInt32) ConversionUtil.ConvertNHibernateNullable(1031, typeof(NullableInt32));
            Assert.AreEqual(new NullableInt32(1031), ret, "3");
        }

        [Test]
        public void TestConvertNHibernateNullable_NullableInt64()
        {
            NullableInt64 ret = (NullableInt64) ConversionUtil.ConvertNHibernateNullable(
                DBNull.Value, typeof(NullableInt64));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableInt64) ConversionUtil.ConvertNHibernateNullable(null, typeof(NullableInt64));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableInt64) ConversionUtil.ConvertNHibernateNullable(1031, typeof(NullableInt64));
            Assert.AreEqual(new NullableInt64(1031), ret, "3");
        }

        [Test]
        public void TestConvertNHibernateNullable_NullableSByte()
        {
            NullableSByte ret = (NullableSByte) ConversionUtil.ConvertNHibernateNullable(
                DBNull.Value, typeof(NullableSByte));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableSByte) ConversionUtil.ConvertNHibernateNullable(null, typeof(NullableSByte));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableSByte) ConversionUtil.ConvertNHibernateNullable(112, typeof(NullableSByte));
            Assert.AreEqual(new NullableSByte(112), ret, "3");
        }

        [Test]
        public void TestConvertNHibernateNullable_NullableSingle()
        {
            NullableSingle ret = (NullableSingle) ConversionUtil.ConvertNHibernateNullable(
                DBNull.Value, typeof(NullableSingle));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (NullableSingle) ConversionUtil.ConvertNHibernateNullable(null, typeof(NullableSingle));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (NullableSingle) ConversionUtil.ConvertNHibernateNullable(1.2f, typeof(NullableSingle));
            Assert.AreEqual(new NullableSingle(1.2f), ret, "3");
        }
#endif

#if !NET_1_1

        [Test]
        public void TestConvertNullable_Boolean()
        {
            var ret = (bool?) ConversionUtil.ConvertNullable(
                DBNull.Value, typeof(bool?));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (bool?) ConversionUtil.ConvertNullable(null, typeof(bool?));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (bool?) ConversionUtil.ConvertNullable(true, typeof(bool?));
            Assert.AreEqual(new bool?(true), ret, "3");
        }

        [Test]
        public void TestConvertNullable_Int32()
        {
            var ret = (int?) ConversionUtil.ConvertNullable(
                DBNull.Value, typeof(int?));
            Assert.IsFalse(ret.HasValue, "1");
            ret = (int?) ConversionUtil.ConvertNullable(null, typeof(int?));
            Assert.IsFalse(ret.HasValue, "2");
            ret = (int?) ConversionUtil.ConvertNullable(77, typeof(int?));
            Assert.AreEqual(new int?(77), ret, "3");
        }

#endif
    }
}
