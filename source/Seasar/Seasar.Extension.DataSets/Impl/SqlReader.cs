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

using System.Collections;
using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Extension.DataSets.Impl
{
	public class SqlReader : IDataReader
	{
		private IDataSource dataSource_;

		private IList tableReaders_ = new ArrayList();

		public SqlReader(IDataSource dataSource)
		{
			dataSource_ = dataSource;
		}

		public IDataSource DataSource 
		{
			get { return dataSource_; }
		}

		public void AddTable(string tableName) 
		{
			AddTable(tableName, null);
		}

		public void AddTable(string tableName, string condition) 
		{
			SqlTableReader reader = new SqlTableReader(dataSource_);
			reader.SetTable(tableName, condition);
			tableReaders_.Add(reader);
		}

		public void AddSql(string sql, string tableName) 
		{
			SqlTableReader reader = new SqlTableReader(dataSource_);
			reader.SetSql(sql, tableName);
			tableReaders_.Add(reader);
		}

		#region IDataReader �����o

		public DataSet Read()
		{
			DataSet dataSet = new DataSet();
			foreach (ITableReader reader in tableReaders_) 
			{
				dataSet.Tables.Add(reader.Read());
			}
			return dataSet;
		}

		#endregion
	}
}