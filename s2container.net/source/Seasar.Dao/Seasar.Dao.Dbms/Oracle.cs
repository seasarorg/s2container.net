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

using System.Text;

namespace Seasar.Dao.Dbms
{
    public class Oracle : Standard
    {
        public override string Suffix
        {
            get { return "_oracle"; }
        }

        public override string GetSequenceNextValString(string sequenceName)
        {
            return "select " + sequenceName + ".nextval from dual";
        }

        public override KindOfDbms Dbms
        {
            get { return KindOfDbms.Oracle; }
        }


        protected override string CreateAutoSelectFromClause(IBeanMetaData beanMetaData)
        {
            StringBuilder buf = new StringBuilder(100);
            buf.Append("FROM ");
            string myTableName = beanMetaData.TableName;
            buf.Append(myTableName);
            StringBuilder whereBuf = new StringBuilder(100);
            for (int i = 0; i < beanMetaData.RelationPropertyTypeSize; ++i)
            {
                IRelationPropertyType rpt = beanMetaData.GetRelationPropertyType(i);
                IBeanMetaData bmd = rpt.BeanMetaData;
                buf.Append(", ");
                buf.Append(bmd.TableName);
                buf.Append(" ");
                string yourAliasName = rpt.PropertyName;
                buf.Append(yourAliasName);
                for (int j = 0; j < rpt.KeySize; ++j)
                {
                    whereBuf.Append(myTableName);
                    whereBuf.Append(".");
                    whereBuf.Append(rpt.GetMyKey(j));
                    whereBuf.Append("=");
                    whereBuf.Append(yourAliasName);
                    whereBuf.Append(".");
                    whereBuf.Append(rpt.GetYourKey(j));
                    whereBuf.Append("(+)");
                    whereBuf.Append(" AND ");
                }
            }
            if (whereBuf.Length > 0)
            {
                whereBuf.Length = whereBuf.Length - 5;
                buf.Append(" WHERE ");
                buf.Append(whereBuf);
            }
            return buf.ToString();
        }
    }
}
