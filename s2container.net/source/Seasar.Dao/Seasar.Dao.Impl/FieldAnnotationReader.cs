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
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Impl
{
    public class FieldAnnotationReader : IDaoAnnotationReader
    {
        protected Type daoBeanType;

        public FieldAnnotationReader(Type daoBeanType)
        {
            this.daoBeanType = daoBeanType;
        }

        #region IDaoAnnotationReader メンバ

        public string GetQuery(string name)
        {
            var mi = daoBeanType.GetMethod(name);
            var queryAttr = AttributeUtil.GetQueryAttribute(mi);
            return queryAttr?.Query;
        }

        public Type GetBeanType()
        {
            var beanAttr = AttributeUtil.GetBeanAttribute(daoBeanType);
            return beanAttr.BeanType;
        }

        public string[] GetNoPersistentProps(string methodName)
        {
            var mi = daoBeanType.GetMethod(methodName);
            var nppAttr = AttributeUtil.GetNoPersistentPropsAttribute(mi);
            return nppAttr?.Props;
        }

        public string[] GetPersistentProps(string methodName)
        {
            var mi = daoBeanType.GetMethod(methodName);
            var ppAttr = AttributeUtil.GetPersistentPropsAttribute(mi);
            return ppAttr?.Props;
        }

        public string GetSql(string name, IDbms dbms)
        {
            var mi = daoBeanType.GetMethod(name);
            var sqlAttrs = AttributeUtil.GetSqlAttributes(mi);
            SqlAttribute defaultSqlAttr = null;
            foreach (var sqlAttr in sqlAttrs)
            {
                if (sqlAttr.Dbms == dbms.Dbms)
                    return sqlAttr.Sql;
                if (sqlAttr.Dbms == KindOfDbms.None)
                    defaultSqlAttr = sqlAttr;
            }

            return defaultSqlAttr?.Sql;
        }

        /// <summary>
        /// プロシージャ名を取得する
        /// </summary>
        /// <param name="methodName">メソッド名</param>
        /// <returns>プロシージャ名</returns>
        public string GetProcedure(string methodName)
        {
            var mi = daoBeanType.GetMethod(methodName);
            var procedureAttribute = AttributeUtil.GetProcedureAttribute(mi);
            return procedureAttribute?.ProcedureName;
        }

        public bool IsSqlFile(string methodName)
        {
            var mi = daoBeanType.GetMethod(methodName);
            var sqlFileAttrs = AttributeUtil.GetSqlFileAttribute(mi);
            return sqlFileAttrs != null;
        }

        public string GetSqlFilePath(string methodName)
        {
            var mi = daoBeanType.GetMethod(methodName);
            var sqlFileAttrs = AttributeUtil.GetSqlFileAttribute(mi);
            return sqlFileAttrs?.FileName;
        }

        #endregion
    }
}
