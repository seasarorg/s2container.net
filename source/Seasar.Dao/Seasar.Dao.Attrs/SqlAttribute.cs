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

namespace Seasar.Dao.Attrs
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SqlAttribute : Attribute
    {
        private readonly string _sql;
        private readonly KindOfDbms _dbms = KindOfDbms.None;

        public SqlAttribute(string sql)
        {
            _sql = sql;
        }

        public SqlAttribute(string sql, KindOfDbms dbms)
            : this(sql)
        {
            _dbms = dbms;
        }

        public string Sql
        {
            get { return _sql; }
        }

        public KindOfDbms Dbms
        {
            get { return _dbms; }
        }
    }
}
