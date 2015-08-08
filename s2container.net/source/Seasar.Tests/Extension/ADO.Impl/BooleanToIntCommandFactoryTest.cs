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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Extension.ADO.Impl
{
    [TestFixture]
    public class BooleanToIntCommandFactoryTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Ado.dicon";

        static BooleanToIntCommandFactoryTest()
        {
            var info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        public void SetUpExecuteNonQuery()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void ExecuteNonQuery()
        {
            var bean = new BasicTypeBean();
            bean.Id = 9999;
            bean.BoolType = true;
            // ODP.NET 10.2.0.100 でSByteをセットすると、「System.ArgumentException: 値が有効な範囲にありません。」が発生するのでコメントアウト。
            // bean.SbyteType = SByte.MaxValue;
            bean.ByteType = Byte.MaxValue;
            bean.Int16Type = Int16.MaxValue;
            bean.Int32Type = Int32.MaxValue;
            bean.Int64Type = Int64.MaxValue;
            bean.DecimalType = Decimal.MaxValue;
            bean.SingleType = 3.14f;
            bean.DoubleType = 6.28f;
            bean.StringType = "abcde";
            bean.DateTimeType = new DateTime(1999, 12, 31, 1, 2, 3);

            var sql = "INSERT INTO basictype (id, booltype, bytetype, int16type, int32type, int64type, singletype, doubletype, decimaltype, stringtype, datetimetype) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            var handler = new BasicUpdateHandler(DataSource, sql, BooleanToIntCommandFactory.INSTANCE);
            var args = new object[] { bean.Id, bean.BoolType, bean.ByteType, bean.Int16Type, bean.Int32Type, bean.Int64Type, bean.DecimalType, bean.SingleType, bean.DoubleType, bean.StringType, bean.DateTimeType };
            var ret = handler.Execute(args);
            Assert.AreEqual(1, ret, "1");
        }

        public void SetUpExecuteReader()
        {
            Include(PATH);
        }

        [Test, S2]
        public void ExecuteReader()
        {
            var handler = new BasicSelectHandler(
                DataSource,
                "SELECT * FROM basictype WHERE id = ?",
                new BeanDataReaderHandler(typeof(BasicTypeBean))
                );
            var ret = (BasicTypeBean) handler.Execute(new object[] { 1 });
            Trace.WriteLine(ToStringUtil.ToString(ret));
            Assert.IsFalse(ret.BoolType, "1");
            Assert.AreEqual(int.MaxValue, ret.Int32Type, "2");

            var retTrue = (BasicTypeBean) handler.Execute(new object[] { 3 });
            Trace.WriteLine(ToStringUtil.ToString(ret));
            Assert.IsTrue(retTrue.BoolType, "3");
            Assert.AreEqual(5, retTrue.Int32Type, "4");
        }
    }
}
