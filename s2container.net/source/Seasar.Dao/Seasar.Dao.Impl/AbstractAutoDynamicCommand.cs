using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Extension.ADO;
using System.Collections;
using Nullables;

namespace Seasar.Dao.Impl
{
	public abstract class AbstractAutoDynamicCommand : AbstractSqlCommand
	{
        private const int NO_UPDATE = 0;

        private IBeanMetaData _beanMetaData;

        private String[] _propertyNames;

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
            if(CanExecute(bean, bmd, propertyTypes, propertyNames) == false)
            {
                return NO_UPDATE;
            }
            AbstractAutoHandler handler = CreateAutoHandler(DataSource, CommandFactory, bmd, propertyTypes);
            handler.Sql = SetupSql(bmd, propertyTypes);
            int i = handler.Execute(args);
            if ( i < 1 )
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
            for ( int i = 0; i < propertyNames.Length; ++i )
            {
                IPropertyType pt = bmd.GetPropertyType(propertyNames[i]);
                if ( IsTargetProperty(pt, timestampPropertyName, versionNoPropertyName, bean) )
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
            if ( propertyName.Equals(timestampPropertyName, StringComparison.CurrentCultureIgnoreCase)
                        || propertyName.Equals(versionNoPropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            
            object value = pt.PropertyInfo.GetValue(bean, null);
            if(value == null)
            {
                return false;
            }
                
            if ( value is INullableType && ( (INullableType)value ).HasValue == false )
            {
                return false;
            }

            return true;
        }

        protected virtual bool CanExecute(object bean, IBeanMetaData bmd, IPropertyType[] propertyTypes, string[] propertyNames)
        {
            if ( propertyTypes.Length == 0 )
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
