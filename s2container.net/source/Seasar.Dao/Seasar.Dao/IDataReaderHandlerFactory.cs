using System;
using System.Reflection;
using Seasar.Extension.ADO;

namespace Seasar.Dao
{
    public interface IDataReaderHandlerFactory
    {
        IDataReaderHandler GetResultSetHandler(Type beanType, IBeanMetaData bmd, MethodInfo mi);
    }
}