#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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

namespace Seasar.Extension.ADO
{
    public interface IDataSource
    {
        IDbConnection GetConnection();
        void CloseConnection(IDbConnection connection);

        IDbCommand GetCommand();
        IDbCommand GetCommand(string cmdText);
        IDbCommand GetCommand(string cmdText, IDbConnection connection);
        IDbCommand GetCommand(string cmdText, IDbConnection connection, IDbTransaction transaction);

        IDataParameter GetParameter();
        IDataParameter GetParameter(string name, DbType dataType);
        IDataParameter GetParameter(string name, object value);
        IDataParameter GetParameter(string name, DbType dataType, int size);
        IDataParameter GetParameter(string name, DbType dataType, int size, string srcColumn);

        IDataAdapter GetDataAdapter();
        IDataAdapter GetDataAdapter(IDbCommand selectCommand);
        IDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString);
        IDataAdapter GetDataAdapter(string selectCommandText, IDbConnection selectConnection);

        IDbTransaction GetTransaction();
        void SetTransaction(IDbCommand cmd);
    }
}
