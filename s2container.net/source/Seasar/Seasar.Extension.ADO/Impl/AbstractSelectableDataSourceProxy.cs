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

using System.Data;
using Seasar.Framework.Exceptions;

namespace Seasar.Extension.ADO.Impl
{
    /// <summary>
    /// 複数データソースへの接続をサポートする抽象クラス
    /// 明示的にデータソースが登録されている場合はIDictionaryの方を
    /// 明示的に登録されていなければS2Containerの方を探します。
    /// </summary>
    public abstract class AbstractSelectableDataSourceProxy : IDataSource
    {
        /// <summary>
        /// 使用するデータソース名の取得
        /// </summary>
        /// <returns></returns>
        public abstract string GetDataSourceName();

        /// <summary>
        /// ローカルデータメモリスロットにデータソース名を設定
        /// </summary>
        /// <param name="dataSourceName"></param>
        public abstract void SetDataSourceName(string dataSourceName);

        /// <summary>
        /// データソースの取得
        /// </summary>
        /// <returns></returns>
        public virtual IDataSource GetDataSource()
        {
            string dataSourceName = GetDataSourceName();
            if ( string.IsNullOrEmpty(dataSourceName) )
            {
                throw new EmptyRuntimeException(dataSourceName + " at slot");
            }
            return GetDataSource(dataSourceName);
        }

        /// <summary>
        /// データソースの取得
        /// </summary>
        /// <param name="dataSourceName">データソース名</param>
        /// <returns></returns>
        public abstract IDataSource GetDataSource(string dataSourceName);

        #region IDataSource メンバ

        public virtual IDbConnection GetConnection()
        {
            return GetDataSource().GetConnection();
        }

        public virtual void CloseConnection(IDbConnection connection)
        {
            GetDataSource().CloseConnection(connection);
        }

        public virtual IDbCommand GetCommand()
        {
            return GetDataSource().GetCommand();
        }

        public virtual IDbCommand GetCommand(string text)
        {
            return GetDataSource().GetCommand(text);
        }

        public virtual IDbCommand GetCommand(string text, IDbConnection connection)
        {
            return GetDataSource().GetCommand(text, connection);
        }

        public virtual IDbCommand GetCommand(string text, IDbConnection connection, IDbTransaction transaction)
        {
            return GetDataSource().GetCommand(text, connection, transaction);
        }

        public virtual IDataParameter GetParameter()
        {
            return GetDataSource().GetParameter();
        }

        public virtual IDataParameter GetParameter(string name, DbType dataType)
        {
            return GetDataSource().GetParameter(name, dataType);
        }

        public virtual IDataParameter GetParameter(string name, object value)
        {
            return GetDataSource().GetParameter(name, value);
        }

        public virtual IDataParameter GetParameter(string name, DbType dataType, int size)
        {
            return GetDataSource().GetParameter(name, dataType, size);
        }

        public virtual IDataParameter GetParameter(string name, DbType dataType, int size, string srcColumn)
        {
            return GetDataSource().GetParameter(name, dataType, size, srcColumn);
        }

        public virtual IDataAdapter GetDataAdapter()
        {
            return GetDataSource().GetDataAdapter();
        }

        public virtual IDataAdapter GetDataAdapter(IDbCommand selectCommand)
        {
            return GetDataSource().GetDataAdapter(selectCommand);
        }

        public virtual IDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString)
        {
            return GetDataSource().GetDataAdapter(selectCommandText, selectConnectionString);
        }

        public virtual IDataAdapter GetDataAdapter(string selectCommandText, IDbConnection selectConnection)
        {
            return GetDataSource().GetDataAdapter(selectCommandText, selectConnection);
        }

        public virtual IDbTransaction GetTransaction()
        {
            return GetDataSource().GetTransaction();
        }

        public virtual void SetTransaction(IDbCommand cmd)
        {
            GetDataSource().SetTransaction(cmd);
        }

        #endregion
    }
}
