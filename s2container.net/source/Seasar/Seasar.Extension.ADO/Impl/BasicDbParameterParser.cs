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
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    /// <summary>
    /// ADO.NETのデータプロバイダに対応したパラメータマーカー(@, :, ?)に切り替えるクラス。
    /// (SqlClientは"@parmname"、OracleClientは":parmname"、OleDbは、"?"、Odbcは、"?")
    /// </summary>
    public class BasicDbParameterParser : IDbParameterParser
    {
        public static readonly IDbParameterParser INSTANCE = new BasicDbParameterParser();

        private const string DEFAULT_PARAMETER_MARKER_FORMAT
            = @"("
            + @"(?<![@\p{Lo}\p{Lu}\p{Ll}\p{Lm}\p{Nd}\uff3f_#\$\.])@[\p{Lo}\p{Lu}\p{Ll}\p{Lm}\p{Nd}\uff3f_#\$\.]*(?=[\s,\(\);]+|$)"
            + @"|"
            + @"(?<![:\p{Lo}\p{Lu}\p{Ll}\p{Lm}\p{Nd}\uff3f_#\$\.]):[\p{Lo}\p{Lu}\p{Ll}\p{Lm}\p{Nd}\uff3f_#\$\.]*(?=[\s,\(\);]+|$)"
            + @"|"
            + @"\?(?=[\s,\(\);]+|$)"
            + @")"
            ;

        private readonly Regex _regex;

        public BasicDbParameterParser()
            : this(DEFAULT_PARAMETER_MARKER_FORMAT)
        {
        }

        public BasicDbParameterParser(string pattern)
        {
            _regex = new Regex(pattern, RegexOptions.Compiled);
        }

        public MatchCollection Parse(string sql)
        {
            return _regex.Matches(sql);
        }

        public Match Match(string sql, int startIndex)
        {
            return _regex.Match(sql, startIndex);
        }

        public virtual string ChangeSignSql(IDbCommand cmd, string original)
        {
            string ret;
            switch (DataProviderUtil.GetBindVariableType(cmd))
            {
                case BindVariableType.AtmarkWithParam:
                    ret = _GetChangeSignCommandText(original, "@");
                    break;
                case BindVariableType.Question:
                    ret = _GetCommandText(original, "?");
                    break;
                case BindVariableType.QuestionWithParam:
                    ret = _GetChangeSignCommandText(original, "?");
                    break;
                case BindVariableType.ColonWithParam:
                    ret = _GetChangeSignCommandText(original, ":");
                    break;
                default:
                    ret = original;
                    break;
            }
            return ret;
        }

        public string[] GetArgNames(IDbCommand cmd, object[] args)
        {
            string sign;
            var vt = DataProviderUtil.GetBindVariableType(cmd);
            switch (vt)
            {
                case BindVariableType.QuestionWithParam:
                    sign = "?";
                    break;
                case BindVariableType.ColonWithParam:
                    sign = ":";
                    break;
                default:
                    sign = "@";
                    break;
            }

            var argNames = new string[args.Length];
            for (var i = 0; i < argNames.Length; ++i)
            {
                argNames[i] = sign + Convert.ToString(i);
            }
            return argNames;
        }

        private string _GetChangeSignCommandText(string sql, string sign)
        {
            var text = new StringBuilder(sql);
            for (int startIndex = 0, parameterIndex = 0; ; ++parameterIndex)
            {
                var match = Match(text.ToString(), startIndex);
                if (!match.Success)
                {
                    break;
                }
                var parameterName = sign + parameterIndex;
                text.Replace(match.Value, parameterName, match.Index, match.Length);
                startIndex = match.Index + parameterName.Length;
            }
            return text.ToString();
        }

        private string _GetCommandText(string sql, string sign)
        {
            return _ReplaceSql(sql, sign);
        }

        private string _ReplaceSql(string sql, string newValue)
        {
            var text = new StringBuilder(sql);
            for (var startIndex = 0; ; )
            {
                var match = Match(text.ToString(), startIndex);
                if (!match.Success)
                {
                    break;
                }
                text.Replace(match.Value, newValue, match.Index, match.Length);
                startIndex = match.Index + newValue.Length;
            }
            return text.ToString();
        }
    }
}
