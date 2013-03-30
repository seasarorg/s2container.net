#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using System.Text.RegularExpressions;

namespace Seasar.Extension.ADO.Impl
{
    [Obsolete("Seasar.Extension.ADO.Impl.BasicDbParameterParserÇégópÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB")]
    public class DbParameterParser : IDbParameterParser
    {
        public static readonly IDbParameterParser INSTANCE = new DbParameterParser();

        private readonly IDbParameterParser _instance;

        public DbParameterParser()
        {
            _instance = new BasicDbParameterParser();
        }

        public DbParameterParser(string pattern)
        {
            _instance = new BasicDbParameterParser(pattern);
        }

        public MatchCollection Parse(string sql)
        {
            return _instance.Parse(sql);
        }

        public Match Match(string sql, int startIndex)
        {
            return _instance.Match(sql, startIndex);
        }

        public virtual string ChangeSignSql(IDbCommand cmd, string original)
        {
            return _instance.ChangeSignSql(cmd, original);
        }

        public virtual string[] GetArgNames(IDbCommand cmd, object[] args)
        {
            return _instance.GetArgNames(cmd, args);
        }
    }
}
