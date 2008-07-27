using System;
using System.Data;
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public class DataReaderHandlerFactory : IDataReaderHandlerFactory
    {
        public virtual IDataReaderHandler GetResultSetHandler(Type beanType, IBeanMetaData bmd, MethodInfo mi)
        {
            Type retType = mi.ReturnType;

            if (typeof(DataSet).IsAssignableFrom(retType))
            {
                return CreateBeanDataSetMetaDataDataReaderHandler(bmd, retType);
            }
            else if (typeof(DataTable).IsAssignableFrom(retType))
            {
                return CreateBeanDataTableMetaDataDataReaderHandler(bmd, retType);
            }
            else if (retType.IsArray)
            {
                // [DAONET-76] (2008/05/05)
                Type elementType = retType.GetElementType();
                if (AssignTypeUtil.IsSimpleType(elementType))
                {
                    return CreateObjectArrayDataReaderHandler(elementType);
                }
                else
                {
                    return CreateBeanArrayMetaDataDataReaderHandler(bmd);
                }
            }
            else if (AssignTypeUtil.IsList(retType))
            {
                // [DAONET-76] (2008/05/05)
                if (AssignTypeUtil.IsSimpleType(beanType))
                {
                    return CreateObjectListDataReaderHandler();
                }
                else
                {
                    return CreateBeanListMetaDataDataReaderHandler(bmd);
                }
            }
            else if (IsBeanTypeAssignable(beanType, retType))
            {
                return CreateBeanMetaDataDataReaderHandler(bmd);
            }
            else if (AssignTypeUtil.IsGenericList(retType))
            {
                // [DAONET-76] (2008/05/05)
                Type elementType = retType.GetGenericArguments()[0];
                if (AssignTypeUtil.IsSimpleType(elementType))
                {
                    return CreateObjectGenericListDataReaderHandler(elementType);
                }
                else
                {
                    return CreateBeanGenericListMetaDataDataReaderHandler(bmd);
                }
            }
            else
            {
                return CreateObjectDataReaderHandler();
            }
        }

        // [DAONET-76] (2008/05/05)
        protected virtual ObjectGenericListDataReaderHandler CreateObjectGenericListDataReaderHandler(Type elementType)
        {
            return new ObjectGenericListDataReaderHandler(elementType);
        }

        // [DAONET-76] (2008/05/05)
        protected virtual ObjectListDataReaderHandler CreateObjectListDataReaderHandler()
        {
            return new ObjectListDataReaderHandler();
        }

        // [DAONET-76] (2008/05/05)
        protected virtual ObjectArrayDataReaderHandler CreateObjectArrayDataReaderHandler(Type elementType)
        {
            return new ObjectArrayDataReaderHandler(elementType);
        }

        protected virtual BeanDataSetMetaDataDataReaderHandler CreateBeanDataSetMetaDataDataReaderHandler(IBeanMetaData bmd, Type returnType)
        {
            return new BeanDataSetMetaDataDataReaderHandler(returnType);
        }

        protected virtual BeanDataTableMetaDataDataReaderHandler CreateBeanDataTableMetaDataDataReaderHandler(IBeanMetaData bmd, Type returnType)
        {
            return new BeanDataTableMetaDataDataReaderHandler(returnType);
        }

        protected virtual BeanListMetaDataDataReaderHandler CreateBeanListMetaDataDataReaderHandler(IBeanMetaData bmd)
        {
            return new BeanListMetaDataDataReaderHandler(bmd, CreateRowCreator(), CreateRelationRowCreator());
        }

        protected virtual BeanMetaDataDataReaderHandler CreateBeanMetaDataDataReaderHandler(IBeanMetaData bmd)
        {
            return new BeanMetaDataDataReaderHandler(bmd, CreateRowCreator(), CreateRelationRowCreator());
        }

        protected virtual BeanArrayMetaDataDataReaderHandler CreateBeanArrayMetaDataDataReaderHandler(IBeanMetaData bmd)
        {
            return new BeanArrayMetaDataDataReaderHandler(bmd, CreateRowCreator(), CreateRelationRowCreator());
        }

        protected virtual BeanGenericListMetaDataDataReaderHandler CreateBeanGenericListMetaDataDataReaderHandler(IBeanMetaData bmd)
        {
            return new BeanGenericListMetaDataDataReaderHandler(bmd, CreateRowCreator(), CreateRelationRowCreator());
        }

        protected virtual ObjectDataReaderHandler CreateObjectDataReaderHandler()
        {
            return new ObjectDataReaderHandler();
        }

        protected virtual IRowCreator CreateRowCreator()
        {// [DAONET-56] (2007/08/29)
            return new RowCreatorImpl();
        }

        protected virtual IRelationRowCreator CreateRelationRowCreator()
        {// [DAONET-56] (2007/08/29)
            return new RelationRowCreatorImpl();
        }

        protected virtual bool IsBeanTypeAssignable(Type beanType, Type type)
        {
            return beanType.IsAssignableFrom(type) ||
                type.IsAssignableFrom(beanType);
        }
    }
}