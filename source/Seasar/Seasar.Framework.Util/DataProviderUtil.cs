#region Copyright

/*
 * Copyright 2005 the Seasar Foundation and the Others.
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

namespace Seasar.Framework.Util
{
    /// <summary>
    /// DataProviderUtil
    /// </summary>
    public sealed class DataProviderUtil
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private DataProviderUtil()
        {
            ;
        }

        /// <summary>
        /// バインド変数タイプを取得する
        /// </summary>
        /// <param name="cn">コネクションオブジェクト</param>
        /// <returns>バインド変数タイプ</returns>
        public static BindVariableType GetBindVariableType(IDbConnection cn)
        {
            if ( "SqlConnection".Equals(cn.GetType().Name) ||
                "DB2Connection".Equals(cn.GetType().Name) )
            {
                return BindVariableType.AtmarkWithParam;
            }
            else if ( "OracleConnection".Equals(cn.GetType().Name) )
            {
                return BindVariableType.ColonWithParam;
            }
            else if ( "MySqlConnection".Equals(cn.GetType().Name) )
            {
                return BindVariableType.QuestionWithParam;
            }
            else if ( "NpgsqlConnection".Equals(cn.GetType().Name) )
            {
                return BindVariableType.ColonWithParamToLower;
            }
            else if ( "FbConnection".Equals(cn.GetType().Name) )
            {
                return BindVariableType.Question;
            }
            else
            {
                return BindVariableType.Question;
            }
        }
    }
}