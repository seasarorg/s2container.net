#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using System.Text;
using Seasar.Extension.ADO;
using Seasar.Framework.Exceptions;

namespace Seasar.Dao.Impl
{
    public class InsertAutoDynamicCommand : AbstractAutoDynamicCommand
    {
        public InsertAutoDynamicCommand(IDataSource dataSource, ICommandFactory commandFactory,
            IBeanMetaData beanMetaData, string[] propertyNames)
            : base(dataSource, commandFactory, beanMetaData, propertyNames)
        {
        }

        protected override AbstractAutoHandler CreateAutoHandler(IDataSource dataSource, ICommandFactory commandFactory,
            IBeanMetaData beanMetaData, IPropertyType[] propertyTypes)
        {
            return new InsertAutoHandler(dataSource, commandFactory, beanMetaData, propertyTypes);
        }

        protected override string SetupSql(IBeanMetaData bmd, IPropertyType[] propertyTypes)
        {
            StringBuilder buf = new StringBuilder(100);
            buf.Append("INSERT INTO ");
            buf.Append(bmd.TableName);
            buf.Append(" (");
            for (int i = 0; i < propertyTypes.Length; ++i)
            {
                IPropertyType pt = propertyTypes[i];
                String columnName = pt.ColumnName;
                if (i > 0)
                {
                    buf.Append(", ");
                }
                buf.Append(columnName);
            }
            buf.Append(") VALUES (");
            for (int i = 0; i < propertyTypes.Length; ++i)
            {
                if (i > 0)
                {
                    buf.Append(", ");
                }
                buf.Append("?");
            }
            buf.Append(")");
            return buf.ToString();
        }

        protected override bool IsTargetProperty(IPropertyType pt, string timestampPropertyName, string versionNoPropertyName, object bean)
        {
            IIdentifierGenerator identifierGenerator = BeanMetaData.IdentifierGenerator;
            if (pt.IsPrimaryKey)
            {
                return identifierGenerator.IsSelfGenerate;
            }
            return base.IsTargetProperty(pt, timestampPropertyName, versionNoPropertyName, bean);
        }

        protected override bool CanExecute(object bean, IBeanMetaData bmd, IPropertyType[] propertyTypes, string[] propertyNames)
        {
            if (propertyTypes.Length == 0)
            {
                throw new SRuntimeException("EDAO0014");
            }
            return true;
        }
    }
}
