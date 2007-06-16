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

using System;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Reflection;
using System.Resources;
using Seasar.Extension.ADO;

namespace Seasar.Dao.Dbms
{
    public sealed class DbmsManager
    {
        private static readonly ResourceManager _resourceManager;

        static DbmsManager()
        {
            _resourceManager = new ResourceManager(
                "Dbms", Assembly.GetExecutingAssembly());
        }

        private DbmsManager()
        {
        }

        public static IDbms GetDbms(IDataSource dataSource)
        {
            // IDbConnectionをDataSourceから取得する
            IDbConnection cn = dataSource.GetConnection();

            //IDbmsの実装クラスを取得するためのKey
            string dbmsKey;

            if (cn is OleDbConnection)
            {
                // OleDbConnectionの場合はKeyをType名とProvider名から作成する
                OleDbConnection oleDbCn = cn as OleDbConnection;
                dbmsKey = cn.GetType().Name + "_" + oleDbCn.Provider;
            }
            else if (cn is OdbcConnection)
            {
                // OdbcConnectionの場合はKeyをType名とDriver名から作成する
                OdbcConnection odbcCn = cn as OdbcConnection;
                dbmsKey = cn.GetType().Name + "_" + odbcCn.Driver;
            }
            else
            {
                dbmsKey = cn.GetType().Name;
            }

            // KeyからIDbms実装クラスのインスタンスを取得する
            return GetDbms(dbmsKey);
        }

        /// <summary>
        /// Dbms.resxをdbmsKeyで探し、IDbms実装クラスのインスタンスを取得する
        /// </summary>
        /// <param name="dbmsKey">Dbms.resxを検索する為のKey</param>
        /// <returns>IDbms実装クラスのインスタンス</returns>
        /// <remarks>dbmsKeyに対応するものが見つからない場合は、
        /// 標準のStandardを使用する</remarks>
        public static IDbms GetDbms(string dbmsKey)
        {
            // Dbms.resxからIDbmsの実装クラス名を取得する
            string typeName = _resourceManager.GetString(dbmsKey);

            // IDbms実装クラスのTypeを取得する
            // Dbms.resxに対応するIDbms実装クラスが無い場合は、標準のStandardを使用する
            Type type = typeName == null ? typeof(Standard) : Type.GetType(typeName);

            // IDbms実装クラスのインスタンスを作成して返す
            return (IDbms) Activator.CreateInstance(type, false);
        }
    }
}
