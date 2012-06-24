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
using Seasar.Framework.Exceptions;
using Seasar.Framework.Log;
using Seasar.Framework.Message;

namespace Seasar.Framework.Util
{
    public sealed class DataSourceUtil
    {
        private static readonly Logger _logger = Logger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DataSourceUtil()
        {
        }

        public static IDbConnection GetConnection(IDataSource dataSource)
        {
            try
            {
                IDbConnection cn = dataSource.GetConnection();
                if (cn.State != ConnectionState.Open)
                {
                    try
                    {
                        cn.Open();
                    }
                    catch (Exception ex)
                    {
                        throw new DataException(MessageFormatter.GetSimpleMessage("ESSR0365", null), ex);
                    }
                    _logger.Log("DSSR0007", null);
                }
                return cn;
            }
            catch (Exception ex)
            {
                throw new SQLRuntimeException(ex);
            }
        }

        [Obsolete("IDataSource.CloseConnection")]
        public static void CloseConnection(IDataSource dataSource, IDbConnection cn)
        {
            dataSource.CloseConnection(cn);
        }

        [Obsolete("IDataSource.SetTransaction")]
        public static void SetTransaction(IDataSource dataSource, IDbCommand cmd)
        {
            dataSource.SetTransaction(cmd);
        }
    }
}
