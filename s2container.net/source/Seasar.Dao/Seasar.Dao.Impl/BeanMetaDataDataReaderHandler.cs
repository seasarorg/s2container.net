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

using System.Data;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public class BeanMetaDataDataReaderHandler
        : AbstractBeanMetaDataDataReaderHandler
    {
        public BeanMetaDataDataReaderHandler(IBeanMetaData beanMetaData, IRowCreator rowCreator, IRelationRowCreator relationRowCreator)
            : base(beanMetaData, rowCreator, relationRowCreator)
        {
        }

        public override object Handle(IDataReader dataReader)
        {
            if (dataReader.Read())
            {
                var columnNames = CreateColumnNames(dataReader.GetSchemaTable());
                var columns = CreateColumnMetaData(columnNames);
                var relationPropertyCache = CreateRelationPropertyCache(columnNames);
                var row = CreateRow(dataReader, columns);
                for (var i = 0; i < BeanMetaData.RelationPropertyTypeSize; ++i)
                {
                    var rpt = BeanMetaData.GetRelationPropertyType(i);
                    if (rpt == null) continue;
                    var relationRow = CreateRelationRow(dataReader, rpt,
                                        columnNames, null, relationPropertyCache);
                    if (relationRow != null)
                    {
                        var pi = rpt.PropertyInfo;
//                        pi.SetValue(row, relationRow, null);
                        PropertyUtil.SetValue(row, row.GetExType(), pi.Name, pi.PropertyType, relationRow);
                    }
                }
                return row;
            }
            else
            {
                return null;
            }
        }
    }
}
