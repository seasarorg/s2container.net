#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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

namespace Seasar.Extension.ADO.Impl
{
    public class BooleanToIntCommandFactory : ICommandFactory
    {
        public static readonly ICommandFactory INSTANCE = new BooleanToIntCommandFactory();

        #region ICommandFactory ÉÅÉìÉo

        public IDbCommand CreateCommand(IDbConnection con, string sql)
        {
            IDbCommand cmd = BasicCommandFactory.INSTANCE.CreateCommand(con, sql);
            return new BooleanToIntCommand(cmd);
        }

        public string GetCompleteSql(string sql, object[] args)
        {
            return BasicCommandFactory.INSTANCE.GetCompleteSql(sql, args);
        }

        public virtual int ExecuteNonQuery(IDataSource dataSource, IDbCommand cmd)
        {
            return BasicCommandFactory.INSTANCE.ExecuteNonQuery(dataSource, cmd);
        }

        public virtual IDataReader ExecuteReader(IDataSource dataSource, IDbCommand cmd)
        {
            return BasicCommandFactory.INSTANCE.ExecuteReader(dataSource, cmd);
        }

        public virtual object ExecuteScalar(IDataSource dataSource, IDbCommand cmd)
        {
            return BasicCommandFactory.INSTANCE.ExecuteScalar(dataSource, cmd);
        }

        #endregion
    }
}
