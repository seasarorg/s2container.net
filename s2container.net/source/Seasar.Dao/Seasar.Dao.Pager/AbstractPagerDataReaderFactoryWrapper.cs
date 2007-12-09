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
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Dao.Pager
{
    public abstract class AbstractPagerDataReaderFactoryWrapper
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex _baseSqlRegex = new Regex("^.*?(SELECT)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        protected static readonly Regex _orderBySqlRegex = new Regex(
            "order\\s+by\\s+([\\w\\p{L}.`\\[\\]]+(\\s+(asc|desc))?|([\\w\\p{L}.`\\[\\]]+(\\s+(asc|desc))?\\s*,\\s*)+[\\w\\p{L}.`\\[\\]])+(\\s+(asc|desc))?\\s*$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public AbstractPagerDataReaderFactoryWrapper(
            IDataReaderFactory dataReaderFactory,
            ICommandFactory commandFactory
            )
        {
            _dataReaderFactory = dataReaderFactory;
            _commandFactory = commandFactory;
        }

        private IDataReaderFactory _dataReaderFactory;

        protected IDataReaderFactory DataReaderFactory
        {
            get { return _dataReaderFactory; }
        }

        private ICommandFactory _commandFactory;

        protected ICommandFactory CommandFactory
        {
            get { return _commandFactory; }
        }

        private bool _chopOrderBy = true;

        public bool ChopOrderBy
        {
            set { _chopOrderBy = value; }
        }

        protected virtual int GetCount(IDataSource dataSource, IDbCommand cmd, string baseSql)
        {
            string countSql = MakeCountSql(baseSql);
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("S2Pager execute SQL : " + countSql);
            }

            IDbCommand countCmd = null;
            IDataReader reader = null;
            try
            {
                countCmd = _commandFactory.CreateCommand(cmd.Connection, countSql);
                foreach (IDbDataParameter src in cmd.Parameters)
                {
                    IDbDataParameter desc = countCmd.CreateParameter();
                    desc.ParameterName = src.ParameterName;
                    desc.DbType = src.DbType;
                    desc.Value = src.Value;
                    desc.Direction = src.Direction;
                    countCmd.Parameters.Add(desc);
                }
                reader = _dataReaderFactory.CreateDataReader(dataSource, countCmd);
                if (reader.Read())
                {
                    return (int) ValueTypes.INT32.GetValue(reader, 0);
                }
                else
                {
                    throw new SQLRuntimeException(new Exception("[S2Pager]Result not found."), countCmd.CommandText);
                }
            }
            finally
            {
                DataReaderUtil.Close(reader);
                CommandUtil.Close(countCmd);
            }
        }

        protected string GetBaseSql(IDbCommand cmd)
        {
            string sql = cmd.CommandText;
            MatchCollection matchs = _baseSqlRegex.Matches(sql);
            if (matchs.Count != 0)
            {
                return _baseSqlRegex.Replace(sql, matchs[0].Value, 1);
            }
            else
            {
                return sql;
            }
        }

        protected string RemoveOrderBySql(string baseSql)
        {
            return _orderBySqlRegex.Replace(baseSql, string.Empty);
        }

        protected string GetOrderBySql(string baseSql)
        {
            MatchCollection matchs = _orderBySqlRegex.Matches(baseSql);
            if (matchs.Count != 0)
            {
                return matchs[0].Value;
            }
            else
            {
                return null;
            }
        }

        protected string MakeCountSql(string baseSql)
        {
            StringBuilder buf = new StringBuilder("SELECT COUNT(*) FROM (");
            if (_chopOrderBy)
            {
                buf.Append(RemoveOrderBySql(baseSql));
            }
            else
            {
                buf.Append(baseSql);
            }
            buf.Append(") AS total");
            return buf.ToString();
        }
    }
}
