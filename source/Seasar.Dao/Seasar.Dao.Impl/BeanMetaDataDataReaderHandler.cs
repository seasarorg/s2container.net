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

namespace Seasar.Dao.Impl
{
    public class BeanMetaDataDataReaderHandler
        : AbstractBeanMetaDataDataReaderHandler
    {
        public BeanMetaDataDataReaderHandler(IBeanMetaData beanMetaData)
            : base(beanMetaData)
        {
        }

        public override object Handle(IDataReader dataReader)
        {
            if (dataReader.Read())
            {
                IList columnNames = CreateColumnNames(dataReader.GetSchemaTable());
                IColumnMetaData[] columns = CreateColumnMetaData(columnNames);
                object row = CreateRow(dataReader, columns);
                for (int i = 0; i < BeanMetaData.RelationPropertyTypeSize; ++i)
                {
                    IRelationPropertyType rpt = BeanMetaData
                        .GetRelationPropertyType(i);
                    if (rpt == null) continue;
                    object relationRow = CreateRelationRow(dataReader, rpt,
                        columnNames, null);
                    if (relationRow != null)
                    {
                        PropertyInfo pi = rpt.PropertyInfo;
                        pi.SetValue(row, relationRow, null);
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
