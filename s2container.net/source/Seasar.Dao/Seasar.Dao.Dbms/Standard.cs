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

using System.Collections;
using System.Text;

namespace Seasar.Dao.Dbms
{
    public class Standard : IDbms
    {
        private readonly Hashtable _autoSelectFromClauseCache = new Hashtable();

        public virtual string Suffix
        {
            get { return string.Empty; }
        }

        public virtual KindOfDbms Dbms
        {
            get { return KindOfDbms.None; }
        }

        public string GetAutoSelectSql(IBeanMetaData beanMetaData)
        {
            StringBuilder buf = new StringBuilder(100);
            buf.Append(beanMetaData.AutoSelectList);
            buf.Append(" ");
            string beanName = beanMetaData.BeanType.Name;
            lock (_autoSelectFromClauseCache)
            {
                string fromClause = (string) _autoSelectFromClauseCache[beanName];
                if (fromClause == null)
                {
                    fromClause = CreateAutoSelectFromClause(beanMetaData);
                    _autoSelectFromClauseCache[beanName] = fromClause;
                }
                buf.Append(fromClause);
            }
            return buf.ToString();
        }

        protected virtual string CreateAutoSelectFromClause(IBeanMetaData beanMetaData)
        {
            StringBuilder buf = new StringBuilder(100);
            buf.Append("FROM ");
            string myTableName = beanMetaData.TableName;
            buf.Append(myTableName);
            for (int i = 0; i < beanMetaData.RelationPropertyTypeSize; ++i)
            {
                IRelationPropertyType rpt = beanMetaData.GetRelationPropertyType(i);
                IBeanMetaData bmd = rpt.BeanMetaData;
                buf.Append(" LEFT OUTER JOIN ");
                buf.Append(bmd.TableName);
                buf.Append(" ");
                string yourAliasName = rpt.PropertyName;
                buf.Append(yourAliasName);
                buf.Append(" ON ");
                for (int j = 0; j < rpt.KeySize; ++j)
                {
                    buf.Append(myTableName);
                    buf.Append(".");
                    buf.Append(rpt.GetMyKey(j));
                    buf.Append(" = ");
                    buf.Append(yourAliasName);
                    buf.Append(".");
                    buf.Append(rpt.GetYourKey(j));
                    buf.Append(" AND ");
                }
                buf.Length = buf.Length - 5;
            }
            return buf.ToString();
        }

        public virtual string IdentitySelectString
        {
            get { return null; }
        }

        public virtual string GetSequenceNextValString(string sequenceName)
        {
            return null;
        }
    }
}
