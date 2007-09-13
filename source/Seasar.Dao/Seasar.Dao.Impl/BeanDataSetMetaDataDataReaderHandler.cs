using System;
using System.Text;
using System.Data;
using System.Collections;

namespace Seasar.Dao.Impl
{
    public class BeanDataSetMetaDataDataReaderHandler : AbstractBeanMetaDataDataReaderHandler
    {
        private Type _returnType;

        public BeanDataSetMetaDataDataReaderHandler(IBeanMetaData beanMetaData, IRowCreator rowCreator, 
            IRelationRowCreator relationRowCreator, Type returnType)
            : base(beanMetaData, rowCreator, relationRowCreator)
        {
            _returnType = returnType;
        }

        public override object Handle(IDataReader dataReader)
        {
            DataSet dataSet = (DataSet)Activator.CreateInstance(_returnType);
            Handle(dataReader, dataSet);
            return dataSet;
        }

        protected void Handle(IDataReader dataReader, DataSet dataSet)
        {
            DataTable[] dataTables = new DataTable[dataSet.Tables.Count];
            dataSet.Tables.CopyTo(dataTables, 0);
            dataSet.Load(dataReader, LoadOption.OverwriteChanges, dataTables);
        }
    }
}
