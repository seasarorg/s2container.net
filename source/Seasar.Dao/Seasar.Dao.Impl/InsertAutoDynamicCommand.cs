using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Extension.ADO;
using Nullables;
using System.Collections;
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
            for (int i = 0; i < propertyTypes.Length; ++i) {
                IPropertyType pt = propertyTypes[i];
                String columnName = pt.ColumnName;
                if (i > 0) {
                    buf.Append(", ");
                }
                buf.Append(columnName);
            }
            buf.Append(") VALUES (");
            for (int i = 0; i < propertyTypes.Length; ++i) {
                if (i > 0) {
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
            if (pt.IsPrimaryKey && identifierGenerator.IsSelfGenerate == false )
            {
                return false;
            }
            return base.IsTargetProperty(pt, timestampPropertyName, versionNoPropertyName, bean);
        }

        protected override bool CanExecute(object bean, IBeanMetaData bmd, IPropertyType[] propertyTypes, string[] propertyNames)
        {
            if ( propertyTypes.Length == 0 )
            {
                throw new SRuntimeException("EDAO0014");
            }
            return true;
        }
	}
}
