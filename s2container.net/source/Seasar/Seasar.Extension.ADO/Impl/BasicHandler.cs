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
using System.Collections;
using System.Data;
using System.Data.SqlTypes;
using System.Reflection;
using System.Text.RegularExpressions;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;
using Nullables;

namespace Seasar.Extension.ADO.Impl
{
    /// <summary>
    /// BasicHandler ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
    /// </summary>
    public class BasicHandler
    {
        private IDataSource dataSource;
        private string sql;
        private MatchCollection sqlParameters;
        private ICommandFactory commandFactory = BasicCommandFactory.INSTANCE;
		private int commandTimeout = -1;
        
        public BasicHandler()
        {
        }

        public BasicHandler(IDataSource ds, string sql)
            : this(ds, sql, BasicCommandFactory.INSTANCE)
        {
        }

        public BasicHandler(IDataSource ds, string sql, ICommandFactory commandFactory)
        {
            this.DataSource = ds;
            this.Sql = sql;
            this.CommandFactory = commandFactory;
        }

        public IDataSource DataSource
        {
            get { return this.dataSource; }
            set { this.dataSource = value; }
        }

        public string Sql
        {
            get { return this.sql; }
            set
            {
                this.sql = value;
                Regex regex = new Regex(@"(@\w+|:\w+|\?)");
                sqlParameters = regex.Matches(sql);
            }
        }

        public ICommandFactory CommandFactory
        {
            get { return commandFactory; }
            set { commandFactory = value; }
        }

		public int CommandTimeout 
		{
			get { return commandTimeout; }
			set { commandTimeout = value; }
		}

        protected IDbConnection Connection
        {
            get
            {
                if(this.dataSource == null) throw new EmptyRuntimeException("dataSource");
                return DataSourceUtil.GetConnection(this.dataSource);
            }
        }

        protected virtual IDbCommand Command(IDbConnection connection)
        {
            if(this.sql == null) throw new EmptyRuntimeException("sql");
			IDbCommand cmd = connection.CreateCommand();
			string changeSignSql = this.sql;
			switch (DataProviderUtil.GetBindVariableType(cmd))
            {
                case BindVariableType.AtmarkWithParam:
                    changeSignSql = GetChangeSignCommandText(this.sql, "@");
                    break;
                case BindVariableType.Question:
                    changeSignSql = GetCommandText(this.sql);
                    break;
                case BindVariableType.QuestionWithParam:
                    changeSignSql = GetChangeSignCommandText(this.sql, "?");
                    break;
                case BindVariableType.ColonWithParam:
                    changeSignSql = GetChangeSignCommandText(this.sql, ":");
                    break;
				case BindVariableType.ColonWithParamToLower:
                    changeSignSql = GetChangeSignCommandText(this.sql, ":");
                    changeSignSql = changeSignSql.ToLower();
					break;
            }
			cmd.CommandText = changeSignSql;
			if (this.commandTimeout > -1) cmd.CommandTimeout = this.commandTimeout;
			return cmd;
        }

        protected void BindArgs(IDbCommand command, object[] args, Type[] argTypes,
            string[] argNames)
        {
            if(args == null) return;
            Hashtable saveArgs = new Hashtable(new CaseInsensitiveHashCodeProvider(), 
                new CaseInsensitiveComparer());

            for(int i = 0; i < args.Length; ++i)
            {
                if (saveArgs.ContainsKey(argNames[i]))
                {
                    continue;
                }
                IValueType valueType = ValueTypes.GetValueType(argTypes[i]);
                try
                {
                    valueType.BindValue(command, argNames[i], args[i]);
                    saveArgs.Add(argNames[i], argNames[i]);
                }
                catch(Exception ex)
                {
                    throw new SQLRuntimeException(ex);
                }
            }
            saveArgs.Clear();
        }

		protected Type[] GetArgTypes(object[] args) 
		{
			if (args == null) 
			{
				return null;
			}
			Type[] argTypes = new Type[args.Length];
			for (int i = 0; i < args.Length; ++i) 
			{
				object arg = args[i];
				if (arg != null) 
				{
					argTypes[i] = arg.GetType();
				}
			}
			return argTypes;
		}

        protected string[] GetArgNames()
        {
            string[] argNames = new string[sqlParameters.Count];
            for (int i = 0; i < argNames.Length; ++i)
            {
                argNames[i] = (sqlParameters[i].Value.Length == 1) ? Convert.ToString(i) : sqlParameters[i].Value.Substring(1);
            }
            return argNames;
        }

        protected string GetCompleteSql(object[] args)
        {
            if(args == null || args.Length == 0) return this.sql;
            return GetCompleteSql(sql, args);
        }

        private string GetCompleteSql(string sql, object[] args)
        {
            return ReplaceSql(sql, args, sqlParameters);
        }

        private string ReplaceSql(string sql, object[] args, MatchCollection matches)
        {
            for(int i = 0; i < matches.Count; ++i)
            {
                string capture = matches[i].Captures[0].Value;
				sql = ReplaceAtFirstElement(sql, capture, GetBindVariableText(args[i]));
            }
            return sql;
        }

        private string GetCommandText(string sql)
        {
            return ReplaceSql(sql, "?", sqlParameters);
        }

        private string GetChangeSignCommandText(string sql, string sign)
        {
            string text = sql;
            for (int i = 0; i < sqlParameters.Count; ++i)
            {
                if (!sqlParameters[i].Success) continue;
                string parameterName = (sqlParameters[i].Value.Length == 1) ? sign + i : sign + sqlParameters[i].Value.Substring(1);
                text = ReplaceAtFirstElement(text, sqlParameters[i].Value, parameterName);
            }
            return text;
        }

        private string ReplaceSql(string sql, string newValue, MatchCollection matches)
        {
            for(int i = 0; i < matches.Count; ++i)
            {
                string capture = matches[i].Captures[0].Value;
				sql = ReplaceAtFirstElement(sql, capture, newValue);
            }
            return sql;
        }

		private string ReplaceAtFirstElement(string source, string original, string replace)
		{
            string pattern = original.Replace("?", "\\?");
            Regex regexp = new Regex(pattern, RegexOptions.IgnoreCase);
			return regexp.Replace(source, replace, 1);
		}

        protected string GetBindVariableText(object bindVariable)
        {
            if(bindVariable is INullable)
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
            else if(bindVariable is string)
            {
                return "'" + bindVariable + "'";
            }
			else if(bindVariable == null)                                                                                                                                                                                                                                                             
			{
				return "null";
			}
            else if(bindVariable.GetType().IsPrimitive)
            {
                return bindVariable.ToString();
            }
            else if(bindVariable is decimal)
            {
                return bindVariable.ToString();
            }
            else if(bindVariable is DateTime)
            {
                if((DateTime) bindVariable == ((DateTime) bindVariable).Date)
                {
                    return "'" + ((DateTime) bindVariable).ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    return "'" + ((DateTime) bindVariable).ToString("yyyy-MM-dd HH.mm.ss") + "'";
                }
            }
            else if(bindVariable is bool)
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
