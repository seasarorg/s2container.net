#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using Seasar.Framework.Exceptions;
using Seasar.Framework.Log;

namespace Seasar.Framework.Util
{
    public sealed class ConnectionUtil
    {
        private static readonly Logger _logger = Logger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ConnectionUtil()
        {
        }

        public static void Close(IDbConnection connection)
        {
            if (connection == null) return;
            try
            {
                connection.Close();
                _logger.Log("DSSR0002", null);
            }
            catch (Exception ex)
            {
                throw new SQLRuntimeException(ex);
            }
        }

        public static IDbCommand Command(IDbConnection connection, string sql)
        {
            try
            {
                IDbCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                return cmd;
            }
            catch (Exception ex)
            {
                throw new SQLRuntimeException(ex);
            }
        }
    }
}
