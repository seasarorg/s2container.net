#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
using Seasar.Extension.ADO;

namespace Seasar.Examples.Reference.Tx
{
    public interface IEmployeeDao
    {
        void Insert();
    }

    public class EmployeeDaoImpl : IEmployeeDao
    {
        private IDataSource _dataSource;

        public IDataSource DataSource
        {
            set { _dataSource = value; }
        }

        #region IEmployeeDao メンバ

        public void Insert()
        {
            using (IDbConnection cn = _dataSource.GetConnection())
            {
                cn.Open();
                string sql = "insert into emp2 values(@empno, @ename, @deptnum)";
                using (IDbCommand cmd = _dataSource.GetCommand(sql, cn))
                {
                    cmd.Parameters.Add(_dataSource.GetParameter("@empno", 99));
                    cmd.Parameters.Add(_dataSource.GetParameter("@ename", "Sugimoto"));
                    cmd.Parameters.Add(_dataSource.GetParameter("@deptnum", 31));
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("EMPを追加しました。");
                }
            }

            // ロールバックのためにExceptionを発生させる
            throw new ApplicationException();
        }

        #endregion
    }
}
