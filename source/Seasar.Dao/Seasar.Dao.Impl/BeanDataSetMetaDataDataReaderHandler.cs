using System;
using System.Text;
using System.Data;
using System.Collections;

namespace Seasar.Dao.Impl
{
    public class BeanDataSetMetaDataDataReaderHandler : AbstractBeanMetaDataDataReaderHandler
    {
        protected const int DEFAULT_TABLE_NUM = 1;
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

        protected virtual void Handle(IDataReader dataReader, DataSet dataSet)
        {
            if ( dataSet.Tables.Count == 0 )
            {
                DataTable table = new DataTable();
                dataSet.Tables.Add(table);
            }
            DataTable[] tables = new DataTable[dataSet.Tables.Count];
            dataSet.Tables.CopyTo(tables, 0);
            dataSet.Load(dataReader, LoadOption.OverwriteChanges, tables);
        }
    }
}
