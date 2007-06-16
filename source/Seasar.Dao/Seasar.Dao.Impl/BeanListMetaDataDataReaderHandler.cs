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
using System.Data;
using System.Reflection;
using Seasar.Extension.ADO;

namespace Seasar.Dao.Impl
{
    public class BeanListMetaDataDataReaderHandler
        : AbstractBeanMetaDataDataReaderHandler
    {
        public BeanListMetaDataDataReaderHandler(IBeanMetaData beanMetaData)
            : base(beanMetaData)
        {
        }

        public override object Handle(IDataReader dataReader)
        {
            ArrayList list = new ArrayList();

            Handle(dataReader, list);

            return list;
        }

        protected void Handle(IDataReader dataReader, IList list)
        {
            IList columnNames = CreateColumnNames(dataReader.GetSchemaTable());

            IColumnMetaData[] columns = CreateColumnMetaData(columnNames);

            int relSize = BeanMetaData.RelationPropertyTypeSize;
            RelationRowCache relRowCache = new RelationRowCache(relSize);
            while (dataReader.Read())
            {
                object row = CreateRow(dataReader, columns);
                for (int i = 0; i < relSize; ++i)
                {
                    IRelationPropertyType rpt = BeanMetaData.GetRelationPropertyType(i);
                    if (rpt == null) continue;

                    object relRow = null;
                    Hashtable relKeyValues = new Hashtable();
                    RelationKey relKey = CreateRelationKey(dataReader, rpt, columnNames,
                        relKeyValues);
                    if (relKey != null)
                    {
                        relRow = relRowCache.GetRelationRow(i, relKey);
                        if (relRow == null)
                        {
                            relRow = CreateRelationRow(dataReader, rpt, columnNames,
                                relKeyValues);
                            relRowCache.AddRelationRow(i, relKey, relRow);
                        }
                    }
                    if (relRow != null)
                    {
                        PropertyInfo pi = rpt.PropertyInfo;
                        pi.SetValue(row, relRow, null);
                    }
                }
                list.Add(row);
            }
        }

        protected RelationKey CreateRelationKey(IDataReader reader,
            IRelationPropertyType rpt, IList columnNames, Hashtable relKeyValues)
        {
            ArrayList keyList = new ArrayList();
            IBeanMetaData bmd = rpt.BeanMetaData;
            for (int i = 0; i < rpt.KeySize; ++i)
            {
                IValueType valueType = null;
                string columnName = rpt.GetMyKey(i);
                IPropertyType pt;
                if (columnNames.Contains(columnName))
                {
                    pt = BeanMetaData.GetPropertyTypeByColumnName(columnName);
                    valueType = pt.ValueType;
                }
                else
                {
                    pt = bmd.GetPropertyTypeByColumnName(rpt.GetYourKey(i));
                    columnName = pt.ColumnName + "_" + rpt.RelationNo;
                    if (columnNames.Contains(columnName))
                        valueType = pt.ValueType;
                    else
                        return null;
                }
                object value = valueType.GetValue(reader, columnName);
                if (value == null) return null;

                relKeyValues[columnName] = value;
                keyList.Add(value);
            }
            if (keyList.Count > 0)
            {
                object[] keys = keyList.ToArray();
                return new RelationKey(keys);
            }
            else return null;
        }
    }
}
