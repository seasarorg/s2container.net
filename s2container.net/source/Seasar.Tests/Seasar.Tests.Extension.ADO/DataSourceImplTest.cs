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
using NUnit.Framework;
using Seasar.Extension.ADO;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Container.Factory;

namespace TestSeasar.Extension.ADO
{
	/// <summary>
	/// テストを実行するためには、s2-dotnet/data/setUpDemo.batを実行し、
	/// デモ用のデータベースをセットアップして下さい。
	/// </summary>
	[TestFixture]
	public class DataSourceImplTest
	{
		private IS2Container container_;

		[SetUp]
		public void SetUp()
		{
			container_ = new S2ContainerImpl();
			IS2Container daoContainer = S2ContainerFactory.Create("Ado.dicon");
			container_.Include(daoContainer);
			container_.Register(new ComponentDefImpl(typeof(EmployeeDaoImpl)));
		}

		[Test]
		public void TestDao()
		{
			IEmployeeDao dao = (IEmployeeDao) container_.GetComponent(typeof(IEmployeeDao));
			Assert.AreEqual("WARD", dao.GetEnameByEmpno(7521));
		}

		[Test]
		public void TestDataSet()
		{
			IEmployeeDao dao = (IEmployeeDao) container_.GetComponent(typeof(IEmployeeDao));
			DataSet dataSet = dao.GetEnameDataSetByEmpno(7521);
			DataTable dt = dataSet.Tables[0];
			Assert.AreEqual("WARD", dt.Rows[0]["ename"]);
		}

		public interface IEmployeeDao
		{
			string GetEnameByEmpno(int empno);
			DataSet GetEnameDataSetByEmpno(int productID);
		}

		public class EmployeeDaoImpl : IEmployeeDao
		{
			private IDataSource dataSource_;

			public EmployeeDaoImpl(IDataSource dataSource)
			{
				dataSource_ = dataSource;
			}

			public string GetEnameByEmpno(int empno)
			{
				string ret = null;
				using(IDbConnection cn = dataSource_.GetConnection())
				{
					cn.Open();
					string sql = "select ename from EMP where empno=@empno";
					using(IDbCommand cmd = dataSource_.GetCommand(sql, cn))
					{
						cmd.Parameters.Add(dataSource_.GetParameter("@empno", empno));
						using(IDataReader reader = cmd.ExecuteReader())
						{
							while(reader.Read())
							{
								ret = (string) reader["ename"];
							}
						}
					}
				}
				return ret;
			}

			public DataSet GetEnameDataSetByEmpno(int empno)
			{
				DataSet dataSet = new DataSet();
				using(IDbConnection cn = dataSource_.GetConnection())
				{
					cn.Open();
					string sql = "select ename from EMP where empno=@empno";
					using(IDbCommand cmd = dataSource_.GetCommand(sql, cn))
					{
						cmd.Parameters.Add(dataSource_.GetParameter("@empno", empno));
						IDataAdapter dataAdapter = dataSource_.GetDataAdapter(cmd);
						dataAdapter.Fill(dataSet);
					}
				}
				return dataSet;
			}
		}
	}
}
