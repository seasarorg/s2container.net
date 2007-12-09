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
            ColumnAttribute attr = AttributeUtil.GetColumnAttribute(pi);
            if (attr != null)
            {
                return attr.ColumnName;
            }
            return null;
        }

        public string GetTable()
        {
            TableAttribute attr = AttributeUtil.GetTableAttribute(beanType);
            if (attr != null)
            {
                return attr.TableName;
            }
            return null;
        }

        public string GetVersionNoProteryName()
        {
            VersionNoPropertyAttribute attr = AttributeUtil.GetVersionNoPropertyAttribute(beanType);
            if (attr != null)
            {
                return attr.PropertyName;
            }
            return null;
        }

        public string GetTimestampPropertyName()
        {
            TimestampPropertyAttribute attr = AttributeUtil.GetTimestampPropertyAttribute(beanType);
            if (attr != null)
            {
                return attr.PropertyName;
            }
            return null;
        }

        public IDAttribute GetIdAttribute(PropertyInfo pi, IDbms dbms)
        {
            IDAttribute[] attrs = AttributeUtil.GetIDAttribute(pi);
            IDAttribute defaultAttr = null;
            foreach (IDAttribute attr in attrs)
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
            NoPersistentPropsAttribute attr = AttributeUtil.GetNoPersistentPropsAttribute(beanType);
            if (attr != null)
            {
                return attr.Props;
            }
            return null;
        }

        public RelnoAttribute GetRelnoAttribute(PropertyInfo pi)
        {
            RelnoAttribute attr = AttributeUtil.GetRelnoAttribute(pi);
            if (attr != null)
            {
                return attr;
            }
            return null;
        }

        public string GetRelationKey(PropertyInfo pi)
        {
            RelkeysAttribute attr = AttributeUtil.GetRelkeysAttribute(pi);
            if (attr != null)
            {
                return attr.Relkeys;
            }
            return null;
        }

        #endregion
    }
}
