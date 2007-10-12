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
using System.Collections;
using System.Text;
using Seasar.Extension.ADO;
using Seasar.Framework.Log;
using Nullables;

namespace Seasar.Dao.Impl
{
	public class UpdateAutoDynamicCommand : AbstractSqlCommand
	{
        private const int NO_UPDATE = 0;

        private IBeanMetaData _beanMetaData;

        private String[] _propertyNames;

        public UpdateAutoDynamicCommand(IDataSource dataSource, ICommandFactory commandFactory,
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
            IPropertyType[] propertyTypes = CreateUpdatePropertyTypes(bmd,
                bean, propertyNames);
            if(CanExecute(bean, bmd, propertyTypes, propertyNames) == false)
            {
                return NO_UPDATE;
            }
            UpdateAutoHandler handler = new UpdateAutoHandler(DataSource,
                    CommandFactory, bmd, propertyTypes);
            handler.Sql = CreateUpdateSql(bmd, propertyTypes);
            int i = handler.Execute(args);
            if ( i < 1 )
            {
                throw new NotSingleRowUpdatedRuntimeException(args[0], i);
            }
            return i;
        }

        protected virtual IPropertyType[] CreateUpdatePropertyTypes(IBeanMetaData bmd, object bean, string[] propertyNames)
        {
            IList types = new ArrayList();
            string timestampPropertyName = bmd.TimestampPropertyName;
            string versionNoPropertyName = bmd.VersionNoPropertyName;
            for ( int i = 0; i < propertyNames.Length; ++i )
            {
                IPropertyType pt = bmd.GetPropertyType(propertyNames[i]);
                if ( IsUpdatableProperty(pt, timestampPropertyName, versionNoPropertyName, bean) )
                {
                    Console.WriteLine("pt = {0}, value = {1}", pt.PropertyName, pt.PropertyInfo.GetValue(bean, null));
                    types.Add(pt);
                }
            }

            IPropertyType[] propertyTypes = new IPropertyType[types.Count];
            types.CopyTo(propertyTypes, 0);
            return propertyTypes;
        }

        protected virtual bool IsUpdatableProperty(IPropertyType pt, string timestampPropertyName, 
            string versionNoPropertyName, object bean)
        {
            if ( pt.IsPrimaryKey == false )
            {
                string propertyName = pt.PropertyName;
                if ( propertyName.Equals(timestampPropertyName, StringComparison.CurrentCultureIgnoreCase)
                            || propertyName.Equals(versionNoPropertyName, StringComparison.CurrentCultureIgnoreCase)
                            || pt.PropertyInfo.GetValue(bean, null) != null )
                {
                    object value = pt.PropertyInfo.GetValue(bean, null);
                    if ( value is INullableType && ( (INullableType)value ).HasValue == false )
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        protected virtual string CreateUpdateSql(IBeanMetaData bmd, IPropertyType[] propertyTypes)
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

        protected virtual bool CanExecute(object bean, IBeanMetaData bmd, IPropertyType[] propertyTypes, string[] propertyNames)
        {
            if ( propertyTypes.Length == 0 )
            {
                throw new NoUpdatePropertyTypeRuntimeException();
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
