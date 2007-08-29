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
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public class RelationRowCreatorImpl : IRelationRowCreator
    {
        public virtual object CreateRelationRow(IDataReader reader, IRelationPropertyType rpt,
            System.Collections.IList columnNames, System.Collections.Hashtable relKeyValues,
            IDictionary<string, IDictionary<string, IColumnMetaData>> columnMetaDataCache)
        {
            object row = null;
            IBeanMetaData bmd = rpt.BeanMetaData;
            for (int i = 0; i < rpt.KeySize; ++i)
            {
                string columnName = rpt.GetMyKey(i);
                if (columnNames.Contains(columnName))
                {
                    if (row == null) row = CreateRelationRow(rpt);
                    if (relKeyValues != null && relKeyValues.ContainsKey(columnName))
                    {
                        object value = relKeyValues[columnName];
                        IPropertyType pt = bmd.GetPropertyTypeByColumnName(rpt.GetYourKey(i));
                        PropertyInfo pi = pt.PropertyInfo;
                        if (value != null) pi.SetValue(row, value, null);
                    }
                }
                continue;
            }
            for (int i = 0; i < bmd.PropertyTypeSize; ++i)
            {
                IPropertyType pt = bmd.GetPropertyType(i);
                if (!IsTargetProperty(pt)) {
                    continue;
                }
                string columnName = pt.ColumnName + "_" + rpt.RelationNo;
                if (!columnNames.Contains(columnName)) continue;
                if (row == null) row = CreateRelationRow(rpt);
                object value = null;
                PropertyInfo pi = pt.PropertyInfo;
                if (relKeyValues != null && relKeyValues.ContainsKey(columnName))
                {
                    value = relKeyValues[columnName];
                }
                else
                {
                    IValueType valueType = pt.ValueType;
                    value = valueType.GetValue(reader, columnName);
                }
                if (value != null) pi.SetValue(row, value, null);
            }
            return row;
        }
        
        protected virtual object CreateRelationRow(IRelationPropertyType rpt)
        {
            return ClassUtil.NewInstance(rpt.PropertyInfo.PropertyType);
        }

        protected virtual bool IsTargetProperty(IPropertyType pt) {// [DAONET-56] (2007/08/29)
            return pt.PropertyInfo.CanWrite;
        }

        protected virtual ColumnMetaDataImpl NewColumnMetaDataImpl(IPropertyType propertyType, string columnName) {
            return new ColumnMetaDataImpl(propertyType, columnName);
        }

        public virtual IDictionary<string, IDictionary<string, IColumnMetaData>> CreateRelationColumnMetaData(System.Collections.IList columnNames, IBeanMetaData beanMetaData) {
            return null;// TODO: @jflute -- RelationÇÃColumnMetaDataÇçÏÇ¡Çƒï‘Ç∑Ç±Ç∆
        }
    }
}
