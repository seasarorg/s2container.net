using System;
using System.Text;
using System.Data;
using Seasar.Framework.Util;
using System.Collections;

namespace Seasar.Dao.Impl
{
    public class BeanDataTableMetaDataDataReaderHandler : AbstractBeanMetaDataDataReaderHandler
    {
        private Type _returnType;

        public BeanDataTableMetaDataDataReaderHandler(IBeanMetaData beanMetaData, IRowCreator rowCreator, 
            IRelationRowCreator relationRowCreator, Type returnType)
            : base(beanMetaData, rowCreator, relationRowCreator)
        {
            _returnType = returnType;
        }

        public override object Handle(IDataReader dataReader)
        {
            DataTable table = (DataTable)Activator.CreateInstance(_returnType);
            table.Load(dataReader);
            return table;
        }
    }
}
