#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.ADO.Impl
{
    [TestFixture]
    public class BeanDataReaderHandlerTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Ado.dicon";

        private ICommandFactory _commandFactory = null;

        private IDataReaderFactory _dataReaderFactory = null;

        public void SetUpHandle()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void Handle()
        {
            IDataReaderHandler handler = new BeanDataReaderHandler(typeof(Employee));
            string sql = "select * from emp where empno = 7788";
            IDbCommand cmd = _commandFactory.CreateCommand(Connection, sql);
            IDataReader reader = _dataReaderFactory.CreateDataReader(DataSource, cmd);
            Employee ret;
            try
            {
                ret = (Employee) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }
            Assert.IsNotNull(ret, "1");
            Trace.WriteLine(ret.Empno + "," + ret.Ename);
        }

        public void SetUpHandle2()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void Handle2()
        {
            IDataReaderHandler handler = new BeanDataReaderHandler(typeof(Employee));
            string sql = "select ename, job from emp where empno = 7788";
            IDbCommand cmd = _commandFactory.CreateCommand(Connection, sql);
            IDataReader reader = _dataReaderFactory.CreateDataReader(DataSource, cmd);
            Employee ret;
            try
            {
                ret = (Employee) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }
            Assert.IsNotNull(ret, "1");
            Trace.WriteLine(ret.Empno + "," + ret.Ename);
        }

        public void SetUpHandlePrimitiveType()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void HandlePrimitiveType()
        {
            IDataReaderHandler handler = new BeanDataReaderHandler(typeof(BasicTypeBean));
            string sql = "select * from basictype where id = 1";
            IDbCommand cmd = _commandFactory.CreateCommand(Connection, sql);
            IDataReader reader = _dataReaderFactory.CreateDataReader(DataSource, cmd);
            BasicTypeBean ret;
            try
            {
                ret = (BasicTypeBean) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }

            Assert.AreEqual(1, ret.Id);
            Assert.AreEqual(true, ret.BoolType);
            Assert.AreEqual(SByte.MinValue, ret.SbyteType);
            Assert.AreEqual(Byte.MaxValue, ret.ByteType);
            Assert.AreEqual(Int16.MaxValue, ret.Int16Type);
            Assert.AreEqual(Int32.MaxValue, ret.Int32Type);
            Assert.AreEqual(Int64.MaxValue, ret.Int64Type);
            Assert.AreEqual(9999999999999999999999999999m, ret.DecimalType);
            Assert.AreEqual(9.876543, ret.SingleType);
            Assert.AreEqual(9.87654321098765, ret.DoubleType);
            Assert.AreEqual(@"í|\ñ˜Å`", ret.StringType);
            Assert.AreEqual(new DateTime(1980, 12, 17, 12, 34, 56), ret.DateTimeType);
        }

#if NHIBERNATE_NULLABLES
        public void SetUpHandleNHibernateNullableType()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void HandleNHibernateNullableType()
        {
            IDataReaderHandler handler = new BeanDataReaderHandler(typeof(NHibernateNullableBasicTypeBean));
            string sql = "select * from basictype where id = 1";
            IDbCommand cmd = _commandFactory.CreateCommand(Connection, sql);
            IDataReader reader = _dataReaderFactory.CreateDataReader(DataSource, cmd);
            NHibernateNullableBasicTypeBean ret;
            try
            {
                ret = (NHibernateNullableBasicTypeBean) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }

            Assert.AreEqual(1, ret.Id.Value);
            Assert.AreEqual(true, ret.BoolType.Value);
            Assert.AreEqual(SByte.MinValue, ret.SbyteType.Value);
            Assert.AreEqual(Byte.MaxValue, ret.ByteType.Value);
            Assert.AreEqual(Int16.MaxValue, ret.Int16Type.Value);
            Assert.AreEqual(Int32.MaxValue, ret.Int32Type.Value);
            Assert.AreEqual(Int64.MaxValue, ret.Int64Type.Value);
            Assert.AreEqual(9999999999999999999999999999m, ret.DecimalType.Value);
            Assert.AreEqual(9.876543, ret.SingleType.Value);
            Assert.AreEqual(9.87654321098765, ret.DoubleType.Value);
            Assert.AreEqual(@"í|\ñ˜Å`", ret.StringType);
            Assert.AreEqual(new DateTime(1980, 12, 17, 12, 34, 56), ret.DateTimeType.Value);
        }

        public void SetUpHandleNHibernateNullableTypeNullValue()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void HandleNHibernateNullableTypeNullValue()
        {
            IDataReaderHandler handler = new BeanDataReaderHandler(typeof(NHibernateNullableBasicTypeBean));
            string sql = "select * from basictype where id = 2";
            IDbCommand cmd = _commandFactory.CreateCommand(Connection, sql);
            IDataReader reader = _dataReaderFactory.CreateDataReader(DataSource, cmd);
            NHibernateNullableBasicTypeBean ret;
            try
            {
                ret = (NHibernateNullableBasicTypeBean) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }

            Assert.AreEqual(2, ret.Id.Value);
            Assert.IsFalse(ret.BoolType.HasValue);
            Assert.IsFalse(ret.SbyteType.HasValue);
            Assert.IsFalse(ret.ByteType.HasValue);
            Assert.IsFalse(ret.Int16Type.HasValue);
            Assert.IsFalse(ret.Int32Type.HasValue);
            Assert.IsFalse(ret.Int64Type.HasValue);
            Assert.IsFalse(ret.DecimalType.HasValue);
            Assert.IsFalse(ret.SingleType.HasValue);
            Assert.IsFalse(ret.DoubleType.HasValue);
            Assert.IsNull(ret.StringType);
            Assert.IsFalse(ret.DateTimeType.HasValue);
        }
#endif

#if !NET_1_1
        public void SetUpHandleNullableType()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void HandleNullableType()
        {
            IDataReaderHandler handler = new BeanDataReaderHandler(typeof(NullableBasicTypeBean));
            string sql = "select * from basictype where id = 1";
            IDbCommand cmd = _commandFactory.CreateCommand(Connection, sql);
            IDataReader reader = _dataReaderFactory.CreateDataReader(DataSource, cmd);
            NullableBasicTypeBean ret;
            try
            {
                ret = (NullableBasicTypeBean) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }

            Assert.AreEqual(1, ret.Id.Value);
            Assert.AreEqual(true, ret.BoolType.Value);
            Assert.AreEqual(SByte.MinValue, ret.SbyteType.Value);
            Assert.AreEqual(Byte.MaxValue, ret.ByteType.Value);
            Assert.AreEqual(Int16.MaxValue, ret.Int16Type.Value);
            Assert.AreEqual(Int32.MaxValue, ret.Int32Type.Value);
            Assert.AreEqual(Int64.MaxValue, ret.Int64Type.Value);
            Assert.AreEqual(9999999999999999999999999999m, ret.DecimalType.Value);
            Assert.AreEqual(9.876543, ret.SingleType.Value);
            Assert.AreEqual(9.87654321098765, ret.DoubleType.Value);
            Assert.AreEqual(@"í|\ñ˜Å`", ret.StringType);
            Assert.AreEqual(new DateTime(1980, 12, 17, 12, 34, 56), ret.DateTimeType.Value);
        }

        public void SetUpHandleNullableTypeNullValue()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void HandleNullableTypeNullValue()
        {
            IDataReaderHandler handler = new BeanDataReaderHandler(typeof(NullableBasicTypeBean));
            string sql = "select * from basictype where id = 2";
            IDbCommand cmd = _commandFactory.CreateCommand(Connection, sql);
            IDataReader reader = _dataReaderFactory.CreateDataReader(DataSource, cmd);
            NullableBasicTypeBean ret;
            try
            {
                ret = (NullableBasicTypeBean) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }

            Assert.AreEqual(2, ret.Id.Value);
            Assert.IsFalse(ret.BoolType.HasValue);
            Assert.IsFalse(ret.SbyteType.HasValue);
            Assert.IsFalse(ret.ByteType.HasValue);
            Assert.IsFalse(ret.Int16Type.HasValue);
            Assert.IsFalse(ret.Int32Type.HasValue);
            Assert.IsFalse(ret.Int64Type.HasValue);
            Assert.IsFalse(ret.DecimalType.HasValue);
            Assert.IsFalse(ret.SingleType.HasValue);
            Assert.IsFalse(ret.DoubleType.HasValue);
            Assert.IsNull(ret.StringType);
            Assert.IsFalse(ret.DateTimeType.HasValue);
        }

#endif

        public void SetUpHandleSqlType()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void HandleSqlType()
        {
            IDataReaderHandler handler = new BeanDataReaderHandler(typeof(SqlTypeBasicTypeBean));
            string sql = "select * from basictype where id = 1";
            IDbCommand cmd = _commandFactory.CreateCommand(Connection, sql);
            IDataReader reader = _dataReaderFactory.CreateDataReader(DataSource, cmd);
            SqlTypeBasicTypeBean ret;
            try
            {
                ret = (SqlTypeBasicTypeBean) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }

            Assert.AreEqual(1, ret.Id.Value);
            Assert.AreEqual(true, ret.BoolType.Value);
            Assert.AreEqual(SByte.MinValue, ret.SbyteType, "SqlTypeÇ≈ÇÕSByteÇÉTÉ|Å[ÉgÇµÇƒÇ¢Ç»Ç¢ÅB");
            Assert.AreEqual(Byte.MaxValue, ret.ByteType.Value);
            Assert.AreEqual(Int16.MaxValue, ret.Int16Type.Value);
            Assert.AreEqual(Int32.MaxValue, ret.Int32Type.Value);
            Assert.AreEqual(Int64.MaxValue, ret.Int64Type.Value);
            Assert.AreEqual(9999999999999999999999999999m, ret.DecimalType.Value);
            Assert.AreEqual(9.876543, ret.SingleType.Value);
            Assert.AreEqual(9.87654321098765, ret.DoubleType.Value);
            Assert.AreEqual(@"í|\ñ˜Å`", ret.StringType.Value);
            Assert.AreEqual(new DateTime(1980, 12, 17, 12, 34, 56), ret.DateTimeType.Value);
        }

        public void SetUpHandleSqlTypeNullValue()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void HandleSqlTypeNullValue()
        {
            IDataReaderHandler handler = new BeanDataReaderHandler(typeof(SqlTypeBasicTypeBean));
            string sql = "select * from basictype where id = 2";
            IDbCommand cmd = _commandFactory.CreateCommand(Connection, sql);
            IDataReader reader = _dataReaderFactory.CreateDataReader(DataSource, cmd);
            SqlTypeBasicTypeBean ret;
            try
            {
                ret = (SqlTypeBasicTypeBean) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }

            Assert.AreEqual(2, ret.Id.Value);
            Assert.IsTrue(ret.BoolType.IsNull);
            Assert.IsTrue(ret.ByteType.IsNull);
            Assert.IsTrue(ret.Int16Type.IsNull);
            Assert.IsTrue(ret.Int32Type.IsNull);
            Assert.IsTrue(ret.Int64Type.IsNull);
            Assert.IsTrue(ret.DecimalType.IsNull);
            Assert.IsTrue(ret.SingleType.IsNull);
            Assert.IsTrue(ret.DoubleType.IsNull);
            Assert.IsTrue(ret.StringType.IsNull);
            Assert.IsTrue(ret.DateTimeType.IsNull);
        }

        public void SetUpHandleEnumType()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void HandleEnumType()
        {
            IDataReaderHandler handler = new BeanDataReaderHandler(typeof(EnumTypeBean));
            string sql = "select id, bytetype, sbytetype, int16type, int32type, int64type, singletype, doubletype, decimaltype, stringtype  from basictype where id = 3";
            IDbCommand cmd = _commandFactory.CreateCommand(Connection, sql);
            IDataReader reader = _dataReaderFactory.CreateDataReader(DataSource, cmd);
            EnumTypeBean ret;
            try
            {
                ret = (EnumTypeBean) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }

            Assert.AreEqual(3, ret.Id);
            Assert.AreEqual(Numbers.TWO, ret.ByteType);
            Assert.AreEqual(Numbers.THREE, ret.SbyteType);
            Assert.AreEqual(Numbers.FOUR, ret.Int16Type);
            Assert.AreEqual(Numbers.FIVE, ret.Int32Type);
            Assert.AreEqual(Numbers.SIX, ret.Int64Type);
            Assert.AreEqual(Numbers.SEVEN, ret.SingleType);
            Assert.AreEqual(Numbers.EIGHT, ret.DoubleType);
            Assert.AreEqual(Numbers.NINE, ret.DecimalType);
            Assert.AreEqual(Numbers.TEN, ret.StringType);
        }

#if !NET_1_1

        public void SetUpHandleNullableEnumType()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void HandleNullableEnumType()
        {
            IDataReaderHandler handler = new BeanDataReaderHandler(typeof(NullableEnumTypeBean));
            string sql = "select id, bytetype, sbytetype, int16type, int32type, int64type, singletype, doubletype, decimaltype, stringtype  from basictype where id = 3";
            IDbCommand cmd = _commandFactory.CreateCommand(Connection, sql);
            IDataReader reader = _dataReaderFactory.CreateDataReader(DataSource, cmd);
            NullableEnumTypeBean ret;
            try
            {
                ret = (NullableEnumTypeBean) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }

            Assert.AreEqual(3, ret.Id);
            Assert.AreEqual(Numbers.TWO, ret.ByteType);
            Assert.AreEqual(Numbers.THREE, ret.SbyteType);
            Assert.AreEqual(Numbers.FOUR, ret.Int16Type);
            Assert.AreEqual(Numbers.FIVE, ret.Int32Type);
            Assert.AreEqual(Numbers.SIX, ret.Int64Type);
            Assert.AreEqual(Numbers.SEVEN, ret.SingleType);
            Assert.AreEqual(Numbers.EIGHT, ret.DoubleType);
            Assert.AreEqual(Numbers.NINE, ret.DecimalType);
            Assert.AreEqual(Numbers.TEN, ret.StringType);
        }

        public void SetUpHandleNullableEnumTypeNullValue()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void HandleNullableEnumTypeNullValue()
        {
            IDataReaderHandler handler = new BeanDataReaderHandler(typeof(NullableEnumTypeBean));
            string sql = "select id, bytetype, sbytetype, int16type, int32type, int64type, singletype, doubletype, decimaltype, stringtype  from basictype where id = 2";
            IDbCommand cmd = _commandFactory.CreateCommand(Connection, sql);
            IDataReader reader = _dataReaderFactory.CreateDataReader(DataSource, cmd);
            NullableEnumTypeBean ret;
            try
            {
                ret = (NullableEnumTypeBean) handler.Handle(reader);
            }
            finally
            {
                reader.Close();
            }

            Assert.AreEqual(2, ret.Id.Value);
            Assert.IsFalse(ret.SbyteType.HasValue);
            Assert.IsFalse(ret.ByteType.HasValue);
            Assert.IsFalse(ret.Int16Type.HasValue);
            Assert.IsFalse(ret.Int32Type.HasValue);
            Assert.IsFalse(ret.Int64Type.HasValue);
            Assert.IsFalse(ret.DecimalType.HasValue);
            Assert.IsFalse(ret.SingleType.HasValue);
            Assert.IsFalse(ret.DoubleType.HasValue);
            Assert.IsNull(ret.StringType);
        }

#endif
    }
}
