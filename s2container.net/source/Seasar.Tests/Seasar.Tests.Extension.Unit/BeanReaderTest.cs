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
using System.Data;
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
			Employee emp = new Employee();
			emp.Empno = 7788;
			emp.Ename = "SCOTT";
			emp.Deptno = 10;
			emp.Dname = "HOGE";
			BeanReader reader = new BeanReader(emp);
			DataSet ds = reader.Read();
			DataTable table = ds.Tables[0];
			DataRow row = table.Rows[0];
			DataColumnCollection columns = table.Columns;
			Assert.AreEqual(7788, row["empno"], "1");
			Assert.AreEqual("SCOTT", row["ename"], "2");
			Assert.AreEqual(10, row["deptno"], "3");
			Assert.AreEqual("HOGE", row["dname"], "4");
			Assert.AreEqual(DataRowState.Unchanged, row.RowState, "5");
			Assert.AreEqual(typeof(long), columns["empno"].DataType, "6");
			Assert.AreEqual(typeof(string), columns["ename"].DataType, "7");
			Assert.AreEqual(typeof(int), columns["deptno"].DataType, "8");
			Assert.AreEqual(typeof(string), columns["dname"].DataType, "9");
		}

		[Test]
		public void TestReadNHibernateNullableType()
		{
			NHibernateNullableEmployee emp = new NHibernateNullableEmployee();
			emp.Empno = 7788;
			emp.Ename = "SCOTT";
			emp.Deptno = 10;
			emp.Dname = "HOGE";
			BeanReader reader = new BeanReader(emp);
			DataSet ds = reader.Read();
			DataTable table = ds.Tables[0];
			DataRow row = table.Rows[0];
			DataColumnCollection columns = table.Columns;
			Assert.AreEqual(7788, row["empno"], "1");
			Assert.AreEqual("SCOTT", row["ename"], "2");
			Assert.AreEqual(10, row["deptno"], "3");
			Assert.AreEqual("HOGE", row["dname"], "4");
			Assert.AreEqual(DataRowState.Unchanged, row.RowState, "5");
			Assert.AreEqual(typeof(long), columns["empno"].DataType, "6");
			Assert.AreEqual(typeof(string), columns["ename"].DataType, "7");
			Assert.AreEqual(typeof(int), columns["deptno"].DataType, "8");
			Assert.AreEqual(typeof(string), columns["dname"].DataType, "9");
		}

		[Test]
		public void TestReadNHibernateNullableTypeNullValue()
		{
			NHibernateNullableEmployee emp = new NHibernateNullableEmployee();
			emp.Empno = null;
			emp.Ename = null;
			emp.Deptno = null;
			emp.Dname = null;
			BeanReader reader = new BeanReader(emp);
			DataSet ds = reader.Read();
			DataTable table = ds.Tables[0];
			DataRow row = table.Rows[0];
			DataColumnCollection columns = table.Columns;
			Assert.AreEqual(DBNull.Value, row["empno"], "1");
			Assert.AreEqual(DBNull.Value, row["ename"], "2");
			Assert.AreEqual(DBNull.Value, row["deptno"], "3");
			Assert.AreEqual(DBNull.Value, row["dname"], "4");
			Assert.AreEqual(DataRowState.Unchanged, row.RowState, "5");
			Assert.AreEqual(typeof(long), columns["empno"].DataType, "6");
			Assert.AreEqual(typeof(string), columns["ename"].DataType, "7");
			Assert.AreEqual(typeof(int), columns["deptno"].DataType, "8");
			Assert.AreEqual(typeof(string), columns["dname"].DataType, "9");
		}
	}
}
