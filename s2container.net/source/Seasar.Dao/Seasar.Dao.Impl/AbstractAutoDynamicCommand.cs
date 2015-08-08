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
using System.Collections;
using System.Data.SqlTypes;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

#if NHIBERNATE_NULLABLES
using Nullables;
#endif

namespace Seasar.Dao.Impl
{
    public abstract class AbstractAutoDynamicCommand : AbstractSqlCommand
    {
        private const int NO_UPDATE = 0;

        protected AbstractAutoDynamicCommand(IDataSource dataSource, ICommandFactory commandFactory,
            IBeanMetaData beanMetaData, string[] propertyNames)
            : base(dataSource, commandFactory)
        {
            BeanMetaData = beanMetaData;
            PropertyNames = propertyNames;
        }

        public override object Execute(object[] args)
        {
            var bean = args[0];
            var bmd = BeanMetaData;
            var propertyNames = PropertyNames;
            var propertyTypes = CreateTargetPropertyTypes(bmd, bean, propertyNames);
            if (CanExecute(bean, bmd, propertyTypes, propertyNames) == false)
            {
                return NO_UPDATE;
            }
            var handler = CreateAutoHandler(DataSource, CommandFactory, bmd, propertyTypes);
            handler.Sql = SetupSql(bmd, propertyTypes);
            var i = handler.Execute(args);
            if (i < 1)
            {
                throw new NotSingleRowUpdatedRuntimeException(args[0], i);
            }
            return i;
        }

        protected abstract string SetupSql(IBeanMetaData bmd, IPropertyType[] propertyTypes);

        protected abstract AbstractAutoHandler CreateAutoHandler(IDataSource dataSource, ICommandFactory commandFactory,
            IBeanMetaData beanMetaData, IPropertyType[] propertyTypes);

        protected virtual IPropertyType[] CreateTargetPropertyTypes(IBeanMetaData bmd, object bean, string[] propertyNames)
        {
            IList types = new ArrayList();
            var timestampPropertyName = bmd.TimestampPropertyName;
            var versionNoPropertyName = bmd.VersionNoPropertyName;
            for (var i = 0; i < propertyNames.Length; ++i)
            {
                var pt = bmd.GetPropertyType(propertyNames[i]);
                if (IsTargetProperty(pt, timestampPropertyName, versionNoPropertyName, bean))
                {
                    types.Add(pt);
                }
            }

            var propertyTypes = new IPropertyType[types.Count];
            types.CopyTo(propertyTypes, 0);
            return propertyTypes;
        }

        protected virtual bool IsTargetProperty(IPropertyType pt, string timestampPropertyName,
            string versionNoPropertyName, object bean)
        {
            var propertyName = pt.PropertyName;
            if (propertyName.Equals(timestampPropertyName, StringComparison.CurrentCultureIgnoreCase)
                        || propertyName.Equals(versionNoPropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

//            object value = pt.PropertyInfo.GetValue(bean, null);
            var value = PropertyUtil.GetValue(bean, bean.GetExType(), pt.PropertyInfo.Name);

            //  for normal type include Nullable<T>
            if (value == null)
            {
                return false;
            }

#if NHIBERNATE_NULLABLES
            //  for Nullables.INullableType
            if (value is INullableType && ((INullableType)value).HasValue == false)
            {
                return false;
            }
#endif

            //  for Sytem.Data.SqlTypes.INullable
            var nullable = value as INullable;
            if (nullable != null && nullable.IsNull)
            {
                return false;
            }

            return true;
        }

        protected virtual bool CanExecute(object bean, IBeanMetaData bmd, IPropertyType[] propertyTypes, string[] propertyNames)
        {
            if (propertyTypes.Length == 0)
            {
                return false;
            }
            return true;
        }

        public IBeanMetaData BeanMetaData { get; }

        public string[] PropertyNames { get; }
    }
}
