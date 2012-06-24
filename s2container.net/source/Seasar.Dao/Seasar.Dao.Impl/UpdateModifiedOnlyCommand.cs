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

using System;
using System.Collections;
using System.Text;
using Seasar.Extension.ADO;
using Seasar.Framework.Log;

namespace Seasar.Dao.Impl
{
    public class UpdateModifiedOnlyCommand : UpdateAutoDynamicCommand
    {
        private static readonly Logger _logger = Logger.GetLogger(typeof(UpdateAutoDynamicCommand));

        public UpdateModifiedOnlyCommand(IDataSource dataSource, ICommandFactory commandFactory,
            IBeanMetaData beanMetaData, string[] propertyNames)
            : base(dataSource, commandFactory, beanMetaData, propertyNames)
        {
        }

        protected override IPropertyType[] CreateTargetPropertyTypes(IBeanMetaData bmd, object bean,
            string[] propertyNames)
        {
            IDictionary modifiedPropertyNames = bmd.GetModifiedPropertyNames(bean);
            IList types = new ArrayList();
            string timestampPropertyName = bmd.TimestampPropertyName;
            string versionNoPropertyName = bmd.VersionNoPropertyName;
            for (int i = 0; i < propertyNames.Length; ++i)
            {
                IPropertyType pt = bmd.GetPropertyType(propertyNames[i]);
                if (pt.IsPrimaryKey == false)
                {
                    string propertyName = pt.PropertyName;
                    if (propertyName.Equals(timestampPropertyName, StringComparison.CurrentCultureIgnoreCase)
                            || propertyName.Equals(versionNoPropertyName, StringComparison.CurrentCultureIgnoreCase)
                            || modifiedPropertyNames.Contains(propertyName))
                    {
                        types.Add(pt);
                    }
                }
            }
            IPropertyType[] propertyTypes = new IPropertyType[types.Count];
            types.CopyTo(propertyTypes, 0);
            return propertyTypes;
        }

        protected override bool CanExecute(object bean, IBeanMetaData bmd, IPropertyType[] propertyTypes,
            string[] propertyNames)
        {
            if (propertyTypes.Length > 0)
            {
                return true;
            }

            if (_logger.IsDebugEnabled)
            {
                string s = CreateNoUpdateLogMessage(bean, bmd);
                _logger.Debug(s);
            }
            return false;
        }

        protected virtual string CreateNoUpdateLogMessage(object bean, IBeanMetaData bmd)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("skip UPDATE: table=");
            builder.Append(bmd.TableName);
            int size = bmd.PrimaryKeySize;
            for (int i = 0; i < size; i++)
            {
                if (i == 0)
                {
                    builder.Append(", key{");
                }
                else
                {
                    builder.Append(", ");
                }
                string keyName = bmd.GetPrimaryKey(i);
                builder.Append(keyName);
                builder.Append("=");
                builder.Append(bmd.GetPropertyTypeByColumnName(keyName)
                        .PropertyInfo.GetValue(bean, null));
                if (i == size - 1)
                {
                    builder.Append("}");
                }
            }

            return builder.ToString();
        }
    }
}
