#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
using Seasar.Extension.ADO;

namespace Seasar.Dao.Impl
{
    public class UpdateAutoDynamicCommand : AbstractAutoDynamicCommand
    {
        public UpdateAutoDynamicCommand(IDataSource dataSource, ICommandFactory commandFactory,
            IBeanMetaData beanMetaData, string[] propertyNames)
            : base(dataSource, commandFactory, beanMetaData, propertyNames)
        {
        }

        protected override AbstractAutoHandler CreateAutoHandler(IDataSource dataSource, ICommandFactory commandFactory, 
            IBeanMetaData beanMetaData, IPropertyType[] propertyTypes)
        {
            return new UpdateAutoHandler(dataSource, commandFactory, beanMetaData, propertyTypes);
        }

        protected override string SetupSql(IBeanMetaData bmd, IPropertyType[] propertyTypes)
        {
            StringBuilder builder = new StringBuilder(100);
            builder.Append("UPDATE ");
            builder.Append(bmd.TableName);
            builder.Append(" SET ");
            for (int i = 0; i < propertyTypes.Length; ++i) {
                IPropertyType pt = propertyTypes[i];
                string columnName = pt.ColumnName;
                if (i > 0) {
                    builder.Append(", ");
                }
                builder.Append(columnName);
                builder.Append(" = ?");
            }

            builder.Append(" WHERE ");
            const string ADD_AND = " AND ";
            for (int i = 0; i < bmd.PrimaryKeySize; ++i) {
                builder.Append(bmd.GetPrimaryKey(i));
                builder.Append(" = ?");
                builder.Append(ADD_AND);
            }
            builder.Length = builder.Length - ADD_AND.Length;
            if (bmd.HasVersionNoPropertyType) {
                IPropertyType pt = bmd.VersionNoPropertyType;
                builder.Append(ADD_AND);
                builder.Append(pt.ColumnName);
                builder.Append(" = ?");
            }
            if (bmd.HasTimestampPropertyType) {
                IPropertyType pt = bmd.TimestampPropertyType;
                builder.Append(ADD_AND);
                builder.Append(pt.ColumnName);
                builder.Append(" = ?");
            }
            return builder.ToString();
        }

        protected override bool IsTargetProperty(IPropertyType pt, string timestampPropertyName, string versionNoPropertyName, object bean)
        {
            if (pt.IsPrimaryKey)
            {
                return false;
            }
            return base.IsTargetProperty(pt, timestampPropertyName, versionNoPropertyName, bean);
        }

        protected override bool CanExecute(object bean, IBeanMetaData bmd, IPropertyType[] propertyTypes, string[] propertyNames)
        {
            if ( propertyTypes.Length == 0 )
            {
                throw new NoUpdatePropertyTypeRuntimeException();
            }
            return true;
        }
    }
}
