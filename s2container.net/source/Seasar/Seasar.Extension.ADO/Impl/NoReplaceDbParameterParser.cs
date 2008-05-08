#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using System.Text.RegularExpressions;

namespace Seasar.Extension.ADO.Impl
{
    /// <summary>
    /// ADO.NETのデータプロバイダに対応したパラメータマーカー(@, :, ?)に切り替えないクラス。
    /// <remarks>
    /// <see cref="http://s2dao.net.seasar.org/ja/">S2Dao.NET</see>では、パラメータマーカーを"?"で確保している。
    /// そのため、本クラスは、パラメータマーカーに"?"を使用する以下の.NET Framework Data Provider専用となる。
    /// ・.NET Framework Data Provider for OLE DB 
    /// ・.NET Framework ODBC 用データ プロバイダ
    /// ・DataDirect Connect for ADO.NET
    /// </summary>
    public class NoReplaceDbParameterParser : IDbParameterParser
    {
        public static readonly IDbParameterParser INSTANCE = new NoReplaceDbParameterParser();

        private const string DEFAULT_PARAMETER_MARKER_FORMAT = @"\?(?=[\s,\(\);]+|$)";

        private readonly Regex _regex;

        public NoReplaceDbParameterParser()
            : this(DEFAULT_PARAMETER_MARKER_FORMAT)
        {
        }

        public NoReplaceDbParameterParser(string pattern)
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
            return original;
        }

        public string[] GetArgNames(IDbCommand cmd, object[] args)
        {
            string[] argNames = new string[args.Length];
            MatchCollection matchs = Parse(cmd.CommandText);
            for (int i = 0; i < matchs.Count; i++)
            {
                argNames[i] = matchs[i].Value;
            }
            return argNames;
        }
    }
}
