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

using System;
using System.Data;
using System.Data.SqlTypes;
using System.Reflection;
using System.Text.RegularExpressions;
using Seasar.Framework.Util;
using Nullables;

namespace Seasar.Extension.ADO.Impl
{
    public class BasicCommandFactory : ICommandFactory
    {
        public static readonly ICommandFactory INSTANCE = new BasicCommandFactory();

        private static readonly Regex regex = new Regex(@"(@+[a-zA-Z\.0-9_$#\.]*|:[A-Za-z0-9_$#\.]*|\?)");

        #region ICommandFactory ÉÅÉìÉo

        public IDbCommand CreateCommand(IDbConnection con, string sql)
        {
            string changeSignSql = sql;
            MatchCollection sqlParameters = regex.Matches(sql);
            IDbCommand cmd = con.CreateCommand();
            switch (DataProviderUtil.GetBindVariableType(cmd))
            {
                case BindVariableType.AtmarkWithParam:
                    changeSignSql = GetChangeSignCommandText(sql, sqlParameters, "@");
                    break;
                case BindVariableType.Question:
                    changeSignSql = GetCommandText(sql, sqlParameters);
                    break;
                case BindVariableType.QuestionWithParam:
                    changeSignSql = GetChangeSignCommandText(sql, sqlParameters, "?");
                    break;
                case BindVariableType.ColonWithParam:
                    changeSignSql = GetChangeSignCommandText(sql, sqlParameters, ":");
                    break;
                case BindVariableType.ColonWithParamToLower:
                    changeSignSql = GetChangeSignCommandText(sql, sqlParameters, ":");
                    changeSignSql = changeSignSql.ToLower();
                    break;
            }
            cmd.CommandText = changeSignSql;
            return cmd;
        }

        public string GetCompleteSql(string sql, object[] args)
        {
            MatchCollection sqlParameters = regex.Matches(sql);
            return ReplaceSql(sql, args, sqlParameters);
        }

        #endregion

        private string GetChangeSignCommandText(string sql, MatchCollection sqlParameters, string sign)
        {
            string text = sql;
            for (int i = 0, paramIndex = 0; i < sqlParameters.Count; ++i)
            {
                if (!sqlParameters[i].Success) continue;

                string capture = sqlParameters[i].Captures[0].Value;
                if (capture.StartsWith("@@")) continue;

                string parameterName = sign + paramIndex;
                text = ReplaceAtFirstElement(text, sqlParameters[i].Value, parameterName);
                paramIndex++;
            }
            return text;
        }

        private string GetCommandText(string sql, MatchCollection sqlParameters)
        {
            return ReplaceSql(sql, "?", sqlParameters);
        }

        private string ReplaceSql(string sql, object[] args, MatchCollection matches)
        {
            for (int i = 0, bindIndex = 0; i < matches.Count; ++i)
            {
                string capture = matches[i].Captures[0].Value;
                if (capture.StartsWith("@@")) continue;
                sql = ReplaceAtFirstElement(sql, capture, GetBindVariableText(args[bindIndex]));
                bindIndex++;
            }
            return sql;
        }

        private string ReplaceSql(string sql, string newValue, MatchCollection matches)
        {
            for (int i = 0; i < matches.Count; ++i)
            {
                string capture = matches[i].Captures[0].Value;
                if (capture.StartsWith("@@")) continue;
                sql = ReplaceAtFirstElement(sql, capture, newValue);
            }
            return sql;
        }

        private string ReplaceAtFirstElement(string source, string original, string replace)
        {
            string pattern = original.Replace("?", "\\?");
            pattern = pattern.Replace("$", "\\$");
            Regex regexp = new Regex(pattern, RegexOptions.IgnoreCase);
            return regexp.Replace(source, replace, 1);
        }

        protected virtual string GetBindVariableText(object bindVariable)
        {
            if (bindVariable is INullable)
            {
                INullable nullable = bindVariable as INullable;
                if (nullable.IsNull)
                {
                    return GetBindVariableText(null);
                }
                else
                {
                    PropertyInfo pi = bindVariable.GetType().GetProperty("Value");
                    return GetBindVariableText(pi.GetValue(bindVariable, null));
                }
            }
            else if (bindVariable is INullableType)
            {
                INullableType nullable = bindVariable as INullableType;
                if (!nullable.HasValue)
                {
                    return GetBindVariableText(null);
                }
                else
                {
                    PropertyInfo pi = bindVariable.GetType().GetProperty("Value");
                    return GetBindVariableText(pi.GetValue(bindVariable, null));
                }
            }
            else if (bindVariable is string)
            {
                return "'" + bindVariable + "'";
            }
            else if (bindVariable == null)
            {
                return "null";
            }
            else if (bindVariable.GetType().IsPrimitive)
            {
                return bindVariable.ToString();
            }
            else if (bindVariable is decimal)
            {
                return bindVariable.ToString();
            }
            else if (bindVariable is DateTime)
            {
                if ((DateTime) bindVariable == ((DateTime) bindVariable).Date)
                {
                    return "'" + ((DateTime) bindVariable).ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    return "'" + ((DateTime) bindVariable).ToString("yyyy-MM-dd HH.mm.ss") + "'";
                }
            }
            else if (bindVariable is bool)
            {
                return bindVariable.ToString();
            }
            else
            {
                return "'" + bindVariable.ToString() + "'";
            }
        }
    }
}
