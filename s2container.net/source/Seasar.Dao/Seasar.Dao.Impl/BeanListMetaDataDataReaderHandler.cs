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

using System.Collections;
using System.Collections.Generic;
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public class BeanListMetaDataDataReaderHandler
        : AbstractBeanMetaDataDataReaderHandler
    {
        public BeanListMetaDataDataReaderHandler(IBeanMetaData beanMetaData, IRowCreator rowCreator, IRelationRowCreator relationRowCreator)
            : base(beanMetaData, rowCreator, relationRowCreator)
        {
        }

        public override object Handle(IDataReader dataReader)
        {
            var list = new ArrayList();

            Handle(dataReader, list);

            return list;
        }

        protected void Handle(IDataReader dataReader, IList list)
        {
            var columnNames = CreateColumnNames(dataReader.GetSchemaTable());

            IColumnMetaData[] columns = null;// [DAONET-56] (2007/08/29)
            IDictionary<string, IDictionary<string, IPropertyType>> relationPropertyCache = null;

            var relSize = BeanMetaData.RelationPropertyTypeSize;
            var relRowCache = new RelationRowCache(relSize);
            while (dataReader.Read())
            {
                // Lazy initialization because if the result is zero, the cache is unused.
                if (columns == null) {
                    columns = CreateColumnMetaData(columnNames);
                }
                if (relationPropertyCache == null) {
                    relationPropertyCache = CreateRelationPropertyCache(columnNames);
                }

                var row = CreateRow(dataReader, columns);
                for (var i = 0; i < relSize; ++i)
                {
                    var rpt = BeanMetaData.GetRelationPropertyType(i);
                    if (rpt == null) continue;

                    object relRow = null;
                    var relKeyValues = new Hashtable();
                    var relKey = CreateRelationKey(dataReader, rpt, columnNames,
                        relKeyValues);
                    if (relKey != null)
                    {
                        relRow = relRowCache.GetRelationRow(i, relKey);
                        if (relRow == null)
                        {
                            relRow = CreateRelationRow(dataReader, rpt, columnNames,
                                relKeyValues, relationPropertyCache);
                            relRowCache.AddRelationRow(i, relKey, relRow);
                        }
                    }
                    if (relRow != null)
                    {
                        var pi = rpt.PropertyInfo;
//                        pi.SetValue(row, relRow, null);
                        PropertyUtil.SetValue(row, row.GetExType(), pi.Name, pi.PropertyType, relRow);
                    }
                }
                list.Add(row);
            }
        }

        protected RelationKey CreateRelationKey(IDataReader reader,
            IRelationPropertyType rpt, IList columnNames, Hashtable relKeyValues)
        {
            var keyList = new ArrayList();
            var bmd = rpt.BeanMetaData;
            for (var i = 0; i < rpt.KeySize; ++i)
            {
                IValueType valueType;
                var columnName = rpt.GetMyKey(i);
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
                var value = valueType.GetValue(reader, columnName);
                if (value == null) return null;

                relKeyValues[columnName] = value;
                keyList.Add(value);
            }
            if (keyList.Count > 0)
            {
                var keys = keyList.ToArray();
                return new RelationKey(keys);
            }
            else
            {
                return null;
            }
        }
    }
}
