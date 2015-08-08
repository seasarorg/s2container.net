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
using System.Reflection;
using System.Text;
using Seasar.Extension.ADO;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Dao.Pager
{
    public class PagerDataReaderFactoryRowNumberWrapper : AbstractPagerDataReaderFactoryWrapper, IDataReaderFactory
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PagerDataReaderFactoryRowNumberWrapper(
            IDataReaderFactory dataReaderFactory,
            ICommandFactory commandFactory
            )
            : base(dataReaderFactory, commandFactory)
        {
        }

        public string RowNumberColumnName { get; set; } = "pagerrownumber";

        #region IDataReaderFactory ƒƒ“ƒo

        public IDataReader CreateDataReader(IDataSource dataSource, IDbCommand cmd)
        {
            var condition = PagerContext.GetContext().PeekArgs();
            if (condition != null)
            {
                var baseSql = GetBaseSql(cmd);
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("S2Pager base SQL : " + baseSql);
                }
                condition.Count = GetCount(dataSource, cmd, baseSql);
                if (condition.Limit > 0 && condition.Offset > -1)
                {
                    cmd.CommandText = MakeRowNumberSql(baseSql, condition.Limit, condition.Offset);
                    if (_logger.IsDebugEnabled)
                    {
                        _logger.Debug("S2Pager execute SQL : " + cmd.CommandText);
                    }
                }
                return DataReaderFactory.CreateDataReader(dataSource, cmd);
            }
            else
            {
                return DataReaderFactory.CreateDataReader(dataSource, cmd);
            }
        }

        #endregion

        protected string MakeRowNumberSql(string baseSql, int limit, int offset)
        {
            var orderBySql = GetOrderBySql(baseSql);
            if (StringUtil.IsEmpty(orderBySql))
            {
                throw new SQLRuntimeException(new Exception("'ORDER BY' is not included."), baseSql);
            }
            var buf = new StringBuilder(baseSql.Length + 64);
            buf.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER (");
            buf.Append(orderBySql);
            buf.Append($") AS {RowNumberColumnName}, ");
            buf.Append(RemoveOrderBySql(baseSql).Substring("SELECT".Length));
            buf.Append(") AS a ");
            buf.Append($"WHERE {RowNumberColumnName} BETWEEN {(offset + 1)} AND {(offset + limit)}");
            return buf.ToString();
        }
    }
}
