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

using System.Text.RegularExpressions;

namespace Seasar.Extension.ADO.Impl
{
    public class DbParameterParser : IDbParameterParser
    {
        public static readonly IDbParameterParser INSTANCE = new DbParameterParser();

        private const string DEFAULT_PARAMETER_MARKER_FORMAT
            = @"("
            + @"(?<![@\p{Lo}\p{Lu}\p{Ll}\p{Lm}\p{Nd}\uff3f_#\$\.])@[\p{Lo}\p{Lu}\p{Ll}\p{Lm}\p{Nd}\uff3f_#\$\.]*(?=[\s,\(\);]+|$)"
            + @"|"
            + @"(?<![:\p{Lo}\p{Lu}\p{Ll}\p{Lm}\p{Nd}\uff3f_#\$\.]):[\p{Lo}\p{Lu}\p{Ll}\p{Lm}\p{Nd}\uff3f_#\$\.]*(?=[\s,\(\);]+|$)"
            + @"|"
            + @"\?(?=[\s,\(\);]+|$)"
            + @")"
            ;

        private readonly Regex regex;

        public DbParameterParser()
            : this(DEFAULT_PARAMETER_MARKER_FORMAT)
        {
        }

        public DbParameterParser(string pattern)
        {
            this.regex = new Regex(pattern, RegexOptions.Compiled);
        }

        public MatchCollection Parse(string sql)
        {
            return regex.Matches(sql);
        }

        public Match Match(string sql, int startIndex)
        {
            return regex.Match(sql, startIndex);
        }
    }
}
