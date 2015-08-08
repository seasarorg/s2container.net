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

using System;
using System.Collections;
using System.Data;
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    /// <summary>
    /// 出力パラメータが複数あるProcedureHandler
    /// </summary>
    public class HashtableBasicProcedureHandler : AbstractProcedureHandler
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataSource">データソース名</param>
        /// <param name="commandFactory">IDbCommand Factory</param>
        /// <param name="procedureName">プロシージャ名</param>
        public HashtableBasicProcedureHandler(IDataSource dataSource, ICommandFactory commandFactory, string procedureName)
            : base(dataSource, commandFactory, procedureName)
        {
            ;
        }

        /// <summary>
        /// 実行する
        /// </summary>
        /// <param name="args">引数</param>
        /// <returns>出力パラメータ値</returns>
        public Hashtable Execute(object[] args)
        {
            if (DataSource == null) throw new EmptyRuntimeException("dataSource");
            var conn = DataSourceUtil.GetConnection(DataSource);

            var ret = new Hashtable();
            try
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(ProcedureName);
                }

                IDbCommand cmd = null;
                try
                {
                    Type[] outParamTypes;
                    var outParamNames = _GetOutParamNames(conn, args, out outParamTypes);

                    cmd = GetCommand(conn, ProcedureName);

                    // パラメータをセットする
                    for (var j = 0; j < outParamNames.Length; j++)
                    {
                        BindReturnValues(cmd, _AddParameterName(outParamNames[j], cmd), GetDbValueType(outParamTypes[j]));
                    }
                    BindParamters(cmd, args, ArgumentTypes, ArgumentNames, ArgumentDirection);

                    CommandFactory.ExecuteNonQuery(DataSource, cmd);

                    // 返り値を取得する
                    for (var j = 0; j < outParamNames.Length; j++)
                    {
                        var paramName = _AddParameterName(outParamNames[j], cmd);
                        var param = (IDbDataParameter) cmd.Parameters[paramName];
                        ret.Add(outParamNames[j], param.Value);
                    }

                    // OutまたはInOutパラメータ値を取得する
                    for (var i = 0; i < args.Length; i++)
                    {
                        if (ArgumentDirection[i] == ParameterDirection.InputOutput ||
                             ArgumentDirection[i] == ParameterDirection.Output)
                        {
                            var param = (IDbDataParameter)cmd.Parameters[i + outParamNames.Length];
                            args[i] = ConversionUtil.ConvertTargetType(param.Value, ArgumentTypes[i]);
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

        private string[] _GetOutParamNames(IDbConnection conn, object[] args, out Type[] outParamTypes)
        {
            var cmd = GetCommand(conn, ProcedureName);
            BindParamters(cmd, args, ArgumentTypes, ArgumentNames, ArgumentDirection);

            var reader = CommandFactory.ExecuteReader(DataSource, cmd);
            var dt = reader.GetSchemaTable();
            var i = 0;
            if (dt != null)
            {
                var outParamNames = new string[dt.Rows.Count];
                outParamTypes = new Type[dt.Rows.Count];
                foreach (DataRow row in dt.Rows)
                {
#if DEBUG
                    Console.Out.WriteLine("Out Param:" + row["ColumnName"]);
                    Console.Out.WriteLine("Out Type:" + row["DataType"]);
                    Console.Out.WriteLine("Out Direction:");
#endif
                    outParamNames[i] = (string) row["ColumnName"];
                    outParamTypes[i] = (Type) row["DataType"];
                    i++;
                }
                reader.Close();

                return outParamNames;
            }
            outParamTypes = null;
            return null;
        }

        private static string _AddParameterName(string paramName, IDbCommand command)
        {
            var vt = DataProviderUtil.GetBindVariableType(command);
            switch (vt)
            {
                case BindVariableType.QuestionWithParam:
                    paramName = "?" + paramName;
                    break;
                case BindVariableType.ColonWithParam:
                    if ("OracleCommand".Equals(command.GetExType().Name))
                    {
                        paramName = string.Empty + paramName;
                    }
                    else
                    {
                        paramName = ":" + paramName;
                    }
                    break;
                default:
                    paramName = "@" + paramName;
                    break;
            }

            return paramName;
        }
    }
}
