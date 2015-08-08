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
using System.Reflection;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Impl
{
    public class FieldBeanAnnotationReader : IBeanAnnotationReader
    {
        protected Type beanType;

        public FieldBeanAnnotationReader(Type beanType)
        {
            this.beanType = beanType;
        }

        #region IBeanAnnotationReader ÉÅÉìÉo

        public string GetColumn(PropertyInfo pi)
        {
            var attr = AttributeUtil.GetColumnAttribute(pi);
            return attr?.ColumnName;
        }

        public string GetTable()
        {
            var attr = AttributeUtil.GetTableAttribute(beanType);
            return attr?.TableName;
        }

        public string GetVersionNoProteryName()
        {
            var attr = AttributeUtil.GetVersionNoPropertyAttribute(beanType);
            return attr?.PropertyName;
        }

        public string GetTimestampPropertyName()
        {
            var attr = AttributeUtil.GetTimestampPropertyAttribute(beanType);
            return attr?.PropertyName;
        }

        public IDAttribute GetIdAttribute(PropertyInfo pi, IDbms dbms)
        {
            var attrs = AttributeUtil.GetIDAttribute(pi);
            IDAttribute defaultAttr = null;
            foreach (var attr in attrs)
            {
                if (attr.Dbms == dbms.Dbms)
                {
                    return attr;
                }
                if (attr.Dbms == KindOfDbms.None)
                {
                    if (attr.IDType == IDType.IDENTITY && dbms.IdentitySelectString != null)
                    {
                        defaultAttr = attr;
                    }
                    if (attr.IDType == IDType.SEQUENCE && dbms.GetSequenceNextValString(attr.SequenceName) != null)
                    {
                        defaultAttr = attr;
                    }
                    if (attr.IDType == IDType.ASSIGNED)
                    {
                        defaultAttr = attr;
                    }
                }
            }
            return defaultAttr;
        }

        public string[] GetNoPersisteneProps()
        {
            var attr = AttributeUtil.GetNoPersistentPropsAttribute(beanType);
            return attr?.Props;
        }

        public RelnoAttribute GetRelnoAttribute(PropertyInfo pi)
        {
            var attr = AttributeUtil.GetRelnoAttribute(pi);
            return attr;
        }

        public string GetRelationKey(PropertyInfo pi)
        {
            var attr = AttributeUtil.GetRelkeysAttribute(pi);
            return attr?.Relkeys;
        }

        #endregion
    }
}
