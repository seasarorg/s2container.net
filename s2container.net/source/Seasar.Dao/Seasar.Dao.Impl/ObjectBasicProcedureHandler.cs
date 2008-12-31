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
using Seasar.Extension.ADO;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    /// <summary>
    /// 出力パラメータが単一であるProcedureHandler
    /// </summary>
    public class ObjectBasicProcedureHandler : AbstractProcedureHandler
    {
        private static readonly Logger _logger = Logger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataSource">データソース名</param>
        /// <param name="commandFactory">IDbCommand Factory</param>
        /// <param name="procedureName">プロシージャ名</param>
        public ObjectBasicProcedureHandler(IDataSource dataSource, ICommandFactory commandFactory, string procedureName)
            : base(dataSource, commandFactory, procedureName)
        {
            ;
        }

        /// <summary>
        /// ストアドプロシージャを実行する
        /// </summary>
        /// <param name="args">引数</param>
        /// <param name="returnType">戻り値タイプ</param>
        /// <returns>出力パラメータ値</returns>
        public object Execute(object[] args, Type returnType)
        {
            if (DataSource == null) throw new EmptyRuntimeException("dataSource");
            IDbConnection conn = DataSourceUtil.GetConnection(DataSource);

            try
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(ProcedureName);
                }

                IDbCommand cmd = null;
                try
                {
                    object ret = null;
                    cmd = GetCommand(conn, ProcedureName);
                    int cnt = 0;

                    // パラメータをセットし、返値を取得する
                    if (returnType != typeof(void))
                    {
                        // ODP.NETでは、最初にRETURNパラメータをセットしないとRETURN値を取得できない？
                        string returnParamName = BindReturnValues(cmd, "RetValue", GetDbValueType(returnType));

                        BindParamters(cmd, args, ArgumentTypes, ArgumentNames, ArgumentDirection);

                        CommandFactory.ExecuteNonQuery(DataSource, cmd);

                        IDbDataParameter param = (IDbDataParameter) cmd.Parameters[returnParamName];
                        ret = param.Value;
                        cnt = 1;
                    }
                    else
                    {
                        BindParamters(cmd, args, ArgumentTypes, ArgumentNames, ArgumentDirection);
                        CommandFactory.ExecuteNonQuery(DataSource, cmd);
                    }

                    // OutまたはInOutパラメータ値を取得する
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (ArgumentDirection[i] == ParameterDirection.InputOutput ||
                             ArgumentDirection[i] == ParameterDirection.Output)
                        {
                            args[i] = ((IDataParameter) cmd.Parameters[i+cnt]).Value;
                        }
                    }

                    return ret;
                }
                finally
                {
                    CommandUtil.Close(cmd);
                }
            }
            catch (Exception e)
            {
                throw new SQLRuntimeException(e);
            }
            finally
            {
                DataSource.CloseConnection(conn);
            }
        }
    }
}