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

using System.Data;
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Extension.DataSets.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Extension.DataSets.Impl
{
    [TestFixture]
    public class SqlServerSqlTableWriterTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Ado.dicon";

        public void SetUpWriteIdentityTable()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void WriteIdentityTable()
        {
            if (DataSource.GetCommand().GetType().Name.Equals("SqlCommand"))
            {
                DataTable table = new DataTable("idtable");
                table.Columns.Add("id", typeof(int));
                table.Columns.Add("id_name", typeof(string));
                DataRow row = table.NewRow();
                row["id"] = 9900;
                row["id_name"] = "hoge";
                table.Rows.Add(row);

                SqlServerSqlTableWriter writer = new SqlServerSqlTableWriter(DataSource);

                writer.Write(table);

                SqlTableReader reader = new SqlTableReader(DataSource);
                reader.SetTable("idtable", "id = 9900");
                DataTable ret = reader.Read();
                Trace.WriteLine(ToStringUtil.ToString(ret));
                Assert.IsNotNull(ret, "1");
            }
        }

        public void SetUpWriteNotIdentityTable()
        {
            Include(PATH);
        }

        [Test, S2(Seasar.Extension.Unit.Tx.Rollback)]
        public void WriteNotIdentityTable()
        {
            DataTable table = new DataTable("emp");
            table.Columns.Add("empno", typeof(int));
            table.Columns.Add("ename", typeof(string));
            table.Columns.Add("dname", typeof(string));
            DataRow row = table.NewRow();
            row["empno"] = 9900;
            row["ename"] = "hoge";
            row["dname"] = "aaa";
            table.Rows.Add(row);

            SqlServerSqlTableWriter writer = new SqlServerSqlTableWriter(DataSource);

            writer.Write(table);

            SqlTableReader reader = new SqlTableReader(DataSource);
            reader.SetTable("emp", "empno = 9900");
            DataTable ret = reader.Read();
            Trace.WriteLine(ToStringUtil.ToString(ret));
            Assert.IsNotNull(ret, "1");
        }
    }
}
