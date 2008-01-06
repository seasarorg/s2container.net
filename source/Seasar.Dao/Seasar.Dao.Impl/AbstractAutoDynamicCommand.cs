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
using Seasar.Extension.ADO;
using System.Collections;
using Nullables;
using System.Data.SqlTypes;

namespace Seasar.Dao.Impl
{
    public abstract class AbstractAutoDynamicCommand : AbstractSqlCommand
    {
        private const int NO_UPDATE = 0;

        private readonly IBeanMetaData _beanMetaData;

        private readonly string[] _propertyNames;

        public AbstractAutoDynamicCommand(IDataSource dataSource, ICommandFactory commandFactory,
            IBeanMetaData beanMetaData, string[] propertyNames)
            : base(dataSource, commandFactory)
        {
            _beanMetaData = beanMetaData;
            _propertyNames = propertyNames;
        }

        public override object Execute(object[] args)
        {
            object bean = args[0];
            IBeanMetaData bmd = BeanMetaData;
            string[] propertyNames = PropertyNames;
            IPropertyType[] propertyTypes = CreateTargetPropertyTypes(bmd,
                bean, propertyNames);
            if (CanExecute(bean, bmd, propertyTypes, propertyNames) == false)
            {
                return NO_UPDATE;
            }
            AbstractAutoHandler handler = CreateAutoHandler(DataSource, CommandFactory, bmd, propertyTypes);
            handler.Sql = SetupSql(bmd, propertyTypes);
            int i = handler.Execute(args);
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
            string timestampPropertyName = bmd.TimestampPropertyName;
            string versionNoPropertyName = bmd.VersionNoPropertyName;
            for (int i = 0; i < propertyNames.Length; ++i)
            {
                IPropertyType pt = bmd.GetPropertyType(propertyNames[i]);
                if (IsTargetProperty(pt, timestampPropertyName, versionNoPropertyName, bean))
                {
                    types.Add(pt);
                }
            }

            IPropertyType[] propertyTypes = new IPropertyType[types.Count];
            types.CopyTo(propertyTypes, 0);
            return propertyTypes;
        }

        protected virtual bool IsTargetProperty(IPropertyType pt, string timestampPropertyName,
            string versionNoPropertyName, object bean)
        {
            string propertyName = pt.PropertyName;
            if (propertyName.Equals(timestampPropertyName, StringComparison.CurrentCultureIgnoreCase)
                        || propertyName.Equals(versionNoPropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            object value = pt.PropertyInfo.GetValue(bean, null);

            //  for normal type include Nullable<T>
            if (value == null)
            {
                return false;
            }

            //  for Nullables.INullableType
            if (value is INullableType && ((INullableType)value).HasValue == false)
            {
                return false;
            }

            //  for Sytem.Data.SqlTypes.INullable
            if (value is INullable && ((INullable)value).IsNull)
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


        public IBeanMetaData BeanMetaData
        {
            get { return _beanMetaData; }
        }

        public string[] PropertyNames
        {
            get { return _propertyNames; }
        }
    }
}
