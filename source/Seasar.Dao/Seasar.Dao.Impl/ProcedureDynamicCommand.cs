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
using System.Collections;
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Framework.Exceptions;

namespace Seasar.Dao.Impl
{
    /// <summary>
    /// ストアドプロシージャ用DynamicComandクラス
    /// </summary>
    public class ProcedureDynamicCommand : AbstractDynamicCommand
    {
        /// <summary>
        /// 戻り値タイプ
        /// </summary>
        private Type _returnType;

        /// <summary>
        /// パラメータの入出力方向
        /// </summary>
        private ParameterDirection[] argDirections = new ParameterDirection[0];

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataSource">データソース</param>
        /// <param name="commandFactory">Commandファクトリ</param>
        public ProcedureDynamicCommand(IDataSource dataSource,
                                       ICommandFactory commandFactory)
            : base(dataSource, commandFactory)
        {
            ;
        }

        /// <summary>
        /// 戻り値タイプ
        /// </summary>
        public Type ReturnType
        {
            get { return _returnType; }
            set { _returnType = value; }
        }

        /// <summary>
        /// パラメータの入出力方向
        /// </summary>
        public ParameterDirection[] ArgDirections
        {
            get { return argDirections; }
            set { argDirections = value; }
        }

        /// <summary>
        /// 実行する
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override object Execute(object[] args)
        {
            ICommandContext ctx = Apply(args);

            if (DataSource == null) throw new EmptyRuntimeException("dataSource");

            if (_returnType != typeof(Hashtable))
            {
                ObjectBasicProcedureHandler handler = new ObjectBasicProcedureHandler(DataSource, CommandFactory, ctx.Sql);
                handler.ArgumentNames = ArgNames;
                handler.ArgumentTypes = ArgTypes;
                handler.ArgumentDirection = ArgDirections;

                return (handler.Execute(args, _returnType));
            }
            else
            {
                HashtableBasicProcedureHandler handler = new HashtableBasicProcedureHandler(DataSource, CommandFactory, ctx.Sql);
                handler.ArgumentNames = ArgNames;
                handler.ArgumentTypes = ArgTypes;
                handler.ArgumentDirection = ArgDirections;

                return (handler.Execute(args));
            }
        }
    }
}