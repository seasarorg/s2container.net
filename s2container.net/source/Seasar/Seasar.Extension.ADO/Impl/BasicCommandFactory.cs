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
using System.Data.SqlTypes;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Seasar.Framework.Util;

#if NHIBERNATE_NULLABLES
using Nullables;
#endif

namespace Seasar.Extension.ADO.Impl
{
    public class BasicCommandFactory : ICommandFactory
    {
        public static readonly ICommandFactory INSTANCE = new BasicCommandFactory();

        private readonly IDbParameterParser _parser;

        private string sqlLogDateFormat = "yyyy-MM-dd";

        private string sqlLogDateTimeFormat = "yyyy-MM-dd HH.mm.ss";

        private int commandTimeout = -1;

        public BasicCommandFactory()
            : this(BasicDbParameterParser.INSTANCE)
        {
        }

        public BasicCommandFactory(IDbParameterParser dbParameterParser)
        {
            _parser = dbParameterParser;
        }

        public string SqlLogDateFormat
        {
            get { return sqlLogDateFormat; }
            set { sqlLogDateFormat = value; }
        }

        public string SqlLogDateTimeFormat
        {
            get { return sqlLogDateTimeFormat; }
            set { sqlLogDateTimeFormat = value; }
        }

        public int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }

        #region ICommandFactory ƒƒ“ƒo

        public virtual IDbCommand CreateCommand(IDbConnection con, string sql)
        {
            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = ChangeSignSql(cmd, sql);
            if (commandTimeout > -1)
            {
                cmd.CommandTimeout = commandTimeout;
            }
            return cmd;
        }

        public string GetCompleteSql(string sql, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                return sql;
            }
            _parser.Parse(sql);
            return ReplaceSql(sql, args);
        }

        public string[] GetArgNames(IDbCommand cmd, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                return new string[0];
            }
            return _parser.GetArgNames(cmd, args);
        }

        public virtual int ExecuteNonQuery(IDataSource dataSource, IDbCommand cmd)
        {
            return CommandUtil.ExecuteNonQuery(dataSource, cmd);
        }

        public virtual IDataReader ExecuteReader(IDataSource dataSource, IDbCommand cmd)
        {
            return CommandUtil.ExecuteReader(dataSource, cmd);
        }

        public virtual object ExecuteScalar(IDataSource dataSource, IDbCommand cmd)
        {
            return CommandUtil.ExecuteScalar(dataSource, cmd);
        }

        #endregion

        protected string ChangeSignSql(IDbCommand cmd, string original)
        {
            return _parser.ChangeSignSql(cmd, original);
        }

        private string ReplaceSql(string sql, object[] args)
        {
            StringBuilder text = new StringBuilder(sql);
            for (int startIndex = 0, argsIndex = 0; argsIndex < args.Length; ++argsIndex)
            {
                Match match = _parser.Match(text.ToString(), startIndex);
                if (!match.Success)
                {
                    break;
                }
                string newValue = GetBindVariableText(args[argsIndex]);
                text.Replace(match.Value, newValue, match.Index, match.Length);
                startIndex = match.Index + newValue.Length;
            }
            return text.ToString();
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
#if NHIBERNATE_NULLABLES
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
#endif
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
                    return "'" + ((DateTime) bindVariable).ToString(SqlLogDateFormat) + "'";
                }
                else
                {
                    return "'" + ((DateTime) bindVariable).ToString(SqlLogDateTimeFormat) + "'";
                }
            }
            else if (bindVariable is bool)
            {
                return bindVariable.ToString();
            }
            else if (bindVariable.GetType().IsEnum)
            {
                object o = Convert.ChangeType(bindVariable, Enum.GetUnderlyingType(bindVariable.GetType()));
                return o.ToString();
            }
            else
            {
                return "'" + bindVariable + "'";
            }
        }
    }
}
