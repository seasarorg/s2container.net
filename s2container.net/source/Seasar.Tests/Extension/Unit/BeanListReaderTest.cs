#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using MbUnit.Framework;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.Unit
{
    [TestFixture]
    public class BeanListReaderTest
    {
        [Test]
        public void TestRead()
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

            IList list = new ArrayList();
            list.Add(bean);

            BeanListReader reader = new BeanListReader(list);
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
    }
}
